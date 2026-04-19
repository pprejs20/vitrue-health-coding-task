using VitrueWebAPI.Models;

namespace VitrueWebAPI.Interfaces
{
    public interface ISuggestionStore
    {
        Task<IReadOnlyList<Suggestion>> GetAllAsync();
        Task<IReadOnlyList<Suggestion>> GetByEmployeeIdAsync(Guid employeeId);
        Task<Suggestion?> GetByIdAsync(Guid id);
        Task AddAsync(Suggestion suggestion);
        Task UpdateAsync(Suggestion suggestion);
        Task DeleteAsync(Guid id);
    }
}
