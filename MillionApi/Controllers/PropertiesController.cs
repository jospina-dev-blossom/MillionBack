using MediatR;
using Microsoft.AspNetCore.Mvc;
using MillionApi.Application.Queries;
using MillionApi.Domain.AggregateModel;
using MillionApi.Domain.Exceptions;

[ApiController]
[Route("api/v1/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IMediator _mediator;
    public PropertiesController(IMediator mediator) => _mediator = mediator;

    /// <summary>
    /// Obtiene todas las propiedades con filtros opcionales y paginación
    /// </summary>
    /// <param name="getPropertiesRequest">Filtros de búsqueda (Name, Address, MinPrice, MaxPrice) y parámetros de paginación (PageNumber, PageSize)</param>
    /// <returns>Lista paginada de propiedades que coinciden con los filtros</returns>
    /// <response code="200">Retorna la lista paginada de propiedades con metadatos de paginación</response>
    /// <response code="400">Error de validación en los parámetros de búsqueda o paginación</response>
    /// <response code="404">No se encontraron propiedades con los filtros especificados</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<PropertyResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Get([FromQuery] GetPropertiesRequest getPropertiesRequest)
    {
        var result = await _mediator.Send(new GetPropertiesQuery(getPropertiesRequest));
        return Ok(result);
    }

    /// <summary>
    /// Obtiene una propiedad por su ID con información completa (Owner, Images, Traces)
    /// </summary>
    /// <param name="id">ID de la propiedad (formato ObjectId de MongoDB)</param>
    /// <returns>Detalles completos de la propiedad incluyendo propietario, imágenes y trazas</returns>
    /// <response code="200">Retorna la propiedad con toda su información relacionada</response>
    /// <response code="400">El ID proporcionado no es un ObjectId válido de MongoDB</response>
    /// <response code="404">La propiedad con el ID especificado no existe</response>
    /// <response code="500">Error interno del servidor</response>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(PropertyDetailResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(string id)
    {
        // La validación del formato ObjectId se hace en el handler
        var result = await _mediator.Send(new GetPropertyByIdQuery(id));
        return Ok(result);
    }
}
