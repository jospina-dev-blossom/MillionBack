using System;
using System.Collections.Generic;

namespace MillionApi.Domain.AggregateModel;

public class PagedResult<T>
{
	public IEnumerable<T> Items { get; set; } = new List<T>();

	public int PageNumber { get; set; }

	public int PageSize { get; set; }

	public long TotalCount { get; set; }

	public int TotalPages => (PageSize > 0) ? ((int)Math.Ceiling((double)TotalCount / (double)PageSize)) : 0;

	public bool HasPreviousPage => PageNumber > 1;

	public bool HasNextPage => PageNumber < TotalPages;
}
