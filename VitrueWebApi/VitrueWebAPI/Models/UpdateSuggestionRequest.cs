namespace VitrueWebAPI.Models
{
    public class UpdateSuggestionRequest
    {
        public required SuggestionType Type { get; init; }
        public required string Description { get; init; }
        public required SuggestionStatus Status { get; init; }
        public required SuggestionPriority Priority { get; init; }
        public string Notes { get; init; } = string.Empty;
    }
}
