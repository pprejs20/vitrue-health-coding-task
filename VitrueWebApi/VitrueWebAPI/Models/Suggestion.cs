namespace VitrueWebAPI.Models
{
    public enum SuggestionType { Equipment, Exercise, Behavioural, Lifestyle }
    public enum SuggestionStatus { Pending, InProgress, Completed, Overdue }
    public enum SuggestionPriority { Low, Medium, High }
    public enum SuggestionSource { Vida, Admin }

    public class Suggestion
    {
        public required Guid Id { get; init; }
        public required Guid EmployeeId { get; init; }
        public required SuggestionType Type { get; set; }
        public required string Description { get; set; }
        public required SuggestionStatus Status { get; set; }
        public required SuggestionPriority Priority { get; set; }
        public required SuggestionSource Source { get; init; }
        public required DateTime DateCreated { get; init; }
        public required DateTime DateUpdated { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
