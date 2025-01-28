using Npgsql;
using System.Data.Common;
using UserService.Application.Abstractions.Persistence.Repositories;
using UserService.Application.Models.Users;

namespace UserService.Infrastructure.Persistence.Repositories;

public class UsersRepository : IUsersRepository
{
    private readonly NpgsqlDataSource _dataSource;

    public UsersRepository(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }

    public async Task<long> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        const string sql = """
        INSERT INTO users (first_name, last_name, email, password, birthdate, sex, tel, created_at)
        VALUES (:first_name, :last_name, :email, :password, :birthdate, :sex, :tel, :created_at)
        RETURNING user_id;
        """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using DbCommand command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("first_name", user.FirstName),
                new NpgsqlParameter("last_name", user.LastName),
                new NpgsqlParameter("email", user.Email),
                new NpgsqlParameter("password", user.Password),
                new NpgsqlParameter("birthdate", user.Birthdate),
                new NpgsqlParameter("sex", user.Sex),
                new NpgsqlParameter("tel", (object?)user.Tel ?? DBNull.Value),
                new NpgsqlParameter("created_at", user.CreatedAt),
            },
        };

        object? generatedId = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt64(generatedId);
    }

    public async Task<UserWithoutConfidentialFields> GetUserWithoutConfidentialFieldsByIdAsync(long userId, CancellationToken cancellationToken)
    {
        const string sql = """
        SELECT user_id, first_name, last_name, email, password, birthdate, sex, tel, created_at
        FROM users
        WHERE user_id = :user_id;
        """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using DbCommand command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("user_id", userId),
            },
        };

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return new UserWithoutConfidentialFields(
                reader.GetInt64(reader.GetOrdinal("user_id")),
                reader.GetString(reader.GetOrdinal("first_name")),
                reader.GetString(reader.GetOrdinal("last_name")),
                reader.GetString(reader.GetOrdinal("email")),
                reader.GetDateTime(reader.GetOrdinal("birthdate")),
                reader.GetFieldValue<Sex>(reader.GetOrdinal("sex")),
                reader.IsDBNull(reader.GetOrdinal("tel")) ? null : reader.GetString(reader.GetOrdinal("tel")),
                reader.GetDateTime(reader.GetOrdinal("created_at")));
        }

        throw new Exception($"User with ID {userId} was not found.");
    }

    public async Task<User?> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        const string sql = """
        SELECT user_id, first_name, last_name, email, password, birthdate, sex, tel, created_at
        FROM users
        WHERE email = :email;
        """;

        await using NpgsqlConnection connection = await _dataSource.OpenConnectionAsync(cancellationToken);
        await using DbCommand command = new NpgsqlCommand(sql, connection)
        {
            Parameters =
            {
                new NpgsqlParameter("email", email),
            },
        };

        await using DbDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return new User(
                reader.GetInt64(reader.GetOrdinal("user_id")),
                reader.GetString(reader.GetOrdinal("first_name")),
                reader.GetString(reader.GetOrdinal("last_name")),
                reader.GetString(reader.GetOrdinal("email")),
                reader.GetString(reader.GetOrdinal("password")),
                reader.GetDateTime(reader.GetOrdinal("birthdate")),
                reader.GetFieldValue<Sex>(reader.GetOrdinal("sex")),
                reader.IsDBNull(reader.GetOrdinal("tel")) ? null : reader.GetString(reader.GetOrdinal("tel")),
                reader.GetDateTime(reader.GetOrdinal("created_at")));
        }

        return null;
    }
}