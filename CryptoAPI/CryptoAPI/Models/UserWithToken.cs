namespace CryptoAPI.Models
{
    public class UserWithToken : User
    {
        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }

        public UserWithToken(User user)
        {
            this.UserId = user.UserId;
            this.Email = user.Email;
            this.Name = user.Name;
            this.RoleId = user.RoleId;
            this.ThemeId = user.ThemeId;
            this.Password = user.Password;
            this.ReceiveNotifications = user.ReceiveNotifications;
            this.CreatedAt = user.CreatedAt;

            this.Role = user.Role;
            this.Theme = user.Theme;
        }
    }
}