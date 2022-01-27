using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace HttpClientProject
{
    public class HttpClientProject
    {
        static void Main(string[] args)
        {
            CallWebAPIAsync().Wait();
        }
        static async Task CallWebAPIAsync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //Get Method
                HttpResponseMessage response = await client.GetAsync("api/v1/department/0e7ca2ad-99fb-4cb8-a021-08d9da505136");
                if (response.IsSuccessStatusCode)
                {
                    string dept = await response.Content.ReadAsStringAsync();
                    Result result = JsonConvert.DeserializeObject<Result>(dept);
                    
                    Console.WriteLine("Id:{0}\tName:{1}", result.Data.Id, result.Data.Description);
                    
                }
                else
                {
                    Console.WriteLine("Internal server Error");
                }
            }
            Console.ReadKey();
        }
    }

    public class Department
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
    public class Result
    {
        public Department Data { get; set; }
    }
}
