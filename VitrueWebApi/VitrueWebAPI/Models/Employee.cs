namespace VitrueWebAPI.Models
{
    public enum RiskLevel { Low, Medium, High }

    public record Employee
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Department { get; init; }
        public required RiskLevel RiskLevel { get; init; }
    }
}
