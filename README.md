# .NET Core Bootcamp
In this bootcamp we are going to focus on creating a webapi project.  We will focus on building a frontend application in a later module
### Prerequisites
Install the .NET Core 3.1 [SDK](https://dotnet.microsoft.com/download/dotnet-core/3.1) for your appropriate operating system   
  
Upon completing the installation, you should be able to run the following command to validate you have installed the SDK correctly.  It should display the installed version of the SDK i.e. 3.1.xxx
 ```
 dotnet --version
 ``` 
 ### Development Environment
 .NET Core is now a cross platform development and runtime platform.  There are many [tools](https://visualstudio.microsoft.com/) you can use develop applications for the .NET Core platform.  This lab will focus on using the command line and [VSCode](https://code.visualstudio.com/)

 Install [VSCode](https://code.visualstudio.com/) 

 # Getting Started - Lab 1
 ## Create a blank solution
 A solution is a structure for organizing projects in Visual Studio. The solution maintains the state information for projects. The solution is not necessary in VSCode, however, one of the benefits of using the .NET Core plaform is allowing developers to use whichever tools they prefer on whichever OS they prefer.  In order to make that experience consistant for all developers that may contribute to your project, we want to create a solution file so those developers that choose the full Visual Studio IDE will have a consistant exerience 
 ```
 mkdir myfirstapp
 cd myfirstapp
 dotnet new sln --name  myfirstapp
 ```
 ## Create a core class library project
 We are going to create a core project that will contain our common components such as entities, utilities, and other things that might be used in multiple projects
 ```
 dotnet new classlib --name myfirstapp.core
 ```
 Add this project to the solution
 ```
 dotnet sln add myfirstapp.core/myfirstapp.core.csproj
 ```
 Verify it was added successfully
 ```
 dotnet sln myfirstapp.sln list
 ```
 ## Create a webapi project
 ```
 dotnet new webapi --name myfirstapp.api
 ```
 Add a project reference to the myfirstapp.core project
 ```
 dotnet add myfirstapp.api/myfirstapp.api.csproj reference myfirstapp.core/myfirstapp.core.csproj
 ```
 Add the project to the solution
 ```
 dotnet sln add myfirstapp.api/myfirstapp.api.csproj
 ```
 Verify it was added successfully
 ```
 dotnet sln myfirstapp.sln list
 ```
 Build our new solution
 ```
 dotnet build
 ```
 You see a message displayed indicating the 'Build succeeded.'
 ## Launch VSCode
 Typing the following command from any path will launch an instance of VSCode and load the content of the folder directly into the IDE
 ```
 code .
 ```
 # Create Entity Model
 * Delete Class1.cs from the myfirstapp.core project
 * Create a new folder "domain" inside of the myfirstapp.core project
 * Create a new class file "Employee.cs" inside myfirstapp.core/domain
 * Add the following code
 ``` csharp
 namespace myfirstapp.core.domain
 {
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
    }
 }
 ```
 # Create Controller API
 * Delete the Controllers/WeatherForecastController.cs from the myfirstapp.api project
 * Delete the WeatherForecast.cs from the myfirstapp.api project
 * Create a new controller class file "EmployeeController.cs" in the myfirstapp.api.Controllers folder
 * Add the following code
 ```csharp
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
 ```
 ### Add Swagger
 [Swagger](https://swagger.io/) tooling for API's built with ASP.NET Core. Generate beautiful API documentation, including a UI to explore and test operations, directly from your routes, controllers and models  
 
 At your command prompt, let's add a [Nuget](https://www.nuget.org/) reference to the Swagger library.  
 ```
 dotnet add myfirstapp.api/myfirstapp.api.csproj package Swashbuckle.AspNetCore
 ```
 #### Modify the Startup.cs
 Add the following to myfirstapp.api/myfirstapp.api/startup.cs
 * Within the ConfigureServices method
 ```csharp
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Acme WebAPI", Version = "v1" });
});
 ```
 * Add the following using statement to the top of the page
 ```csharp
 using Microsoft.OpenApi.Models;
 ```
 * Within the Configure method
 ```csharp
app.UseSwagger();

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Acme WebAPI v1");
});
 ```
 ## Run the application
 At your command prompt
 ```
 dotnet run --project myfirstapp.api/myfirstapp.api.csproj
 ```
 Now open a browser and navigate to https://localhost:5001/swagger
 You should see the Swagger UI appear with one new api listed "Employee". Try it out and click Execute.  You should see 5 employee names returns in the response body