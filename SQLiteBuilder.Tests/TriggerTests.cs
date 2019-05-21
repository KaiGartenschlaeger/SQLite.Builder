using System;
using Xunit;

namespace PureFreak.SQLite.Builder.Tests
{
    public class TriggerTests
    {

        [Fact]
        public void ShouldGenerateValidTriggerEntity()
        {
            var trigger = TriggerBuilder.Create("trigger_history")
                .WithExistsCheck()
                .Before(TriggerActionType.Update)
                .OnTable("Account")
                .WithCondition("old.ContentHash <> new.ContentHash")
                .WithScript("UPDATE PageAttachments SET LastUpdateDateUtc = DATETIME('now'), Version = Version + 1 WHERE Id = old.Id")
                .Build();

            Assert.Equal("trigger_history", trigger.Name);
            Assert.True(trigger.ExistsCheck);
            Assert.Equal(SQLiteTriggerEventType.Before, trigger.EventType);
            Assert.Equal(TriggerActionType.Update, trigger.ActionType);
            Assert.Equal("Account", trigger.TableName);
            Assert.Equal("old.ContentHash <> new.ContentHash", trigger.Condition);
            Assert.Equal("UPDATE PageAttachments SET LastUpdateDateUtc = DATETIME('now'), Version = Version + 1 WHERE Id = old.Id" + Environment.NewLine,
                trigger.Script.ToString());
        }

        [Fact]
        public void ShouldGenerateValidSqlWithSingleScriptLine()
        {
            var trigger = TriggerBuilder.Create("trigger_account")
               .Before(TriggerActionType.Update)
               .OnTable("Account")
               .WithCondition("old.Username <> new.Username")
               .WithScript("UPDATE Account SET LastUpdateDateUtc = DATETIME('now') WHERE Id = old.Id")
               .Build();

            var generator = new SqlGenerator();
            generator.IndentSize = 2;

            var actual = generator.Generate(trigger);

            var expected =
                "CREATE TRIGGER \"trigger_account\" BEFORE UPDATE ON \"Account\" " + Environment.NewLine +
                "WHEN old.Username <> new.Username " + Environment.NewLine +
                "BEGIN " + Environment.NewLine +
                "  UPDATE Account SET LastUpdateDateUtc = DATETIME('now') WHERE Id = old.Id;" + Environment.NewLine +
                "END;";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldGenerateValidSqlWithMultipleScriptLines()
        {
            var trigger = TriggerBuilder.Create("trigger_account")
               .WithExistsCheck()
               .Before(TriggerActionType.Update)
               .OnTable("Account")
               .WithCondition("old.Username <> new.Username")
               .WithScript("UPDATE Account SET LastUpdateDateUtc = DATETIME('now') WHERE Id = old.Id")
               .WithScript("INSERT INTO AccountHistor (AccountId) VALUES (old.Id)")
               .Build();

            var generator = new SqlGenerator();
            generator.IndentSize = 2;

            var actual = generator.Generate(trigger);

            var expected =
                "CREATE TRIGGER IF NOT EXISTS \"trigger_account\" BEFORE UPDATE ON \"Account\" " + Environment.NewLine +
                "WHEN old.Username <> new.Username " + Environment.NewLine +
                "BEGIN " + Environment.NewLine +
                "  UPDATE Account SET LastUpdateDateUtc = DATETIME('now') WHERE Id = old.Id;" + Environment.NewLine +
                "  INSERT INTO AccountHistor (AccountId) VALUES (old.Id);" + Environment.NewLine +
                "END;";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldRemoveWhenInScript()
        {
            var trigger = TriggerBuilder.Create("test")
               .After(TriggerActionType.Update)
               .OnTable("Test")
               .WithCondition("WHEN old.Version <> new.Version")
               .WithScript("UPDATE Test SET version = new.Version")
               .Build();

            var generator = new SqlGenerator();
            generator.IndentSize = 2;

            var actual = generator.Generate(trigger);

            var expected =
                "CREATE TRIGGER \"test\" AFTER UPDATE ON \"Test\" " + Environment.NewLine +
                "WHEN old.Version <> new.Version " + Environment.NewLine +
                "BEGIN " + Environment.NewLine +
                "  UPDATE Test SET version = new.Version;" + Environment.NewLine +
                "END;";

            Assert.Equal(expected, actual);
        }

    }
}
