using MonoCloud.SDK.Core.Exception;
using MonoCloud.SDK.UsersBackend.Models;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Text.Json;

namespace MonoCloud.SDK.UsersBackend.UnitTests;

public class SDKTests
{
  private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
  private Dictionary<string, object> _requestMessage = new();
  private readonly MonoCloudUsersBackendClient _usersClient;

  public SDKTests()
  {
    _httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
    var httpClient = _httpMessageHandlerMock.CreateClient();
    httpClient.BaseAddress = new Uri("https://mock.com");
    _usersClient = new MonoCloudUsersBackendClient(httpClient);
  }

  [Fact]
  public async Task Create_should_only_send_set_fields()
  {
    SetMockResponse(new CreateUserRequest());

    await _usersClient.CreateUserAsync(new CreateUserRequest()
    {
      Name = "user"
    });

    Assert.NotEmpty(_requestMessage);
    Assert.Equal("user", _requestMessage["name"]);
    Assert.Single(_requestMessage);
  }

  [Fact]
  public async Task Create_should_send_correct_enum()
  {
    SetMockResponse(new ExternalAuthenticatorDisconnectRequest());

    await _usersClient.ExternalAuthenticatorDisconnectAsync("user", new ExternalAuthenticatorDisconnectRequest
    {
      Authenticator = ExternalAuthenticators.Apple
    });

    Assert.NotEmpty(_requestMessage);
    Assert.Equal("apple", _requestMessage["authenticator"]);
    Assert.Single(_requestMessage);
  }

  [Fact]
  public async Task Patch_should_send_null_fields_when_set()
  {
    SetMockResponse(new UserPrivateData());

    await _usersClient.PatchPrivateDataAsync("1234", new UpdatePrivateDataRequest
    {
      PrivateData = new Dictionary<string, object>
      {
        { "Test", null! }
      }
    });

    Assert.NotEmpty(_requestMessage);
    var data = JsonConvert.DeserializeObject<Dictionary<string, object>>((_requestMessage["private_data"] as JObject)!.ToString())!;
    Assert.Null(data["Test"]);
    Assert.Single(data);
  }

  [Fact]
  public async Task Get_should_receive_correct_datetime()
  {
    var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

    SetMockResponse(new
    {
      creation_time = now
    });

    var result = await _usersClient.FindUserByIdAsync("user");

    Assert.NotNull(result);
    Assert.Equivalent(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(now), result.Data.CreationTime);
  }

  [Fact]
  public async Task Get_should_receive_correct_nullable_datetime()
  {
    SetMockResponse(new
    {
      password_updated_at = (long?)null
    });

    var result = await _usersClient.FindUserByIdAsync("user");

    Assert.Null(result.Data.PasswordUpdatedAt);
  }

  [Fact]
  public async Task Get_with_paging_should_receive_correct_result()
  {
    SetMockResponse(new List<User>
    {
      new(),
      new()
    }, headers: new Dictionary<string, string>
    {
      { "x-Pagination", """{"total_count":20,"page_size":2,"current_page":3,"has_next":true,"has_previous":true}""" }
    });

    var result = await _usersClient.GetAllUsersAsync();

    Assert.Equal(2, result.Data.Count);
    Assert.Equal(20, result.PageData.TotalCount);
    Assert.Equal(2, result.PageData.PageSize);
    Assert.Equal(3, result.PageData.CurrentPage);
    Assert.True(result.PageData.HasNext);
    Assert.True(result.PageData.HasPrevious);
  }

  [Fact]
  public async Task Get_with_paging_should_handle_no_pagination_header()
  {
    SetMockResponse(new List<User>
    {
      new(),
      new()
    });

    var result = await _usersClient.GetAllUsersAsync();

    Assert.Equal(2, result.Data.Count);
    Assert.Equal(0, result.PageData.TotalCount);
    Assert.Equal(0, result.PageData.PageSize);
    Assert.Equal(0, result.PageData.CurrentPage);
    Assert.False(result.PageData.HasNext);
    Assert.False(result.PageData.HasPrevious);
  }

  [Fact]
  public async Task Identity_error_should_handle_correctly()
  {
    const string response = """
    {
        "type": "https://httpstatuses.io/422#identity-validation-error",
        "title": "Unprocessable Entity",
        "status": 422,
        "errors": [
            {
                "code": "PasswordTooShort",
                "description": "Passwords must be at least 8 characters."
            },
            {
                "code": "PasswordRequiresNonAlphanumeric",
                "description": "Passwords must have at least one non alphanumeric character."
            },
            {
                "code": "PasswordRequiresUpper",
                "description": "Passwords must have at least one uppercase ('A'-'Z')."
            }
        ],
        "traceId": "00-cd3f24e893675e2dae242875e99e7c85-296286fe1c04c085-01"
    }
    """;

    _httpMessageHandlerMock.SetupRequest(_ => Task.FromResult(true)).ReturnsResponse(HttpStatusCode.UnprocessableEntity, new StringContent(response, Encoding.UTF8, "application/problem+json"));

    try
    {
      await _usersClient.CreateUserAsync(new CreateUserRequest());
      throw new Exception("Invalid");
    }
    catch (Exception e)
    {
      Assert.True(e is MonoCloudErrorCodeValidationException);
      var mcError = (e as MonoCloudErrorCodeValidationException)!;
      Assert.StartsWith("Unprocessable Entity", mcError.Message);
      Assert.NotNull(mcError.Response);
      Assert.Equal(3, mcError.Errors.Count());
      Assert.Equal("PasswordTooShort", mcError.Errors.First().Code);
      Assert.Equal("PasswordRequiresNonAlphanumeric", mcError.Errors.Skip(1).First().Code);
      Assert.Equal("PasswordRequiresUpper", mcError.Errors.Last().Code);
    }
  }

