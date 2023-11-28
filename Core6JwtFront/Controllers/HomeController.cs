using System;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using Core6JwtFront.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using NuGet.Common;
using Core6JwtFront.Entities;
using Microsoft.AspNetCore.Http;

namespace Core6JwtFront.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string baseUrl = "http://localhost:4000";

        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(
            ILogger<HomeController> logger,
            IHttpClientFactory httpClientFactory
        ) {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index() {
            //// Calling the Api and Populate the data in view
            //DataTable dt = new DataTable();

            //using (var client = new HttpClient()) {
            //    client.BaseAddress = new Uri(baseUrl);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //}

            return View();
        }

        [HttpGet]
        public  IActionResult Login() {
            var model = new LoginInfo();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult>  Login(LoginInfo model) {

            if (ModelState.IsValid) {

                var client = _httpClientFactory.CreateClient(baseUrl);
                var request = new HttpRequestMessage(HttpMethod.Post, $"{baseUrl}/users/authenticate") {
                    Content = new StringContent(JsonConvert.SerializeObject(model), null, "application/json")
                };
                var response = await client.SendAsync(request);
                var responseString = await response.Content.ReadAsStringAsync();
                UserResponse? result = JsonConvert.DeserializeObject<UserResponse>(responseString);

                HttpContext.Session.SetString("JWToken", result?.jwtToken??"");

                return Redirect("Index");
            }

            return View(model);
        }

        //public async Task PostRequestAsync(string baseUrl, string url, TReq requestModel, string token = null)
        //    where TRes : class
        //    where TReq : class
        //{
        //    var client = CreateClient(baseUrl, token);
        //    var request = new HttpRequestMessage(HttpMethod.Post, url)
        //    {
        //        Content = new StringContent(JsonConvert.SerializeObject(requestModel), null, "application/json")
        //    };
        //    return await GetResponseResultAsync(client, request);
        //}

        //private async Task GetResponseResultAsync(HttpClient client, HttpRequestMessage request) // where TRes : class
        //{
        //    var response = await client.SendAsync(request);
        //    var responseString = await response.Content.ReadAsStringAsync();
        //    var result = JsonConvert.DeserializeObject(responseString);
        //    return response.IsSuccessStatusCode ? result : throw new ArgumentException(response.ReasonPhrase);
        //}

        public async Task<IActionResult> Users() {
            string? accessToken = HttpContext.Session.GetString("JWToken");
            if (accessToken == null) {
                return Redirect("Error");
            }
           
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"{baseUrl}/users") {
                Headers = {
                    { HeaderNames.Authorization, "Bearer " + accessToken }
                }
            };

            var httpClient = _httpClientFactory.CreateClient();
            var response = await httpClient.SendAsync(httpRequestMessage);

            if (response.StatusCode == HttpStatusCode.OK) {
                var responseString = await response.Content.ReadAsStringAsync();
                List<User>? result = JsonConvert.DeserializeObject<List<User>>(responseString);

                return View(result);

            }

            return Redirect("Error");
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}