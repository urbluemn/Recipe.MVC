using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipe.MVC.Models;
using Recipe.MVC.src.Models;
using System.Reflection;
using System.Text;

namespace Recipe.MVC.src.Controllers
{
    [Route("[controller]")]
    public class RecipesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _environment;

        public RecipesController(IHttpClientFactory httpClientFactory, IWebHostEnvironment environment)
        {
            _httpClientFactory = httpClientFactory;
            _environment = environment;
        }

        [HttpGet("/Recipes")]
        public async Task<ActionResult<RecipeListVm>> Index()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");
                var response = await client.GetAsync(client.BaseAddress);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var modelList = JsonConvert.DeserializeObject<RecipeListVm>(data);
                    return View(modelList);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = $"Something went wrong, {ex.Message}";
                return View();
            }
            TempData["errorMessage"] = "Something went wrong, please try again later.";
            return View();
        }

        [HttpGet]
        [Route("{username}")]
        public async Task<ActionResult<RecipeListVm>> GetAllUsersRecipes(string username)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");
                var response = await client.GetAsync(client.BaseAddress + $"/{username}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RecipeListVm>(data);
                    if (username == User.FindFirst("name")!.Value)
                    {
                        return View("GetMyCreatedRecipes", result);
                    }
                    ViewBag.Username = username;
                    return View(result);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            TempData["errorMessage"] = "Something went wrong.";
            return View();
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<RecipeDetailsVm>> GetDetails(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");
                var response = await client.GetAsync(client.BaseAddress + $"/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RecipeDetailsVm>(data);
                    return View(result);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            TempData["errorMessage"] = "Something went wrong.";
            return View();
        }

        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<ActionResult<CreateRecipeDto>> Create(CreateRecipeDto model/*, IFormFile image*/)
        {
            try
            {
                // if(image.Length > 0)
                // {
                //     var path = Path.Combine(_environment.WebRootPath, "Images/", image.FileName);
                //     using (var stream = new FileStream(path, FileMode.Create))
                //     {
                //         await image.CopyToAsync(stream);
                //     }

                //     var recipe = new CreateRecipeDto{
                //         Name = model.Name,
                //         Details = model.Details,
                //         Description = model.Description
                //     };
                // }

                var client = _httpClientFactory.CreateClient("RecipeWebApi");
                string data = JsonConvert.SerializeObject(model);
                var content = new StringContent(data, Encoding.UTF8,
                    "application/json");
                var response = await client.PostAsync(client.BaseAddress, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Recipe created.";
                    return RedirectToAction("GetAllUsersRecipes",
                        new { username = User.FindFirst("name")!.Value });
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            TempData["errorMessage"] = "Something went wrong.";
            return View();
        }

        [HttpGet("Edit")]
        public async Task<ActionResult<UpdateRecipeDto>> Edit(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");
                var response = await client.GetAsync(client.BaseAddress + $"/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<UpdateRecipeDto>(data);
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            TempData["errorMessage"] = "Something went wrong.";
            return View();
        }

        [HttpPost("Edit")]
        public async Task<ActionResult<UpdateRecipeDto>> Edit(UpdateRecipeDto model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");

                string data = JsonConvert.SerializeObject(model);
                var content = new StringContent(data, Encoding.UTF8,
                    "application/json");
                var response = await client.PutAsync(client.BaseAddress, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = $"Recipe updated {model.Name}.";
                    return RedirectToAction("GetAllUsersRecipes", new { username = User?.FindFirst("name").Value });
                    //return RedirectToAction("GetDetails", new { id = model.Id });
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            TempData["errorMessage"] = "Something went wrong.";
            return View();
        }

        [HttpGet("Delete")]
        public async Task<ActionResult<RecipeDetailsVm>> Delete(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");
                var response = await client.GetAsync(client.BaseAddress + $"/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RecipeDetailsVm>(data);
                    return View(result);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            TempData["errorMessage"] = "Something went wrong.";
            return View();
        }

        [HttpPost("Delete"), ActionName("Delete")]
        public async Task<ActionResult<RecipeDetailsVm>> DeleteConfirmed(Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");
                var response = await client.DeleteAsync(client.BaseAddress + $"/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Recipe deleted.";
                    return RedirectToAction("GetAllUsersRecipes",
                        new { username = User.FindFirst("name")!.Value });
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetAllUsersRecipes",
                    new { username = User.FindFirst("name")!.Value });
            }
            TempData["errorMessage"] = "Something went wrong.";
            return RedirectToAction("GetAllUsersRecipes",
                new { username = User.FindFirst("name")!.Value });
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save([FromQuery]Guid recipeId)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");

                var dto = new SaveRecipeDto { RecipeId = recipeId };

                string data = JsonConvert.SerializeObject(dto);
                var content = new StringContent(data, Encoding.UTF8,
                    "application/json");
                var response = await client.PostAsync(client.BaseAddress + $"/Save", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Recipe saved.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("Index");
            }
            TempData["errorMessage"] = "Already saved.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Saved")]
        public async Task<ActionResult<RecipeListVm>> GetSaved()
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");
                var response = await client.GetAsync(client.BaseAddress + $"/Saved");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<RecipeListVm>(data);
                    return View(result);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View("GetMyCreatedRecipes");
            }
            TempData["errorMessage"] = "Something went wrong.";
            return View("GetMyCreatedRecipes");
        }

        [HttpPost]
        [Route("DeleteSaved")]
        public async Task<IActionResult> DeleteSaved([FromQuery] Guid id)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");
                var response = await client.DeleteAsync(client.BaseAddress + $"/DeleteSaved/{id}");
                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Saved recipe removed.";
                    return RedirectToAction("GetSaved");
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return RedirectToAction("GetSavedRecipes");
            }
            TempData["errorMessage"] = "Something went wrong.";
            return RedirectToAction("GetSavedRecipes");
        }
    }
}
