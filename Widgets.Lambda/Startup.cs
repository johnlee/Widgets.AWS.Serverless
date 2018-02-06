using Amazon;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Widgets.Lambda
{
    public class Startup
    {
        public const string AppS3BucketKey = "AppS3Bucket";
        public static IConfiguration Configuration { get; private set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AWSConfigs.AWSRegion = Configuration.GetSection("AWSConfiguration")["AWSRegion"];
            AWSConfigs.AWSProfileName = Configuration.GetSection("AWSConfiguration")["AWSProfileName"];
        }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();

            // Add DynamoDB
            services.AddAWSService<Amazon.DynamoDBv2.IAmazonDynamoDB>();

            // Add S3 to the ASP.NET Core dependency injection framework.
            services.AddAWSService<Amazon.S3.IAmazonS3>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Configure CORS
            app.UseCors(policy =>
            {
                policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials();
            });

            // Configure Routing
            //app.UseMvc(routes =>
            //    routes.MapRoute("default", "{controller=Widgets}/{action=Index}/{id?}")
            //);
            app.UseMvc();

            // Static web hosting
            app.UseDefaultFiles();
            app.UseStaticFiles();
            
            // Configure Lambda logging
            loggerFactory.AddLambdaLogger(Configuration.GetLambdaLoggerOptions());
        }
    }
}
