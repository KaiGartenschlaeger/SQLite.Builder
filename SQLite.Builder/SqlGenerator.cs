using PureFreak.SQLite.Builder.Extensions;
using System;
using System.Linq;
using System.Text;

namespace PureFreak.SQLite.Builder
{
    public class SqlGenerator
    {
        private int _indentSize;
        private string _indent;

        public SqlGenerator()
        {
            IndentSize = 4;
        }

        private string GetTypeString(SQLiteDbType type)
        {
            switch (type)
            {
                case SQLiteDbType.Boolean:
                    return "BOOLEAN";
                case SQLiteDbType.Integer:
                    return "INTEGER";
                case SQLiteDbType.Double:
                    return "DOUBLE";
                case SQLiteDbType.Text:
                    return "TEXT";
                case SQLiteDbType.Blob:
                    return "BLOB";
                case SQLiteDbType.DateTime:
                    return "DATETIME";

                default:
                    throw new NotImplementedException();
            }
        }
        private string GetReactionString(SQLiteReactType type)
        {
            switch (type)
            {
                case SQLiteReactType.NoAction:
                    return "NO ACTION";
                case SQLiteReactType.SetNull:
                    return "SET NULL";
                case SQLiteReactType.SetDefault:
                    return "SET DEFAULT";
                case SQLiteReactType.Cascade:
                    return "CASCADE";
                case SQLiteReactType.Restrict:
                    return "RESTRICT";

                default:
                    throw new NotImplementedException();
            }
        }
        private string GetConflictString(SQLiteConflictType type)
        {
            switch (type)
            {
                case SQLiteConflictType.Rollback:
                    return "ROLLBACK";
                case SQLiteConflictType.Abort:
                    return "ABORT";
                case SQLiteConflictType.Fail:
                    return "FAIL";
                case SQLiteConflictType.Ignore:
                    return "IGNORE";
                case SQLiteConflictType.Replace:
                    return "REPLACE";

                default:
                    throw new NotImplementedException();
            }
        }
        private string GetSortingString(SQLiteSorting type)
        {
            switch (type)
            {
                case SQLiteSorting.Ascending:
                    return "ASC";
                case SQLiteSorting.Descending:
                    return "DESC";

                default:
                    throw new NotImplementedException();
            }
        }

        public string Generate(Table table)
        {
            var buffer = new StringBuilder();
            AppendTableHeader(buffer, table);
            AppendColumns(buffer, table);
            AppendUniqueIndex(buffer, table);
            AppendPrimaryKey(buffer, table);
            AppendTableFooter(buffer, table);

            return buffer.ToString();
        }

        private void AppendTableHeader(StringBuilder sql, Table table)
        {
            sql.Append("CREATE TABLE ");

            if (table.ExistsCheck)
                sql.Append("IF NOT EXISTS ");

            sql.Append("[");
            sql.Append(table.Name);
            sql.Append("]");

            sql.Append(" (");
            sql.AppendLine();
        }

        private void AppendColumns(StringBuilder sql, Table table)
        {
            foreach (var column in table.Columns)
            {
                sql.Append(_indent);
                sql.Append(column.Name);
                sql.Append(" ");
                sql.Append(GetTypeString(column.Type));
                sql.Append(" ");

                if (column.Flags.HasFlag(ColumnFlags.PrimaryKey))
                {
                    sql.Append("PRIMARY KEY ");

                    if (column.Flags.HasFlag(ColumnFlags.PrimaryKey))
                        sql.Append("AUTOINCREMENT");
                }

                if (column.Flags.HasFlag(ColumnFlags.NotNull))
                {
                    sql.Append("NOT NULL ");
                    if (column.NotNullConflict.HasValue)
                        sql.AppendFormat("ON CONFLICT {0} ", GetConflictString(column.NotNullConflict.Value));
                }

                if (column.Flags.HasFlag(ColumnFlags.Unique))
                {
                    sql.Append("UNIQUE ");
                    if (column.UniqueConflict.HasValue)
                        sql.AppendFormat("ON CONFLICT {0} ", GetConflictString(column.UniqueConflict.Value));
                }

                if (!string.IsNullOrEmpty(column.ReferenceTableName))
                {
                    sql.AppendFormat("REFERENCES {0}({1}) ",
                        column.ReferenceTableName,
                        column.ReferenceColumnName);

                    if (column.ReferenceDeleteReaction.HasValue)
                        sql.AppendFormat("ON DELETE {0} ", GetReactionString(column.ReferenceDeleteReaction.Value));
                    if (column.ReferenceUpdateReaction.HasValue)
                        sql.AppendFormat("ON UPDATE {0} ", GetReactionString(column.ReferenceUpdateReaction.Value));
                }

                if (!string.IsNullOrEmpty(column.DefaultValue))
                {
                    sql.AppendFormat("DEFAULT({0}) ",
                        column.DefaultValue);
                }

                sql.TrimEnd();
                sql.Append(",");
                sql.AppendLine();
            }
        }

        private void AppendUniqueIndex(StringBuilder sql, Table table)
        {
            if (table.UniqueIndex != null)
            {
                sql.Append(_indent);
                sql.Append("UNIQUE (");
                sql.AppendLine();

                for (int i = 0; i < table.UniqueIndex.Columns.Count; i++)
                {
                    var column = table.UniqueIndex.Columns[i];

                    sql.Append(_indent);
                    sql.Append(_indent);
                    sql.Append(column);

                    if (i + 1 < table.UniqueIndex.Columns.Count)
                        sql.Append(",");

                    sql.AppendLine();
                }

                sql.Append(_indent);
                sql.Append(")");
                sql.Append(",");
                sql.AppendLine();
            }
        }

        private void AppendPrimaryKey(StringBuilder sql, Table table)
        {
            if (table.PrimaryKey != null)
            {
                sql.Append(_indent);
                sql.Append("PRIMARY KEY (");
                sql.AppendLine();

                for (int i = 0; i < table.PrimaryKey.Columns.Count; i++)
                {
                    var column = table.PrimaryKey.Columns[i];

                    sql.Append(_indent);
                    sql.Append(_indent);
                    sql.Append(column.Name);

                    if (column.Sort.HasValue)
                    {
                        sql.Append(" ");
                        sql.Append(GetSortingString(column.Sort.Value));
                    }

                    if (i + 1 < table.PrimaryKey.Columns.Count)
                        sql.Append(",");

                    sql.AppendLine();
                }

                sql.Append(_indent);
                sql.Append(")");

                if (table.PrimaryKey.ConflictType.HasValue)
                {
                    sql.Append(" ON CONFLICT ");
                    sql.Append(GetConflictString(table.PrimaryKey.ConflictType.Value));
                }

                sql.Append(",");
                sql.AppendLine();
            }
        }

        private void AppendTableFooter(StringBuilder sql, Table table)
        {
            sql.TrimEnd('\r', '\n', ',');

            sql.AppendLine();
            sql.Append(")");

            if (table.WithoutRowId &&
                (table.PrimaryKey != null || table.Columns.Any(c => (c.Flags & ColumnFlags.PrimaryKey) == ColumnFlags.PrimaryKey)))
            {
                sql.Append(" WITHOUT ROWID");
            }

            sql.Append(";");
        }

        public int IndentSize
        {
            get { return _indentSize; }
            set
            {
                _indentSize = value;
                _indent = new string(' ', _indentSize);
            }
        }
    }
}
