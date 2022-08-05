using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace CryptoBlazor.Data
{
    public class User
    {
        public int UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }

        public short? RoleId { get; set; }
        public virtual Role? Role { get; set; }
        public string? Name { get; set; }
        public short? ThemeId { get; set; }
        public virtual Theme? Theme { get; set; }

    }
}
