using System.Collections.Generic;

namespace PureFreak.SQLite.Builder
{
    public class TableEntity
    {
        public TableEntity()
        {
            Columns = new List<ColumnEntity>();
            UniqueIndizees = new List<IndexEntity>();
        }

        public string Name { get; set; }

        public List<ColumnEntity> Columns { get; }

        public List<IndexEntity> UniqueIndizees { get; }

        public PrimaryKeyEntity PrimaryKey { get; set; }

        public bool ExistsCheck { get; internal set; }
        public bool WithoutRowId { get; internal set; }
    }
}
