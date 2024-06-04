using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using MonoCloud.SDK.Core.Base;
using MonoCloud.SDK.Core.Exception;
using MonoCloud.SDK.Core.Helpers;
using MonoCloud.SDK.UsersBackend.Models;

namespace MonoCloud.SDK.UsersBackend.Clients;

/// <summary>
/// Represents a collection of functions to interact with the Users Api endpoints
/// </summary>
public class UsersClient : MonoCloudClientBase
{
  /// <summary>
  /// Initializes a new instance of the <see cref="UsersClient"/> class.
  /// </summary>
  /// <param name="configuration">The <see cref="MonoCloudConfig">MonoCloud Configuration</see></param>
  /// <returns></returns>
  public UsersClient(MonoCloudConfig configuration) : base(configuration)
  {
  }

  /// <summary>
  /// Initializes a new instance of the <see cref="UsersClient"/> class.
  /// </summary>
  /// <param name="httpClient">The <see cref="HttpClient"/> which will be used to communicate with the MonoCloud Api</param>
  /// <returns></returns>
  public UsersClient(HttpClient httpClient) : base(httpClient)
  {
  }

  /// <summary>
  /// Get all users
  /// </summary>
  /// <param name="page">Page Number</param>
  /// <param name="size">Page Size</param>
  /// <param name="filter">Value by which the users needs to be filtered.</param>
  /// <param name="sort">Value in &#39;sort_key:sort_order&#39; format, by which results will be sorted. Sort order value can be &#39;1&#39; for ascending and &#39;-1&#39; for descending.  Acceptable sort key values are &#39;given_name&#39;, &#39;middle_name&#39;, &#39;family_name&#39;, &#39;name&#39;, &#39;creation_time&#39;, and &#39;last_updated&#39;</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>List&lt;UserSummary&gt;</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<List<UserSummary>, PageModel>> GetAllUsersAsync(int? page = 1, int? size = 10, string? filter = default, string? sort = default, CancellationToken cancellationToken = default)
  { 
    var urlBuilder = new StringBuilder();
    urlBuilder.Append("users?");

    if (page != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("page") + "=").Append(HttpUtility.UrlEncode(page.ToString())).Append("&");
    }

