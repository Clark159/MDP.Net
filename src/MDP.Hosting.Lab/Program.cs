﻿using Autofac;
using Microsoft.Extensions.Configuration;
using System;

namespace MDP.Hosting.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // ConfigurationBuilder
            var configurationBuilder = new ConfigurationBuilder();
            {
                configurationBuilder.SetBasePath(CLK.IO.Directory.GetEntryDirectory());
                configurationBuilder.AddJsonFile("appsettings.json");
                configurationBuilder.RegisterModule();
            }

            // ContainerBuilder
            var containerBuilder = new ContainerBuilder();
            {
                // Register
                containerBuilder.RegisterInstance(configurationBuilder.Build()).As<IConfiguration>();
                containerBuilder.RegisterModule();
            }

            // Container
            using (var container = containerBuilder.Build())
            {
                // ResolveA
                var workServiceA = container.Resolve<WorkService>();
                if (workServiceA == null) throw new InvalidOperationException($"{nameof(workServiceA)}=null");

                // ResolveB
                var workServiceB = container.Resolve<WorkService>("HelloWorkService");
                if (workServiceB == null) throw new InvalidOperationException($"{nameof(workServiceB)}=null");

                // ResolveC
                var workServiceC = container.Resolve<WorkService>("HelloWorkService[456]");
                if (workServiceC == null) throw new InvalidOperationException($"{nameof(workServiceC)}=null");

                // ResolveD
                var workServiceD = container.Resolve<WorkService>("DecorateWorkService[AAA]");
                if (workServiceD == null) throw new InvalidOperationException($"{nameof(workServiceD)}=null");

                // ResolveE
                var workServiceE = container.Resolve<WorkService>("DecorateWorkService[BBB]");
                if (workServiceE == null) throw new InvalidOperationException($"{nameof(workServiceE)}=null");
                
                // ResolveF
                var workServiceF = container.Resolve<WorkService>("DecorateWorkService[CCC]");
                if (workServiceF == null) throw new InvalidOperationException($"{nameof(workServiceF)}=null");

                // Execute
                Console.WriteLine(workServiceA.GetValue());
                Console.WriteLine(workServiceB.GetValue());
                Console.WriteLine(workServiceC.GetValue());
                Console.WriteLine(workServiceD.GetValue());
                Console.WriteLine(workServiceE.GetValue());
                Console.WriteLine(workServiceF.GetValue());
            }
        }
    }
}
