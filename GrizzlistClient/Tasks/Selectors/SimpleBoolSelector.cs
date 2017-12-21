namespace Grizzlist.Client.Tasks.Selectors
{
    public class SimpleBoolSelector : IBoolSelector
    {
        public bool Value { get; set; }

        public SimpleBoolSelector(bool value)
        {
            Value = value;
        }

        public bool Select()
        {
            return Value;
        }
    }
}
