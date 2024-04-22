using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The User Email response class
/// </summary>
public class UserEmail
{
   /// <summary>
   /// Unique identifier of the user email.
   /// </summary>
   public required Guid Id { get; set; }

   /// <summary>
   /// Specifies whether the email is the primary email of the user.
   /// </summary>
   public required bool Primary { get; set; }

   /// <summary>
   /// A list of identity providers the email is connected to.
   /// </summary>
   public required List<IdPs> Idps { get; set; }

   /// <summary>
   /// A list of authenticators the email is connected to.
   /// </summary>
   public required List<Authenticators> Authenticators { get; set; }

   /// <summary>
   /// Specifies whether the email is verified or not.
   /// </summary>
   public required bool Verified { get; set; }

   /// <summary>
   /// The email id.
   /// </summary>
   public required string Email { get; set; }
}


