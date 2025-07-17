using RecipeApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Text;
using RecipeApplication.Areas.Identity.Data;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using RecipeApplication.Services;

namespace RecipeApplication.Controllers
{
	public class RecipeController : Controller
	{
		static readonly HttpClient client = new HttpClient();
		private readonly RecipeApplicationDbContext _context;
		private readonly ILogger<RecipeController> _logger;
		private readonly IConfiguration _configuration;
		private readonly bool _useDemoData;
		private readonly DemoDataService _demoDataService;
		private string apiKey;

		
		public RecipeController(RecipeApplicationDbContext context, ILogger<RecipeController> logger, IConfiguration configuration, IWebHostEnvironment env)
		{
			_context = context;
			_logger = logger;
			_configuration = configuration;
			_useDemoData = env.IsDevelopment();
			_demoDataService = new DemoDataService();
			apiKey = _configuration["SpoonacularApiKey"] ?? Environment.GetEnvironmentVariable("API_KEY") ?? throw new InvalidOperationException("API_KEY not found. Please set it in appsettings.Development.json or as an environment variable.");
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		public async Task<ActionResult<AddFoodModel>> ShowSearchResults(string searchPhrase)
		{
			try
			{
				RecipeModel recipeModel;
				if (_useDemoData)
				{
					recipeModel = _demoDataService.GetDemoRecipes(searchPhrase);
				}
				else
				{
					// Construct the search URL
					string searchUrl = $"https://api.spoonacular.com/recipes/complexSearch?apiKey={apiKey}&query={searchPhrase}&number=6&addRecipeInformation=true";

					// Send the HTTP request and read the response
					HttpResponseMessage searchResponse = await client.GetAsync(searchUrl);
					searchResponse.EnsureSuccessStatusCode();
					string searchResponseBody = await searchResponse.Content.ReadAsStringAsync();

					// Deserialize the JSON response into a RecipeModel object
					recipeModel = JsonConvert.DeserializeObject<RecipeModel>(searchResponseBody);
				}

				AddFoodModel afm = new AddFoodModel
				{
					showMealPlan = new ShowMealPlan(),
					recipes = recipeModel
				};

				return View("SearchResults", afm);
			}
			catch (HttpRequestException e)
			{
				_logger.LogError(e, "HTTP Request Exception");
				return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An unexpected error occurred.");
				return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
		}


		public async Task<IActionResult> ClearMealPlanDay()
		{
			string user = GetUserByIdentity().SpoonacularUsername;
			string date = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd");
			string hash = GetUserByIdentity().SpoonacularHash;
			string url = $"https://api.spoonacular.com/mealplanner/{user}/day/{date}?apiKey={apiKey}&hash={hash}";

			try
			{
				using HttpResponseMessage response = await client.DeleteAsync(url);
				if (response.IsSuccessStatusCode)
				{
					string responseBody = response.Content.ReadAsStringAsync().Result;
					return NoContent();
				}
				else
				{
					return View("Error");
				}
			}
			catch (HttpRequestException e)
			{
				return View("Error");
			}
		}

		[Authorize]
		[HttpPost]
		public async Task<IActionResult> AddMealPlanItem(int itemId, string itemTitle, string itemSourceUrl, string itemSlot, string itemServing)
		{
			try
			{
				// Create MealPlannerModel
				MealPlannerModel MPM = new MealPlannerModel
				{
					date = (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
					type = "RECIPE",
					slot = int.Parse(itemSlot),
					value = new ValueModel
					{
						id = itemId,
						title = itemTitle,
						SourceUrl = itemSourceUrl,
						servings = int.Parse(itemServing)
					}
				};

				// Serialize the MealPlannerModel to JSON
				var newMealJson = JsonConvert.SerializeObject(MPM);

				// Create HTTP content from the JSON data
				var content = new StringContent(newMealJson, Encoding.UTF8, "application/json");

				// Retrieve user information based on their identity
				var user = GetUserByIdentity();

				// Construct the API endpoint URL
				var endpoint = new Uri($"https://api.spoonacular.com/mealplanner/{user.SpoonacularUsername}/items?apiKey={apiKey}&hash={user.SpoonacularHash}");


				// Send an asynchronous HTTP POST request to the API
				var response = await client.PostAsync(endpoint, content);

				// Ensure the response indicates success (HTTP status code 2xx)
				if (response.IsSuccessStatusCode)
				{
					return RedirectToAction("DisplayMealPlan");
				}

				return View("Index");
			}
			catch (Exception ex)
			{
				return View("Error");
			}
		}

		[Authorize]
		[HttpDelete]
		public async Task<IActionResult> DeleteMealPlanItem(int itemId)
		{
			try
			{
				// Retrieve user information based on their identity
				var user = GetUserByIdentity();

				// Construct the API endpoint URL
				var endpoint = new Uri($"https://api.spoonacular.com/mealplanner/{user.SpoonacularUsername}/items/{itemId}?apiKey={apiKey}&hash={user.SpoonacularHash}");


				// Send an asynchronous HTTP DELETE request to the API
				var response = await client.DeleteAsync(endpoint);

				// Ensure the response indicates success (HTTP status code 2xx)
				if (response.IsSuccessStatusCode)
				{
					return NoContent();
				}
				else
				{
					_logger.LogError($"Failed to delete item with ID: {itemId}. API returned status code: {response.StatusCode}");
					return View("Error");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while deleting the meal plan item.");

				return View("Error");
			}
		}


		[Authorize]
		[HttpGet]
		public async Task<ActionResult<ShowMealPlan>> DisplayMealPlan()
		{
			try
			{
				ShowMealPlan mealPlan;
				if (_useDemoData)
				{
					mealPlan = _demoDataService.GetDemoMealPlan();
					return Json(mealPlan);
				}
				else
				{
					string user = GetUserByIdentity().SpoonacularUsername;
					string date = DateTimeOffset.UtcNow.ToString("yyyy-MM-dd");
					string hash = GetUserByIdentity().SpoonacularHash;

					string url = string.Format("https://api.spoonacular.com/mealplanner/{0}/day/{1}?apiKey={2}&hash={3}", user, date, apiKey, hash);

					using var response = await client.GetAsync(url);

					if (response.IsSuccessStatusCode)
					{
						string responseBody = await response.Content.ReadAsStringAsync();
						mealPlan = JsonConvert.DeserializeObject<ShowMealPlan>(responseBody)
							?? throw new JsonException("Error deserializing meal plan data.");

						return Json(mealPlan);
					}

					// Handle non-successful responses here (e.g., log the error, provide user feedback).
					// You may choose to return a different partial view or take other actions.
					return View("Index");
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while displaying the meal plan.");
				return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
		}


		[HttpGet]
		public async Task<IActionResult> GenerateMealPlanAsync(string calories, string inlineRadioOptions)
		{
			try
			{
				string url;
				if (inlineRadioOptions == "No Diet")
				{
					url = string.Format("https://api.spoonacular.com/mealplanner/generate?timeFrame=day&apiKey={0}&targetCalories={1}", apiKey, calories);
				}
				else
				{
					url = string.Format("https://api.spoonacular.com/mealplanner/generate?timeFrame=day&apiKey={0}&targetCalories={1}&diet={2}", apiKey, calories, inlineRadioOptions);
				}

				using var response = await client.GetAsync(url);

				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					GeneratedMealPlan generatedMealPlan = JsonConvert.DeserializeObject<GeneratedMealPlan>(responseBody)
						?? throw new JsonException("Error deserializing meal plan data.");

					return Json(generatedMealPlan);
				}

				return View("Index");
			}
			catch (HttpRequestException ex)
			{
				_logger.LogError(ex, "HTTP request error.");
				return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
			catch (JsonException ex)
			{
				_logger.LogError(ex, "JSON deserialization error.");
				return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
			}
		}

		private RecipeApplicationUser GetUserByIdentity()
		{
			var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
			return _context.Users.FirstOrDefault(x => x.Id == currentUserId);
		}


		private void HandleHttpRequestException(HttpRequestException ex)
		{
			_logger.LogError(ex, "HTTP request error.");
		}

		private void HandleJsonException(JsonException ex)
		{
			_logger.LogError(ex, "JSON deserialization error.");
		}
	}
}
