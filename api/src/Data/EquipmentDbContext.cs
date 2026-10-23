using EquipmentCare.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace EquipmentCare.Api.Data;

public sealed class EquipmentDbContext(DbContextOptions<EquipmentDbContext> options) : DbContext(options)
{
    public DbSet<EquipmentItem> Equipment => Set<EquipmentItem>();
    public DbSet<ServiceRecord> ServiceRecords => Set<ServiceRecord>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EquipmentItem>().HasIndex(item => item.SerialNumber).IsUnique();
        modelBuilder.Entity<ServiceRecord>().HasIndex(item => new { item.EquipmentId, item.CompletedAt });
    }
}

public static class DemoData
{
    public static void Seed(EquipmentDbContext db)
    {
        if (db.Equipment.Any()) return;
        var neo = new EquipmentItem { Id = Guid.Parse("10000000-0000-0000-0000-000000000001"), SerialNumber = "EQP-MN-2048", Model = "Med Neo Demo", Location = "Clínica Vila Olímpia", Status = "service_due", InstalledOn = new(2025, 9, 12), LastServiceOn = new(2026, 4, 28), NextServiceOn = new(2026, 10, 28) };
        var vet = new EquipmentItem { Id = Guid.Parse("10000000-0000-0000-0000-000000000002"), SerialNumber = "EQP-VT-1092", Model = "Vet Plus Demo", Location = "Hospital Veterinário Central", Status = "operational", InstalledOn = new(2026, 2, 8), LastServiceOn = new(2026, 8, 8), NextServiceOn = new(2027, 2, 8) };
        db.Equipment.AddRange(neo, vet);
        db.ServiceRecords.Add(new ServiceRecord { Id = Guid.Parse("20000000-0000-0000-0000-000000000001"), EquipmentId = neo.Id, Technician = "Equipe técnica demo", Notes = "Calibração preventiva e inspeção visual concluídas.", CompletedAt = new(2026, 4, 28, 14, 20, 0, TimeSpan.FromHours(-3)) });
        db.SaveChanges();
    }
}
