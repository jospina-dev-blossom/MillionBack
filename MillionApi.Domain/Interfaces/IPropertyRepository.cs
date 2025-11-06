using System.Collections.Generic;
using System.Threading.Tasks;
using MillionApi.Domain.AggregateModel;

namespace MillionApi.Application.Interfaces;

public interface IPropertyRepository
{
	Task<(IEnumerable<Property> Items, long TotalCount)> GetAsync(GetPropertiesRequest getPropertiesRequest);

	Task<Property> GetByIdAsync(string id);

	Task<(Property property, Owner owner, List<PropertyImage> images, List<PropertyTrace> traces)> GetByIdWithDetailsAsync(string id);

	Task<Property> CreateAsync(Property property);

	Task UpdateAsync(string id, Property property);

	Task DeleteAsync(string id);
}
