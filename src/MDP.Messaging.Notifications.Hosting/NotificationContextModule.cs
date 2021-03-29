﻿using Autofac;
using MDP.Messaging.Notifications.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP;
using MDP.Messaging.Notifications.Accesses;
using MDP.Messaging.Notifications.Firebase;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace MDP.Messaging.Notifications.Hosting
{
    public class NotificationContextModule : MDP.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // NotificationContext
            {
                // Register
                container.RegisterType<NotificationContext>().As<NotificationContext>()

                // Start
                .OnActivated((handler) =>
                {

                })

                // Lifetime
                .AutoActivate().SingleInstance();
            }

            // RegistrationRepository
            {
                // Register
                container.RegisterSelected<IConfiguration, RegistrationRepository>(configuration =>
                {
                    // Configuration
                    return configuration.GetServiceName<RegistrationRepository>();
                });
                container.RegisterNamed<MockRegistrationRepository, RegistrationRepository>();
                container.RegisterNamed<SqlRegistrationRepository, RegistrationRepository>();
            }

            // NotificationProvider
            {
                container.RegisterSelected<IConfiguration, NotificationProvider>(configuration =>
                {
                    // Configuration
                    return configuration.GetServiceName<NotificationProvider>();
                });
                container.RegisterNamed<MockNotificationProvider, NotificationProvider>();
                container.RegisterNamed<FirebaseNotificationProvider, NotificationProvider>();
            }
        }
    }
}
