using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The User Phone response class
/// </summary>
public class UserPhone
{
   /// <summary>
   /// Unique identifier of the user phone.
   /// </summary>
   public required Guid Id { get; set; }

   /// <summary>
   /// Specifies whether the phone is the primary phone of the user.
   /// </summary>
   public required bool Primary { get; set; }

   /// <summary>
   /// A list of identity providers the phone is connected to.
   /// </summary>
   public required List<IdPs> Idps { get; set; }

   /// <summary>
   /// A list of authenticators the phone is connected to.
   /// </summary>
   public required List<Authenticators> Authenticators { get; set; }

   /// <summary>
   /// Specifies whether the phone is verified or not.
   /// </summary>
   public required bool Verified { get; set; }

   /// <summary>
   /// The phone number.
   /// </summary>
   public required string Phone { get; set; }
}


