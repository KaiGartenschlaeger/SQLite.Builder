using System.Collections.Generic;

namespace SQLite.Builder
{
    public class UniqueIndex
    {
        public UniqueIndex()
        {
            Columns = new List<string>();
        }

        public List<string> Columns { get; }
    }
}
