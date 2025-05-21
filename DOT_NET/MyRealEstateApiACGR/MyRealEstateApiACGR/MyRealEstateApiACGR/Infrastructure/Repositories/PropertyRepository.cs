// ------------------------------------------------------------------------------------------------------------
// Archivo: PropertyRepository.cs
// Proyecto: MyRealEstateApi
// Capa: Infrastructure > Repositories
//
// Descripción:
// Implementación concreta de la interfaz IPropertyRepository.
// Esta clase forma parte de la capa de Infraestructura, y su única responsabilidad
// es conectarse a la base de datos MySQL y recuperar información de propiedades inmobiliarias.
//
// Esta clase usa Dapper como micro ORM para la ejecución de consultas SQL con alto rendimiento,
// lo que permite trabajar directamente con SQL pero con mapeo automático de resultados a objetos DTO.
//
// Principios SOLID aplicados:
// ------------------------------------------------------------------------------------------------------------
// S - Single Responsibility (Responsabilidad Única):
//     Esta clase sólo se encarga de ejecutar una consulta SQL y retornar resultados relacionados con propiedades.
//
// O - Open/Closed (Abierta/Cerrada):
//     El comportamiento puede extenderse o modificarse sin alterar esta clase (por ejemplo, usando estrategias
//     o envolviendo los filtros con un builder externo).
//
// L - Liskov Substitution (Sustitución de Liskov):
//     Puede ser sustituida por cualquier clase que implemente IPropertyRepository sin afectar al sistema.
//
// I - Interface Segregation (Segregación de Interfaces):
//     Implementa sólo una interfaz específica para propiedades, no se ve obligada a implementar métodos innecesarios.
//
// D - Dependency Inversion (Inversión de Dependencias):
//     Es inyectada desde la capa superior (Application) a través de la abstracción IPropertyRepository.
//
// Arquitectura Limpia:
// ------------------------------------------------------------------------------------------------------------
// - Esta clase depende exclusivamente de la infraestructura (MySQL, Dapper).
// - Implementa un "puerto" definido en la capa de dominio.
// - Está completamente desacoplada de la lógica de negocio y la capa de presentación.
// - Se inyecta por medio del contenedor de dependencias en tiempo de ejecución.
//
// ------------------------------------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using MyRealEstateApi.Domain.DTOs;
using MyRealEstateApi.Domain.Interfaces;
using MySql.Data.MySqlClient;

namespace MyRealEstateApi.Infrastructure.Repositories
{
    /// <summary>
    /// Repositorio concreto que implementa IPropertyRepository.
    /// Utiliza Dapper para ejecutar consultas SQL contra una base de datos MySQL.
    /// </summary>
    public class PropertyRepository : IPropertyRepository
    {
        private readonly string _connectionString;

        /// <summary>
        /// Constructor que recibe la cadena de conexión a la base de datos.
        /// </summary>
        /// <param name="connectionString">Cadena de conexión a MySQL, obtenida desde la configuración.</param>
        public PropertyRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// Recupera propiedades desde la base de datos utilizando filtros opcionales.
        /// La consulta está optimizada para traer solo una imagen habilitada por propiedad (si existe).
        /// </summary>
        /// <param name="name">Nombre de la propiedad (opcional).</param>
        /// <param name="address">Dirección de la propiedad (opcional).</param>
        /// <param name="priceMin">Precio mínimo (opcional).</param>
        /// <param name="priceMax">Precio máximo (opcional).</param>
        /// <returns>Una colección de objetos PropertyDto que cumplen con los filtros especificados.</returns>
        public async Task<IEnumerable<PropertyDto>> GetPropertiesAsync(string name, string address, decimal? priceMin, decimal? priceMax)
        {
            // Abre la conexión a la base de datos utilizando la cadena provista
            using IDbConnection db = new MySqlConnection(_connectionString);

            // Consulta SQL con filtros dinámicos y LEFT JOIN para incluir una imagen (si existe)
            string sql = @"
                SELECT 
                    p.owner_id AS IdOwner,
                    p.name AS Name,
                    p.address AS AddressProperty,
                    p.price AS PriceProperty,
                    pi.file AS ImageFile
                FROM 
                    properties p
                LEFT JOIN 
                    property_images pi ON pi.property_id = p.id AND pi.enabled = TRUE
                    AND (pi.id = (
                        SELECT MIN(pi2.id) 
                        FROM property_images pi2 
                        WHERE pi2.property_id = p.id AND pi2.enabled = TRUE
                    ) OR pi.id IS NULL)
                WHERE
                    (@Name IS NULL OR p.name LIKE CONCAT('%', @Name, '%'))
                    AND (@Address IS NULL OR p.address LIKE CONCAT('%', @Address, '%'))
                    AND (@PriceMin IS NULL OR p.price >= @PriceMin)
                    AND (@PriceMax IS NULL OR p.price <= @PriceMax)
                ORDER BY p.name
                LIMIT 100;";

            // Ejecuta la consulta y mapea los resultados a una colección de DTOs
            var result = await db.QueryAsync<PropertyDto>(sql, new
            {
                Name = name,
                Address = address,
                PriceMin = priceMin,
                PriceMax = priceMax
            });

            return result;
        }
    }
}
