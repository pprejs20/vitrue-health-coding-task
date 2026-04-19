using Microsoft.AspNetCore.Mvc;
using VitrueWebAPI.Interfaces;
using VitrueWebAPI.Models;

namespace VitrueWebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SuggestionController : ControllerBase
    {
        private readonly ISuggestionStore _suggestionStore;
        private readonly IEmployeeStore _employeeStore;

        public SuggestionController(ISuggestionStore suggestionStore, IEmployeeStore employeeStore)
        {
            _suggestionStore = suggestionStore;
            _employeeStore = employeeStore;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _suggestionStore.GetAllAsync());

        [HttpGet("view")]
        public async Task<IActionResult> GetView()
        {
            var suggestions = await _suggestionStore.GetAllAsync();

            var viewModels = await Task.WhenAll(suggestions.Select(async s =>
            {
                var employee = await _employeeStore.GetByIdAsync(s.EmployeeId);
                return new SuggestionViewModel
                {
                    Id = s.Id,
                    EmployeeName = employee?.Name ?? "Unknown",
                    Type = s.Type,
                    Description = s.Description,
                    Status = s.Status,
                    Priority = s.Priority,
                    Source = s.Source,
                    DateCreated = s.DateCreated,
                    Notes = s.Notes,
                };
            }));

            return Ok(viewModels);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var suggestion = await _suggestionStore.GetByIdAsync(id);
            return suggestion is null ? NotFound() : Ok(suggestion);
        }

        [HttpPost]
        public async Task<IActionResult> Add(CreateSuggestionRequest request)
        {
            var suggestion = new Suggestion
            {
                Id = Guid.NewGuid(),
                EmployeeId = request.EmployeeId,
                Type = request.Type,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                Source = request.Source,
                DateCreated = DateTime.UtcNow,
                DateUpdated = DateTime.UtcNow,
                Notes = request.Notes,
            };

            await _suggestionStore.AddAsync(suggestion);
            return CreatedAtAction(nameof(GetById), new { id = suggestion.Id }, suggestion);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UpdateSuggestionRequest request)
        {
            var existing = await _suggestionStore.GetByIdAsync(id);
            if (existing is null)
                return NotFound();

            var updated = new Suggestion
            {
                Id = existing.Id,
                EmployeeId = existing.EmployeeId,
                Source = existing.Source,
                DateCreated = existing.DateCreated,
                Type = request.Type,
                Description = request.Description,
                Status = request.Status,
                Priority = request.Priority,
                Notes = request.Notes,
                DateUpdated = DateTime.UtcNow,
            };

            await _suggestionStore.UpdateAsync(updated);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _suggestionStore.DeleteAsync(id);
            return NoContent();
        }
    }
}
