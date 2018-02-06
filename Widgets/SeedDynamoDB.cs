using System;
using System.Collections.Generic;
using Widgets.Data;

namespace Widgets
{
    public partial class Program
    {
        private static List<string> _typeIds = new List<string> { "T1", "T2", "T3", "T4", "T5" };
        private static List<string> _widgetIds = new List<string> { "WDGT1", "WDGT2", "WDGT3" };

        public static void AddSampleData()
        {
            Repository repository = new Repository(_client);

            // Types
            List<Data.Type> types = new List<Data.Type>();
            foreach (var typeId in _typeIds)
            {
                types.Add(new Data.Type
                {
                    Id = typeId,
                    Name = $"WDGT_TYPE_{typeId}",
                    Description = $"Widget Type {typeId}"
                });
            }
            repository.Types.AddBatch(types);

            // Widgets
            List<Widget> widgets = new List<Widget>();
            Random r = new Random();
            foreach (var widgetId in _widgetIds)
            {
                widgets.Add(new Widget
                {
                    Id = widgetId,
                    Created = DateTime.Now,
                    Description = $"Widget {widgetId}",
                    Keywords = new List<string> { $"KEY1-{widgetId}", $"KEY2-{widgetId}", $"KEY3-{widgetId}" },
                    Name = $"Widget {widgetId}",
                    Price = 100 * r.Next(0, 5),
                    Types = new List<Data.Type> { types[r.Next(0, 5)], types[r.Next(0, 5)] }
                });
            }
            repository.Widgets.AddBatch(widgets);
        }

        public static void DeleteSampleData()
        {
            Repository repository = new Repository(_client);
            foreach(var typeId in _typeIds)
            {
                repository.Types.Delete(typeId);
            }
            foreach (var widgetId in _widgetIds)
            {
                repository.Widgets.Delete(widgetId);
            }
        }

        public static void UpdateSampleData()
        {
            Repository repository = new Repository(_client);
            var types = repository.Types.GetAll(_typeIds);
            foreach (var type in types)
            {
                type.Name = "Type " + type.Id;
                repository.Types.Update(type.Id, type);
            }
        }

        public static void PrintSampleData()
        {
            Repository repository = new Repository(_client);
            var types = repository.Types.GetAll(_typeIds);
            foreach (var type in types)
            {
                Console.WriteLine("Type: " + type.Name);
            }
            var widgets = repository.Widgets.GetAll(_widgetIds);
            foreach (var widget in widgets)
            {
                Console.WriteLine("Widget: " + widget.Name);
            }
        }
    }
}
