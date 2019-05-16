using System;
using System.Linq;

namespace PureFreak.SQLite.Builder
{
    public class IndexBuilder
    {
        private readonly Index _index;

        public IndexBuilder(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Cannot be null or empty.", nameof(name));

            _index = new Index();
            _index.Name = name;
        }

        public static IndexBuilder Create(string name)
        {
            return new IndexBuilder(name);
        }

        public IndexBuilder WithExistsCheck()
        {
            _index.ExistsCheck = true;

            return this;
        }

        public IndexBuilder Unique()
        {
            _index.IsUnique = true;

            return this;
        }

        public IndexBuilder WithTable(string table)
        {
            _index.Table = table;

            return this;
        }

        public IndexBuilder WithColumn(string column)
        {
            if (string.IsNullOrWhiteSpace(column))
                throw new ArgumentException("Cannot be null or empty.", nameof(column));

            if (_index.Columns.Any(c => c.Equals(column, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException($"A column with name \"{column}\" has been already added.");

            _index.Columns.Add(column);

            return this;
        }

        public Index Build()
        {
            return _index;
        }
    }
}
