using System;
using System.Collections.Generic;

namespace MonoCloud.SDK.UsersBackend.Models;

/// <summary>
/// The Pagination Header response class
/// </summary>
public class PaginationHeader
{
   /// <summary>
   /// Page Size
   /// </summary>
   public required int PageSize { get; set; }

   /// <summary>
   /// Current Page
   /// </summary>
   public required int CurrentPage { get; set; }

   /// <summary>
   /// Total Number of Items
   /// </summary>
   public required int TotalCount { get; set; }

   /// <summary>
   /// Specfies whether there is a previous page
   /// </summary>
   public required bool HasPrevious { get; set; }

   /// <summary>
   /// Specfies whether there is a next page
   /// </summary>
   public required bool HasNext { get; set; }
}


