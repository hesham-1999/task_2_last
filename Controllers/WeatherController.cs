using api1.DTO;
using api1.Model;
using api1.XMLHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

using weatherForm.DTO;

using static System.Net.WebRequestMethods;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace api1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeatherController : ControllerBase
    {

        XmlHelper xmlHelper;
        HttpClient client ;
        public WeatherController()
        {
            xmlHelper = new XmlHelper();
            client = new HttpClient();
        }

        [HttpGet]
        public async Task<IActionResult> get(string name)
        {

            try
            {
                string url = $" http://api.weatherapi.com/v1/current.json?key=cf816923d55440db88580453232712&q={name}";
                HttpResponseMessage response =  client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    Weateher data =  response.Content.ReadAsAsync<Weateher>().Result;

                    string apiuRL = $"https://api.sunrise-sunset.org/json?lat={data.Location.lat}&lng={data.Location.lon}";
                    HttpResponseMessage response2 = await client.GetAsync(apiuRL);
                   
                        var result = await response2.Content.ReadAsStringAsync();
                        var resualtToJSON = JObject.Parse(result);
                        string sunSetTime = resualtToJSON["results"]?["sunrise"]?.ToString();
                        string sunRaiseTime = resualtToJSON["results"]?["sunset"]?.ToString();


                        data.sunRaiseTime = sunRaiseTime;
                        data.sunSetTime = sunSetTime;
                        xmlHelper.saveToXml(data);
                        return Ok(data);
                }
                return NotFound($"Not Found {name}");
            }
            catch (Exception e)
            {
                return BadRequest($"error {e.Message}");
            }
        }

        
        [HttpGet()]
        [Route("/getCountery")]
        public async Task<IActionResult> GetByLocation(string lat ,string lon)
        {
            try
            {
                string path = $"https://api.geoapify.com/v1/geocode/reverse?lat={lat}&lon={lon}&apiKey=7069deeedfd64c95a6f483d762bd0c6b";
                HttpResponseMessage response = client.GetAsync(path).Result;
                if (response.IsSuccessStatusCode)
                {
                    var data = response.Content.ReadAsAsync<CountryName>().Result;
                    string counteryName = data.features[0].properties.state.ToString();
                    return await get(counteryName);
                }
                return NotFound($" Not Found ({lat},{lon}) ");
            }
            catch (Exception e)
            {
                return BadRequest($" error {e.Message} ");
            }
        }





    }
}
