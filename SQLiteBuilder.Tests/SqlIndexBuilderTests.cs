using System;
using Xunit;

namespace PureFreak.SQLite.Builder.Tests
{
    public class SqlIndexBuilderTests
    {

        [Fact]
        public void ShouldGenerateValidSqlForIndex()
        {
            var index = IndexBuilder.Create("IX_Category_ParentId")
                .WithTable("Category")
                .WithColumn("ParentId")
                .Build();

            var translator = new SqlGenerator();
            translator.IndentSize = 2;

            var actual = translator.Generate(index);

            var expected =
                "CREATE INDEX [IX_Category_ParentId] ON [Category] (" + Environment.NewLine +
                "  ParentId" + Environment.NewLine +
                ");";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldGenerateValidSqlForMultipleColumnsIndex()
        {
            var index = IndexBuilder.Create("UQ_Account_Name")
                .WithTable("Account")
                .WithColumn("Firstname")
                .WithColumn("Lastname")
                .Build();

            var translator = new SqlGenerator();
            translator.IndentSize = 2;

            var actual = translator.Generate(index);

            var expected =
                "CREATE INDEX [UQ_Account_Name] ON [Account] (" + Environment.NewLine +
                "  Firstname," + Environment.NewLine +
                "  Lastname" + Environment.NewLine +
                ");";

            Assert.Equal(expected, actual);
        }

    }
}
