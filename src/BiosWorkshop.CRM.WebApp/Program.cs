using MDP.NetCore;
using MDP.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiosWorkshop.CRM.WebApp
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Host
            BiosWorkshop.CRM.WebPlatform.Host.CreateHostBuilder(args).Build().Run();
        }
    }
}
