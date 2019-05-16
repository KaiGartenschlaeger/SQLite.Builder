using System.Collections.Generic;

namespace PureFreak.SQLite.Builder
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
