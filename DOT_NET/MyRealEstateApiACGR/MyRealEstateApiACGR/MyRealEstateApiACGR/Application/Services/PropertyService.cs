using System.Collections.Generic;
using System.Threading.Tasks;
using MyRealEstateApi.Domain.DTOs;
using MyRealEstateApi.Domain.Interfaces;

namespace MyRealEstateApi.Application.Services
{
    /// <summary>
    /// Servicio de aplicación encargado de coordinar operaciones relacionadas con propiedades.
    /// Forma parte de la capa de "Application" en la arquitectura limpia,
    /// actuando como intermediario entre los controladores (UI) y el dominio.
    /// </summary>
    public class PropertyService
    {
        // Principio de Inversión de Dependencias (D - SOLID):
        // En lugar de depender de una implementación concreta del repositorio,
        // se depende de una abstracción (interfaz), facilitando el desacoplamiento y pruebas unitarias.
        private readonly IPropertyRepository _propertyRepository;

        /// <summary>
        /// Constructor que inyecta una dependencia mediante inyección de dependencias.
        /// Esta práctica promueve bajo acoplamiento y adherencia a los principios SOLID.
        /// </summary>
        /// <param name="propertyRepository">Interfaz del repositorio que proporciona acceso a los datos de propiedades.</param>
        public PropertyService(IPropertyRepository propertyRepository)
        {
            _propertyRepository = propertyRepository;
        }

        /// <summary>
        /// Recupera una lista de propiedades filtradas por nombre, dirección y rango de precios.
        /// 
        /// Principios aplicados:
        /// - Principio de Responsabilidad Única (S - SOLID): este servicio se encarga únicamente de coordinar la lógica de recuperación de propiedades.
        /// - Principio de Abierto/Cerrado (O - SOLID): el servicio puede extenderse (p. ej., agregar validaciones o lógica de negocio) sin modificar su estructura.
        /// 
        /// Arquitectura limpia:
        /// - El servicio no tiene conocimiento de cómo se obtienen los datos (infraestructura), solo interactúa con una abstracción.
        /// - Los DTOs (Data Transfer Objects) encapsulan la información transferida, sin exponer entidades del dominio directamente.
        /// </summary>
        /// <param name="name">Nombre de la propiedad (opcional).</param>
        /// <param name="address">Dirección de la propiedad (opcional).</param>
        /// <param name="priceMin">Precio mínimo (opcional).</param>
        /// <param name="priceMax">Precio máximo (opcional).</param>
        /// <returns>Lista de propiedades como DTOs que cumplen con los filtros especificados.</returns>
        public Task<IEnumerable<PropertyDto>> GetPropertiesAsync(string name, string address, decimal? priceMin, decimal? priceMax)
        {
            // Principio de Sustitución de Liskov (L - SOLID):
            // Podemos sustituir cualquier implementación concreta de IPropertyRepository sin afectar el funcionamiento.
            return _propertyRepository.GetPropertiesAsync(name, address, priceMin, priceMax);
        }
    }
}
