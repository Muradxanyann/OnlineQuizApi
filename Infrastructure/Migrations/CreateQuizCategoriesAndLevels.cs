using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(2)]
public class CreateQuizCategoriesAndLevels : Migration
{
    public override void Up()
    {
        Create.Table("QuizCategory")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(100).NotNullable().Unique();

        Create.Table("QuizLevels")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(50).NotNullable().Unique();
    }

    public override void Down()
    {
        Delete.Table("QuizLevels");
        Delete.Table("QuizCategory");
    }
}