using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using myfirstapp.core.domain;

namespace myfirstapp.api.Controllers
{
   [ApiController]
   [Route("api/[controller]")]
   public class EmployeeController : ControllerBase
   {
       private static readonly string[] Names = new[]
       {
           "Bob", "Jim", "Fred", "Sally", "Linda", "Mary", "Tracy", "Taylor", "Steve", "David"
       };

       private readonly ILogger<EmployeeController> _logger;

       public EmployeeController(ILogger<EmployeeController> logger)
       {
           _logger = logger;
       }

       [HttpGet]
       public IEnumerable<Employee> Get()
       {
           _logger.LogInformation("get employees");
           var employees = new List<Employee>();
           var rng = new Random();
           return Enumerable.Range(1, 5).Select(index => {
               var first = Names[rng.Next(Names.Length)];
               return new Employee
               {
                   FirstName = first,
                   LastName = "Smith",
                   Phone = "(555)555-55-5555",
                   Email = $"{first}.smith@test.com"
               };
           })
           .ToArray();
       }
   }
}