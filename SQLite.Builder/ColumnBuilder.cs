using System;

namespace PureFreak.SQLite.Builder
{
    public class ColumnBuilder
    {
        private readonly TableBuilder _tableBuilder;
        private readonly Column _column;

        public ColumnBuilder(TableBuilder tableBuilder, Column column)
        {
            if (tableBuilder == null)
                throw new ArgumentNullException(nameof(tableBuilder));
            if (column == null)
                throw new ArgumentNullException(nameof(column));

            _tableBuilder = tableBuilder;
            _column = column;
        }

        public ColumnBuilder PrimaryKey()
        {
            _column.Flags |= ColumnFlags.PrimaryKey;

            return this;
        }

        public ColumnBuilder AutoIncrement()
        {
            _column.Flags |= ColumnFlags.AutoIncrement;

            return this;
        }

        public ColumnBuilder Unique()
        {
            _column.Flags |= ColumnFlags.Unique;

            return this;
        }

        public ColumnBuilder UniqueOnConflict(SQLiteConflictType type)
        {
            Unique();
            OnUniqueConflict(type);

            return this;
        }

        public ColumnBuilder OnUniqueConflict(SQLiteConflictType type)
        {
            _column.UniqueConflict = type;

            return this;
        }

        public ColumnBuilder NotNull()
        {
            _column.Flags |= ColumnFlags.NotNull;

            return this;
        }

        public ColumnBuilder NotNullOnConflict(SQLiteConflictType type)
        {
            NotNull();
            OnNotNullConflict(type);

            return this;
        }

        public ColumnBuilder OnNotNullConflict(SQLiteConflictType type)
        {
            _column.NotNullConflict = type;

            return this;
        }

        public ColumnBuilder Ref(string tableName, string columnName)
        {
            _column.ReferenceTableName = tableName;
            _column.ReferenceColumnName = columnName;

            return this;
        }

        public ColumnBuilder OnRefDelete(SQLiteReactType reaction)
        {
            _column.ReferenceDeleteReaction = reaction;

            return this;
        }

        public ColumnBuilder OnRefUpdate(SQLiteReactType reaction)
        {
            _column.ReferenceUpdateReaction = reaction;

            return this;
        }

        public ColumnBuilder Default(string defaultValue)
        {
            _column.DefaultValue = defaultValue;

            return this;
        }

        public TableBuilder BuildColumn()
        {
            return _tableBuilder;
        }
    }
}
