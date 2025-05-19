// --------------------------------------------------------------
// Programa principal de la API MyRealEstateApi
// --------------------------------------------------------------
// Este archivo configura y lanza la aplicación ASP.NET Core.
// Se aplica el principio de inversión de dependencias (D de SOLID)
// al registrar servicios e interfaces que cumplen la arquitectura limpia:
// - Separación por capas (Application, Domain, Infrastructure).
// - Bajo acoplamiento y alta cohesión.
// --------------------------------------------------------------

// Importación de la capa de Aplicación: contiene los servicios de negocio (casos de uso)
using MyRealEstateApi.Application.Services;

// Importación de la capa de Dominio: define las interfaces (contratos) y entidades del negocio
using MyRealEstateApi.Domain.Interfaces;

// Importación de la capa de Infraestructura: contiene implementaciones concretas como repositorios
using MyRealEstateApi.Infrastructure.Repositories;

// Crea el objeto builder para configurar servicios y middleware
var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------
// Configuración de la cadena de conexión a la base de datos
// -----------------------------------------------
// Se obtiene desde el archivo de configuración appsettings.json.
// Esto permite mantener la configuración separada del código,
// siguiendo el principio de Responsabilidad Única (S de SOLID).
string connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// -----------------------------------------------
// Inyección de dependencias (IoC - Inversión de control)
// -----------------------------------------------
// Esta sección aplica el principio de inversión de dependencias (D de SOLID).
// El servicio (caso de uso) depende de una abstracción (IPropertyRepository),
// y no de una implementación concreta. La implementación concreta es inyectada aquí.

// Se registra la implementación PropertyRepository para la interfaz IPropertyRepository.
// Se pasa la cadena de conexión al constructor usando una expresión lambda.
// Esto respeta el principio de Abierto/Cerrado (O de SOLID): podemos cambiar la implementación
// del repositorio sin modificar el código que lo consume.
builder.Services.AddScoped<IPropertyRepository>(_ => new PropertyRepository(connectionString));

// Se registra el servicio PropertyService como Scoped (una instancia por solicitud).
// Este servicio representa un caso de uso del sistema (Aplicación) y orquesta la lógica del dominio.
builder.Services.AddScoped<PropertyService>();

// -----------------------------------------------
// Configuración de controladores MVC
// -----------------------------------------------
// Los controladores permiten exponer la lógica como servicios HTTP REST.
// Aquí se activa el soporte para atributos como [ApiController], [HttpGet], etc.
builder.Services.AddControllers();

// -----------------------------------------------
// Documentación automática con Swagger (OpenAPI)
// -----------------------------------------------
// Esto permite explorar y probar los endpoints desde un navegador web.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------------------------
// Construcción del objeto 'app' a partir de la configuración
// -----------------------------------------------
var app = builder.Build();

// -----------------------------------------------
// Configuración del middleware del pipeline HTTP
// -----------------------------------------------

// Si el entorno es Desarrollo, se habilita Swagger para depurar y documentar.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de autorización:
// Si en un futuro se implementa seguridad/autenticación (JWT, OAuth, etc.),
// aquí debe llamarse primero a UseAuthentication() antes de UseAuthorization().
app.UseAuthorization();

// Mapeo de los controladores:
// Esto conecta las rutas HTTP con los controladores definidos en la aplicación.
app.MapControllers();

// -----------------------------------------------
// Ejecución de la aplicación (punto de entrada)
// -----------------------------------------------
// Aquí inicia el servidor web y se mantiene en escucha para recibir solicitudes HTTP.
app.Run();
