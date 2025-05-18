using System;
using System.Text;
using MySql.Data.MySqlClient;

/**
 * 
 * @author : Aura Cristina Garzon Rodriguez
 * @since : 18 Mayo 2025 2:33 PM
 * 
 */


namespace DataSeeder
{
    /*
     * Documentación Técnica - Principios SOLID aplicados en este código
     *
     * S - Single Responsibility Principle (SRP):
     * Cada método tiene una única responsabilidad clara:
     *   - ClearTables: eliminar datos existentes.
     *   - InsertOwner: insertar un dueño.
     *   - InsertProperty: insertar una propiedad.
     *   - InsertPropertyImage: insertar una imagen de propiedad.
     *   - InsertPropertyTrace: insertar una traza de propiedad.
     *   - GenerateId: generar un ID único.
     *   - Main: orquesta la secuencia de operaciones, pero no mezcla la lógica interna de cada acción.
     *
     * O - Open/Closed Principle (OCP):
     * El código está abierto a extensión (puedes agregar nuevas funciones para otras tablas o nuevos tipos de inserciones),
     * pero cerrado a modificación (no es necesario modificar la lógica existente para añadir nuevas funcionalidades).
     *
     * L - Liskov Substitution Principle (LSP):
     * No aplica fuertemente en este código dado que no hay herencia ni subtipos,
     * pero las funciones mantienen contratos claros (parámetros y efectos) que podrían ser sustituidos sin romper la lógica.
     *
     * I - Interface Segregation Principle (ISP):
     * No se usan interfaces explícitamente, pero cada método ofrece una función específica y no obliga a los usuarios a depender de métodos que no usan.
     *
     * D - Dependency Inversion Principle (DIP):
     * El código depende de abstracciones mínimas (la conexión MySQL se pasa como parámetro),
     * lo que facilita en un futuro usar interfaces o mocks para pruebas sin modificar la lógica interna.
     *
     * Comentarios detallados línea a línea se incluyen para explicar el comportamiento específico del código.
     */

    class Program
    {
        static Random random = new Random(); // Generador de números aleatorios compartido

        // Cadena de conexión para MySQL (debe ajustarse según entorno)
        static string connectionString = "server=localhost;user=root;password=root;database=million_properties;";

        static void Main(string[] args)
        {
            // Cantidad total de dueños a generar
            int totalOwners = 10_000_000;
            // Propiedades por dueño
            int propertiesPerOwner = 5;
            // Imágenes por propiedad
            int imagesPerProperty = 3;
            // Trazas por propiedad
            int tracesPerProperty = 2;

            // Contadores de inserciones para feedback
            int totalPropertiesInserted = 0;
            int totalImagesInserted = 0;
            int totalTracesInserted = 0;

            Console.WriteLine("Iniciando generación de datos...");

            // Crear conexión con la base de datos usando MySQL
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open(); // Abrir conexión

                ClearTables(connection);  // Eliminar datos existentes para evitar duplicados

                // Generar dueños
                for (int i = 1; i <= totalOwners; i++)
                {
                    string ownerId = GenerateId(); // Generar ID único para dueño
                    InsertOwner(connection, ownerId); // Insertar dueño

                    // Generar propiedades para cada dueño
                    for (int j = 1; j <= propertiesPerOwner; j++)
                    {
                        string propertyId = GenerateId(); // ID único para propiedad
                        InsertProperty(connection, propertyId, ownerId); // Insertar propiedad
                        totalPropertiesInserted++; // Incrementar contador

                        // Insertar imágenes para la propiedad
                        for (int k = 1; k <= imagesPerProperty; k++)
                        {
                            InsertPropertyImage(connection, GenerateId(), propertyId);
                            totalImagesInserted++; // Contar imágenes insertadas
                        }

                        // Insertar trazas para la propiedad
                        for (int t = 1; t <= tracesPerProperty; t++)
                        {
                            InsertPropertyTrace(connection, GenerateId(), propertyId);
                            totalTracesInserted++; // Contar trazas insertadas
                        }

                        // Mostrar progreso cada 1000 propiedades insertadas
                        if (totalPropertiesInserted % 1000 == 0)
                            Console.WriteLine($"Propiedades insertadas: {totalPropertiesInserted}");
                        if (totalImagesInserted % 1000 == 0)
                            Console.WriteLine($"Imágenes insertadas: {totalImagesInserted}");
                        if (totalTracesInserted % 1000 == 0)
                            Console.WriteLine($"Trazas insertadas: {totalTracesInserted}");
                    }

                    // Mostrar progreso cada 1000 dueños insertados
                    if (i % 1000 == 0)
                        Console.WriteLine($"Dueños insertados: {i}");
                }

                connection.Close(); // Cerrar conexión
            }

            Console.WriteLine("Finalizado.");
        }

