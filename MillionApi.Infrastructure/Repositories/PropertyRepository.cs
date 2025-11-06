using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MillionApi.Application.Interfaces;
using MillionApi.Domain.AggregateModel;
using MillionApi.Infrastructure.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MillionApi.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
	private readonly MongoContext _context;

	public PropertyRepository(MongoContext context)
	{
		_context = context;
	}

	public async Task<(IEnumerable<Property> Items, long TotalCount)> GetAsync(GetPropertiesRequest getPropertiesRequest)
	{
		FilterDefinitionBuilder<Property> filterBuilder = Builders<Property>.Filter;
		List<FilterDefinition<Property>> filters = new List<FilterDefinition<Property>>();
		if (!string.IsNullOrEmpty(getPropertiesRequest.Name))
		{
			filters.Add(filterBuilder.Regex((Property p) => p.Name, new BsonRegularExpression(getPropertiesRequest.Name, "i")));
		}
		if (!string.IsNullOrEmpty(getPropertiesRequest.Address))
		{
			filters.Add(filterBuilder.Regex((Property p) => p.AddressProperty, new BsonRegularExpression(getPropertiesRequest.Address, "i")));
		}
		if (!string.IsNullOrEmpty(getPropertiesRequest.MinPrice) && decimal.TryParse(getPropertiesRequest.MinPrice, out var minPrice))
		{
			filters.Add(filterBuilder.Gte((Property p) => p.PriceProperty, minPrice));
		}
		if (!string.IsNullOrEmpty(getPropertiesRequest.MaxPrice) && decimal.TryParse(getPropertiesRequest.MaxPrice, out var maxPrice))
		{
			filters.Add(filterBuilder.Lte((Property p) => p.PriceProperty, maxPrice));
		}
		FilterDefinition<Property> combinedFilter = ((filters.Count > 0) ? filterBuilder.And(filters) : filterBuilder.Empty);
		long totalCount = await _context.Properties.CountDocumentsAsync(combinedFilter);
		int skip = (getPropertiesRequest.PageNumber - 1) * getPropertiesRequest.PageSize;
		return (Items: await _context.Properties.Find(combinedFilter).Skip(skip).Limit(getPropertiesRequest.PageSize)
			.ToListAsync(), TotalCount: totalCount);
	}

	public Task<Property> GetByIdAsync(string id)
	{
		return _context.Properties.Find<Property>((Property p) => p.Id == id).FirstOrDefaultAsync();
	}

	public async Task<(Property property, Owner owner, List<PropertyImage> images, List<PropertyTrace> traces)> GetByIdWithDetailsAsync(string id)
	{
		Property property = await _context.Properties.Find<Property>((Property p) => p.Id == id).FirstOrDefaultAsync();
		if (property == null)
		{
			return (property: null, owner: null, images: null, traces: null);
		}
		return new ValueTuple<Property, Owner, List<PropertyImage>, List<PropertyTrace>>(item2: await _context.Owners.Find<Owner>((Owner o) => o.IdOwner == property.IdOwner).FirstOrDefaultAsync(), item3: await _context.PropertyImages.Find<PropertyImage>((PropertyImage pi) => pi.IdProperty == id).ToListAsync(), item4: await _context.PropertyTraces.Find<PropertyTrace>((PropertyTrace pt) => pt.IdProperty == id).SortByDescending((PropertyTrace pt) => pt.DateSale).ToListAsync(), item1: property);
	}

	public async Task<Property> CreateAsync(Property property)
	{
		await _context.Properties.InsertOneAsync(property);
		return property;
	}

	public Task UpdateAsync(string id, Property property)
	{
		return _context.Properties.ReplaceOneAsync<Property>((Property p) => p.Id == id, property);
	}

	public Task DeleteAsync(string id)
	{
		return _context.Properties.DeleteOneAsync<Property>((Property p) => p.Id == id);
	}
}
