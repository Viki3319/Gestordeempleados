dotnet new webapi -n GestorEmpleados
cd GestorEmpleados

dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Design



using System;

namespace GestorEmpleados.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaContratacion { get; set; }
        // Otros campos según necesidades
    }
}


{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=GestorEmpleadosDB;Trusted_Connection=True;"
  }
}

using Microsoft.EntityFrameworkCore;
using GestorEmpleados.Models;

namespace GestorEmpleados.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
    }
}


using Microsoft.EntityFrameworkCore;
using GestorEmpleados.Data;

public void ConfigureServices(IServiceCollection services)
{
    services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

    // Otros servicios
}

using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using GestorEmpleados.Data;
using GestorEmpleados.Models;

namespace GestorEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmployeeController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Employee>> Get()
        {
            return _context.Employees.ToList();
        }

        [HttpPost]
        public ActionResult<Employee> Post(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

    }
}

dotnet run
