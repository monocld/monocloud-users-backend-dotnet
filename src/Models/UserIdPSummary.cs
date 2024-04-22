using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The User IDP Summary response class
/// </summary>
public class UserIdPSummary
{
   /// <summary>
   /// Specifies the External Authenticator.
   /// </summary>
   public required ExternalAuthenticators Authenticator { get; set; }

   /// <summary>
   /// Specifies the idp user Id.
   /// </summary>
   public required string ProviderUserId { get; set; }
}


