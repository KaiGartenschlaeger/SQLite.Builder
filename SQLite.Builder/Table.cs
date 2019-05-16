using System.Collections.Generic;

namespace PureFreak.SQLite.Builder
{
    public class Table
    {
        public string Name { get; set; }

        public List<Column> Columns { get; set; }

        public UniqueIndex UniqueIndex { get; set; }

        public PrimaryKey PrimaryKey { get; set; }
        public bool ExistsCheck { get; internal set; }
        public bool WithoutRowId { get; internal set; }
    }
}
