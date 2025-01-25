using FluentMigrator;

namespace UserService.Infrastructure.Persistence.Migrations;

[Migration(20250121_1)]
public class CreateUsersTable : Migration
{
    public override void Up()
    {
        Execute.Sql(@"
            CREATE TYPE sex AS ENUM ('unspecified', 'male', 'female');
        ");

        Execute.Sql(@"
            CREATE TABLE users (
                user_id BIGINT PRIMARY KEY GENERATED BY DEFAULT AS IDENTITY,
                first_name VARCHAR(255) NOT NULL,
                last_name VARCHAR(255) NOT NULL,
                email VARCHAR(255) NOT NULL UNIQUE,
                password VARCHAR(255) NOT NULL,
                birthdate DATE NOT NULL,
                sex sex NOT NULL,
                tel VARCHAR(15),
                created_at TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP NOT NULL
            );
        ");
    }

    public override void Down()
    {
        Execute.Sql(@"
            DROP TABLE users;
            DROP TYPE user_sex;
        ");
    }
}
