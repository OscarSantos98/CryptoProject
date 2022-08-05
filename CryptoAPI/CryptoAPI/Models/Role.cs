using System;
using System.Collections.Generic;

namespace CryptoAPI.Models
{
    public partial class Role
    {
        public Role()
        {
            UserActions = new HashSet<UserAction>();
            Users = new HashSet<User>();
        }

        public short RoleId { get; set; }
        public string RoleDesc { get; set; } = null!;

        public virtual ICollection<UserAction> UserActions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
