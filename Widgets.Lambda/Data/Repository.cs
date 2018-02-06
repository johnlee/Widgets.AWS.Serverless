using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Widgets.Lambda.Data
{
    public class Repository
    {
        IAmazonDynamoDB _client;
        DynamoDBContext _context;

        public Repository(IAmazonDynamoDB client)
        {
            _client = client;
            _context = new DynamoDBContext(client);
        }

        /// <summary>
        ///  Low level interface requires data type conversions
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWidgetUsingLowLevel()
        {
            var widget = new Widget();

            var putRequest = new PutItemRequest
            {
                TableName = "Widgets",
                Item = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = widget.Id } },
                    { "Name", new AttributeValue { S = widget.Name } },
                    { "Description", new AttributeValue { S = widget.Description } },
                    { "Created", new AttributeValue { S = widget.Created.ToString("yyyy-MM-ddTHH:mm:ss.fffZ") } },
                    { "Price", new AttributeValue { N = widget.Price.ToString() } },
                }
            };
            await _client.PutItemAsync(putRequest);

            var getRequest = new GetItemRequest
            {
                TableName = "Widgets",
                Key = new Dictionary<string, AttributeValue>
                {
                    { "Id", new AttributeValue { S = widget.Id } }
                }
            };

            var getItem = await _client.GetItemAsync(getRequest);
            var result = getItem.Item;
            return result["Name"].S;
        }

        /// <summary>
        /// Document model interface 
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWidgetUsingDocumentModel()
        {
            var widget = new Widget();
            var widgetTable = Table.LoadTable(_client, "Widgets");
            var widgetDocument = new Document();

            widgetDocument["Id"] = widget.Id;
            widgetDocument["Name"] =  widget.Name;
            widgetDocument["Description"] = widget.Description;
            widgetDocument["Created"] = widget.Created;
            widgetDocument["Price"] = widget.Price;
            await widgetTable.PutItemAsync(widgetDocument);

            var resultDocument = await widgetTable.GetItemAsync(widget.Id);
            return resultDocument["Name"].AsString();
        }

        /// <summary>
        /// Using the Object Persistance Model
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetWidgetUsingObjectPersistence()
        {
            var widget = new Widget();

            await _context.SaveAsync(widget);

            var result = await _context.LoadAsync<Widget>(widget.Id);
            return result.Name;
        }

        #region ObjectPersistenceExamples
        /// <summary>
        /// Batch Read using Object Persistence Model
        /// </summary>
        /// <returns></returns>
        public async Task<List<Widget>> GetWidgetsByBatchUsingOP()
        {
            List<string> ids = new List<string> { "WDGT1", "WDGT2", "WDGT3" };
            var widgets = _context.CreateBatchGet<Widget>();
            foreach (var key in ids)
            {
                widgets.AddKey(key);
            }
            await widgets.ExecuteAsync();
            return widgets.Results;
        }

        /// <summary>
        /// Single Read using Object Persistence Model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Widget> GetWidgetByIdUsingOP(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "WDGT1";
            }
            var widget = await _context.LoadAsync<Widget>(id);
            return widget;
        }

        /// <summary>
        /// Write and Read with Arbitrary Data using Object Persistence Model
        /// </summary>
        public async Task<Widget> GetWidgetAndArbitraryTypeUsingOP()
        {
            WidgetType widgetType1 = new WidgetType();
            WidgetType widgetType2 = new WidgetType();
            WidgetType widgetType3 = new WidgetType();
            Widget widget = new Widget
            {
                Types = new List<WidgetType> { widgetType1, widgetType2, widgetType3 }
            };

            // Write the new widget
            await _context.SaveAsync(widget);

            var result = await _context.LoadAsync<Widget>(widget.Id);
            return result;
        }

        /// <summary>
        /// Multiple Query using Object Persistence Model
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Widget>> GetWidgetsByQueryUsingOP(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "WDGT1";
            }
            var queryfilter = new QueryOperationConfig()
            {
                IndexName = "Id-Created-index",
                Filter = new QueryFilter("Id", QueryOperator.Equal, id)
            };
            var query = _context.FromQueryAsync<Widget>(queryfilter);
            return await query.GetRemainingAsync();
        }

        /// <summary>
        /// Get all items over 200 price
        /// </summary>
        /// <returns></returns>
        public async Task<List<Widget>> GetWidgetsByScanUsingOP()
        {
            var scanConditions = new List<ScanCondition>
            {
                new ScanCondition("Price", ScanOperator.GreaterThan, 200m)
            };
            var scan = _context.ScanAsync<Widget>(scanConditions);
            return await scan.GetRemainingAsync();
        }

        /// <summary>
        /// Adding record using Object Persistence Model
        /// </summary>
        /// <param name="widget"></param>
        public async void AddWidget(Widget widget)
        {
            await _context.SaveAsync<Widget>(widget);
        }

        /// <summary>
        /// Updating record using Object Persistence Model
        /// </summary>
        /// <param name="widget"></param>
        public async void UpdateWidget(Widget widget)
        {
            //Widget original = await _context.LoadAsync<Widget>(widget.Id);
            //if (original != null)
            //{
                await _context.SaveAsync<Widget>(widget);
            //}
        }

        /// <summary>
        /// Deleting record using Object Persistence Model
        /// </summary>
        /// <param name="id"></param>
        public async void DeleteWidget(string id)
        {
            Widget original = await _context.LoadAsync<Widget>(id);
            if (original != null)
            {
                await _context.DeleteAsync<Widget>(original);
            }
        }
        #endregion
    }
}
