using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace CrudApp.Models
{
    public class ProjectDB:DbContext
    {
        public ProjectDB(): base("CrudApp")
        {

        }

        public DbSet<Employee> Employees { get; set; }
    }
}