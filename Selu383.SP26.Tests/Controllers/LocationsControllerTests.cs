using FluentAssertions;
using Selu383.SP26.Tests.Dtos;
using Selu383.SP26.Tests.Helpers;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace Selu383.SP26.Tests.Controllers;

[TestClass]
public class LocationsControllerTests
{
    private WebTestContext context = new();

    [TestInitialize]
    public void Init()
    {
        context = new WebTestContext();
    }

    [TestCleanup]
    public void Cleanup()
    {
        context.Dispose();
    }

    [TestMethod]
    public async Task ListAllLocations_Returns200AndData()
    {
        //arrange
        var webClient = context.GetStandardWebClient();

        //act
        var httpResponse = await webClient.GetAsync("/api/locations");

        //assert
        await httpResponse.AssertLocationListAllFunctions();
    }

    [TestMethod]
    public async Task GetLocationById_Returns200AndData()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var target = await webClient.GetLocation();
        if (target == null)
        {
            Assert.Fail("Make List All locations work first");
            return;
        }

        //act
        var httpResponse = await webClient.GetAsync($"/api/locations/{target.Id}");

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK, "we expect an HTTP 200 when calling GET /api/locations/{id} ");
        var resultDto = await httpResponse.Content.ReadAsJsonAsync<LocationDto>();
        resultDto.Should().BeEquivalentTo(target, "we expect get product by id to return the same data as the list all product endpoint");
    }

    [TestMethod]
    public async Task GetLocationById_NoSuchId_Returns404()
    {
        //arrange
        var webClient = context.GetStandardWebClient();

        //act
        var httpResponse = await webClient.GetAsync("/api/locations/999999");

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, "we expect an HTTP 404 when calling GET /api/locations/{id} with an invalid id");
    }

    [TestMethod]
    public async Task CreateLocation_NoName_Returns400()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Address = "asd",
            TableCount = 1
        };

        //act
        var httpResponse = await webClient.PostAsJsonAsync("/api/locations", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling POST /api/locations with no name");
    }

    [TestMethod]
    public async Task CreateLocation_NameTooLong_Returns400()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Name = "a".PadLeft(121, '0'),
            Address = "asd",
            TableCount = 1
        };

        //act
        var httpResponse = await webClient.PostAsJsonAsync("/api/locations", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling POST /api/locations with a name that is too long");
    }

    [TestMethod]
    public async Task CreateLocation_NoAddress_ReturnsError()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var target = await webClient.GetLocation();
        if (target == null)
        {
            Assert.Fail("You are not ready for this test");
            return;
        }
        var request = new LocationDto
        {
            Name = "asd",
            TableCount = 1
        };

        //act
        var httpResponse = await webClient.PostAsJsonAsync("/api/locations", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling POST /api/locations with no address");
    }

    [TestMethod]
    public async Task CreateLocation_InvalidTableCount_ReturnsError()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Name = "asd",
            Address = "asd",
            TableCount = 0 
        };

        //act
        var httpResponse = await webClient.PostAsJsonAsync("/api/locations", request);

        //assert
         httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling POST /api/locations with table count < 1");
    }

    [TestMethod]
    public async Task CreateLocation_Returns201AndData()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Name = "a",
            Address = "asd",
            TableCount = 5
        };

        //act
        var httpResponse = await webClient.PostAsJsonAsync("/api/locations", request);

        //assert
        await httpResponse.AssertCreateLocationFunctions(request, webClient);
    }

    [TestMethod]
    public async Task UpdateLocation_NoName_Returns400()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Name = "a",
            Address = "desc",
            TableCount = 5
        };
        await using var target = await webClient.CreateLocation(request);
        if (target == null)
        {
            Assert.Fail("You are not ready for this test");
        }

        request.Name = null!;

        //act
        var httpResponse = await webClient.PutAsJsonAsync($"/api/locations/{request.Id}", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling PUT /api/locations/{id} with a missing name");
    }

    [TestMethod]
    public async Task UpdateLocation_NameTooLong_Returns400()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Name = "a",
            Address = "desc",
            TableCount = 5
        };
        await using var target = await webClient.CreateLocation(request);
        if (target == null)
        {
            Assert.Fail("You are not ready for this test");
        }

        request.Name = "a".PadLeft(121, '0');

        //act
        var httpResponse = await webClient.PutAsJsonAsync($"/api/locations/{request.Id}", request);


        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling PUT /api/locations/{id} with a name that is too long");
    }

    [TestMethod]
    public async Task UpdateLocation_NoAddress_Returns400()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Name = "a",
            Address = "desc",
            TableCount = 5
        };
        await using var target = await webClient.CreateLocation(request);
        if (target == null)
        {
            Assert.Fail("You are not ready for this test");
        }

        request.Address = null!;

        //act
        var httpResponse = await webClient.PutAsJsonAsync($"/api/locations/{request.Id}", request);

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest, "we expect an HTTP 400 when calling PUT /api/locations/{id} with a missing address");
    }

    [TestMethod]
    public async Task UpdateLocation_Valid_Returns200()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Name = "a",
            Address = "desc",
            TableCount = 5
        };
        await using var target = await webClient.CreateLocation(request);
        if (target == null)
        {
            Assert.Fail("You are not ready for this test");
        }

        request.Address = "cool new address";
        request.TableCount = 10;

        //act
        var httpResponse = await webClient.PutAsJsonAsync($"/api/locations/{request.Id}", request);

        //assert
        await httpResponse.AssertLocationUpdateFunctions(request, webClient);
    }

    [TestMethod]
    public async Task DeleteLocation_NoSuchItem_ReturnsNotFound()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Address = "asd",
            Name = "asd",
            TableCount = 5
        };
        await using var itemHandle = await webClient.CreateLocation(request);
        if (itemHandle == null)
        {
            Assert.Fail("You are not ready for this test");
            return;
        }

        //act
        var httpResponse = await webClient.DeleteAsync($"/api/locations/{request.Id + 21}");

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, "we expect an HTTP 404 when calling DELETE /api/locations/{id} with an invalid Id");
    }

    [TestMethod]
    public async Task DeleteLocation_ValidItem_ReturnsOk()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Address = "asd",
            Name = "asd",
            TableCount = 5
        };
        await using var itemHandle = await webClient.CreateLocation(request);
        if (itemHandle == null)
        {
            Assert.Fail("You are not ready for this test");
            return;
        }

        //act
        var httpResponse = await webClient.DeleteAsync($"/api/locations/{request.Id}");

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK, "we expect an HTTP 200 when calling DELETE /api/locations/{id} with a valid id");
    }

    [TestMethod]
    public async Task DeleteLocation_SameItemTwice_ReturnsNotFound()
    {
        //arrange
        var webClient = context.GetStandardWebClient();
        var request = new LocationDto
        {
            Address = "asd",
            Name = "asd",
            TableCount = 5
        };
        await using var itemHandle = await webClient.CreateLocation(request);
        if (itemHandle == null)
        {
            Assert.Fail("You are not ready for this test");
            return;
        }

        //act
        await webClient.DeleteAsync($"/api/locations/{request.Id}");
        var httpResponse = await webClient.DeleteAsync($"/api/locations/{request.Id}");

        //assert
        httpResponse.StatusCode.Should().Be(HttpStatusCode.NotFound, "we expect an HTTP 404 when calling DELETE /api/locations/{id} on the same item twice");
    }
}
