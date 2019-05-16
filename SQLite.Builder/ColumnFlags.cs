using System;

namespace PureFreak.SQLite.Builder
{
    [Flags]
    public enum ColumnFlags
    {
        None = 0,
        Unique = 1,
        NotNull = 2,
        PrimaryKey = 4,
        AutoIncrement = 8
    }
}
