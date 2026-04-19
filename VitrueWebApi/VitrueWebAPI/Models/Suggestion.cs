namespace VitrueWebAPI.Models
{
    public enum SuggestionType { Equipment, Exercise, Behavioural, Lifestyle }
    public enum SuggestionStatus { Pending, InProgress, Completed, Overdue }
    public enum SuggestionPriority { Low, Medium, High }
    public enum SuggestionSource { Vida, Admin }

    public record Suggestion
    {
        public required Guid Id { get; init; }
        public required Guid EmployeeId { get; init; }
        public required SuggestionType Type { get; init; }
        public required string Description { get; init; }
        public required SuggestionStatus Status { get; init; }
        public required SuggestionPriority Priority { get; init; }
        public required SuggestionSource Source { get; init; }
        public required DateTime DateCreated { get; init; }
        public required DateTime DateUpdated { get; init; }
        public string Notes { get; init; } = string.Empty;
    }
}
