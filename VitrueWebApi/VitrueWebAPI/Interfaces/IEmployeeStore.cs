using VitrueWebAPI.Models;

namespace VitrueWebAPI.Interfaces
{
    public interface IEmployeeStore
    {
        Task<Employee?> GetByIdAsync(Guid id);
    }
}
