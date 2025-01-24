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

                // TODO. Check if it automatically maps to db type
                new NpgsqlParameter("sex", user.Sex),

                // TODO. Check if type casting is useful
                new NpgsqlParameter("tel", (object?)user.Tel ?? DBNull.Value),
                new NpgsqlParameter("created_at", user.CreatedAt),
            },
        };

        object? generatedId = await command.ExecuteScalarAsync(cancellationToken);
        return Convert.ToInt64(generatedId);
    }
}