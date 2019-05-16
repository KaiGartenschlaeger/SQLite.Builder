namespace PureFreak.SQLite.Builder
{
    public class Column
    {
        public string Name { get; set; }
        public SQLiteDbType Type { get; set; }
        public string DefaultValue { get; set; }

        public ColumnFlags Flags { get; set; }

        public SQLiteConflictType? UniqueConflict { get; set; }

        public SQLiteConflictType? NotNullConflict { get; set; }

        public string ReferenceTableName { get; set; }
        public string ReferenceColumnName { get; set; }
        public SQLiteReactType? ReferenceDeleteReaction { get; set; }
        public SQLiteReactType? ReferenceUpdateReaction { get; set; }
    }
}
