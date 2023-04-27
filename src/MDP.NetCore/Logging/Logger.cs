﻿using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.NetCore
{
    internal class Logger<TCategoryName> : MDP.Logging.ILogger<TCategoryName>
    {
        // Fields
        private readonly Microsoft.Extensions.Logging.ILogger<TCategoryName> _logger;


        // Constructors
        public Logger(Microsoft.Extensions.Logging.ILogger<TCategoryName> logger)
        {
            #region Contracts

            if (logger == null) throw new ArgumentException($"{nameof(logger)}=null");

            #endregion

            // Default
            _logger = logger;
        }


        // Debug
        public void LogDebug(string? message, params object?[] args)
        {
            _logger.LogDebug(message, args);
        }

        public void LogDebug(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogDebug(exception, message, args);
        }

        // Trace
        public void LogTrace(string? message, params object?[] args)
        {
            _logger.LogTrace(message, args);
        }

        public void LogTrace(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogTrace(exception, message, args);
        }

        // Information
        public void LogInformation(string? message, params object?[] args)
        {
            _logger.LogInformation(message, args);
        }

        public void LogInformation(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogInformation(exception, message, args);
        }

        // Warning
        public void LogWarning(string? message, params object?[] args)
        {
            _logger.LogWarning(message, args);
        }

        public void LogWarning(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogWarning(exception, message, args);
        }

        // Error
        public void LogError(string? message, params object?[] args)
        {
            _logger.LogError(message, args);
        }

        public void LogError(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogError(exception, message, args);
        }

        // Critical
        public void LogCritical(string? message, params object?[] args)
        {
            _logger.LogCritical(message, args);
        }

        public void LogCritical(Exception? exception, string? message, params object?[] args)
        {
            _logger.LogCritical(exception, message, args);
        }
    }
}