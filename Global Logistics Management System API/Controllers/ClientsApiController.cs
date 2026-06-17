using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Global_Logistics_Management_System.Data;
using Global_Logistics_Management_System.Models;

namespace Global_Logistics_Management_System.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ClientsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ClientsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // POST: api/ClientsApi
        [HttpPost]
        public async Task<ActionResult<Client>> CreateClient(Client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClients), new { id = client.ClientId }, client);
        }
    }
}