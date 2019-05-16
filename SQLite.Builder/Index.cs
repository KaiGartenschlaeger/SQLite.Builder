using System.Collections.Generic;

namespace PureFreak.SQLite.Builder
{
    public class Index
    {
        public Index()
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
