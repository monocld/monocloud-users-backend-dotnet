using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The Unblock Ip Request class.
/// </summary>
public class UnblockIpRequest
{
   /// <summary>
   /// The ip address to be unblocked. Use &#39;all&#39; for unblocking all the ip addresses.
   /// </summary>
   public required string IpAddress { get; set; }
}


