using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CompanyManagement.Models
{
    public class UsersViewModel
    {
        public int IdUser { get; set; }
        [Required]
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Phone { get; set; }

    }
}