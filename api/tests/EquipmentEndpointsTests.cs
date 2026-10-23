using System.Net;
using System.Net.Http.Json;
using EquipmentCare.Api.Domain;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EquipmentCare.Api.Tests;

public sealed class EquipmentEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public EquipmentEndpointsTests(WebApplicationFactory<Program> factory) => _client = factory.CreateClient();

    [Fact]
    public async Task EquipmentDetail_ReturnsServiceHistory()
    {
        var response = await _client.GetAsync("/api/equipment/EQP-MN-2048");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Contains("Calibração preventiva", await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task ServiceRegistration_UpdatesEquipmentSchedule()
    {
        var response = await _client.PostAsJsonAsync("/api/equipment/EQP-VT-1092/service", new ServiceRequest("Gustavo Demo", "Revisão funcional.", null));
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var detail = await _client.GetStringAsync("/api/equipment/EQP-VT-1092");
        Assert.Contains("Gustavo Demo", detail);
    }
}
