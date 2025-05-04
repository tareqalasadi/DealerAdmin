using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using DealerAdmin.Models;
using System.Net.Http;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _IConfiguration;

    public HomeController(IHttpClientFactory httpClientFactory,IConfiguration configuration)
    {
        _httpClientFactory=httpClientFactory;
        _IConfiguration=configuration;
    }
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync(_IConfiguration["baseUrl"]+"GetCategories");
        List<PropertyCategory> categories = new();

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            categories = JsonConvert.DeserializeObject<List<PropertyCategory>>(json);
        }

        ViewBag.Categories = categories;
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> AddCategory(PropertyCategory category, IFormFile imageFile)
    {
        if (imageFile != null && imageFile.Length > 0)
        {
            // Set your exact physical path
            var folderPath = _IConfiguration["ImagePath"];

            // Ensure the folder exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Create a unique file name to avoid conflicts
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(folderPath, fileName);

            // Save the file to the specified path
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Optionally set the relative image path for the category (if needed)
            category.ImagePath = "~/images/MainImage/" + fileName;
        }

        using (var client = new HttpClient())
        {
            var content = new StringContent(JsonConvert.SerializeObject(category), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(_IConfiguration["baseUrl"]+"AddCategory", content);
            if (response.IsSuccessStatusCode)
                ViewBag.Message = "Category Added Successfully!";
        }

        return RedirectToAction("Index");
    }



    [HttpPost]
    public async Task<ActionResult> AddProperty(Propertys property)
    {
        if (property.mainImage != null && property.mainImage.Length > 0)
        {
            using (var ms = new MemoryStream())
            {
                await property.mainImage.CopyToAsync(ms);
                property.Image = ms.ToArray();
            }
        }

        var imageList = new List<byte[]>();
        if (property.images != null && property.images.Count > 0)
        {
            foreach (var img in property.images)
            {
                if (img.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await img.CopyToAsync(ms);
                        imageList.Add(ms.ToArray()); 
                    }
                }
            }
        }

        using (var client = _httpClientFactory.CreateClient())
        {
            var propertyDto = new
            {
                Property = property,
                Images = imageList
            };

            var json = JsonConvert.SerializeObject(propertyDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(_IConfiguration["baseUrl"]+"AddProperty", content);

            if (response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Property Added Successfully!";
                return RedirectToAction("Index");
            }
        }

        return View();
    }
}
