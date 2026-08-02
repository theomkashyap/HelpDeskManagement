using HelpDesk.Mvc.Models;
using System.Text;
using System.Text.Json;

namespace HelpDesk.Mvc.Services
{
    public class TicketService : ITicketService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public TicketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            var response = await _httpClient.GetAsync("api/Ticket/All");
            if (!response.IsSuccessStatusCode)
                return new List<Ticket>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Ticket>>(json, _jsonOptions) ?? new List<Ticket>();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/Ticket/{id}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Ticket>(json, _jsonOptions);
        }

        public async Task<bool> CreateTicketAsync(Ticket ticket)
        {
            var json = JsonSerializer.Serialize(ticket);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Ticket", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTicketAsync(int id, Ticket ticket)
        {
            var json = JsonSerializer.Serialize(ticket);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/Ticket/{id}", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteTicketAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Ticket/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Ticket>> GetTicketsByStatusAsync(string status)
        {
            var response = await _httpClient.GetAsync($"api/Ticket/Status/{status}");
            if (!response.IsSuccessStatusCode)
                return new List<Ticket>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<Ticket>>(json, _jsonOptions) ?? new List<Ticket>();
        }
    }
}