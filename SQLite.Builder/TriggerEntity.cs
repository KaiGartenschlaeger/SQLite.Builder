using System.Text;

namespace PureFreak.SQLite.Builder
{
    public class TriggerEntity
    {
        public TriggerEntity()
        {
            Script = new StringBuilder();
        }

        public string Name { get; set; }
        public bool ExistsCheck { get; set; }
        public TriggerActionType ActionType { get; set; }
        public SQLiteTriggerEventType EventType { get; set; }
        public string Condition { get; set; }
        public StringBuilder Script { get; }
        public string TableName { get; set; }
    }
}
