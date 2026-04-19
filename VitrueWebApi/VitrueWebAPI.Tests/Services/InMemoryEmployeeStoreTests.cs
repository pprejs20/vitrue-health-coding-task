using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using VitrueWebAPI.Services;

namespace VitrueWebAPI.Tests.Services
{
    [TestClass]
    public class InMemoryEmployeeStoreTests
    {
        private InMemoryEmployeeStore _store = null!;

        private static readonly Guid ExistingId = Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479");
        private static readonly Guid NonExistentId = Guid.NewGuid();

        [TestInitialize]
        public void Setup()
        {
            _store = new InMemoryEmployeeStore();
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingId_ReturnsEmployee()
        {
            var result = await _store.GetByIdAsync(ExistingId);

            result.ShouldNotBeNull();
            result.Id.ShouldBe(ExistingId);
        }

        [TestMethod]
        public async Task GetByIdAsync_ExistingId_ReturnsCorrectEmployee()
        {
            var result = await _store.GetByIdAsync(ExistingId);

            result.ShouldNotBeNull();
            result.Name.ShouldBe("Priya Patel");
        }

        [TestMethod]
        public async Task GetByIdAsync_NonExistentId_ReturnsNull()
        {
            var result = await _store.GetByIdAsync(NonExistentId);

            result.ShouldBeNull();
        }
    }
}
