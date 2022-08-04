using System;
using System.Collections.Generic;

namespace CryptoAPI.Models
{
    public partial class Theme
    {
        public Theme()
        {
            Users = new HashSet<User>();
        }

        public short ThemeId { get; set; }
        public string ThemeName { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
