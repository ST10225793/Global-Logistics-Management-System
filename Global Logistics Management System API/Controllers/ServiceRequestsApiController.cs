using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Global_Logistics_Management_System.Data;
using Global_Logistics_Management_System.Models;

namespace Global_Logistics_Management_System.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
            return await _context.ServiceRequests.ToListAsync();
        }

        // POST: api/ServiceRequestsApi
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> CreateServiceRequest(ServiceRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetServiceRequests), new { id = request.RequestId }, request);
        }
    }
}