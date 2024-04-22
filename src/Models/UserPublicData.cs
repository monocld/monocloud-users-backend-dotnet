using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The User Public Data response class
/// </summary>
public class UserPublicData
{
   /// <summary>
   /// User&#39;s Public Data
   /// </summary>
   public required Dictionary<string, object> PublicData { get; set; }
}


