using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using OrdersService.Context;
using OrdersService.DB_Access;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace TestAPI
{
    public class ApiWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected HttpClient TestClient;
        public ApiWebApplicationFactory()
        {           
            CreateClient();
        }
        
        protected override IHost CreateHost(IHostBuilder builder)
        {
            var root = new Microsoft.EntityFrameworkCore.Storage.InMemoryDatabaseRoot();
            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(OrdersServiceContext));
                services.RemoveAll(typeof(DbContextOptions<OrdersServiceContext>));


                services.AddDbContext<OrdersServiceContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDataBase");
                });

            });
            return base.CreateHost(builder);
        }

        protected void CreateClient()
        {
            TestClient = CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }
    }
}
