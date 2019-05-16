using System;
using System.Collections.Generic;
using System.Linq;

namespace SQLite.Builder
{
    public class TableBuilder
    {
        private readonly Table _table;

        public TableBuilder(string tableName)
        {
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException($"{nameof(tableName)} cannot be empty.");

            _table = new Table();
            _table.Name = tableName;
            _table.Columns = new List<Column>();
        }

        public static TableBuilder Create(string tableName)
        {
            return new TableBuilder(tableName);
        }

        public TableBuilder WithExistsCheck()
        {
            _table.ExistsCheck = true;
            return this;
        }

        public TableBuilder WithoutRowId()
        {
            _table.WithoutRowId = true;
            return this;
        }

        private ColumnBuilder PrepareColumn(string columnName, SQLiteDbType type)
        {
            if (columnName == null)
                throw new ArgumentNullException(nameof(columnName));
            if (string.IsNullOrEmpty(columnName))
                throw new InvalidOperationException($"{nameof(columnName)} cannot be empty.");

            if (_table.Columns.Any(c => c.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"A column with name \"{columnName}\" has already been added.");

            var column = new Column();
            column.Name = columnName;
            column.Type = type;

            _table.Columns.Add(column);

            return new ColumnBuilder(this, column);
        }

        public TableBuilder WithColumn(string columnName, SQLiteDbType type)
        {
            var builder = PrepareColumn(columnName, type);

            return builder.BuildColumn();
        }

        public TableBuilder WithColumn(string columnName, SQLiteDbType type, Action<ColumnBuilder> config)
        {
            if (columnName == null)
                throw new ArgumentNullException(nameof(columnName));
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var builder = PrepareColumn(columnName, type);
            config(builder);

            return builder.BuildColumn();
        }

        public TableBuilder WithUniqueIndex(Action<UniqueIndexBuilder> config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var builder = new UniqueIndexBuilder(this, _table);

            config(builder);

            return builder.Build();
        }

        public TableBuilder WithPrimaryKey(Action<PrimaryKeyBuilder> config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var builder = new PrimaryKeyBuilder(this, _table);

            config(builder);

            return builder.Build();
        }

        public Table Build()
        {
            if (_table.Columns.Count == 0)
                throw new InvalidOperationException("There must be at least one column.");

            return _table;
        }
    }
}
