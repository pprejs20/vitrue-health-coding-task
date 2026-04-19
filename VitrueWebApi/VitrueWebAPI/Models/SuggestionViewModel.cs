namespace VitrueWebAPI.Models
{
    public record SuggestionViewModel
    {
        public required Guid Id { get; init; }
        public required string EmployeeName { get; init; }
        public required SuggestionType Type { get; init; }
        public required string Description { get; init; }
        public required SuggestionStatus Status { get; init; }
        public required SuggestionPriority Priority { get; init; }
        public required SuggestionSource Source { get; init; }
        public required DateTime DateCreated { get; init; }
        public required string Notes { get; init; }
    }
}
