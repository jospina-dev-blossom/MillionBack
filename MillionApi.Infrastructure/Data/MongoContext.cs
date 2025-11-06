using Microsoft.Extensions.Options;
using MillionApi.Domain.AggregateModel;
using MongoDB.Driver;

namespace MillionApi.Infrastructure.Data;

public class MongoContext
{
	private readonly IMongoDatabase _db;

	public IMongoCollection<Property> Properties => _db.GetCollection<Property>("Properties");

	public IMongoCollection<Owner> Owners => _db.GetCollection<Owner>("Owner");

	public IMongoCollection<PropertyImage> PropertyImages => _db.GetCollection<PropertyImage>("PropertyImage");

	public IMongoCollection<PropertyTrace> PropertyTraces => _db.GetCollection<PropertyTrace>("PropertyTrace");

	public MongoContext(IOptions<MongoSettings> options)
	{
		MongoClient mongoClient = new MongoClient(options.Value.Connection);
		_db = mongoClient.GetDatabase(options.Value.DatabaseName);
	}
}
