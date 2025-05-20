// ------------------------------------------------------------------------------------------------------------
// Archivo: PropertyDto.cs
// Proyecto: MyRealEstateApi
// Capa: Dominio > DTOs (Data Transfer Objects)
//
// Descripción:
// Esta clase define un DTO (Objeto de Transferencia de Datos) para encapsular y transportar información
// de una propiedad inmobiliaria entre distintas capas de la aplicación, particularmente entre la capa de 
// aplicación y la capa de presentación (API).
//
// El DTO no contiene lógica de negocio, validaciones ni reglas de persistencia, lo cual favorece el 
// principio de Responsabilidad Única (SRP del acrónimo SOLID), permitiendo mantener la clase enfocada 
// exclusivamente en representar una estructura de datos.
//
// Aplicación de principios SOLID:
// ------------------------------------------------------------------------------------------------------------
// S - Single Responsibility (Responsabilidad Única):
//     Esta clase tiene una única responsabilidad: representar los datos de una propiedad inmobiliaria.
//
// O - Open/Closed (Abierta/Cerrada):
//     Puede extenderse fácilmente (por ejemplo, agregando nuevos campos) sin modificar el comportamiento
//     existente ya que no tiene lógica embebida.
//
// L - Liskov Substitution (Sustitución de Liskov):
//     Es compatible con el uso en interfaces que esperen objetos de tipo DTO sin alterar funcionalidad.
//
// I - Interface Segregation (Segregación de Interfaces):
//     No aplica directamente aquí, pero en general, los DTO no implementan múltiples interfaces innecesarias.
//
// D - Dependency Inversion (Inversión de Dependencias):
//     Las capas superiores (como la API) dependen de esta clase del dominio, en lugar de depender de 
//     entidades específicas o de la infraestructura.
//
// Esta clase facilita el cumplimiento de los principios de la Arquitectura Limpia al promover:
// - Independencia de frameworks
// - Independencia de UI (no depende de tecnologías web)
// - Independencia de la base de datos
// - Separación entre dominio y lógica de aplicación/presentación
// ------------------------------------------------------------------------------------------------------------

namespace MyRealEstateApi.Domain.DTOs
{
    /// <summary>
    /// Objeto de transferencia de datos (DTO) que representa los datos de una propiedad inmobiliaria.
    /// Esta clase es utilizada para transportar datos entre capas sin exponer directamente la entidad de dominio.
    /// </summary>
    public class PropertyDto
    {
        /// <summary>
        /// Identificador único del propietario de la propiedad.
        /// Por ejemplo, puede corresponder a un UUID o a una cédula del dueño.
        /// Este dato permite asociar la propiedad a su propietario.
        /// </summary>
        public string IdOwner { get; set; }

        /// <summary>
        /// Nombre o título de la propiedad.
        /// Suele ser una descripción breve como "Apartamento en el centro" o "Casa campestre".
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dirección física de la propiedad inmobiliaria.
        /// Este campo contiene la ubicación exacta o aproximada para identificar el inmueble.
        /// </summary>
        public string AddressProperty { get; set; }

        /// <summary>
        /// Valor o precio monetario de la propiedad.
        /// Utiliza tipo decimal para mayor precisión en operaciones financieras.
        /// </summary>
        public decimal PriceProperty { get; set; }

        /// <summary>
        /// Ruta, nombre de archivo o URL de la imagen asociada a la propiedad.
        /// Este campo puede ser usado para mostrar una foto del inmueble en interfaces de usuario.
        /// </summary>
        public string ImageFile { get; set; }
    }
}
