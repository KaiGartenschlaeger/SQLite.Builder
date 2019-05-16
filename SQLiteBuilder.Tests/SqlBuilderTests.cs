using PureFreak.SQLite.Builder;
using System;
using Xunit;

namespace SQLiteBuilder.Tests
{
    public class SqlBuilderTests
    {
        [Fact]
        public void ShouldCreateValidSqlForSimpleTable()
        {
            var table = TableBuilder.Create("Test")
                .WithColumn("Id", SQLiteDbType.Integer, c => c.PrimaryKey().AutoIncrement())
                .WithColumn("FirstName", SQLiteDbType.Text)
                .WithColumn("LastName", SQLiteDbType.Text)
                .Build();

            var genertator = new SqlGenerator();
            genertator.IndentSize = 2;

            var actual = genertator.Generate(table);

            var expected =
                "CREATE TABLE [Test] (" + Environment.NewLine +
                "  Id INTEGER PRIMARY KEY AUTOINCREMENT," + Environment.NewLine +
                "  FirstName TEXT," + Environment.NewLine +
                "  LastName TEXT" + Environment.NewLine +
                ");";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ShouldCreateValidSqlForSimpleTableWithPrimaryKey()
        {
            var table = TableBuilder.Create("Test")
                .WithColumn("Id", SQLiteDbType.Integer)
                .WithColumn("ZipCode", SQLiteDbType.Text)
                .WithColumn("Town", SQLiteDbType.Text)
                .WithPrimaryKey(k => k.WithColumns("ZipCode", "Town"))
                .Build();

            var genertator = new SqlGenerator();
            genertator.IndentSize = 2;
            var actual = genertator.Generate(table);

            var expected =
                "CREATE TABLE [Test] (" + Environment.NewLine +
                "  Id INTEGER," + Environment.NewLine +
                "  ZipCode TEXT," + Environment.NewLine +
                "  Town TEXT," + Environment.NewLine +
                "  PRIMARY KEY (" + Environment.NewLine +
                "    ZipCode," + Environment.NewLine +
                "    Town" + Environment.NewLine +
                "  )" + Environment.NewLine +
                ");";

            Assert.Equal(expected, actual);
        }
    }
}
