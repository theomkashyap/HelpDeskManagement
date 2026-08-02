using Microsoft.AspNetCore.Mvc;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;

namespace HelpDesk.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _repository;

        public TicketController(ITicketRepository repository)
        {
            _repository = repository;
        }

        // GET: api/Ticket/All
        [HttpGet("All")]
        public async Task<IActionResult> GetAllTickets()
        {
            try
            {
                var tickets = await _repository.GetAllTicketsAsync();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving tickets: {ex.Message}");
            }
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            try
            {
                var ticket = await _repository.GetTicketByIdAsync(id);
                if (ticket == null)
                    return NotFound($"Ticket with id {id} not found.");

                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving the ticket: {ex.Message}");
            }
        }

        // POST: api/Ticket
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
        {
            try
            {
                if (ticket == null)
                    return BadRequest("Ticket data is required.");

                ticket.CreatedDate = DateTime.Now;
                var newId = await _repository.CreateTicketAsync(ticket);
                ticket.Id = newId;
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while creating the ticket: {ex.Message}");
            }
        }

        // PUT: api/Ticket/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] Ticket ticket)
        {
            try
            {
                var existing = await _repository.GetTicketByIdAsync(id);
                if (existing == null)
                    return NotFound($"Ticket with id {id} not found.");

                ticket.Id = id;
                await _repository.UpdateTicketAsync(ticket);
                return Ok(ticket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the ticket: {ex.Message}");
            }
        }

        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            try
            {
                var existing = await _repository.GetTicketByIdAsync(id);
                if (existing == null)
                    return NotFound($"Ticket with id {id} not found.");

                await _repository.DeleteTicketAsync(id);
                return Ok($"Ticket with id {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while deleting the ticket: {ex.Message}");
            }
        }

        // GET: api/Ticket/Status/Open
        [HttpGet("Status/{status}")]
        public async Task<IActionResult> GetTicketsByStatus(string status)
        {
            try
            {
                var tickets = await _repository.GetTicketsByStatusAsync(status);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while retrieving tickets by status: {ex.Message}");
            }
        }
    }
}