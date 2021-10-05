using System;
using System.ComponentModel.DataAnnotations;

namespace todoList.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Password { get; set; }

        public bool isAdmin { get; set; }



    }
}