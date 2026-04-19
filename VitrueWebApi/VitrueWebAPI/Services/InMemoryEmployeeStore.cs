using System.Text.Json;
using System.Text.Json.Serialization;
using VitrueWebAPI.Interfaces;
using VitrueWebAPI.Models;

namespace VitrueWebAPI.Services
{
    public class InMemoryEmployeeStore : IEmployeeStore
    {
        private readonly Dictionary<Guid, Employee> _employees;

        public InMemoryEmployeeStore()
        {
            var json = File.ReadAllText("TestData/employee_data.json");
            var employees = JsonSerializer.Deserialize<List<Employee>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                Converters = { new JsonStringEnumConverter(JsonNamingPolicy.SnakeCaseLower) }
            });

            _employees = employees?.ToDictionary(e => e.Id)
                ?? throw new InvalidOperationException("Employee data could not be loaded.");
        }

        public Task<Employee?> GetByIdAsync(Guid id) =>
            Task.FromResult(_employees.TryGetValue(id, out var employee) ? employee : null);
    }
}
