using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The Update Private Data Request class.
/// </summary>
public class UpdatePrivateDataRequest
{
   /// <summary>
   /// User&#39;s Private Data
   /// </summary>
   public required Dictionary<string, object> PrivateData { get; set; }
}


