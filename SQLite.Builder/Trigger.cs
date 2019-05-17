using System.Text;

namespace PureFreak.SQLite.Builder
{
    public class Trigger
    {
        public Trigger()
        {
            Script = new StringBuilder();
        }

        public string Name { get; set; }
        public bool ExistsCheck { get; set; }
        public TriggerActionType ActionType { get; set; }
        public TriggerEventType EventType { get; set; }
        public string Condition { get; set; }
        public StringBuilder Script { get; }
        public string TableName { get; set; }
    }
}
