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

        public SuggestionController(ISuggestionStore suggestionStore)
        {
            _suggestionStore = suggestionStore;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _suggestionStore.GetAllAsync());

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
