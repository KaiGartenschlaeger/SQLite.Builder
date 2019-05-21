using System;
using System.Linq;

namespace PureFreak.SQLite.Builder
{
    public class TableBuilder
    {
        private readonly TableEntity _table;

        public TableBuilder(string tableName)
        {
            if (tableName == null)
                throw new ArgumentNullException(nameof(tableName));
            if (string.IsNullOrEmpty(tableName))
                throw new ArgumentException($"{nameof(tableName)} cannot be empty.");

            _table = new TableEntity();
            _table.Name = tableName;
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

            var column = new ColumnEntity();
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

        public TableBuilder WithUniqueIndex(Action<IndexBuilder> config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var builder = new IndexBuilder($"uq_{_table.Name}_{_table.UniqueIndizees.Count}");
            builder.Unique();

            config(builder);

            var index = builder.Build();

            _table.UniqueIndizees.Add(index);

            return this;
        }

        public TableBuilder WithPrimaryKey(Action<PrimaryKeyBuilder> config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            var builder = new PrimaryKeyBuilder(this, _table);

            config(builder);

            return builder.Build();
        }

        public TableEntity Build()
        {
            if (_table.Columns.Count == 0)
                throw new InvalidOperationException("There must be at least one column.");

            return _table;
        }
    }
}
