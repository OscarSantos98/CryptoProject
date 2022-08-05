using CryptoAPI.Models;

namespace IntegrationTests.Helpers
{
    public static class Utilities
    {
        private static readonly List<Role> _roles = new List<Role>()
        {
            new Role() { RoleId = 1, RoleDesc = "Admin" }, //, Users =  new List<User>(){_users[0] } },
            new Role() { RoleId = 2, RoleDesc = "User"  }//, Users =  new List<User>(){_users[1], _users [2]}}
        };

        private static readonly List<Theme> _themes = new List<Theme>()
        {
            new Theme(){ ThemeId = 1, ThemeName = "Light" },//,  Users = new List<User>(){_users[1], _users [0] } },
            new Theme(){ ThemeId = 2, ThemeName = "Dark" }// , Users = new List<User>() {_users[2] } }
        };


        private static readonly List<User> _users = new List<User>()
        {
                new User(){
                    UserId = 1, Email = "oscar@gmail.com", Name = "Oscar", RoleId = 1, ThemeId = 1,
                    Password = "Pa$$word1" },// Role = _roles[0], Theme = _themes[0] },
                new User(){
                    UserId = 2, Email = "alice@gmail.com", Name = "Alice", RoleId = 2, ThemeId = 1,
                    Password = "Pa$$word2" },// Role = _roles[1], Theme = _themes[0] },
                new User() { UserId = 3, Email = "rim@gmail.com", Name = "Rim", RoleId = 2, ThemeId = 2,
                    Password = "Pa$$word3" }//, Role = _roles[1], Theme = _themes[1] }
        };


        #region snippet1
        public static void InitializeDbForTests(CryptoDBContext db)
        {
            db.Roles.AddRangeAsync(GetSeedingRoles());
            db.Themes.AddRangeAsync(GetSeedingThemes());
            db.Users.AddRange(GetSeedingUsers());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(CryptoDBContext db)
        {
            db.Users.RemoveRange(db.Users);
            db.Roles.RemoveRange(db.Roles);
            db.Themes.RemoveRange(db.Themes);
            InitializeDbForTests(db);
        }

        public static List<User> GetSeedingUsers()
        {
            return _users;
        }

        public static List<Role> GetSeedingRoles()
        {
            return _roles;
        }

        public static List<Theme> GetSeedingThemes()
        {
            return _themes;
        }
        #endregion
    }
}
