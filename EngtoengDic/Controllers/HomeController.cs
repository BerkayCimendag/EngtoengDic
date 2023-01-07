using EngtoengDic.data;
using EngtoengDic.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Newtonsoft.Json;

namespace EngtoengDic.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> IndexAsync(Word word)
        {
            try
            {
                string url = $"https://api.dictionaryapi.dev/api/v2/entries/en/{word.Name}";
                HttpClient client = new HttpClient();

                HttpResponseMessage cevap = await client.GetAsync(url);

                string? json = await cevap.Content.ReadAsStringAsync();
                dynamic? data = JsonConvert.DeserializeObject(json);

                word.Meaning = data[0].meanings[0].definitions[0].definition;
                ViewBag.Meaning = word.Meaning;

                word.Example = data[0].meanings[2].definitions[0].example;
                ViewBag.Example = word.Example;

                word.Synonym = data[0].meanings[0].synonyms[0];
                ViewBag.Synonym = word.Synonym;

            }
            catch (Exception ex)
            {

                ViewBag.Error = ex.Message;
            }


            return View(word);
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
    }
}