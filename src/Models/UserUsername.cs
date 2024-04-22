using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The User Username response class
/// </summary>
public class UserUsername
{
   /// <summary>
   /// Unique identifier of the user email.
   /// </summary>
   public required Guid Id { get; set; }

   /// <summary>
   /// A list of identity providers the username is connected to.
   /// </summary>
   public required List<IdPs> Idps { get; set; }

   /// <summary>
   /// A list of authenticators the username is connected to.
   /// </summary>
   public required List<Authenticators> Authenticators { get; set; }

   /// <summary>
   /// The username.
   /// </summary>
   public required string Username { get; set; }
}


