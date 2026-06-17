using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Global_Logistics_Management_System.Data;
using Global_Logistics_Management_System.Models;

namespace Global_Logistics_Management_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var clients = await _context.Clients.ToListAsync();
            return Ok(clients); // Explicitly returns HTTP 200 OK
        }

        // GET: api/ClientsApi/5
        [HttpGet("{id}", Name = "GetClientById")] // Named attribute ensures exact route resolution
        public async Task<ActionResult<Client>> GetClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound(); // Explicitly returns HTTP 404 Not Found
            }

            return Ok(client); // Explicitly returns HTTP 200 OK
        }

        // POST: api/ClientsApi
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            if (client == null)
            {
                return BadRequest(); // Explicitly returns HTTP 400 Bad Request
            }

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            // Explicitly uses the named route segment to eliminate cross-project 404 context matching errors
            return CreatedAtRoute("GetClientById", new { id = client.ClientId }, client); // Returns HTTP 201 Created
        }

        // PUT: api/ClientsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(int id, Client client)
        {
            if (client == null || id != client.ClientId)
            {
                return BadRequest(); // Explicitly returns HTTP 400 Bad Request
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Clients.Any(e => e.ClientId == id))
                {
                    return NotFound(); // Explicitly returns HTTP 404 Not Found
                }
                throw;
            }

            return NoContent(); // Explicitly returns HTTP 204 No Content
        }

        // DELETE: api/ClientsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound(); // Explicitly returns HTTP 404 Not Found
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent(); // Explicitly returns HTTP 204 No Content
        }
    }
}