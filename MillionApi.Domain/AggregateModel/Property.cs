using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionApi.Domain.AggregateModel;

public class Property
{
	[BsonId]
	[BsonRepresentation(BsonType.ObjectId)]
	public string Id { get; set; }

	public string IdOwner { get; set; }

	public string Name { get; set; }

	public string AddressProperty { get; set; }

	public decimal PriceProperty { get; set; }

	public string ImageUrl { get; set; }

	public int CodeInternal { get; set; }

	public int Year { get; set; }
}
