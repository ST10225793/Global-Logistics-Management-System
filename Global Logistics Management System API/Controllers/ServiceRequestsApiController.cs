using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Global_Logistics_Management_System.Data;
using Global_Logistics_Management_System.Models;

namespace Global_Logistics_Management_System_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceRequestsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
        {
            return await _context.ServiceRequests.Include(s => s.Contract).ToListAsync();
        }

        // GET: api/ServiceRequestsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceRequest>> GetServiceRequest(int id)
        {
            var serviceRequest = await _context.ServiceRequests.Include(s => s.Contract).FirstOrDefaultAsync(s => s.RequestId == id);
            if (serviceRequest == null) return NotFound();
            return serviceRequest;
        }

        // POST: api/ServiceRequestsApi
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> PostServiceRequest(ServiceRequest serviceRequest)
        {
            _context.ServiceRequests.Add(serviceRequest);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetServiceRequest), new { id = serviceRequest.RequestId }, serviceRequest);
        }

        // PUT: api/ServiceRequestsApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutServiceRequest(int id, ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.RequestId) return BadRequest();

            _context.Entry(serviceRequest).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.ServiceRequests.Any(e => e.RequestId == id)) return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/ServiceRequestsApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteServiceRequest(int id)
        {
            var serviceRequest = await _context.ServiceRequests.FindAsync(id);
            if (serviceRequest == null) return NotFound();

            _context.ServiceRequests.Remove(serviceRequest);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}