        // Método para eliminar datos existentes de las tablas en orden para respetar claves foráneas
        static void ClearTables(MySqlConnection conn)
        {
            Console.WriteLine("Eliminando datos existentes...");

            // Consultas DELETE en orden inverso por dependencia
            string[] deleteQueries = new string[]
            {
                "DELETE FROM property_traces;",
                "DELETE FROM property_images;",
                "DELETE FROM properties;",
                "DELETE FROM owners;"
            };

            // Ejecutar cada DELETE y mostrar filas afectadas
            foreach (var query in deleteQueries)
            {
                using var cmd = new MySqlCommand(query, conn);
                int affectedRows = cmd.ExecuteNonQuery();
                Console.WriteLine($"Ejecutado: {query} Filas afectadas: {affectedRows}");
            }

            Console.WriteLine("Datos existentes eliminados.");
        }

        // Genera un ID hexadecimal de 24 caracteres aleatorios (simula ObjectId)
        static string GenerateId()
        {
            const string chars = "abcdef0123456789"; // Caracteres hexadecimales
            var sb = new StringBuilder();
            for (int i = 0; i < 24; i++)
                sb.Append(chars[random.Next(chars.Length)]); // Añade un caracter aleatorio
            return sb.ToString();
        }

        // Inserta un nuevo dueño en la tabla owners con datos generados aleatoriamente
        static void InsertOwner(MySqlConnection conn, string id)
        {
            string query = @"INSERT INTO owners (id, name, address, photo, birthday)
                             VALUES (@id, @name, @address, @photo, @birthday)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id); // ID generado
            cmd.Parameters.AddWithValue("@name", "Owner " + random.Next(1, 1000000)); // Nombre aleatorio
            cmd.Parameters.AddWithValue("@address", "Calle " + random.Next(1, 9999)); // Dirección aleatoria
            cmd.Parameters.AddWithValue("@photo", $"https://picsum.photos/200?random={random.Next(1, 1000)}"); // URL foto aleatoria
            cmd.Parameters.AddWithValue("@birthday", DateTime.Now.AddYears(-random.Next(20, 70)).ToString("yyyy-MM-dd")); // Fecha cumpleaños aleatoria
            cmd.ExecuteNonQuery(); // Ejecutar inserción
        }

        // Inserta una propiedad vinculada a un dueño específico
        static void InsertProperty(MySqlConnection conn, string id, string ownerId)
        {
            string query = @"INSERT INTO properties (id, name, address, price, codeInternal, year, owner_id)
                             VALUES (@id, @name, @address, @price, @codeInternal, @year, @owner_id)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id); // ID propiedad
            cmd.Parameters.AddWithValue("@name", "Propiedad " + random.Next(1, 1000000)); // Nombre aleatorio
            cmd.Parameters.AddWithValue("@address", "Avenida " + random.Next(1, 9999)); // Dirección aleatoria
            cmd.Parameters.AddWithValue("@price", random.Next(10000, 500000)); // Precio aleatorio
            cmd.Parameters.AddWithValue("@codeInternal", "INT-" + Guid.NewGuid().ToString("N").Substring(0, 10)); // Código interno único
            cmd.Parameters.AddWithValue("@year", random.Next(1950, 2024)); // Año aleatorio
            cmd.Parameters.AddWithValue("@owner_id", ownerId); // Dueño asociado
            cmd.ExecuteNonQuery(); // Ejecutar inserción
        }

        // Inserta una imagen para una propiedad específica
        static void InsertPropertyImage(MySqlConnection conn, string id, string propertyId)
        {
            string query = @"INSERT INTO property_images (id, property_id, file, enabled)
                             VALUES (@id, @property_id, @file, @enabled)";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@id", id); // ID imagen
            cmd.Parameters.AddWithValue("@property_id", propertyId); // Propiedad asociada
            cmd.Parameters.AddWithValue("@file", $"https://picsum.photos/300?random={random.Next(1, 1000)}"); // URL archivo imagen
            cmd.Parameters.AddWithValue("@enabled", random.Next(0, 2) == 1); // Booleano habilitado/deshabilitado
            cmd.ExecuteNonQuery(); // Ejecutar inserción
        }

        // Inserta una traza de venta para una propiedad específica
        static void InsertPropertyTrace(MySqlConnection conn, string id, string propertyId)
        {
            string query = @"INSERT INTO property_traces (id, property_id, date_sale, name, value, tax)
                             VALUES (@id, @property_id, @date_sale, @name, @value, @tax)";
            using var cmd = new MySqlCommand(query, conn);
            decimal value = random.Next(20000, 100000); // Valor venta aleatorio
            decimal tax = value * 0.19m; // IVA calculado al 19%
            cmd.Parameters.AddWithValue("@id", id); // ID traza
            cmd.Parameters.AddWithValue("@property_id", propertyId); // Propiedad asociada
            cmd.Parameters.AddWithValue("@date_sale", DateTime.Now.AddDays(-random.Next(1, 1000)).ToString("yyyy-MM-dd")); // Fecha venta aleatoria
            cmd.Parameters.AddWithValue("@name", "Venta " + random.Next(1, 10000)); // Nombre venta aleatorio
            cmd.Parameters.AddWithValue("@value", value); // Valor venta
            cmd.Parameters.AddWithValue("@tax", tax); // Impuesto venta
            cmd.ExecuteNonQuery(); // Ejecutar inserción
        }
    }
}
