// ------------------------------------------------------------------------------------------------------------
// Archivo: IPropertyRepository.cs
// Proyecto: MyRealEstateApi
// Capa: Dominio > Interfaces
//
// Descripción:
// Esta interfaz define el contrato que deben cumplir todas las implementaciones del repositorio de propiedades.
// Forma parte de la capa de Dominio y representa un puerto (en términos de Arquitectura Hexagonal o Limpia),
// es decir, una abstracción que permite desacoplar el acceso a datos del resto de la aplicación.
//
// Esta interfaz es utilizada por la capa de Aplicación para recuperar propiedades sin preocuparse de
// los detalles de la infraestructura (por ejemplo, acceso a base de datos o almacenamiento externo).
//
// Aplicación de principios SOLID:
// ------------------------------------------------------------------------------------------------------------
// S - Single Responsibility (Responsabilidad Única):
//     La interfaz tiene una única responsabilidad: definir las operaciones necesarias para acceder
//     a los datos de propiedades inmobiliarias.
//
// O - Open/Closed (Abierta/Cerrada):
//     Las implementaciones pueden extenderse (por ejemplo, agregando filtros más complejos)
//     sin necesidad de modificar la interfaz base.
//
// L - Liskov Substitution (Sustitución de Liskov):
//     Cualquier clase que implemente esta interfaz puede sustituir su uso sin romper la funcionalidad.
//
// I - Interface Segregation (Segregación de Interfaces):
//     Se define una interfaz específica, centrada solo en propiedades, en lugar de una interfaz general con métodos innecesarios.
//
// D - Dependency Inversion (Inversión de Dependencias):
//     La aplicación depende de esta abstracción (la interfaz) en lugar de depender directamente
//     de una clase concreta de base de datos (como un repositorio con Entity Framework o ADO.NET).
//
// Relación con Arquitectura Limpia:
// - Esta interfaz es parte del dominio y no tiene dependencias externas.
// - La capa de infraestructura implementará esta interfaz sin afectar a las capas superiores.
// - La inversión de dependencias permite inyectar cualquier implementación sin modificar
//   ni recompilar las capas de dominio o aplicación.
//
// ------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Threading.Tasks;
using MyRealEstateApi.Domain.DTOs;

namespace MyRealEstateApi.Domain.Interfaces
{
    /// <summary>
    /// Interfaz que define el contrato para operaciones de acceso a propiedades inmobiliarias.
    /// Cualquier clase que implemente esta interfaz se encargará de recuperar propiedades según filtros definidos.
    /// </summary>
    public interface IPropertyRepository
    {
        /// <summary>
        /// Recupera de forma asíncrona una colección de propiedades aplicando filtros opcionales.
        /// </summary>
        /// <param name="name">Nombre o título de la propiedad (opcional).</param>
        /// <param name="address">Dirección o parte de la dirección de la propiedad (opcional).</param>
        /// <param name="priceMin">Precio mínimo (opcional).</param>
        /// <param name="priceMax">Precio máximo (opcional).</param>
        /// <returns>Una tarea que representa la operación asincrónica, cuyo resultado es una colección de <see cref="PropertyDto"/>.</returns>
        Task<IEnumerable<PropertyDto>> GetPropertiesAsync(
            string name,
            string address,
            decimal? priceMin,
            decimal? priceMax
        );
    }
}
