﻿using Autofac;
using Autofac.Builder;
using CLK.Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace MDP.Hosting
{
    public static partial class ContainerBuilderExtensions
    {
        // Methods
        public static void RegisterServiceType<TServiceType, TService, TFactory>
        (
            this ContainerBuilder container,
            Action<IRegistrationBuilder<TServiceType, IConcreteActivatorData, SingleRegistrationStyle>> configureRegistrationBuilder = null
        )
            where TServiceType : class
            where TService : class, TServiceType
            where TFactory : Factory<TService>
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion
    
            // Register-Factory
            container.RegisterType<TFactory>().As<TFactory>().SingleInstance();

            // Register-NamedService-Default
            {
                // Register
                var registrationBuilder = container.RegisterType<TService>
                (

                )
                .As<TServiceType>().Named<TServiceType>(typeof(TService).FullName);
                
                // Configure
                if (configureRegistrationBuilder != null) configureRegistrationBuilder(registrationBuilder);
            }

            // Register-DefaultService
            container.Register<TServiceType>((componentContext) =>
            {
                // Resolve
                var serviceType = componentContext.ResolveNamed<TServiceType>(typeof(TService).FullName);
                if (serviceType == null) throw new InvalidOperationException($"Service not found: type={typeof(TServiceType).FullName}, name={typeof(TService).FullName}=null");

                // Return
                return serviceType;
            })
            .As<TServiceType>();
        }

        public static void RegisterServiceType<TServiceType, TService, TFactory, TOptions>
        (
            this ContainerBuilder container,
            IConfiguration configuration,
            Action<IRegistrationBuilder<TServiceType, IConcreteActivatorData, SingleRegistrationStyle>> configureRegistrationBuilder = null
        )
            where TServiceType : class
            where TService : class, TServiceType
            where TFactory : Factory<TService, TOptions>
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configuration == null) throw new ArgumentException(nameof(configuration));

            #endregion

            // Register-Factory
            container.RegisterType<TFactory>().As<TFactory>().SingleInstance();

            // ServiceConfigList
            var serviceConfigList = configuration.GetChildren<TService, TOptions>();
            if (serviceConfigList == null) throw new InvalidOperationException($"{nameof(serviceConfigList)}=null");

            // Register-Options
            foreach (var serviceConfig in serviceConfigList)
            {
                // Register
                container.Configure<TOptions>(serviceConfig.Path, () => serviceConfig);
            }

            // Register-NamedService
            foreach (var serviceConfig in serviceConfigList)
            {
                // Register
                var registrationBuilder = container.RegisterService<TServiceType, TFactory>((factory) =>
                {
                    return factory.Create(serviceConfig.Path);
                })
                .As<TServiceType>().Named<TServiceType>(serviceConfig.Path);

                // Configure
                if (configureRegistrationBuilder != null) configureRegistrationBuilder(registrationBuilder);
            }

            // Register-NamedService-Default
            if (serviceConfigList.Find(serviceConfig => String.Equals(serviceConfig.Path, typeof(TService).FullName, StringComparison.OrdinalIgnoreCase) == true) == null)
            {
                // Register
                var registrationBuilder = container.RegisterType<TService>
                (

                )
                .As<TServiceType>().Named<TServiceType>(typeof(TService).FullName);

                // Configure
                if (configureRegistrationBuilder != null) configureRegistrationBuilder(registrationBuilder);
            }

            // Register-DefaultService
            container.Register<TServiceType>((componentContext) =>
            {
                // Resolve
                var serviceType = componentContext.ResolveNamed<TServiceType>(typeof(TService).FullName);
                if (serviceType == null) throw new InvalidOperationException($"Service not found: type={typeof(TServiceType).FullName}, name={typeof(TService).FullName}=null");

                // Return
                return serviceType;
            })
            .As<TServiceType>();
        }
    }

    public static partial class ContainerBuilderExtensions
    {
        // Methods
        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions>(this ContainerBuilder container, Func<IConfiguration> configFactory)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Return
            return container.Configure<TOptions>(Microsoft.Extensions.Options.Options.DefaultName, configFactory);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1>(this ContainerBuilder container, Func<T1, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Return
            return container.Configure<TOptions, T1>(Microsoft.Extensions.Options.Options.DefaultName, configFactory);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2>(this ContainerBuilder container, Func<T1, T2, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
            where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Return
            return container.Configure<TOptions, T1, T2>(Microsoft.Extensions.Options.Options.DefaultName, configFactory);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2, T3>(this ContainerBuilder container, Func<T1, T2, T3, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
            where T2 : class
            where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Return
            return container.Configure<TOptions, T1, T2, T3>(Microsoft.Extensions.Options.Options.DefaultName, configFactory);
        }


        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions>(this ContainerBuilder container, string name, Func<IConfiguration> configFactory)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Register
            return container.RegisterService<IConfigureOptions<TOptions>>(() =>
            {
                // Create
                return new NamedConfigureFromConfigurationOptions<TOptions>(name, configFactory());
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1>(this ContainerBuilder container, string name, Func<T1, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Register
            return container.RegisterService<IConfigureOptions<TOptions>, T1>((t1) =>
            {
                // Create
                return new NamedConfigureFromConfigurationOptions<TOptions>(name, configFactory(t1));
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2>(this ContainerBuilder container, string name, Func<T1, T2, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
            where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Register
            return container.RegisterService<IConfigureOptions<TOptions>, T1, T2>((t1, t2) =>
            {
                // Create
                return new NamedConfigureFromConfigurationOptions<TOptions>(name, configFactory(t1, t2));
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2, T3>(this ContainerBuilder container, string name, Func<T1, T2, T3, IConfiguration> configFactory)
            where TOptions : class
            where T1 : class
            where T2 : class
            where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configFactory == null) throw new ArgumentException(nameof(configFactory));

            #endregion

            // Register
            return container.RegisterService<IConfigureOptions<TOptions>, T1, T2, T3>((t1, t2, t3) =>
            {
                // Create
                return new NamedConfigureFromConfigurationOptions<TOptions>(name, configFactory(t1, t2, t3));
            })
            .SingleInstance();
        }
    }

    public static partial class ContainerBuilderExtensions
    {
        // Methods
        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions>(this ContainerBuilder container, Action<TOptions> configureOptions)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.Configure<TOptions>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1>(this ContainerBuilder container, Action<TOptions, T1> configureOptions)
           where TOptions : class
           where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.Configure<TOptions, T1>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2>(this ContainerBuilder container, Action<TOptions, T1, T2> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.Configure<TOptions, T1, T2>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2, T3>(this ContainerBuilder container, Action<TOptions, T1, T2, T3> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
           where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.Configure<TOptions, T1, T2, T3>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }


        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions>(this ContainerBuilder container, string name, Action<TOptions> configureOptions)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.RegisterService<IConfigureOptions<TOptions>>(() =>
            {
                // Create
                return new ConfigureNamedOptions<TOptions>(name, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1>(this ContainerBuilder container, string name, Action<TOptions, T1> configureOptions)
           where TOptions : class
           where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.RegisterService<IConfigureOptions<TOptions>, T1>((t1) =>
            {
                // Create
                return new ConfigureNamedOptions<TOptions, T1>(name, t1, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2>(this ContainerBuilder container, string name, Action<TOptions, T1, T2> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.RegisterService<IConfigureOptions<TOptions>, T1, T2>((t1, t2) =>
            {
                // Create
                return new ConfigureNamedOptions<TOptions, T1, T2>(name, t1, t2, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> Configure<TOptions, T1, T2, T3>(this ContainerBuilder container, string name, Action<TOptions, T1, T2, T3> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
           where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.RegisterService<IConfigureOptions<TOptions>, T1, T2, T3>((t1, t2, t3) =>
            {
                // Create
                return new ConfigureNamedOptions<TOptions, T1, T2, T3>(name, t1, t2, t3, configureOptions);
            })
            .SingleInstance();
        }
    }

    public static partial class ContainerBuilderExtensions
    {
        // Methods
        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions>(this ContainerBuilder container, Action<TOptions> configureOptions)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.PostConfigure<TOptions>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1>(this ContainerBuilder container, Action<TOptions, T1> configureOptions)
           where TOptions : class
           where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.PostConfigure<TOptions, T1>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1, T2>(this ContainerBuilder container, Action<TOptions, T1, T2> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.PostConfigure<TOptions, T1, T2>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1, T2, T3>(this ContainerBuilder container, Action<TOptions, T1, T2, T3> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
           where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Return
            return container.PostConfigure<TOptions, T1, T2, T3>(Microsoft.Extensions.Options.Options.DefaultName, configureOptions);
        }


        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions>(this ContainerBuilder container, string name, Action<TOptions> configureOptions)
            where TOptions : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.RegisterService<IPostConfigureOptions<TOptions>>(() =>
            {
                // Create
                return new PostConfigureOptions<TOptions>(name, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1>(this ContainerBuilder container, string name, Action<TOptions, T1> configureOptions)
           where TOptions : class
           where T1 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.RegisterService<IPostConfigureOptions<TOptions>, T1>((t1) =>
            {
                // Create
                return new PostConfigureOptions<TOptions, T1>(name, t1, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1, T2>(this ContainerBuilder container, string name, Action<TOptions, T1, T2> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.RegisterService<IPostConfigureOptions<TOptions>, T1, T2>((t1, t2) =>
            {
                // Create
                return new PostConfigureOptions<TOptions, T1, T2>(name, t1, t2, configureOptions);
            })
            .SingleInstance();
        }

        public static IRegistrationBuilder<IPostConfigureOptions<TOptions>, SimpleActivatorData, SingleRegistrationStyle> PostConfigure<TOptions, T1, T2, T3>(this ContainerBuilder container, string name, Action<TOptions, T1, T2, T3> configureOptions)
           where TOptions : class
           where T1 : class
           where T2 : class
           where T3 : class
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));
            if (name == null) throw new ArgumentException(nameof(name));
            if (configureOptions == null) throw new ArgumentException(nameof(configureOptions));

            #endregion

            // Register
            return container.RegisterService<IPostConfigureOptions<TOptions>, T1, T2, T3>((t1, t2, t3) =>
            {
                // Create
                return new PostConfigureOptions<TOptions, T1, T2, T3>(name, t1, t2, t3, configureOptions);
            })
            .SingleInstance();
        }
    }
}