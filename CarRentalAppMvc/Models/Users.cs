using System;
using Microsoft.AspNetCore.Identity;

namespace CarRentalAppMvc.Models
{
   
        public class Users


    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 


    }
    
}

