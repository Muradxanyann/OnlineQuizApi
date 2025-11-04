using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(5)]
public class CreateQuizAccessLog : Migration
{
    public override void Up()
    {
        Create.Table("QuizAccessLog")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt32().NotNullable().ForeignKey("Users", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
            .WithColumn("quiz_id").AsInt32().NotNullable().ForeignKey("Quizzes", "id").OnDeleteOrUpdate(System.Data.Rule.Cascade)
            .WithColumn("submitted_at").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);

        Create.Index("idx_quizaccesslog_user_quiz")
            .OnTable("QuizAccessLog")
            .OnColumn("user_id").Ascending()
            .OnColumn("quiz_id").Ascending();
    }

    public override void Down()
    {
        Delete.Table("QuizAccessLog");
    }
}