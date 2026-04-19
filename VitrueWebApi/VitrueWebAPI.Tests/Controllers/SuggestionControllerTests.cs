using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Shouldly;
using VitrueWebAPI.Controllers;
using VitrueWebAPI.Interfaces;
using VitrueWebAPI.Models;

namespace VitrueWebAPI.Tests.Controllers
{
    [TestClass]
    public class SuggestionControllerTests
    {
        private ISuggestionStore _suggestionStore = null!;
        private IEmployeeStore _employeeStore = null!;
        private SuggestionController _controller = null!;

        [TestInitialize]
        public void Setup()
        {
            _suggestionStore = Substitute.For<ISuggestionStore>();
            _employeeStore = Substitute.For<IEmployeeStore>();
            _controller = new SuggestionController(_suggestionStore, _employeeStore);
        }

        // GetAll

        [TestMethod]
        public async Task GetAll_ReturnsOk_WithSuggestions()
        {
            var suggestions = new List<Suggestion> { NewSuggestion() };
            _suggestionStore.GetAllAsync().Returns(suggestions.AsReadOnly());

            var result = await _controller.GetAll();

            var ok = result.ShouldBeOfType<OkObjectResult>();
            ok.Value.ShouldBe(suggestions.AsReadOnly());
        }

        // GetView

        [TestMethod]
        public async Task GetView_ResolvesEmployeeName()
        {
            var employeeId = Guid.NewGuid();
            var suggestion = NewSuggestion(employeeId: employeeId);
            var employee = NewEmployee(id: employeeId, name: "Jane Doe");

            _suggestionStore.GetAllAsync().Returns(new List<Suggestion> { suggestion }.AsReadOnly());
            _employeeStore.GetByIdAsync(employeeId).Returns(employee);

            var result = await _controller.GetView();

            var ok = result.ShouldBeOfType<OkObjectResult>();
            var viewModels = ok.Value.ShouldBeOfType<SuggestionViewModel[]>();
            viewModels[0].EmployeeName.ShouldBe("Jane Doe");
        }

        [TestMethod]
        public async Task GetView_UnknownEmployee_FallsBackToUnknown()
        {
            var suggestion = NewSuggestion();
            _suggestionStore.GetAllAsync().Returns(new List<Suggestion> { suggestion }.AsReadOnly());
            _employeeStore.GetByIdAsync(Arg.Any<Guid>()).Returns((Employee?)null);

            var result = await _controller.GetView();

            var ok = result.ShouldBeOfType<OkObjectResult>();
            var viewModels = ok.Value.ShouldBeOfType<SuggestionViewModel[]>();
            viewModels[0].EmployeeName.ShouldBe("Unknown");
        }

        // GetById

        [TestMethod]
        public async Task GetById_ExistingSuggestion_ReturnsOk()
        {
            var suggestion = NewSuggestion();
            _suggestionStore.GetByIdAsync(suggestion.Id).Returns(suggestion);

            var result = await _controller.GetById(suggestion.Id);

            var ok = result.ShouldBeOfType<OkObjectResult>();
            ok.Value.ShouldBe(suggestion);
        }

        [TestMethod]
        public async Task GetById_NonExistentSuggestion_ReturnsNotFound()
        {
            _suggestionStore.GetByIdAsync(Arg.Any<Guid>()).Returns((Suggestion?)null);

            var result = await _controller.GetById(Guid.NewGuid());

            result.ShouldBeOfType<NotFoundResult>();
        }

        // Add

        [TestMethod]
        public async Task Add_ValidRequest_ReturnsCreated()
        {
            var request = NewCreateRequest();

            var result = await _controller.Add(request);

            var created = result.ShouldBeOfType<CreatedAtActionResult>();
            var suggestion = created.Value.ShouldBeOfType<Suggestion>();
            suggestion.EmployeeId.ShouldBe(request.EmployeeId);
            suggestion.Description.ShouldBe(request.Description);
        }

        [TestMethod]
        public async Task Add_ValidRequest_CallsStoreAdd()
        {
            var request = NewCreateRequest();

            await _controller.Add(request);

            await _suggestionStore.Received(1).AddAsync(Arg.Any<Suggestion>());
        }

        // Update

        [TestMethod]
        public async Task Update_ExistingSuggestion_ReturnsNoContent()
        {
            var existing = NewSuggestion();
            _suggestionStore.GetByIdAsync(existing.Id).Returns(existing);

            var result = await _controller.Update(existing.Id, NewUpdateRequest());

            result.ShouldBeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task Update_ExistingSuggestion_CallsStoreUpdate()
        {
            var existing = NewSuggestion();
            _suggestionStore.GetByIdAsync(existing.Id).Returns(existing);

            await _controller.Update(existing.Id, NewUpdateRequest());

            await _suggestionStore.Received(1).UpdateAsync(Arg.Any<Suggestion>());
        }

        [TestMethod]
        public async Task Update_NonExistentSuggestion_ReturnsNotFound()
        {
            _suggestionStore.GetByIdAsync(Arg.Any<Guid>()).Returns((Suggestion?)null);

            var result = await _controller.Update(Guid.NewGuid(), NewUpdateRequest());

            result.ShouldBeOfType<NotFoundResult>();
        }

        [TestMethod]
        public async Task Update_PreservesImmutableFields()
        {
            var existing = NewSuggestion();
            _suggestionStore.GetByIdAsync(existing.Id).Returns(existing);

            await _controller.Update(existing.Id, NewUpdateRequest());

            await _suggestionStore.Received(1).UpdateAsync(Arg.Is<Suggestion>(s =>
                s.Id == existing.Id &&
                s.EmployeeId == existing.EmployeeId &&
                s.Source == existing.Source &&
                s.DateCreated == existing.DateCreated));
        }

        // Delete

        [TestMethod]
        public async Task Delete_ReturnsNoContent()
        {
            var result = await _controller.Delete(Guid.NewGuid());

            result.ShouldBeOfType<NoContentResult>();
        }

        [TestMethod]
        public async Task Delete_CallsStoreDelete()
        {
            var id = Guid.NewGuid();

            await _controller.Delete(id);

            await _suggestionStore.Received(1).DeleteAsync(id);
        }

        // Helpers

        private static Suggestion NewSuggestion(Guid? employeeId = null) => new()
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId ?? Guid.NewGuid(),
            Type = SuggestionType.Exercise,
            Description = "Test suggestion",
            Status = SuggestionStatus.Pending,
            Priority = SuggestionPriority.Low,
            Source = SuggestionSource.Admin,
            DateCreated = DateTime.UtcNow,
            DateUpdated = DateTime.UtcNow,
        };

        private static Employee NewEmployee(Guid? id = null, string name = "Test Employee") => new()
        {
            Id = id ?? Guid.NewGuid(),
            Name = name,
            Department = "Engineering",
            RiskLevel = RiskLevel.Low,
        };

        private static CreateSuggestionRequest NewCreateRequest() => new()
        {
            EmployeeId = Guid.NewGuid(),
            Type = SuggestionType.Exercise,
            Description = "Test suggestion",
            Status = SuggestionStatus.Pending,
            Priority = SuggestionPriority.Low,
            Source = SuggestionSource.Admin,
        };

        private static UpdateSuggestionRequest NewUpdateRequest() => new()
        {
            Type = SuggestionType.Lifestyle,
            Description = "Updated description",
            Status = SuggestionStatus.Completed,
            Priority = SuggestionPriority.High,
        };
    }
}
