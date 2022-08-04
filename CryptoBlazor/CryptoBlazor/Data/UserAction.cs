using System;
using System.Collections.Generic;

namespace CryptoBlazor.Data
{
    public partial class UserAction
    {
        public short UserActionsId { get; set; }
        public string? UserActions { get; set; }
        public short? RoleId { get; set; }

        public virtual Role? Role { get; set; }
    }
}
