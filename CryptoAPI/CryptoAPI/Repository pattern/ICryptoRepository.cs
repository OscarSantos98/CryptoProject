using CryptoAPI.Models;

namespace CryptoAPI.Repository_pattern
{
    public interface ICryptoRepository
    {
        #region UserController
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIDAsync(int id);
        Task<User> GetUserDetailsAsync(int id);
        void ModifyUser(User user);
        bool UserExists(int id);
        void AddUser(User user);
        void DeleteUser(User user);
        Task<User> GetUserWithTokenAsync(User user);
        Task<User> GetUserWithRoleAsync(User user);
        RefreshToken GetRefreshTokenUser(string refreshToken);
        Task<User> GetUserFromAccessToken(string userId);
        #endregion

        #region CryptoController
        Task<IEnumerable<Crypto>> GetAllCryptosAsync();
        Task<Crypto> GetCryptoByIdAsync(int id);
        Task<Crypto> GetCryptoByCoupleNameAsync(string name);
        void AddCrypto(Crypto crypto);
        void DeleteCrypto(Crypto crypto);
        Task SaveChangesAsync();
        #endregion
    }
}
