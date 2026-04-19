namespace VitrueWebAPI.Models
{
    public class CreateSuggestionRequest
    {
        public required Guid EmployeeId { get; init; }
        public required SuggestionType Type { get; init; }
        public required string Description { get; init; }
        public required SuggestionStatus Status { get; init; }
        public required SuggestionPriority Priority { get; init; }
        public required SuggestionSource Source { get; init; }
        public string Notes { get; init; } = string.Empty;
    }
}
