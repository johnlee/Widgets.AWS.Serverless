using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System.Collections.Generic;

namespace Widgets.Data
{
    public class WidgetRepository
    {
        DynamoDBContext _context;

        public WidgetRepository(IAmazonDynamoDB client)
        {
            _context = new DynamoDBContext(client);
        }

        public List<Widget> GetAll(List<string> ids)
        {
            if (ids.Count > 0)
            {
                var widgets = _context.CreateBatchGet<Widget>();
                foreach (var key in ids)
                {
                    widgets.AddKey(key);
                }
                widgets.Execute();
                if (widgets.Results.Count > 0)
                {
                    return widgets.Results;
                }
            }
            return new List<Widget>();
        }

        public List<Widget> Scan()
        {


            var result = _context.Scan<Widget>();

            List<Widget> widgets = new List<Widget>();
            foreach (var w in result)
            {
                widgets.Add(w);
            }
            return widgets;
        }

        public void Add(Widget widget)
        {
            _context.Save<Widget>(widget);
        }

        public void AddBatch(List<Widget> widgets)
        {
            var batchTypes = _context.CreateBatchWrite<Widget>();
            batchTypes.AddPutItems(widgets);
            batchTypes.Execute();
        }

        public void Delete(string id)
        {
            Widget original = _context.Load<Widget>(id);
            if (original != null)
            {
                _context.Delete<Widget>(original);
            }
        }

        public void Update(string id, Widget widget)
        {
            Widget original = _context.Load<Widget>(id);
            if (original != null)
            {
                widget.Id = id;
                _context.Save<Widget>(widget);
            }
        }
    }
}
