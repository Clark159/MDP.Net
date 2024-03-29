﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDP;
using System.IO;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace MDP.FirebaseAdmin.Hosting
{
    public class FirebaseAdminModule : MDP.Hosting.Module
    {
        // Methods
        protected override void ConfigureContainer(ContainerBuilder container)
        {
            #region Contracts

            if (container == null) throw new ArgumentException(nameof(container));

            #endregion

            // EntryDirectory
            var entryDirectory = AppContext.BaseDirectory;
            if (Directory.Exists(entryDirectory) == false) throw new InvalidOperationException($"{nameof(entryDirectory)}=null");

            // CredentialFilePath
            var credentialFilePath = Path.Combine(entryDirectory, @"FirebaseAdmin.GoogleCredential.json");
            if (string.IsNullOrEmpty(credentialFilePath) == true) throw new InvalidOperationException($"{nameof(credentialFilePath)}=null");
            if (File.Exists(credentialFilePath) == false) return;

            // FirebaseApp
            container.Register<FirebaseApp>(componentContext =>
            {
                // Create
                return FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile(credentialFilePath)
                });

            }).SingleInstance();

            // FirebaseMessaging
            container.Register<FirebaseMessaging>(componentContext =>
            {
                // Require
                componentContext.Resolve<FirebaseApp>();

                // Create
                return FirebaseMessaging.DefaultInstance;

            }).SingleInstance();
        }
    }
}
