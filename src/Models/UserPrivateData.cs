using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The User Private Data response class
/// </summary>
public class UserPrivateData
{
   /// <summary>
   /// User&#39;s Private Data
   /// </summary>
   public required Dictionary<string, object> PrivateData { get; set; }
}


