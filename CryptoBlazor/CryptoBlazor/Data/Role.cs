using System;
using System.Collections.Generic;

namespace CryptoBlazor.Data
{
    public partial class Role
    {
        public short RoleId { get; set; }
        public string RoleDesc { get; set; } = null!;

        public virtual ICollection<UserAction> UserActions { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
