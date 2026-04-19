using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using VitrueWebAPI.Models;
using VitrueWebAPI.Services;

namespace VitrueWebAPI.Tests.Services
{
    [TestClass]
    public class InMemorySuggestionStoreTests
    {
        private InMemorySuggestionStore _store = null!;

        private static readonly Guid ExistingId = Guid.Parse("550e8400-e29b-41d4-a716-446655440001");
        private static readonly Guid NonExistentId = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            _store = new InMemorySuggestionStore();
        }

        [TestMethod]
        public async Task GetAllAsync_ReturnsAllSuggestions()
        {
            var result = await _store.GetAllAsync();

            result.ShouldNotBeEmpty();
            result.Count.ShouldBe(13);
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingId_ReturnsSuggestion()
        {
            var result = await _store.GetByIdAsync(ExistingId);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(ExistingId);
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistentId_ReturnsNull()
        {
            var result = await _store.GetByIdAsync(NonExistentId);
                
            result.ShouldBeNull();
        }

        [TestMethod]
        public async Task GetByEmployeeIdAsync_ExistingEmployeeId_ReturnsSuggestions()
        {
            var employeeId = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479");

            var result = await _store.GetByEmployeeIdAsync(employeeId);

            result.ShouldNotBeEmpty();
            result.ShouldAllBe(s => s.EmployeeId == employeeId);
        }

        [TestMethod]
        public async Task GetByEmployeeIdAsync_NonExistentEmployeeId_ReturnsEmptyList()
        {
            var result = await _store.GetByEmployeeIdAsync(Guid.NewGuid());

            result.ShouldBeEmpty();
        }

        [TestMethod]
        public async Task AddAsync_NewSuggestion_CanBeRetrieved()
        {
            var suggestion = NewSuggestion();

            await _store.AddAsync(suggestion);
            var result = await _store.GetByIdAsync(suggestion.Id);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(suggestion.Id);
        }

        [TestMethod]
        public async Task UpdateAsync_ExistingSuggestion_UpdatesCorrectly()
        {
            var existing = await _store.GetByIdAsync(ExistingId);
            var updated = existing! with { Status = SuggestionStatus.Completed };

            await _store.UpdateAsync(updated);
            var result = await _store.GetByIdAsync(ExistingId);

            result!.Status.ShouldBe(SuggestionStatus.Completed);
        }

        [TestMethod]
        public async Task UpdateAsync_NonExistentSuggestion_ThrowsKeyNotFoundException()
        {
            var suggestion = NewSuggestion();

            await Should.ThrowAsync<KeyNotFoundException>(() => _store.UpdateAsync(suggestion));
        }

        [TestMethod]
        public async Task DeleteAsync_ExistingSuggestion_CanNoLongerBeRetrieved()
        {
            await _store.DeleteAsync(ExistingId);
            var result = await _store.GetByIdAsync(ExistingId);

            result.ShouldBeNull();
        }

        private static Suggestion NewSuggestion() => new()
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            Type = SuggestionType.Exercise,
            Description = "Test suggestion",
            Status = SuggestionStatus.Pending,
            Priority = SuggestionPriority.Low,
            Source = SuggestionSource.Admin,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow,
        };
    }
}
