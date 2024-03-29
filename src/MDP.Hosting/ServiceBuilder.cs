﻿using System;

namespace MDP.Hosting
{
    internal abstract partial class ServiceBuilder
    {
        // Properties
        public abstract string InstanceName { get; }


        // Methods
        public abstract object Resolve(IServiceProvider serviceProvider);
    }

    internal class ServiceBuilder<TService> : ServiceBuilder
        where TService : class
    {
        // Fields
        private readonly string _instanceName;

        private readonly Func<IServiceProvider, object> _resolveAction;

        private object _instance = null;


        // Constructors
        public ServiceBuilder(string instanceName, Func<IServiceProvider, object> resolveAction)
        {
            #region Contracts

            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");
            if (resolveAction == null) throw new ArgumentException($"{nameof(resolveAction)}=null");

            #endregion

            // Default
            _instanceName = instanceName;
            _resolveAction = resolveAction;
        }


        // Properties
        public override string InstanceName { get { return _instanceName; } }


        // Methods
        public override object Resolve(IServiceProvider serviceProvider)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");

            #endregion

            // Require
            if (_instance != null) return _instance;

            // Resolve
            var instance = _resolveAction(serviceProvider);
            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

            // Attach
            _instance = instance;

            // Return
            return instance;
        }
    }
}