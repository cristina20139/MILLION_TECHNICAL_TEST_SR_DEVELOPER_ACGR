// --------------------------------------------------------------
// Programa principal de la API MyRealEstateApi
// --------------------------------------------------------------
// Este archivo configura y lanza la aplicaci�n ASP.NET Core.
// Se aplica el principio de inversi�n de dependencias (D de SOLID)
// al registrar servicios e interfaces que cumplen la arquitectura limpia:
// - Separaci�n por capas (Application, Domain, Infrastructure).
// - Bajo acoplamiento y alta cohesi�n.
// --------------------------------------------------------------

// Importaci�n de la capa de Aplicaci�n: contiene los servicios de negocio (casos de uso)
using MyRealEstateApi.Application.Services;

// Importaci�n de la capa de Dominio: define las interfaces (contratos) y entidades del negocio
using MyRealEstateApi.Domain.Interfaces;

// Importaci�n de la capa de Infraestructura: contiene implementaciones concretas como repositorios
using MyRealEstateApi.Infrastructure.Repositories;

// Crea el objeto builder para configurar servicios y middleware
var builder = WebApplication.CreateBuilder(args);

// -----------------------------------------------
// Configuraci�n de la cadena de conexi�n a la base de datos
// -----------------------------------------------
// Se obtiene desde el archivo de configuraci�n appsettings.json.
// Esto permite mantener la configuraci�n separada del c�digo,
// siguiendo el principio de Responsabilidad �nica (S de SOLID).
string connectionString = builder.Configuration.GetConnectionString("MySqlConnection");

// -----------------------------------------------
// Inyecci�n de dependencias (IoC - Inversi�n de control)
// -----------------------------------------------
// Esta secci�n aplica el principio de inversi�n de dependencias (D de SOLID).
// El servicio (caso de uso) depende de una abstracci�n (IPropertyRepository),
// y no de una implementaci�n concreta. La implementaci�n concreta es inyectada aqu�.

// Se registra la implementaci�n PropertyRepository para la interfaz IPropertyRepository.
// Se pasa la cadena de conexi�n al constructor usando una expresi�n lambda.
// Esto respeta el principio de Abierto/Cerrado (O de SOLID): podemos cambiar la implementaci�n
// del repositorio sin modificar el c�digo que lo consume.
builder.Services.AddScoped<IPropertyRepository>(_ => new PropertyRepository(connectionString));

// Se registra el servicio PropertyService como Scoped (una instancia por solicitud).
// Este servicio representa un caso de uso del sistema (Aplicaci�n) y orquesta la l�gica del dominio.
builder.Services.AddScoped<PropertyService>();

// -----------------------------------------------
// Configuraci�n de controladores MVC
// -----------------------------------------------
// Los controladores permiten exponer la l�gica como servicios HTTP REST.
// Aqu� se activa el soporte para atributos como [ApiController], [HttpGet], etc.
builder.Services.AddControllers();

// -----------------------------------------------
// Documentaci�n autom�tica con Swagger (OpenAPI)
// -----------------------------------------------
// Esto permite explorar y probar los endpoints desde un navegador web.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------------------------
// Construcci�n del objeto 'app' a partir de la configuraci�n
// -----------------------------------------------
var app = builder.Build();

// -----------------------------------------------
// Configuraci�n del middleware del pipeline HTTP
// -----------------------------------------------

// Si el entorno es Desarrollo, se habilita Swagger para depurar y documentar.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware de autorizaci�n:
// Si en un futuro se implementa seguridad/autenticaci�n (JWT, OAuth, etc.),
// aqu� debe llamarse primero a UseAuthentication() antes de UseAuthorization().
app.UseAuthorization();

// Mapeo de los controladores:
// Esto conecta las rutas HTTP con los controladores definidos en la aplicaci�n.
app.MapControllers();

// -----------------------------------------------
// Ejecuci�n de la aplicaci�n (punto de entrada)
// -----------------------------------------------
// Aqu� inicia el servidor web y se mantiene en escucha para recibir solicitudes HTTP.
app.Run();
