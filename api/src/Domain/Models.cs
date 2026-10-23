namespace EquipmentCare.Api.Domain;

public sealed class EquipmentItem
{
    public Guid Id { get; set; }
    public required string SerialNumber { get; set; }
    public required string Model { get; set; }
    public required string Location { get; set; }
    public required string Status { get; set; }
    public DateOnly InstalledOn { get; set; }
    public DateOnly LastServiceOn { get; set; }
    public DateOnly NextServiceOn { get; set; }
}

public sealed class ServiceRecord
{
    public Guid Id { get; set; }
    public Guid EquipmentId { get; set; }
    public required string Technician { get; set; }
    public required string Notes { get; set; }
    public DateTimeOffset CompletedAt { get; set; }
}

public sealed record ServiceRequest(string Technician, string Notes, DateTimeOffset? CompletedAt);
public sealed record EquipmentDetail(EquipmentItem Equipment, IReadOnlyList<ServiceRecord> ServiceHistory);
