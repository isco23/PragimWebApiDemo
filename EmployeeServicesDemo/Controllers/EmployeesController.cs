using EmployeeDataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EmployeeServicesDemo.Controllers
{
    [EnableCorsAttribute("*","*","*")]
    public class EmployeesController : ApiController
    {        
        [BasicAuthentication]
        public HttpResponseMessage Get(string gender = "All")
        {
            string username = Thread.CurrentPrincipal.Identity.Name;
            using(EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                switch (username.ToLower())
                {                   
                    case "male":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(x=>x.Gender == "male").ToList());
                    case "female":
                        return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.Where(x => x.Gender == "female").ToList());
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Gender must be Male or Female");
                }                
            }
        }

        [HttpGet]
        public HttpResponseMessage LoadEmployeeById(int id)
        {
            using (EmployeeDBEntities entities = new EmployeeDBEntities())
            {
                var emp = entities.Employees.FirstOrDefault(e => e.ID == id);
                if(emp != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, emp);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee cannot be found");
                }
            }                      
        }

        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entites = new EmployeeDBEntities())
                {
                    entites.Employees.Add(employee);
                    entites.SaveChanges();
                    var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                    message.Headers.Location = new Uri(Request.RequestUri + "/"+ employee.ID.ToString());
                    return message;
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        public HttpResponseMessage Delete(int id)
        {
            try
            {
                using (EmployeeDBEntities entites = new EmployeeDBEntities())
                {
                    var emp = entites.Employees.FirstOrDefault(x => x.ID == id);
                    if (emp == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee Not Found!");
                    }
                    else
                    {
                        entites.Employees.Remove(emp);
                        entites.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, ex);
            }
            
        }

        public HttpResponseMessage Put(int id, [FromBody] Employee employee)
        {
            try
            {
                using (EmployeeDBEntities entities = new EmployeeDBEntities())
                {
                    var emp = entities.Employees.FirstOrDefault(e => e.ID == id);
                    if (emp == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee Not Found!");
                    }
                    else
                    {
                        emp.FirstName = employee.FirstName;
                        emp.LastName = employee.LastName;
                        emp.Gender = employee.Gender;
                        emp.Salary = employee.Salary;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, emp);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee Not Found!",ex);
            }
            
        }
    }
}
