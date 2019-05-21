using System.Collections.Generic;

namespace PureFreak.SQLite.Builder
{
    public class PrimaryKeyEntity
    {
        public PrimaryKeyEntity()
        {
            Columns = new List<PrimaryKeyColumn>();
            ConflictType = null;
        }

        public List<PrimaryKeyColumn> Columns { get; set; }

        public SQLiteConflictType? ConflictType { get; set; }
    }
}
