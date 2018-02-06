using Amazon.DynamoDBv2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Widgets.Data;

namespace Widgets
{
    /// <summary>
    /// This program is used to intialize AWS with the following:
    /// API
    /// DYNAMODB
    /// LAMBDA
    /// S3
    /// </summary>
    public partial class Program
    {
        public static IAmazonDynamoDB _client;

        public static void Main(string[] args)
        {
            Console.WriteLine("*** PROGRAM START ***");

            //AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
            //ddbConfig.ServiceURL = "http://localhost:8000";
            try
            {
                //_client = new AmazonDynamoDBClient(ddbConfig);
                _client = new AmazonDynamoDBClient();
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n Error: failed to create a DynamoDB client; " + ex.Message);
                PauseForDebugWindow();
                return;
            }

            // Configure the DynamoDB 
            //Console.WriteLine("Configuring DynamoDB");
            //Console.Write("Deleting tables...");
            //DeleteTables();
            //Thread.Sleep(TimeSpan.FromSeconds(5));
            //Console.Write("done!");
            //Console.WriteLine();
            //Console.Write("Creating tables...");
            //CreateTables();
            //Thread.Sleep(TimeSpan.FromSeconds(5));
            //Console.Write("done!");
            //Console.WriteLine();
            //Console.Write("Updating tables...");
            //UpdateTable();
            //Console.Write("done!");
            //Console.WriteLine();

            // Add sample data
            Console.Write("Deleting data...");
            DeleteSampleData();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            Console.Write("done!");
            Console.WriteLine();
            Console.Write("Seeding data...");
            AddSampleData();
            Thread.Sleep(TimeSpan.FromSeconds(5));
            Console.Write("done!");
            Console.WriteLine();
            Console.Write("Updating data...");
            UpdateSampleData();
            Thread.Sleep(TimeSpan.FromSeconds(2));
            Console.Write("done!");
            Console.WriteLine();

            Console.WriteLine("*** PROGRAM END ***");
            Console.ReadKey();
        }

        public static void PauseForDebugWindow()
        {
            // Keep the console open if in Debug mode...
            Console.Write("\n\n ...Press any key to continue");
            Console.ReadKey();
            Console.WriteLine();
        }
    }
}
