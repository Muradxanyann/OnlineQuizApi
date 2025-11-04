using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(3)]
public class CreateQuizzesAndQuestions : Migration
{
    public override void Up()
    {
        Create.Table("Quizzes")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString(150).NotNullable()
            .WithColumn("description").AsString(int.MaxValue).Nullable()
            .WithColumn("category_id").AsInt32().Nullable().ForeignKey("QuizCategory", "id")
            .WithColumn("level_id").AsInt32().Nullable().ForeignKey("QuizLevels", "id")
            .WithColumn("user_id").AsInt32().Nullable().ForeignKey("Users", "id")
            .WithColumn("code_to_join").AsString(20).NotNullable().Unique()
            .WithColumn("is_active").AsBoolean().WithDefaultValue(true)
            .WithColumn("is_published").AsBoolean().WithDefaultValue(false)
            .WithColumn("created_at").AsDateTime().WithDefault(SystemMethods.CurrentDateTime)
            .WithColumn("updated_at").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);

        Create.Table("Questions")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("quiz_id").AsInt32().NotNullable().ForeignKey("Quizzes", "id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade)
            .WithColumn("text").AsString(int.MaxValue).NotNullable()
            .WithColumn("duration").AsInt32().WithDefaultValue(30);
        
        Create.Table("Options")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("question_id").AsInt32().NotNullable().ForeignKey("Questions", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
            .WithColumn("text").AsString(int.MaxValue).NotNullable()
            .WithColumn("is_correct").AsBoolean().WithDefaultValue(false);

        Create.Index("idx_questions_quiz_id").OnTable("Questions").OnColumn("quiz_id");
        Create.Index("idx_options_question_id").OnTable("Options").OnColumn("question_id");
    }

    public override void Down()
    {
        Delete.Table("Options");
        Delete.Table("Questions");
        Delete.Table("Quizzes");
    }
}