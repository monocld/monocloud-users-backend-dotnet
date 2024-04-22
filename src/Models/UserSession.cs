using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The User Session response class
/// </summary>
public class UserSession
{
   /// <summary>
   /// The session key
   /// </summary>
   public required string SessionKey { get; set; }

   /// <summary>
   /// The unique identifier of the session
   /// </summary>
   public required string SessionId { get; set; }

   /// <summary>
   /// The list of client ids which are associated with the session
   /// </summary>
   public required List<string> ClientIds { get; set; }

   /// <summary>
   /// The initial time when the session was created
   /// </summary>
   public required DateTime InitiatedAt { get; set; }

   /// <summary>
   /// The expiration time of the session
   /// </summary>
   public required DateTime ExpiresAt { get; set; }

   /// <summary>
   /// The last updated time
   /// </summary>
   public required DateTime LastUpdated { get; set; }

   /// <summary>
   /// The last session metadata
   /// </summary>
   public required UserSessionMetadata LastMetadata { get; set; }
}


