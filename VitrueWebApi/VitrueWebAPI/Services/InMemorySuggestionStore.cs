using System.Text.Json;
using System.Text.Json.Serialization;
using VitrueWebAPI.Interfaces;
using VitrueWebAPI.Models;

namespace VitrueWebAPI.Services
{
    public class InMemorySuggestionStore : ISuggestionStore
    {
        private readonly Dictionary<Guid, Suggestion> _suggestions = new();

        public InMemorySuggestionStore()
        {
            var json = File.ReadAllText("TestData/suggestions_data.json");
            var suggestions = JsonSerializer.Deserialize<List<Suggestion>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
            });

            if (suggestions is not null)
                _suggestions = suggestions.ToDictionary(s => s.Id);
        }

        public Task<IReadOnlyList<Suggestion>> GetAllAsync() =>
            Task.FromResult(_suggestions.Values.ToList().AsReadOnly() as IReadOnlyList<Suggestion>);

        public Task<IReadOnlyList<Suggestion>> GetByEmployeeIdAsync(Guid employeeId) =>
            Task.FromResult(_suggestions.Values.Where(s => s.EmployeeId == employeeId).ToList().AsReadOnly() as IReadOnlyList<Suggestion>);

        public Task<Suggestion?> GetByIdAsync(Guid id) =>
            Task.FromResult(_suggestions.TryGetValue(id, out var suggestion) ? suggestion : null);

        public Task AddAsync(Suggestion suggestion)
        {
            _suggestions.Add(suggestion.Id, suggestion);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Suggestion suggestion)
        {
            if (!_suggestions.ContainsKey(suggestion.Id))
                throw new KeyNotFoundException($"Suggestion with id {suggestion.Id} not found.");

            _suggestions[suggestion.Id] = suggestion;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _suggestions.Remove(id);
            return Task.CompletedTask;
        }
    }
}
