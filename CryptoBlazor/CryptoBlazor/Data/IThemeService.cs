namespace CryptoBlazor.Data
{
    public interface IThemeService
    {
        public Task<Theme> GetTheme(short id);
    }
}
