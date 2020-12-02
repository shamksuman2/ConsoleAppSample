
namespace Dover.Fleet.Common.CosmoDB.Handler
{
    public class DocumentUpdate
    {
        public DocumentUpdate(string name, object value)
        {
            Name = name;
            Value = value;
        }
        public string Name { get; set; }
        public object Value { get; set; }
    }
}
