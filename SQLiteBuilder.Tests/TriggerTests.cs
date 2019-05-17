using System;
using Xunit;

namespace PureFreak.SQLite.Builder.Tests
{
    public class TriggerTests
    {

        [Fact]
        public void ShouldGenerateValidTrigger()
        {
            var trigger = TriggerBuilder.Create("trigger_history")
                .WithExistsCheck()
                .Before()
                .Update()
                .OnTable("Account")
                .WithCondition("old.ContentHash <> new.ContentHash")
                .WithScript("UPDATE PageAttachments SET LastUpdateDateUtc = DATETIME('now'), Version = Version + 1 WHERE Id = old.Id")
                .Build();

            Assert.Equal("trigger_history", trigger.Name);
            Assert.True(trigger.ExistsCheck);
            Assert.Equal(TriggerEventType.Before, trigger.EventType);
            Assert.Equal(TriggerActionType.Update, trigger.ActionType);
            Assert.Equal("Account", trigger.TableName);
            Assert.Equal("old.ContentHash <> new.ContentHash", trigger.Condition);
            Assert.Equal("UPDATE PageAttachments SET LastUpdateDateUtc = DATETIME('now'), Version = Version + 1 WHERE Id = old.Id;" + Environment.NewLine,
                trigger.Script.ToString());
        }

        [Fact]
        public void ShouldGenerateValidSqlForTrigger()
        {
            var trigger = TriggerBuilder.Create("trigger_account")
               .WithExistsCheck()
               .Before()
               .Update()
               .OnTable("Account")
               .WithCondition("old.Username <> new.Username")
               .WithScript("UPDATE Account SET LastUpdateDateUtc = DATETIME('now') WHERE Id = old.Id")
               .WithScript("INSERT INTO AccountHistor (AccountId) VALUES (old.Id)")
               .Build();

            var generator = new SqlGenerator();
            generator.IndentSize = 2;

            var actual = generator.Generate(trigger);

            var expected =
                "CREATE TRIGGER \"trigger_account\" BEFORE UPDATE ON \"Account\" " + Environment.NewLine +
                "WHEN old.Username <> new.Username " + Environment.NewLine +
                "BEGIN " + Environment.NewLine +
                "  UPDATE Account SET LastUpdateDateUtc = DATETIME('now') WHERE Id = old.Id;" + Environment.NewLine +
                "  INSERT INTO AccountHistor (AccountId) VALUES (old.Id);" + Environment.NewLine +
                "END;";

            Assert.Equal(expected, actual);
        }

    }
}
