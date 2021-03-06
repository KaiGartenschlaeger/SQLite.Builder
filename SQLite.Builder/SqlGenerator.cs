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
        private string GetTriggerEventString(SQLiteTriggerEventType type)
        {
            switch (type)
            {
                case SQLiteTriggerEventType.Before:
                    return "BEFORE";
                case SQLiteTriggerEventType.After:
                    return "AFTER";

                default:
                    throw new NotImplementedException();
            }
        }
        private string GetTriggerActionString(TriggerActionType type)
        {
            switch (type)
            {
                case TriggerActionType.Update:
                    return "UPDATE";
                case TriggerActionType.Insert:
                    return "INSERT";
                case TriggerActionType.Delete:
                    return "DELETE";

                default:
                    throw new NotImplementedException();
            }
        }

        public string Generate(TableEntity table)
        {
            var buffer = new StringBuilder();
            AppendTableHeader(buffer, table);
            AppendColumns(buffer, table);
            AppendUniqueIndizees(buffer, table);
            AppendPrimaryKey(buffer, table);
            AppendTableFooter(buffer, table);

            return buffer.ToString();
        }

        private void AppendTableHeader(StringBuilder sql, TableEntity table)
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
        private void AppendColumns(StringBuilder sql, TableEntity table)
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
        private void AppendUniqueIndizees(StringBuilder sql, TableEntity table)
        {
            for (int i = 0; i < table.UniqueIndizees.Count; i++)
            {
                var index = table.UniqueIndizees[i];

                sql.Append(_indent);
                sql.Append("UNIQUE (");
                sql.AppendLine();

                for (int c = 0; c < index.Columns.Count; c++)
                {
                    var column = index.Columns[c];

                    sql.Append(_indent);
                    sql.Append(_indent);
                    sql.Append(column);

                    if (c + 1 < index.Columns.Count)
                        sql.Append(",");

                    sql.AppendLine();
                }

                sql.Append(_indent);
                sql.Append(")");
                sql.Append(",");
                sql.AppendLine();
            }
        }
        private void AppendPrimaryKey(StringBuilder sql, TableEntity table)
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
        private void AppendTableFooter(StringBuilder sql, TableEntity table)
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

        public string Generate(IndexEntity index)
        {
            if (index == null)
                throw new ArgumentNullException(nameof(index));

            var sql = new StringBuilder();

            sql.Append("CREATE ");

            if (index.IsUnique)
                sql.Append("UNIQUE ");

            sql.Append("INDEX ");

            if (index.ExistsCheck)
                sql.Append("IF NOT EXISTS ");

            sql.Append("[");
            sql.Append(index.Name);
            sql.Append("]");

            sql.Append(" ON ");

            sql.Append("[");
            sql.Append(index.Table);
            sql.Append("]");

            sql.Append(" (");
            sql.AppendLine();

            for (int i = 0; i < index.Columns.Count; i++)
            {
                var column = index.Columns[i];

                sql.Append(_indent);
                sql.Append(column);

                if (i + 1 < index.Columns.Count)
                    sql.Append(",");

                sql.AppendLine();
            }

            sql.Append(");");

            return sql.ToString();
        }

        public string Generate(TriggerEntity trigger)
        {
            if (trigger == null)
                throw new ArgumentNullException(nameof(trigger));

            var sql = new StringBuilder();

            sql.Append("CREATE TRIGGER ");

            if (trigger.ExistsCheck)
                sql.Append("IF NOT EXISTS ");

            sql.Append($"\"{trigger.Name}\" ");

            sql.Append($"{GetTriggerEventString(trigger.EventType)} ");
            sql.Append($"{GetTriggerActionString(trigger.ActionType)} ");

            sql.Append($"ON \"{trigger.TableName}\" ");

            sql.AppendLine();

            if (!string.IsNullOrEmpty(trigger.Condition))
            {
                sql.Append("WHEN ");
                sql.Append(trigger.Condition);
                sql.Append(" ");
                sql.AppendLine();
            }

            sql.Append("BEGIN ");
            sql.AppendLine();

            foreach (var line in trigger.Script.ToString()
                .Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries))
            {
                sql.Append(_indent);
                sql.Append(line.Trim());
                sql.Append(";");
                sql.AppendLine();
            }

            sql.Append("END;");

            return sql.ToString();
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
