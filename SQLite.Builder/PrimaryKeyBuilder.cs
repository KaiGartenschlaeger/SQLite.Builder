using System;

namespace PureFreak.SQLite.Builder
{
    public class PrimaryKeyBuilder
    {
        private readonly TableBuilder _tableBuilder;
        private readonly Table _table;

        public PrimaryKeyBuilder(TableBuilder tableBuilder, Table table)
        {
            _tableBuilder = tableBuilder;
            _table = table;

            if (_table.PrimaryKey == null)
                _table.PrimaryKey = new PrimaryKey();
        }

        public PrimaryKeyBuilder WithColumn(string column)
        {
            _table.PrimaryKey.Columns.Add(
                new PrimaryKeyColumn
                {
                    Name = column
                });

            return this;
        }

        public PrimaryKeyBuilder WithColumn(string column, SQLiteSorting sorting)
        {
            _table.PrimaryKey.Columns.Add(
                new PrimaryKeyColumn
                {
                    Name = column,
                    Sort = sorting
                });

            return this;
        }

        public PrimaryKeyBuilder WithColumns(params string[] columns)
        {
            if (columns == null)
                throw new ArgumentNullException(nameof(columns));

            foreach (var column in columns)
                WithColumn(column);

            return this;
        }

        public PrimaryKeyBuilder OnConflict(SQLiteConflictType type)
        {
            _table.PrimaryKey.ConflictType = type;

            return this;
        }

        public TableBuilder Build()
        {
            if (_table.PrimaryKey.Columns.Count == 0)
                throw new InvalidOperationException("A primary key must have at least one column assigned.");

            return _tableBuilder;
        }
    }
}
