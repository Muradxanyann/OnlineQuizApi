namespace Infrastructure.Migrations;

using FluentMigrator;

[Migration(1)]
public class CreateUsersAndRoles : Migration
{
    public override void Up()
    {
        Create.Table("Users")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable()
            .WithColumn("username").AsString(50).NotNullable().Unique()
            .WithColumn("email").AsString(100).Nullable()
            .WithColumn("phone_number").AsString(30).Nullable()
            .WithColumn("password_hash").AsString().NotNullable()
            .WithColumn("is_active").AsBoolean().WithDefaultValue(true)
            .WithColumn("created_at").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("last_login").AsDateTime().Nullable()
            .WithColumn("refresh_token").AsString().Nullable()
            .WithColumn("refresh_token_expiration").AsDateTime().Nullable();

        Create.Table("Roles")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(50).NotNullable().Unique();

        Create.Table("UserRoles")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt32().NotNullable().ForeignKey("Users", "id")
            .WithColumn("role_id").AsInt32().NotNullable().ForeignKey("Roles", "id");

        Create.UniqueConstraint("UQ_UserRoles_User_Role")
            .OnTable("UserRoles")
            .Columns("user_id", "role_id");
    }

    public override void Down()
    {
        Delete.Table("UserRoles");
        Delete.Table("Roles");
        Delete.Table("Users");
    }
}
