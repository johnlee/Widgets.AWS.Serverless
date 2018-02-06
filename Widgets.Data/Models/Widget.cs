using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;

namespace Widgets.Data
{
    [DynamoDBTable("Widgets")]
    public class Widget
    {
        [DynamoDBHashKey]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }

        public decimal Price { get; set; }

        [DynamoDBProperty("Types")]
        public List<Type> Types { get; set; }

        public List<string> Keywords { get; set; }

        public override string ToString()
        {
            return string.Format(@"{0} : {1} - {2}", Id, Name, Description);
        }
    }
}
