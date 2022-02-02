using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
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
            CallWebAPIAsync(5, "219c0dee-6eb7-4c92-a022-08d9da505136").Wait();
        }
        static async Task CallWebAPIAsync(int select, string id =null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5000/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                switch (select)
                {
                    //get method
                    case 0:
                        HttpResponseMessage response_get = await client.GetAsync("api/v1/department/"+id);
                        
                        if (response_get.IsSuccessStatusCode)
                        {
                            string dept = await response_get.Content.ReadAsStringAsync();
                            Result result = JsonConvert.DeserializeObject<Result>(dept);

                            Console.WriteLine("Id:{0}\tName:{1}", result.Data.Id, result.Data.Name);

                        }
                        else
                        {
                            Console.WriteLine("Internal server Error");
                        }
                        break;
                    //post method
                    case 1:
                        
                        var department = new Department()
                        {
                            Name = "QA",
                            Description = "They shall be responsible for testing newly developed applications"
                        };
                        HttpResponseMessage response_post = await client.PostAsJsonAsync("api/v1/department", department);

                        if (response_post.IsSuccessStatusCode)
                        {
                            // Get the URI of the created resource.
                            Uri returnUrl = response_post.Headers.Location;
                            Console.WriteLine(returnUrl);
                        }
                        break;
                    case 3:
                        //Put method
                        var department_update = new Department()
                        {
                            Name = "QA/QC",
                            Description = "They shall be responsible for testing newly developed applications"
                        };
                        HttpResponseMessage response_update = await client.PutAsJsonAsync("api/v1/department/"+id, department_update);

                        if (response_update.IsSuccessStatusCode)
                        {
                            // Get the URI of the created resource.
                            Uri returnUrl = response_update.Headers.Location;
                            Console.WriteLine(returnUrl+" Success, department with id "+id+" has been updated");
                        }
                        break;
                    case 4:
                        //Delete method
                        
                        HttpResponseMessage response_delete = await client.DeleteAsync("api/v1/department/" + id);

                        if (response_delete.IsSuccessStatusCode)
                        {
                            // Get the URI of the created resource.
                            Uri returnUrl = response_delete.Headers.Location;
                            Console.WriteLine(returnUrl + " Success, department with id " + id + " has been deleted");
                        }
                        break;
                    case 5:
                        //Get employees under a department
                        HttpResponseMessage get_dept_employess = await client.GetAsync("api/v1/department/" + id + "/employees");
                        var check = get_dept_employess.Content.ReadAsStringAsync();
                        if (get_dept_employess.IsSuccessStatusCode)
                        {
                            string dept = await get_dept_employess.Content.ReadAsStringAsync();
                            Result result = JsonConvert.DeserializeObject<Result>(dept);

                            //Console.WriteLine("Id:{0}\tName:{1}", result.Data.Employees, result.Data.Employees);
                            foreach (var item in result.Data.Employees)
                            {
                                Console.WriteLine("Id:{0}\tName:{1}", item.Id, item.FirstName);
                            }
                        }
                        else
                        {
                            Console.WriteLine("Internal server Error");
                        }
                        break;
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
        public ICollection<Employee> Employees { get; set; }

    }
    public class Employee
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String StaffId { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public String Street { get; set; }
        public String City { get; set; }
        public String State { get; set; }
    }
    public class Result
    {
        public Department Data { get; set; }        
    }
}
