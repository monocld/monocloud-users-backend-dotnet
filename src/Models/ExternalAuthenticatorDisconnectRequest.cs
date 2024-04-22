using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The External Authenticator Disconnect Request class.
/// </summary>
public class ExternalAuthenticatorDisconnectRequest
{
   /// <summary>
   /// External Authenticator to be disconnected
   /// </summary>
   public required ExternalAuthenticators Authenticator { get; set; }

   /// <summary>
   /// Provider User Id of the external authenticator.
   /// </summary>
   public required string ProviderUserId { get; set; }
}


