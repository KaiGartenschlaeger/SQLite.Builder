using System;

namespace PureFreak.SQLite.Builder
{
    public class TriggerBuilder
    {
        private readonly TriggerEntity _trigger;

        public TriggerBuilder(string name)
        {
            _trigger = new TriggerEntity();
            _trigger.Name = name;
        }

        public static TriggerBuilder Create(string name)
        {
            return new TriggerBuilder(name);
        }

        public TriggerBuilder WithExistsCheck()
        {
            _trigger.ExistsCheck = true;

            return this;
        }

        public TriggerBuilder After(TriggerActionType type)
        {
            _trigger.EventType = SQLiteTriggerEventType.After;
            _trigger.ActionType = type;

            return this;
        }

        public TriggerBuilder Before(TriggerActionType type)
        {
            _trigger.EventType = SQLiteTriggerEventType.Before;
            _trigger.ActionType = type;

            return this;
        }

        public TriggerBuilder OnTable(string table)
        {
            _trigger.TableName = table;

            return this;
        }

        public TriggerBuilder WithCondition(string condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            var fixedCondition = condition.Trim();
            if (fixedCondition.StartsWith("WHEN", StringComparison.OrdinalIgnoreCase))
            {
                fixedCondition = fixedCondition.Substring(4);
                fixedCondition = fixedCondition.Trim();
            }

            _trigger.Condition = fixedCondition;

            return this;
        }

        public TriggerBuilder WithScript(string script)
        {
            if (script == null)
                throw new ArgumentNullException(nameof(script));

            var fixedScript = script.Trim();

            _trigger.Script.AppendLine(fixedScript);

            return this;
        }

        public TriggerEntity Build()
        {
            return _trigger;
        }
    }
}
