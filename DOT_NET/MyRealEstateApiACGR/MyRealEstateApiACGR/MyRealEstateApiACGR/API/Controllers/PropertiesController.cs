// -----------------------------------------------------------------------------
// PropertiesController.cs
// -----------------------------------------------------------------------------
// Controlador de propiedades expuesto como API RESTful en ASP.NET Core.
// Pertenece a la capa de Presentación en una arquitectura limpia.
// Su responsabilidad es recibir las solicitudes HTTP, delegar la lógica de negocio 
// al servicio de aplicación (Application Layer), y devolver respuestas apropiadas.
// Aplica los principios SOLID como:
// - Responsabilidad única: solo se encarga de orquestar la solicitud/respuesta HTTP.
// - Inversión de dependencias: depende de la abstracción del servicio, no lo implementa.
// -----------------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc; // Funcionalidad base para controladores HTTP en ASP.NET Core
using MyRealEstateApi.Application.Services; // Servicio de aplicación que contiene lógica de negocio
using System;
using System.Threading.Tasks;

namespace MyRealEstateApi.API.Controllers // Capa de Presentación (API)
{
    // -------------------------------------------------------------------------
    // El atributo [ApiController] habilita funcionalidades automáticas como:
    // - Validación automática de modelos
    // - Respuestas HTTP 400 BadRequest cuando hay errores en el modelo
    // - Binding automático de parámetros desde query, body, etc.
    // -------------------------------------------------------------------------
    [ApiController]

    // -------------------------------------------------------------------------
    // Define la ruta base del controlador: "api/properties"
    // La parte [controller] se reemplaza automáticamente por "Properties"
    // -------------------------------------------------------------------------
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        // ---------------------------------------------------------------------
        // Campo privado solo lectura para el servicio de propiedades
        // Se inyecta mediante el constructor (inyección de dependencias)
        // Esto permite cumplir el principio de inversión de dependencias (D de SOLID)
        // ---------------------------------------------------------------------
        private readonly PropertyService _propertyService;

        // ---------------------------------------------------------------------
        // Constructor con inyección del servicio de aplicación.
        // Este servicio actúa como orquestador entre el controlador (Presentación)
        // y el repositorio (Infraestructura) a través de interfaces (Dominio).
        // ---------------------------------------------------------------------
        public PropertiesController(PropertyService propertyService)
        {
            _propertyService = propertyService;
        }

        // ---------------------------------------------------------------------
        // Endpoint GET: api/properties
        // Permite obtener una lista de propiedades filtradas por nombre, dirección y rango de precios.
        // Los parámetros se reciben desde la URL como query strings (?name=...&priceMin=...)
        // ---------------------------------------------------------------------
        [HttpGet]
        public async Task<IActionResult> GetProperties(
            [FromQuery] string name = null,
            [FromQuery] string address = null,
            [FromQuery] decimal? priceMin = null,
            [FromQuery] decimal? priceMax = null)
        {
            try
            {
                // -----------------------------------------------------------------
                // Llamada al servicio de aplicación para obtener las propiedades filtradas.
                // La lógica de negocio (filtrado, validación) debe estar encapsulada en el servicio,
                // no en el controlador, siguiendo el principio de responsabilidad única (S de SOLID).
                // -----------------------------------------------------------------
                var properties = await _propertyService.GetPropertiesAsync(name, address, priceMin, priceMax);

                // -----------------------------------------------------------------
                // Devuelve un resultado HTTP 200 OK con la lista de propiedades en formato JSON
                // -----------------------------------------------------------------
                return Ok(properties);
            }
            catch (Exception ex)
            {
                // -----------------------------------------------------------------
                // Manejo de errores: si ocurre una excepción, se devuelve un error 500 (Internal Server Error)
                // Idealmente se debería usar un servicio de logging para registrar la excepción
                // -----------------------------------------------------------------
                return StatusCode(500, new
                {
                    message = "Error interno en el servidor",
                    detail = ex.Message
                });
            }
        }
    }
}
