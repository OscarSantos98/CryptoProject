using CryptoAPI.Models;
using CryptoAPI.Repository_pattern;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CryptoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoControllerWithRepository : Controller
    {

        private readonly ICryptoRepository _repository;
        public CryptoControllerWithRepository(ICryptoRepository repository)
            => _repository = repository;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Crypto>>> GetAllCryptos()
        {
            var cryptos = await _repository.GetAllCryptosAsync();
            return Ok(cryptos);
        }

        // GET: api/Cryptos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Crypto>> GetCryptoByID(int id)
        {
            var crypto = await _repository.GetCryptoByIdAsync(id);

            if (crypto == null)
            {
                return NotFound();
            }

            return Ok(crypto);
        }

        [HttpGet("GetCryptoFromName")]
        public async Task<ActionResult<Crypto>> GetCryptoFromName([FromBody] string name)
        {
            var crypto = await _repository.GetCryptoByCoupleNameAsync(name);

            if (crypto == null)
            {
                return NotFound();
            }

            return Ok(crypto);
        }

        // POST: api/Cryptos
        [HttpPost]
        public async Task<ActionResult<Crypto>> PostCrypto(Crypto crypto)
        {
            _repository.AddCrypto(crypto);
            await _repository.SaveChangesAsync();

            return CreatedAtAction("PostCrypto", new { id = crypto.CryptoId }, crypto);
        }

        // DELETE: api/Cryptos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCrypto(int id)
        {
            var crypto = await _repository.GetCryptoByIdAsync(id);
            if (crypto == null)
            {
                return NotFound();
            }

            _repository.DeleteCrypto(crypto);
            await _repository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCryptoValue(string name, double value)
        {
            var crypto = await _repository.GetCryptoByCoupleNameAsync(name);
            if (crypto is null)
            {
                return NotFound();
            }

            crypto.Value = value;
            await _repository.SaveChangesAsync();

            return Ok();
        }
    }
}
