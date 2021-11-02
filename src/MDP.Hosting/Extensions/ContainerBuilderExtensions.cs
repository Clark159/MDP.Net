﻿using Autofac;
using Autofac.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace MDP.Hosting
{
    public static class ContainerBuilderExtensions
    {
        // Methods   
        public static ContainerBuilder RegisterModule(this ContainerBuilder containerBuilder, string moduleAssemblyFileName = @"*.Hosting.dll")
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException(nameof(containerBuilder));
            if (string.IsNullOrEmpty(moduleAssemblyFileName) == true) throw new ArgumentException(nameof(moduleAssemblyFileName));

            #endregion

            // ModuleAssembly
            var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(moduleAssemblyFileName);
            if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

            // EntryAssembly
            var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
            if (moduleAssemblyList.Contains(entryAssembly) == false) moduleAssemblyList.Add(entryAssembly);

            // RegisterAssemblyTypes
            var moduleContainerBuilder = new ContainerBuilder();
            {
                moduleAssemblyList.ForEach(moduleAssembly =>
                {
                    moduleContainerBuilder
                        .RegisterAssemblyTypes(moduleAssembly)
                        .Where(assemblyType => typeof(MDP.Hosting.Module).IsAssignableFrom(assemblyType))
                        .As<MDP.Hosting.Module>();
                });
            }

            // RegisterModule
            using (var moduleContainer = moduleContainerBuilder.Build())
            {
                foreach (var module in moduleContainer.Resolve<IEnumerable<MDP.Hosting.Module>>())
                {
                    containerBuilder.RegisterModule(module);
                }
            }

            // Return
            return containerBuilder;
        }

        public static IRegistrationBuilder<TFactory, ReflectionActivatorData, SingleRegistrationStyle> RegisterFactory<TService, TFactory>(this ContainerBuilder containerBuilder)
            where TService : class
            where TFactory : Factory<TService>
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException(nameof(containerBuilder));

            #endregion

            // RegisterFactory
            return containerBuilder.RegisterType<TFactory>().As<Factory<TService>>();
        }

        public static IRegistrationBuilder<TService, SimpleActivatorData, SingleRegistrationStyle> RegisterService<TService>(this ContainerBuilder containerBuilder)
               where TService : class
        {
            #region Contracts

            if (containerBuilder == null) throw new ArgumentException(nameof(containerBuilder));

            #endregion

            // Register
            return containerBuilder.Register<TService>(componentContext =>
            {
                // ServiceFactoryList
                var serviceFactoryList = componentContext.Resolve<IEnumerable<Factory<TService>>>();
                if (serviceFactoryList == null) throw new InvalidOperationException($"{nameof(serviceFactoryList)}=null");

                // Create
                foreach (var serviceFactory in serviceFactoryList)
                {
                    // Service
                    var service = serviceFactory.Create(componentContext);
                    if (service != null) return service;
                }

                // Throw
                {
                    // Exception
                    throw new InvalidOperationException($"Service not exists: serviceName={typeof(TService).FullName}");
                }
            }).IfNotRegistered(typeof(TService));
        }
    }
}