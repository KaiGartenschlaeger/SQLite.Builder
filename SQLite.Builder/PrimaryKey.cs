using System.Collections.Generic;

namespace PureFreak.SQLite.Builder
{
    public class PrimaryKey
    {
        public PrimaryKey()
        {
            Columns = new List<PrimaryKeyColumn>();
            ConflictType = null;
        }

        public List<PrimaryKeyColumn> Columns { get; set; }

        public SQLiteConflictType? ConflictType { get; set; }
    }
}
