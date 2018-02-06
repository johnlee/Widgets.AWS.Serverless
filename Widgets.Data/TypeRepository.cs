using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using System.Collections.Generic;

namespace Widgets.Data
{
    public class TypeRepository
    {
        DynamoDBContext _context;

        public TypeRepository(IAmazonDynamoDB client)
        {
            _context = new DynamoDBContext(client);
        }

        public List<Type> GetAll(List<string> ids)
        {
            if (ids.Count > 0)
            {
                var types = _context.CreateBatchGet<Type>();
                foreach(var key in ids)
                {
                    types.AddKey(key);
                }
                types.Execute();
                if (types.Results.Count > 0)
                {
                    return types.Results;
                }
            }
            return new List<Type>();
        }

        public void Add(Type type)
        {
            _context.Save<Type>(type);
        }

        public void AddBatch(List<Type> types)
        {
            var batchTypes = _context.CreateBatchWrite<Type>();
            batchTypes.AddPutItems(types);
            batchTypes.Execute();
        }

        public void Delete(string id)
        {
            Type original = _context.Load<Type>(id);
            if (original != null)
            {
                _context.Delete<Type>(original);
            }
        }

        public void Update(string id, Type type)
        {
            Type original = _context.Load<Type>(id);
            if (original != null)
            {
                type.Id = id;
                _context.Save<Type>(type);
            }
        }
    }
}
