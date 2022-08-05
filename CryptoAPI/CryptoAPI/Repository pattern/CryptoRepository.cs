using CryptoAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoAPI.Repository_pattern
{
    public class CryptoRepository : ICryptoRepository
    {
        private readonly CryptoDBContext _context;

        public CryptoRepository(CryptoDBContext context)
            => _context = context;

        #region UserController
        public async Task<IEnumerable<User>> GetUsersAsync()
            => await _context.Users.ToListAsync();
        public async Task<User> GetUserByIDAsync(int id)
            => await _context.Users.FindAsync(id);
        public async Task<User> GetUserDetailsAsync(int id)
        {
            return await _context.Users.Include(u => u.Role)
                                           .Include(u => u.Theme)
                                           .Include(u => u.Cryptos)
                                            .Where(u => u.UserId == id)
                                            .FirstOrDefaultAsync();
        }
        public void ModifyUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public bool UserExists(int id)
            => _context.Users.Any(e => e.UserId == id);
        public void AddUser(User user)
            => _context.Users.Add(user);
        public void DeleteUser(User user)
            => _context.Users.Remove(user);
        public async Task<User> GetUserWithTokenAsync(User user)
            => await _context.Users.Include(u => u.Role)
                                        .Where(u => u.Email == user.Email
                                                && u.Password == user.Password).FirstOrDefaultAsync();
        public async Task<User> GetUserWithRoleAsync(User user)
            => await _context.Users.Include(u => u.Role)
                                        .Where(u => u.UserId == user.UserId).FirstOrDefaultAsync();

        public RefreshToken GetRefreshTokenUser(string refreshToken)
            => _context.RefreshTokens.Where(rt => rt.Token == refreshToken)
                                                .OrderByDescending(rt => rt.ExpiryDate)
                                                .FirstOrDefault();
        public async Task<User> GetUserFromAccessToken(string userId)
            => await _context.Users.Include(u => u.Role)
                                    .Where(u => u.UserId == Convert.ToInt32(userId)).FirstOrDefaultAsync();
        #endregion

        #region CryptoController
        public async Task<IEnumerable<Crypto>> GetAllCryptosAsync()
            => await _context.Cryptos.ToListAsync();
        public async Task<Crypto> GetCryptoByIdAsync(int id)
            => await _context.Cryptos.FindAsync(id);
        public async Task<Crypto> GetCryptoByCoupleNameAsync(string name)
            => await _context.Cryptos.Where(c => c.CoupleName == name).FirstOrDefaultAsync();
        public void AddCrypto(Crypto crypto)
            => _context.Cryptos.Add(crypto);
        public void DeleteCrypto(Crypto crypto)
            => _context.Cryptos.Remove(crypto);
        #endregion

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
