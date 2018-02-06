using Amazon.DynamoDBv2;

namespace Widgets.Data
{
    public class Repository
    {
        IAmazonDynamoDB _client;

        public Repository(IAmazonDynamoDB client)
        {
            _client = client;
        }

        public TypeRepository Types
        {
            get
            {
                return new TypeRepository(_client);
            }
        }

        public WidgetRepository Widgets
        {
            get
            {
                return new WidgetRepository(_client);
            }
        }
    }
}
