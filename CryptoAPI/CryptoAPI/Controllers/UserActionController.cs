using CryptoAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoAPI.Controllers
{
    
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserActionsController : Controller
    {
        private readonly CryptoDBContext _context;

        public UserActionsController(CryptoDBContext context)
        {
            _context = context;
        }

        // GET: api/UserActions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAction>>> GetUserActions()
        {
            return await _context.UserActions.ToListAsync();
        }

        // GET: api/UserActions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserAction>> GetUserActionByID(short id)
        {
            var useraction = await _context.UserActions.FindAsync(id);

            if (useraction == null)
            {
                return NotFound();
            }

            return useraction;
        }

        // GET: UserActions/Details/5
        [HttpGet("GetUserActionDetails/{id}")]
        public async Task<ActionResult<UserAction>> GetUserActionDetails(short id)
        {
            if (_context.UserActions == null)
            {
                return NotFound();
            }

            var useraction = await _context.UserActions.Include(ua => ua.Role)
                                            .Where(ua => ua.UserActionsId == id)
                                            .FirstOrDefaultAsync();
            if (useraction == null)
            {
                return NotFound();
            }

            return useraction;
        }

        // POST: api/UserActions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserAction>> PostUserAction([FromBody] UserAction useraction)
        {
            _context.UserActions.Add(useraction);
            await _context.SaveChangesAsync();

            return CreatedAtAction("PostUserAction", new { id = useraction.UserActionsId }, useraction);
        }

        //PUT: api/UserActions/5     
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserAction(short id, [FromBody] UserAction useraction)
        {
            if (id != useraction.UserActionsId)
            {
                return BadRequest();
            }

            _context.Entry(useraction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserActionExists(id))
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

        private bool UserActionExists(short id)
        {
            return _context.UserActions.Any(t => t.UserActionsId == id);
        }

        // DELETE: api/UserActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAction(short id)
        {
            var useraction = await _context.UserActions.FindAsync(id);
            if (useraction == null)
            {
                return NotFound();
            }

            _context.UserActions.Remove(useraction);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}