using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Recipe.MVC.Models;
using System.Net.Http;
using System.Text;

namespace Recipe.MVC.Controllers
{
    [Route("[controller]")]
    public class RecipesController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RecipesController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Route("/Recipes")]
        [HttpGet]
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
                TempData["errorMessage"] = ex.Message;
                return View();
            }
            TempData["errorMessage"] = "Something went wrong.";
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
        public async Task<ActionResult<CreateRecipeDto>> Create(CreateRecipeDto model)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("RecipeWebApi");
                string data = JsonConvert.SerializeObject(model);
                StringContent content = new StringContent(data, Encoding.UTF8,
                    "application/json");
                var response = await client.PostAsync(client.BaseAddress, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Recipe created.";
                    return RedirectToAction("GetAllUsersRecipes",
                        new {username = User.FindFirst("name")!.Value});
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
                StringContent content = new StringContent(data, Encoding.UTF8,
                    "application/json");
                var response = await client.PutAsync(client.BaseAddress, content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["successMessage"] = "Recipe updated.";
                    return RedirectToAction("GetDetails", new {id = model.Id});
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
                return View();
            }
            TempData["errorMessage"] = "Something went wrong.";
            return View();
        }
    }
}
