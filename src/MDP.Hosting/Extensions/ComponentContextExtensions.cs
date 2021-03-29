﻿using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP
{
    internal static partial class ComponentContextExtensions
    {
        // Methods
        public static TResult Build<TResult>(this IComponentContext componentContext, Func<TResult> resolveAction) where TResult : notnull
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (resolveAction == null) throw new ArgumentException(nameof(resolveAction));

            #endregion

            // Resolve
            return resolveAction
            (
                
            );
        }

        public static TResult Build<T1, TResult>(this IComponentContext componentContext, Func<T1, TResult> resolveAction) where TResult : notnull
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (resolveAction == null) throw new ArgumentException(nameof(resolveAction));

            #endregion

            // Resolve
            return resolveAction
            (
                componentContext.ResolveRequired<T1>()
            );
        }

        public static TResult Build<T1, T2, TResult>(this IComponentContext componentContext, Func<T1, T2, TResult> resolveAction) where TResult : notnull
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (resolveAction == null) throw new ArgumentException(nameof(resolveAction));

            #endregion

            // Resolve
            return resolveAction
            (
                componentContext.ResolveRequired<T1>(),
                componentContext.ResolveRequired<T2>()
            );
        }

        public static TResult Build<T1, T2, T3, TResult>(this IComponentContext componentContext, Func<T1, T2, T3, TResult> resolveAction) where TResult : notnull
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (resolveAction == null) throw new ArgumentException(nameof(resolveAction));

            #endregion

            // Resolve
            return resolveAction
            (
                componentContext.ResolveRequired<T1>(),
                componentContext.ResolveRequired<T2>(),
                componentContext.ResolveRequired<T3>()
            );
        }
    }

    internal static partial class ComponentContextExtensions
    {
        // Methods
        public static TService ResolveNamed<TService>(this IComponentContext componentContext, Func<string> selectAction) where TService : notnull
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));
            if (selectAction == null) throw new ArgumentException(nameof(selectAction));

            #endregion

            // ServiceName
            var serviceName = selectAction();
            if (string.IsNullOrEmpty(serviceName) == true) throw new InvalidOperationException($"{nameof(serviceName)}=null");

            // Resolve
            return componentContext.ResolveNamed<TService>(serviceName);
        }

        public static TService ResolveRequired<TService>(this IComponentContext componentContext) where TService : notnull
        {
            #region Contracts

            if (componentContext == null) throw new ArgumentException(nameof(componentContext));

            #endregion

            // ComponentContext
            if (typeof(TService) == typeof(IComponentContext)) return (TService)componentContext;

            // Service
            var service = componentContext.Resolve<TService>();
            if (service == null) throw new InvalidOperationException($"{nameof(service)}=null");

            // Return
            return service;
        }
    }
}