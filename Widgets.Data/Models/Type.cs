using Amazon.DynamoDBv2.DataModel;

namespace Widgets.Data
{
    [DynamoDBTable("WTypes")]
    public class Type
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0} : {1} - {2}", Id, Name, Description);
        }
    }
}
