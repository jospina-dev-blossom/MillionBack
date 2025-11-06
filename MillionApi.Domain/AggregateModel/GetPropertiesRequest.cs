namespace MillionApi.Domain.AggregateModel;

public class GetPropertiesRequest
{
	public string? Name { get; set; }

	public string? Address { get; set; }

	public string? MinPrice { get; set; }

	public string? MaxPrice { get; set; }

	public int PageNumber { get; set; } = 1;

	public int PageSize { get; set; } = 10;
}