    if (size != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("size") + "=").Append(HttpUtility.UrlEncode(size.ToString())).Append("&");
    }

    if (filter != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("filter") + "=").Append(HttpUtility.UrlEncode(filter)).Append("&");
    }

    if (sort != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("sort") + "=").Append(HttpUtility.UrlEncode(sort)).Append("&");
    }

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("GET"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<List<UserSummary>, PageModel>(request, cancellationToken);
  }

  /// <summary>
  /// Create a user
  /// </summary>
  /// <param name="createUserRequest">User&#39;s data</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>User</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<User>> CreateUserAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken = default)
  { 
    if (createUserRequest == null)
    {
      throw new ArgumentNullException(nameof(createUserRequest));
    }
    
    var urlBuilder = new StringBuilder();
    urlBuilder.Append("users?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("POST"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Content = new StringContent(Serialize(createUserRequest), Encoding.UTF8, "application/json"),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<User>(request, cancellationToken);
  }

  /// <summary>
  /// Get a user
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>UserWithAccessDetails</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<UserWithAccessDetails>> FindUserByIdAsync(string userId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("GET"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<UserWithAccessDetails>(request, cancellationToken);
  }

  /// <summary>
  /// Delete a user
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns></returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse> DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("DELETE"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
    };

    return ProcessRequestAsync(request, cancellationToken);
  }

  /// <summary>
  /// Enable a user
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>User</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<User>> EnableUserAsync(string userId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/enable?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("POST"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<User>(request, cancellationToken);
  }

  /// <summary>
  /// Disable a user
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="disableUserRequest">The disable user request.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>User</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<User>> DisableUserAsync(string userId, DisableUserRequest disableUserRequest, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    if (disableUserRequest == null)
    {
      throw new ArgumentNullException(nameof(disableUserRequest));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/disable?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("POST"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Content = new StringContent(Serialize(disableUserRequest), Encoding.UTF8, "application/json"),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<User>(request, cancellationToken);
  }

  /// <summary>
  /// Unblock a user
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>User</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<User>> UnblockUserAsync(string userId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/unblock?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("POST"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<User>(request, cancellationToken);
  }

  /// <summary>
  /// Set email as primary
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="identifierId">The Id of the email to be set as primary.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>User</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<User>> SetPrimaryEmailAsync(string userId, Guid identifierId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    if (identifierId == null)
    {
      throw new ArgumentNullException(nameof(identifierId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var encodedIdentifierId = HttpUtility.UrlEncode(identifierId.ToString());

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/emails/{encodedIdentifierId}/primary?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("POST"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<User>(request, cancellationToken);
  }

  /// <summary>
  /// Mark email as verified
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="identifierId">The Id of the email to be marked as verified.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>User</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<User>> SetEmailVerifiedEndpointAsync(string userId, Guid identifierId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    if (identifierId == null)
    {
      throw new ArgumentNullException(nameof(identifierId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var encodedIdentifierId = HttpUtility.UrlEncode(identifierId.ToString());

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/emails/{encodedIdentifierId}/verify?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("POST"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<User>(request, cancellationToken);
  }

  /// <summary>
  /// Set phone as primary
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="identifierId">The Id of the phone to be set as primary.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>User</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<User>> SetPrimaryPhoneAsync(string userId, Guid identifierId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    if (identifierId == null)
    {
      throw new ArgumentNullException(nameof(identifierId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var encodedIdentifierId = HttpUtility.UrlEncode(identifierId.ToString());

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/phones/{encodedIdentifierId}/primary?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("POST"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<User>(request, cancellationToken);
  }

  /// <summary>
  /// Mark phone as verified
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="identifierId">The Id of the phone to be marked as verified.</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>User</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<User>> SetPhoneVerifiedEndpointAsync(string userId, Guid identifierId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    if (identifierId == null)
    {
      throw new ArgumentNullException(nameof(identifierId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var encodedIdentifierId = HttpUtility.UrlEncode(identifierId.ToString());

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/phones/{encodedIdentifierId}/verify?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("POST"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<User>(request, cancellationToken);
  }

  /// <summary>
  /// Get user private data
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>UserPrivateData</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<UserPrivateData>> GetPrivateDataAsync(string userId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/private_data?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("GET"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<UserPrivateData>(request, cancellationToken);
  }

  /// <summary>
  /// Update user private data
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="updatePrivateDataRequest">Data to be updated</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>UserPrivateData</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<UserPrivateData>> PatchPrivateDataAsync(string userId, UpdatePrivateDataRequest updatePrivateDataRequest, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    if (updatePrivateDataRequest == null)
    {
      throw new ArgumentNullException(nameof(updatePrivateDataRequest));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/private_data?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("PATCH"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Content = new StringContent(Serialize(updatePrivateDataRequest), Encoding.UTF8, "application/json"),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<UserPrivateData>(request, cancellationToken);
  }

  /// <summary>
  /// Get user public data
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>UserPublicData</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<UserPublicData>> GetPublicDataAsync(string userId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/public_data?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("GET"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<UserPublicData>(request, cancellationToken);
  }

  /// <summary>
  /// Update user public data
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="updatePublicDataRequest">Data to be updated</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>UserPublicData</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<UserPublicData>> PatchPublicDataAsync(string userId, UpdatePublicDataRequest updatePublicDataRequest, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    if (updatePublicDataRequest == null)
    {
      throw new ArgumentNullException(nameof(updatePublicDataRequest));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/public_data?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("PATCH"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Content = new StringContent(Serialize(updatePublicDataRequest), Encoding.UTF8, "application/json"),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<UserPublicData>(request, cancellationToken);
  }

  /// <summary>
  /// Get all blocked ips
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="page">Page Number</param>
  /// <param name="size">Page Size</param>
  /// <param name="filter">Ip address by which the blocked ips needs to be filtered.</param>
  /// <param name="sort">Value in &#39;sort_key:sort_order&#39; format, by which results will be sorted. Sort order value can be &#39;1&#39; for ascending and &#39;-1&#39; for descending.  Acceptable sort key values are &#39;block_until&#39; and &#39;last_login_attempt&#39;</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>List&lt;UserIpAccessDetails&gt;</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<List<UserIpAccessDetails>, PageModel>> GetAllBlockedIpsAsync(string userId, int? page = 1, int? size = 10, string? filter = default, string? sort = default, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/blocked_ips?");

    if (page != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("page") + "=").Append(HttpUtility.UrlEncode(page.ToString())).Append("&");
    }

    if (size != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("size") + "=").Append(HttpUtility.UrlEncode(size.ToString())).Append("&");
    }

    if (filter != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("filter") + "=").Append(HttpUtility.UrlEncode(filter)).Append("&");
    }

    if (sort != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("sort") + "=").Append(HttpUtility.UrlEncode(sort)).Append("&");
    }

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("GET"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<List<UserIpAccessDetails>, PageModel>(request, cancellationToken);
  }

  /// <summary>
  /// Unblock an ip address
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="unblockIpRequest">The unblock ip request</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>User</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<User>> UnblockIpAsync(string userId, UnblockIpRequest unblockIpRequest, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    if (unblockIpRequest == null)
    {
      throw new ArgumentNullException(nameof(unblockIpRequest));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/blocked_ips/unblock?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("POST"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Content = new StringContent(Serialize(unblockIpRequest), Encoding.UTF8, "application/json"),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<User>(request, cancellationToken);
  }

  /// <summary>
  /// Get all sessions
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="page">Page Number</param>
  /// <param name="size">Page Size</param>
  /// <param name="filter">Client Id by which the user sessions needs to be filtered.</param>
  /// <param name="sort">Value in &#39;sort_key:sort_order&#39; format, by which results will be sorted. Sort order value can be &#39;1&#39; for ascending and &#39;-1&#39; for descending.  Acceptable sort key values are &#39;session_id&#39;, &#39;initiated_at&#39;, &#39;expires_at&#39; and &#39;last_updated&#39;</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>List&lt;UserSession&gt;</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<List<UserSession>, PageModel>> GetAllUserSessionsAsync(string userId, int? page = 1, int? size = 10, string? filter = default, string? sort = default, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/sessions?");

    if (page != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("page") + "=").Append(HttpUtility.UrlEncode(page.ToString())).Append("&");
    }

    if (size != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("size") + "=").Append(HttpUtility.UrlEncode(size.ToString())).Append("&");
    }

    if (filter != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("filter") + "=").Append(HttpUtility.UrlEncode(filter)).Append("&");
    }

    if (sort != null)
    {
      urlBuilder.Append(Uri.EscapeDataString("sort") + "=").Append(HttpUtility.UrlEncode(sort)).Append("&");
    }

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("GET"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<List<UserSession>, PageModel>(request, cancellationToken);
  }

  /// <summary>
  /// Revoke a session
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="sessionId">Session Id</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns></returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse> RevokeUserSessionAsync(string userId, string sessionId, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    if (sessionId == null)
    {
      throw new ArgumentNullException(nameof(sessionId));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var encodedSessionId = HttpUtility.UrlEncode(sessionId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/sessions/{encodedSessionId}?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("DELETE"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
    };

    return ProcessRequestAsync(request, cancellationToken);
  }

  /// <summary>
  /// Disconnect External Authenticator
  /// </summary>
  /// <param name="userId">User Id</param>
  /// <param name="externalAuthenticatorDisconnectRequest">Idp disconnect data</param>
  /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
  /// <returns>User</returns>
  /// <exception cref="MonoCloudException">A server side error occurred.</exception>
  public Task<MonoCloudResponse<User>> ExternalAuthenticatorDisconnectAsync(string userId, ExternalAuthenticatorDisconnectRequest externalAuthenticatorDisconnectRequest, CancellationToken cancellationToken = default)
  { 
    if (userId == null)
    {
      throw new ArgumentNullException(nameof(userId));
    }
    
    if (externalAuthenticatorDisconnectRequest == null)
    {
      throw new ArgumentNullException(nameof(externalAuthenticatorDisconnectRequest));
    }
    
    var encodedUserId = HttpUtility.UrlEncode(userId);

    var urlBuilder = new StringBuilder();
    urlBuilder.Append($"users/{encodedUserId}/external_authenticator/disconnect?");

    urlBuilder.Length--;

    var request = new HttpRequestMessage
    {
      Method = new HttpMethod("POST"),
      RequestUri = new Uri(urlBuilder.ToString(), UriKind.RelativeOrAbsolute),
      Content = new StringContent(Serialize(externalAuthenticatorDisconnectRequest), Encoding.UTF8, "application/json"),
      Headers =
      {
        { "Accept", "application/json" }
      }
    };

    return ProcessRequestAsync<User>(request, cancellationToken);
  }
}

