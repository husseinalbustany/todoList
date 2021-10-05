using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using todoList.Models;

namespace todoList.Data
{
    public class todoListContext : DbContext
    {
        public todoListContext (DbContextOptions<todoListContext> options)
            : base(options)
        {
        }

        public DbSet<todoList.Models.todo> todo { get; set; }

        public DbSet<todoList.Models.Users> Users { get; set; }
    }
}
