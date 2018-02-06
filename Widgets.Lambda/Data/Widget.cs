using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;

namespace Widgets.Lambda.Data
{
    [DynamoDBTable("Widgets")]
    public class Widget
    {
        public Widget()
        {
            var id = "WDGT" + DateTime.Now.Ticks;
            this.Id = id;
            this.Name = $"Widget {id}";
            this.Description = $"This is a widget {id}";
            this.Created = DateTime.Now;
            this.Price = (decimal) DateTime.Now.Second + DateTime.Now.Millisecond;
        }

        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }
        
        public decimal Price { get; set; }

        [DynamoDBProperty("Types")]
        public List<WidgetType> Types { get; set; }

        public List<string> Keywords { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0} : {1} - {2}", Id, Name, Description);
        }
    }
}
