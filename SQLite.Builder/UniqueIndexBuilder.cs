using System;

namespace SQLite.Builder
{
    public class UniqueIndexBuilder
    {
        private readonly TableBuilder _tableBuilder;
        private readonly Table _table;

        public UniqueIndexBuilder(TableBuilder tableBuilder, Table table)
        {
            if (tableBuilder == null)
                throw new ArgumentNullException(nameof(tableBuilder));
            if (table == null)
                throw new ArgumentNullException(nameof(table));

            _tableBuilder = tableBuilder;
            _table = table;
        }

        public UniqueIndexBuilder IncludeColumn(string columnName)
        {
            if (columnName == null)
                throw new ArgumentNullException(nameof(columnName));

            if (_table.UniqueIndex == null)
                _table.UniqueIndex = new UniqueIndex();

            _table.UniqueIndex.Columns.Add(columnName);

            return this;
        }

        public UniqueIndexBuilder IncludeColumns(params string[] columnNames)
        {
            if (columnNames == null)
                throw new ArgumentNullException(nameof(columnNames));

            foreach (var columnName in columnNames)
                IncludeColumn(columnName);

            return this;
        }

        public TableBuilder Build()
        {
            if (_table.UniqueIndex == null || _table.UniqueIndex.Columns.Count == 0)
                throw new InvalidOperationException("A unique index must have at least one column.");

            return _tableBuilder;
        }
    }
}
