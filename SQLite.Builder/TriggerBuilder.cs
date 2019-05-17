using System;

namespace PureFreak.SQLite.Builder
{
    public class TriggerBuilder
    {
        private readonly Trigger _trigger;

        public TriggerBuilder(string name)
        {
            _trigger = new Trigger();
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

        public TriggerBuilder After()
        {
            _trigger.EventType = TriggerEventType.After;
            return this;
        }

        public TriggerBuilder Before()
        {
            _trigger.EventType = TriggerEventType.Before;

            return this;
        }

        public TriggerBuilder Update()
        {
            _trigger.ActionType = TriggerActionType.Update;

            return this;
        }

        public TriggerBuilder OnInsert()
        {
            _trigger.ActionType = TriggerActionType.Insert;
            return this;
        }

        public TriggerBuilder OnDelete()
        {
            _trigger.ActionType = TriggerActionType.Delete;

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

            _trigger.Condition = condition;

            return this;
        }

        public TriggerBuilder WithScript(string script)
        {
            if (script == null)
                throw new ArgumentNullException(nameof(script));

            _trigger.Script.AppendLine(script.Trim());

            return this;
        }

        public Trigger Build()
        {
            return _trigger;
        }
    }
}
