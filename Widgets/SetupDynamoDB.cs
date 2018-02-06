using System.Collections.Generic;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;

namespace Widgets
{
    public partial class Program
    {
        private static string _tableNameWidget = "Widgets";
        private static string _tableNameWidgetType = "WTypes";

        // Creates tables and it's key schema elements
        public static void CreateTables()
        {
            // Create Widget table if not exist
            List<string> currentTables = _client.ListTables().TableNames;
            if (!currentTables.Contains(_tableNameWidget))
            {
                // Create table request
                var request = new CreateTableRequest
                {
                    TableName = _tableNameWidget,
                    KeySchema = new List<KeySchemaElement>()
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Id",
                            KeyType = KeyType.HASH
                        }
                    },
                    AttributeDefinitions = new List<AttributeDefinition>()
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "Id",
                            AttributeType = ScalarAttributeType.S
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 10,
                        WriteCapacityUnits = 5
                    }
                };

                // Create the table
                var response = _client.CreateTable(request);
            }

            // Create WidgetType table if not exist
            if (!currentTables.Contains(_tableNameWidgetType))
            {
                // Create table request
                var request = new CreateTableRequest
                {
                    TableName = _tableNameWidgetType,
                    KeySchema = new List<KeySchemaElement>()
                    {
                        new KeySchemaElement
                        {
                            AttributeName = "Id",
                            KeyType = KeyType.HASH
                        }
                    },
                    AttributeDefinitions = new List<AttributeDefinition>()
                    {
                        new AttributeDefinition
                        {
                            AttributeName = "Id",
                            AttributeType = ScalarAttributeType.S
                        }
                    },
                    ProvisionedThroughput = new ProvisionedThroughput
                    {
                        ReadCapacityUnits = 10,
                        WriteCapacityUnits = 5
                    }
                };

                // Create the table
                var response = _client.CreateTable(request);
            }
        }

        // Updates tables
        public static void UpdateTable()
        {
            List<string> currentTables = _client.ListTables().TableNames;
            if (currentTables.Contains(_tableNameWidget))
            {
                var request = new UpdateTableRequest()
                {
                    TableName = _tableNameWidget,
                    ProvisionedThroughput = new ProvisionedThroughput()
                    {
                        // Provide new values.
                        ReadCapacityUnits = 20,
                        WriteCapacityUnits = 10
                    }
                };
                var response = _client.UpdateTable(request);
            }
        }

        // Deletes tables
        public static void DeleteTables()
        {
            List<string> currentTables = _client.ListTables().TableNames;
            if (currentTables.Contains(_tableNameWidget))
            {
                _client.DeleteTable(_tableNameWidget);
            }
            if (currentTables.Contains(_tableNameWidgetType))
            {
                _client.DeleteTable(_tableNameWidgetType);
            }
        }
    }
}
