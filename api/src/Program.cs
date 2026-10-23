using EquipmentCare.Api.Data;
using EquipmentCare.Api.Domain;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
var connectionString = builder.Configuration.GetConnectionString("Postgres");
builder.Services.AddDbContext<EquipmentDbContext>(options =>
{
    if (string.IsNullOrWhiteSpace(connectionString)) options.UseInMemoryDatabase("equipment-care-demo");
    else options.UseNpgsql(connectionString);
});

var app = builder.Build();
app.UseCors();
if (app.Environment.IsDevelopment()) app.MapOpenApi();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<EquipmentDbContext>();
    db.Database.EnsureCreated();
    DemoData.Seed(db);
}

app.MapGet("/health", () => Results.Ok(new { status = "healthy", service = "equipment-care-api" }));
app.MapGet("/api/equipment", async (EquipmentDbContext db) => Results.Ok(await db.Equipment.AsNoTracking().OrderBy(item => item.NextServiceOn).ToListAsync()));
app.MapGet("/api/equipment/due", async (int days, EquipmentDbContext db) =>
{
    var limit = DateOnly.FromDateTime(DateTime.UtcNow).AddDays(Math.Clamp(days, 0, 180));
    return Results.Ok(await db.Equipment.AsNoTracking().Where(item => item.NextServiceOn <= limit).OrderBy(item => item.NextServiceOn).ToListAsync());
});
app.MapGet("/api/equipment/{serial}", async (string serial, EquipmentDbContext db) =>
{
    var equipment = await db.Equipment.AsNoTracking().SingleOrDefaultAsync(item => item.SerialNumber == serial);
    if (equipment is null) return Results.NotFound();
    var records = await db.ServiceRecords.AsNoTracking().Where(item => item.EquipmentId == equipment.Id).OrderByDescending(item => item.CompletedAt).ToListAsync();
    return Results.Ok(new EquipmentDetail(equipment, records));
});
app.MapPost("/api/equipment/{serial}/service", async (string serial, ServiceRequest request, EquipmentDbContext db) =>
{
    var equipment = await db.Equipment.SingleOrDefaultAsync(item => item.SerialNumber == serial);
    if (equipment is null) return Results.NotFound();
    var record = new ServiceRecord { Id = Guid.NewGuid(), EquipmentId = equipment.Id, Technician = request.Technician, Notes = request.Notes, CompletedAt = request.CompletedAt ?? DateTimeOffset.UtcNow };
    equipment.LastServiceOn = DateOnly.FromDateTime(record.CompletedAt.UtcDateTime);
    equipment.NextServiceOn = equipment.LastServiceOn.AddMonths(6);
    equipment.Status = "operational";
    db.ServiceRecords.Add(record);
    await db.SaveChangesAsync();
    return Results.Created($"/api/equipment/{serial}", record);
});

app.Run();
public partial class Program;
