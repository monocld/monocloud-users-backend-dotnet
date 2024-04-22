using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The User IDP response class
/// </summary>
public class UserIdP
{
   /// <summary>
   /// Specifies the External Authenticator.
   /// </summary>
   public required ExternalAuthenticators Authenticator { get; set; }

   /// <summary>
   /// Specifies the idp user Id.
   /// </summary>
   public required string ProviderUserId { get; set; }

   /// <summary>
   /// Claims related to the idp.
   /// </summary>
   public required Dictionary<string, object> Claims { get; set; }
}


