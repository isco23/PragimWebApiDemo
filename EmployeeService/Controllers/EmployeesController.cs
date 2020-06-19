using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeService.Controllers
{
    //[Authorize]
    [RoutePrefix("api/employees")]
    public class EmployeesController : ApiController
    {
        [Route("")]
        public IEnumerable<Employee> Get()
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.ToList();
            }
        }

        [Route("{id:int}" , Name = "GetStudentById")]
        public Employee GetById(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.FirstOrDefault(x => x.ID == id);
            }
        }

        [Route("{name:alpha}")]
        public Employee GetByName(string name)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                return entities.Employees.Where(x => x.FirstName == name).FirstOrDefault();
            }
        }

        [HttpPost]
        [Route("")]
        public HttpResponseMessage Post(Employee employee)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                entities.Employees.Add(employee);
                entities.SaveChanges();
                var response = Request.CreateResponse(HttpStatusCode.Created);
                response.Headers.Location = new Uri(Url.Link("GetStudentById",new { id = employee.ID }));
                return response.EnsureSuccessStatusCode();
            }
        }
    }
}
