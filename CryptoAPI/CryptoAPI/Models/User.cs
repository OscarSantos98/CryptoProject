using System;
using System.Collections.Generic;

namespace CryptoAPI.Models
{
    public partial class User
    {
        public User()
        {
            Cryptos = new HashSet<Crypto>();
            RefreshTokens = new HashSet<RefreshToken>();
        }

        public int UserId { get; set; }
        public short? RoleId { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public short? ThemeId { get; set; }
        public bool? ReceiveNotifications { get; set; }
        public DateTime? CreatedAt { get; set; }

        public virtual Role? Role { get; set; }
        public virtual Theme? Theme { get; set; }
        public virtual ICollection<Crypto> Cryptos { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
    }
}
