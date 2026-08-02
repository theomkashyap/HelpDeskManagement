using Microsoft.AspNetCore.Mvc;
using Moq;
using HelpDesk.Api.Controllers;
using HelpDesk.Api.Models;
using HelpDesk.Api.Repositories;
using Xunit;

namespace HelpDesk.Tests
{
    public class TicketControllerTests
    {
        private readonly Mock<ITicketRepository> _mockRepo;
        private readonly TicketController _controller;

        public TicketControllerTests()
        {
            _mockRepo = new Mock<ITicketRepository>();
            _controller = new TicketController(_mockRepo.Object);
        }

        [Fact]
        public async Task GetAllTickets_ReturnsOkResult_WhenTicketsExist()
        {
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "T1", Description = "D", Priority = "Low", Status = "Open", RaisedBy = "Om", CreatedDate = DateTime.Now }
            };
            _mockRepo.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(tickets);

            var result = await _controller.GetAllTickets();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Single(returnedTickets);
        }

        [Fact]
        public async Task GetTicketById_ReturnsOkResult_WhenTicketExists()
        {
            var ticket = new Ticket { Id = 1, Title = "T1", Description = "D", Priority = "Low", Status = "Open", RaisedBy = "Om", CreatedDate = DateTime.Now };
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(1)).ReturnsAsync(ticket);

            var result = await _controller.GetTicketById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTicket = Assert.IsType<Ticket>(okResult.Value);
            Assert.Equal("T1", returnedTicket.Title);
        }

        [Fact]
        public async Task GetTicketById_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(It.IsAny<int>())).ReturnsAsync((Ticket)null);

            var result = await _controller.GetTicketById(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task CreateTicket_ReturnsOkResult_WhenTicketIsCreatedSuccessfully()
        {
            var ticket = new Ticket { Title = "New", Description = "D", Priority = "High", Status = "Open", RaisedBy = "Om" };
            _mockRepo.Setup(repo => repo.CreateTicketAsync(It.IsAny<Ticket>())).ReturnsAsync(1);

            var result = await _controller.CreateTicket(ticket);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task CreateTicket_ReturnsBadRequest_WhenTicketIsNull()
        {
            var result = await _controller.CreateTicket(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task GetTicketsByStatus_ReturnsOkResult_WhenMatchingTicketsExist()
        {
            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, Title = "T1", Status = "Open", Description = "D", Priority = "Low", RaisedBy = "Om" }
            };
            _mockRepo.Setup(repo => repo.GetTicketsByStatusAsync("Open")).ReturnsAsync(tickets);

            var result = await _controller.GetTicketsByStatus("Open");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Single(returnedTickets);
        }

        [Fact]
        public async Task UpdateTicket_ReturnsOkResult_WhenUpdateIsSuccessful()
        {
            var existing = new Ticket { Id = 1, Title = "Old", Description = "D", Priority = "Low", Status = "Open", RaisedBy = "Om" };
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(1)).ReturnsAsync(existing);
            _mockRepo.Setup(repo => repo.UpdateTicketAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

            var updated = new Ticket { Title = "New", Description = "D", Priority = "High", Status = "Closed", RaisedBy = "Om" };
            var result = await _controller.UpdateTicket(1, updated);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task UpdateTicket_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(It.IsAny<int>())).ReturnsAsync((Ticket)null);

            var result = await _controller.UpdateTicket(1, new Ticket());

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTicket_ReturnsOkResult_WhenTicketIsDeletedSuccessfully()
        {
            var ticket = new Ticket { Id = 1, Title = "T1", Description = "D", Priority = "Low", Status = "Open", RaisedBy = "Om" };
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(1)).ReturnsAsync(ticket);
            _mockRepo.Setup(repo => repo.DeleteTicketAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.DeleteTicket(1);

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task DeleteTicket_ReturnsNotFound_WhenTicketDoesNotExist()
        {
            _mockRepo.Setup(repo => repo.GetTicketByIdAsync(It.IsAny<int>())).ReturnsAsync((Ticket)null);

            var result = await _controller.DeleteTicket(99);

            Assert.IsType<NotFoundObjectResult>(result);
        }

        [Fact]
        public async Task GetAllTickets_ReturnsEmptyList_WhenNoTicketsExist()
        {
            _mockRepo.Setup(repo => repo.GetAllTicketsAsync()).ReturnsAsync(new List<Ticket>());

            var result = await _controller.GetAllTickets();

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Empty(returnedTickets);
        }

        [Fact]
        public async Task GetTicketsByStatus_ReturnsEmptyList_WhenNoMatchingTicketsExist()
        {
            _mockRepo.Setup(repo => repo.GetTicketsByStatusAsync("Closed")).ReturnsAsync(new List<Ticket>());

            var result = await _controller.GetTicketsByStatus("Closed");

            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedTickets = Assert.IsAssignableFrom<List<Ticket>>(okResult.Value);
            Assert.Empty(returnedTickets);
        }
    }
}