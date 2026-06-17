using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Global_Logistics_Management_System.Data;
using Global_Logistics_Management_System.Models;

namespace Global_Logistics_Management_System.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContractsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ContractsApi?status=Approved
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts([FromQuery] string? status)
        {
            var query = _context.Contracts.Include(c => c.Client).AsQueryable();

            // Convert string query parameters safely to your exact enum type
            if (!string.IsNullOrEmpty(status) && Enum.TryParse<ContractStatus>(status, true, out var parsedStatus))
            {
                query = query.Where(c => c.Status == parsedStatus);
            }

            return await query.ToListAsync();
        }

        // POST: api/ContractsApi
        [HttpPost]
        public async Task<ActionResult<Contract>> CreateContract(Contract contract)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetContracts), new { id = contract.ContractId }, contract);
        }

        // PATCH: api/ContractsApi/{id}/status
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string newStatus)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound($"Contract with ID {id} not found.");
            }

            // Convert incoming text body payload safely into your matching model enum
            if (!Enum.TryParse<ContractStatus>(newStatus, true, out var parsedStatus))
            {
                return BadRequest($"Invalid status value provided. Allowed choices include: {string.Join(", ", Enum.GetNames(typeof(ContractStatus)))}");
            }

            contract.Status = parsedStatus;

            _context.Entry(contract).Property(x => x.Status).IsModified = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}