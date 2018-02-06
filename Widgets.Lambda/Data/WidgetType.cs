using Amazon.DynamoDBv2.DataModel;
using System;

namespace Widgets.Lambda.Data
{
    [DynamoDBTable("WTypes")]
    public class WidgetType
    {
        public WidgetType()
        {
            var id = "WDGT_TYPE_" + DateTime.Now.Ticks;
            this.Id = id;
            this.Name = $"Widget Type {id}";
            this.Description = $"This is a type of widget {id}";
        }

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
