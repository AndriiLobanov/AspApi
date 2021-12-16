using ASPnetAuto.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ASPnetAuto.Helpers;
namespace ASPnetAuto.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static List<Auto> _autos;
        private static List<AutoInfo> _Model;
        private static List<AutoModel> _modelAuto;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
           // _autos = new List<Auto>();
            //_Model = new List<AutoInfo>();
        }
        public async Task<IActionResult> Index()
        {
            _autos = new List<Auto>();

            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://vpic.nhtsa.dot.gov/api/vehicles/getallmakes?format=json"),

            };
            using (var response = await client.SendAsync(request))
            {
                //response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var body1 = Helper.GetUntilOrEmpty(body);
                _autos = JsonConvert.DeserializeObject<List<Auto>>(body1);
            }
            
            return View(_autos);
        }
        [Route("Get_Auto/{ID:int}")]
        public async Task<IActionResult> Get_Auto(int ID)
        {
            _Model = new List<AutoInfo>();
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://vpic.nhtsa.dot.gov/api/vehicles/getvehiclevariablelist?format=json"),

            };
            using (var response = await client.SendAsync(request))
            {
                //response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var body1 = Helper.GetUntilOrEmpty(body);
                _Model = JsonConvert.DeserializeObject<List<AutoInfo>>(body1);
            }
            return View(_Model.FirstOrDefault(a => a.ID == ID.ToString()));
        }

        public ViewResult GetByName() => View();

        [HttpPost]
        public  IActionResult GetByName(int id)
        {
            return View(_autos[id]);
        }

        public async Task<IActionResult> ModelAuto(int Make_ID)
        {
            _modelAuto = new List<AutoModel>();
            var client = new HttpClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://vpic.nhtsa.dot.gov/api/vehicles/GetModelsForMakeId/" + Make_ID + "?format=json"),

            };
            using (var response = await client.SendAsync(request))
            {
                //response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var body1 = Helper.GetUntilOrEmpty(body);
                _modelAuto = JsonConvert.DeserializeObject<List<AutoModel>>(body1);
            }
            int counter = 0;
            foreach (var auto in _modelAuto)
            {
                auto.ID = counter;
                counter++;
            }
            return View(_modelAuto.Where(x => x.Make_ID.Equals(Make_ID.ToString())));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
