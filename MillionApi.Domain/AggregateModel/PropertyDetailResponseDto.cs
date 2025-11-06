using System.Collections.Generic;

namespace MillionApi.Domain.AggregateModel;

public class PropertyDetailResponseDto
{
	public string Id { get; set; }

	public string Name { get; set; }

	public string AddressProperty { get; set; }

	public decimal PriceProperty { get; set; }

	public string ImageUrl { get; set; }

	public int CodeInternal { get; set; }

	public int Year { get; set; }

	public OwnerDto Owner { get; set; }

	public List<PropertyImageDto> Images { get; set; }

	public List<PropertyTraceDto> Traces { get; set; }
}
