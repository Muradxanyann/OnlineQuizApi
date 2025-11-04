using FluentMigrator;

namespace Infrastructure.Migrations;

[Migration(4)]
public class CreateUserResultsAndAnswers : Migration
{
    public override void Up()
    {
        Create.Table("UserResults")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt32().NotNullable().ForeignKey("Users", "id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade)
            .WithColumn("quiz_id").AsInt32().NotNullable().ForeignKey("Quizzes", "id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade)
            .WithColumn("score").AsInt32().NotNullable()
            .WithColumn("time_spent").AsInt32().WithDefaultValue(0)
            .WithColumn("completed_at").AsDateTime().WithDefault(SystemMethods.CurrentDateTime);

        Create.UniqueConstraint("UQ_UserResults_User_Quiz")
            .OnTable("UserResults").Columns("user_id", "quiz_id");

        Create.Table("UserAnswers")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("user_result_id").AsInt32().NotNullable().ForeignKey("UserResults", "id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade)
            .WithColumn("question_id").AsInt32().NotNullable().ForeignKey("Questions", "id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade)
            .WithColumn("option_id").AsInt32().NotNullable().ForeignKey("Options", "id")
            .OnDeleteOrUpdate(System.Data.Rule.Cascade);
    }

    public override void Down()
    {
        Delete.Table("UserAnswers");
        Delete.Table("UserResults");
    }
}