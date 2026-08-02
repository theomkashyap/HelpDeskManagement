using Microsoft.AspNetCore.Mvc;
using HelpDesk.Mvc.Models;
using HelpDesk.Mvc.Services;

namespace HelpDesk.Mvc.Controllers
{
    public class TicketsController : Controller
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        // GET: Tickets/Dashboard
        public async Task<IActionResult> Dashboard()
        {
            var allTickets = await _ticketService.GetAllTicketsAsync();

            var viewModel = new DashboardViewModel
            {
                TotalTickets = allTickets.Count,
                OpenTickets = allTickets.Count(t => t.Status == "Open"),
                ClosedTickets = allTickets.Count(t => t.Status == "Closed")
            };

            return View(viewModel);
        }

        // GET: Tickets
        public async Task<IActionResult> Index()
        {
            var tickets = await _ticketService.GetAllTicketsAsync();
            return View(tickets);
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tickets/Create
        [HttpPost]
        public async Task<IActionResult> Create(Ticket ticket)
        {
            ticket.Status = "Open"; // hardcoded as per requirement

            if (!ModelState.IsValid)
                return View(ticket);

            await _ticketService.CreateTicketAsync(ticket);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(int id, Ticket ticket)
        {
            if (!ModelState.IsValid)
                return View(ticket);

            await _ticketService.UpdateTicketAsync(id, ticket);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var ticket = await _ticketService.GetTicketByIdAsync(id);
            if (ticket == null)
                return NotFound();

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _ticketService.DeleteTicketAsync(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Tickets/FilterByStatus
        public IActionResult FilterByStatus()
        {
            return View(new List<Ticket>());
        }

        // POST: Tickets/FilterByStatus
        [HttpPost]
        public async Task<IActionResult> FilterByStatus(string status)
        {
            var tickets = await _ticketService.GetTicketsByStatusAsync(status);
            ViewBag.SelectedStatus = status;
            return View(tickets);
        }
    }
}