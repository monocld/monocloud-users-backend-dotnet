using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The Update Public Data Request class.
/// </summary>
public class UpdatePublicDataRequest
{
   /// <summary>
   /// User&#39;s Public Data
   /// </summary>
   public required Dictionary<string, object> PublicData { get; set; }
}


