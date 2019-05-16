using System.Collections.Generic;

namespace PureFreak.SQLite.Builder
{
    public class Table
    {
        public Table()
        {
            Columns = new List<Column>();
            UniqueIndizees = new List<Index>();
        }

        public string Name { get; set; }

        public List<Column> Columns { get; }

        public List<Index> UniqueIndizees { get; }

        public PrimaryKey PrimaryKey { get; set; }

        public bool ExistsCheck { get; internal set; }
        public bool WithoutRowId { get; internal set; }
    }
}
