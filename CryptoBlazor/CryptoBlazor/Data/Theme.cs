using System;
using System.Collections.Generic;

namespace CryptoBlazor.Data
{
    public partial class Theme
    {
        public short ThemeId { get; set; }
        public string ThemeName { get; set; } = null!;

        public virtual ICollection<User> Users { get; set; }
    }
}
