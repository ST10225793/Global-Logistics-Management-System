using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Global_Logistics_Management_System.Data;
using Global_Logistics_Management_System.Models;

namespace Global_Logistics_Management_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContractsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ContractsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts()
        {
            return await _context.Contracts.Include(c => c.Client).ToListAsync();
        }

        // GET: api/ContractsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contract>> GetContract(int id)
        {
            var contract = await _context.Contracts.Include(c => c.Client).FirstOrDefaultAsync(c => c.ContractId == id);
            if (contract == null) return NotFound();
            return contract;
        }

        // POST: api/ContractsApi
        [HttpPost]
        public async Task<ActionResult<Contract>> PostContract(Contract contract)
        {
            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContract), new { id = contract.ContractId }, contract);
        }

        // PUT: api/ContractsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContract(int id, Contract contract)
        {
            if (id != contract.ContractId) return BadRequest();

            _context.Entry(contract).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Contracts.Any(e => e.ContractId == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // PATCH: api/ContractsApi/5/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> PatchStatus(int id, [FromBody] string newStatus)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            // Explicitly map string values safely to the application's underlying enum structure
            if (Enum.TryParse<Global_Logistics_Management_System.Models.ContractStatus>(newStatus, true, out var parsedStatus))
            {
                contract.Status = parsedStatus;
                await _context.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest("Invalid status string value provided.");
        }

        // DELETE: api/ContractsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContract(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return NotFound();

            _context.Contracts.Remove(contract);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}