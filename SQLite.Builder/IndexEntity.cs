using System.Collections.Generic;

namespace PureFreak.SQLite.Builder
{
    public class IndexEntity
    {
        public IndexEntity()
        {
            Columns = new List<string>();
        }

        public string Name { get; set; }

        public bool ExistsCheck { get; set; }

        public bool IsUnique { get; set; }

        public List<string> Columns { get; }

        public string Table { get; set; }
    }
}