  [Fact]
  public async Task Key_validation_error_should_handle_correctly()
  {
    const string response = """
    {
        "type": "https://httpstatuses.io/422#validation-error",
        "title": "Unprocessable Entity",
        "status": 422,
        "errors": {
          "name": ["Invalid Name"],
          "password": ["Invalid Password"]
        },
        "traceId": "00-cd3f24e893675e2dae242875e99e7c85-296286fe1c04c085-01"
    }
    """;

    _httpMessageHandlerMock.SetupRequest(_ => Task.FromResult(true)).ReturnsResponse(HttpStatusCode.UnprocessableEntity, new StringContent(response, Encoding.UTF8, "application/problem+json"));

    try
    {
      await _usersClient.CreateUserAsync(new CreateUserRequest());
      throw new Exception("Invalid");
    }
    catch (Exception e)
    {
      Assert.True(e is MonoCloudKeyValidationException);
      var mcError = (e as MonoCloudKeyValidationException)!;
      Assert.NotNull(mcError.Response);
      Assert.StartsWith("Unprocessable Entity", mcError.Message);
      Assert.Equal(2, mcError.Errors.Count);
      Assert.Equal("name", mcError.Errors.First().Key);
      Assert.Equal("Invalid Name", mcError.Errors.First().Value.Single());
      Assert.Equal("password", mcError.Errors.Last().Key);
      Assert.Equal("Invalid Password", mcError.Errors.Last().Value.Single());
    }
  }

  [Fact]
  public async Task Internal_server_error_should_handle_correctly()
  {
    const string response = """
    {
      "type": "https://httpstatuses.io/500",
      "title": "Internal Server Error",
      "status": 500,
      "detail": "Internal Server Error Detail",
      "traceId": "00-b2ceddefca0cf958ed678f144872e3c7-d0b2da5c8fe32598-01"
    }
    """;

    _httpMessageHandlerMock.SetupRequest(_ => Task.FromResult(true)).ReturnsResponse(HttpStatusCode.InternalServerError, new StringContent(response, Encoding.UTF8, "application/problem+json"));

    try
    {
      await _usersClient.CreateUserAsync(new CreateUserRequest());
      throw new Exception("Invalid");
    }
    catch (Exception e)
    {
      Assert.True(e is MonoCloudServerException);
      var mcError = (e as MonoCloudServerException)!;
      Assert.NotNull(mcError.Response);
      Assert.Equal("Internal Server Error", mcError.Message);
      Assert.Equal("Internal Server Error Detail", mcError.Response.Detail);
      Assert.Equal("Internal Server Error", mcError.Response.Title);
      Assert.Equal("https://httpstatuses.io/500", mcError.Response.Type);
      Assert.Equal(500, mcError.Response.Status);
      var traceIdJsonElement = (JsonElement)mcError.Response.ExtensionData["traceId"];
      Assert.Equal("00-b2ceddefca0cf958ed678f144872e3c7-d0b2da5c8fe32598-01", traceIdJsonElement.GetString());
    }
  }

  [Fact]
  public async Task Bad_request_error_should_handle_correctly()
  {
    const string response = """
    {
      "type": "https://httpstatuses.io/400",
      "title": "Bad Request",
      "status": 400,
      "detail": "Bad Request Detail",
      "traceId": "00-2e0cd141be28223b233dd3907cbe58b4-2ba9f9375b4b78e0-01"
    }
    """;

    _httpMessageHandlerMock.SetupRequest(_ => Task.FromResult(true)).ReturnsResponse(HttpStatusCode.MethodNotAllowed, new StringContent(response, Encoding.UTF8, "application/problem+json"));

    try
    {
      await _usersClient.CreateUserAsync(new CreateUserRequest());
      throw new Exception("Invalid");
    }
    catch (Exception e)
    {
      Assert.True(e is MonoCloudBadRequestException);
      var mcError = (e as MonoCloudBadRequestException)!;
      Assert.NotNull(mcError.Response);
      Assert.Equal("Bad Request", mcError.Message);
      Assert.Equal("Bad Request Detail", mcError.Response.Detail);
      Assert.Equal("Bad Request", mcError.Response.Title);
      Assert.Equal("https://httpstatuses.io/400", mcError.Response.Type);
      Assert.Equal(400, mcError.Response.Status);
      var traceIdJsonElement = (JsonElement)mcError.Response.ExtensionData["traceId"];
      Assert.Equal("00-2e0cd141be28223b233dd3907cbe58b4-2ba9f9375b4b78e0-01", traceIdJsonElement.GetString());
    }
  }

  private void SetMockResponse(object response, HttpStatusCode code = HttpStatusCode.OK, IDictionary<string, string>? headers = null) =>
    _httpMessageHandlerMock.SetupRequest(async message =>
    {
      try
      {
        var val = await message.Content!.ReadAsStringAsync();
        _requestMessage = JsonConvert.DeserializeObject<Dictionary<string, object>>(val)!;
      }
      catch
      {
        //
      }
      return true;
    }).ReturnsJsonResponse(code, response, configure: (message) =>
    {
      if (headers is not null)
      {
        foreach (var httpResponseHeader in headers)
        {
          message.Headers.Add(httpResponseHeader.Key, httpResponseHeader.Value);
        }
      }
    });
}
