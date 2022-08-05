using CryptoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoAPI.Controllers
{
    
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ThemesController : Controller
    {
        private readonly CryptoDBContext _context;

        public ThemesController(CryptoDBContext context)
        {
            _context = context;
        }

        // GET: api/Themes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Theme>>> GetThemes()
        {
            return await _context.Themes.ToListAsync();
        }

        // GET: api/Themes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Theme>> GetThemeByID(short id)
        {
            var theme = await _context.Themes.FindAsync(id);

            if (theme == null)
            {
                return NotFound();
            }

            return theme;
        }

        // GET: Themes/Details/5
        [HttpGet("GetThemeDetails/{id}")]
        public async Task<ActionResult<Theme>> GetThemeDetails(short id)
        {
            if (_context.Themes == null)
            {
                return NotFound();
            }

            var theme = await _context.Themes.Include(t => t.Users)
                                            .Where(t => t.ThemeId == id)
                                            .FirstOrDefaultAsync();
            if (theme == null)
            {
                return NotFound();
            }

            return theme;
        }

        // POST: api/Themes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Theme>> PostTheme([FromBody] Theme theme)
        {
            _context.Themes.Add(theme);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostTheme", new { id = theme.ThemeId }, theme);
        }

        //PUT: api/Themes/5     
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTheme(short id, [FromBody] Theme theme)
        {
            if (id != theme.ThemeId)
            {
                return BadRequest();
            }

            _context.Entry(theme).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ThemeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool ThemeExists(short id)
        {
            return _context.Themes.Any(t => t.ThemeId == id);
        }

        // DELETE: api/Themes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTheme(short id)
        {
            var theme = await _context.Themes.FindAsync(id);
            if (theme == null)
            {
                return NotFound();
            }

            _context.Themes.Remove(theme);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}