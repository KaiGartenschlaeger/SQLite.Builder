namespace PureFreak.SQLite.Builder
{
    public enum SQLiteConflictType
    {
        Rollback,
        Abort,
        Fail,
        Ignore,
        Replace
    }
}
