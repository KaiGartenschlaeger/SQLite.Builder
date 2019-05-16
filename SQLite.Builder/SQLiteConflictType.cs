namespace SQLite.Builder
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
