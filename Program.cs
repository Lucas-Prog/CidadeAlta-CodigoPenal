using CidadeAlta_CodigoPenal.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CidadeAlta_CodigoPenal
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //CriminalCodeContext _context = new CriminalCodeContext();
            //_context.Status.Add(new Status { Name = "Preso" });
            //_context.SaveChanges();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
