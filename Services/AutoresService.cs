using Dapper;
using Microsoft.Data.Sqlite;
using Jason_P1_P4.Models;

namespace Jason_P1_P4.Services;

public class AutoresService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("SqliteConnection")!;

    private SqliteConnection CreateConnection() => new(_connectionString);

    public async Task InitializeAsync()
    {
        const string consulta = """
            CREATE TABLE IF NOT EXISTS Autores(
                IdAutor INTEGER PRIMARY KEY AUTOINCREMENT,
                Nombres TEXT NOT NULL,
                Nacionalidad TEXT NOT NULL,
                FechaNacimiento TEXT NOT NULL,
                Sueldo INTEGER NOT NULL
            );
            """;
        using var connection = CreateConnection();
        await connection.ExecuteAsync(consulta);
    }

    public async Task<IEnumerable<Datos>> GetListAsync()
    {
        const string consulta = """
            SELECT IdAutor, Nombres, Nacionalidad, FechaNacimiento, Sueldo
            FROM Autores
            ORDER BY IdAutor;
            """;
        using var connection = CreateConnection();
        return await connection.QueryAsync<Datos>(consulta);
    }

    public async Task<Datos?> GetByIdAsync(int id)
    {
        const string consulta = """
            SELECT IdAutor, Nombres, Nacionalidad, FechaNacimiento, Sueldo
            FROM Autores
            WHERE IdAutor = @IdAutor;
            """;
        using var connection = CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Datos>(consulta, new { IdAutor = id });
    }
    public async Task<int> SaveAsync(Datos autor)
    {
        const string consulta = """
            INSERT INTO Autores (Nombres, Nacionalidad, FechaNacimiento, Sueldo)
            VALUES (@Nombres, @Nacionalidad, @FechaNacimiento, @Sueldo);
            SELECT last_insert_rowid();
            """;
        using var connection = CreateConnection();
        return await connection.ExecuteScalarAsync<int>(consulta, autor);
    }

    public async Task<bool> UpdateAsync(Datos autor)
    {
        const string consulta = """
            UPDATE Autores
            SET Nombres = @Nombres,
                Nacionalidad = @Nacionalidad,
                FechaNacimiento = @FechaNacimiento,
                Sueldo = @Sueldo
            WHERE IdAutor = @IdAutor;
            """;
        using var connection = CreateConnection();
        return await connection.ExecuteAsync(consulta, autor) > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string consulta = "DELETE FROM Autores WHERE IdAutor = @IdAutor;";
        using var connection = CreateConnection();
        return await connection.ExecuteAsync(consulta, new { IdAutor = id }) > 0;
    }
}