﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public abstract class IdentityService
    {
        // Constructors
        internal void Initialize
        (
            RoleRepository roleRepository,
            UserRepository userRepository,
            UserRoleRepository userRoleRepository,
            UserLoginRepository userLoginRepository,
            UserTokenRepository userTokenRepository
        )
        {
            #region Contracts

            if (roleRepository == null) throw new ArgumentException(nameof(roleRepository));
            if (userRepository == null) throw new ArgumentException(nameof(userRepository));
            if (userRoleRepository == null) throw new ArgumentException(nameof(userRoleRepository));
            if (userLoginRepository == null) throw new ArgumentException(nameof(userLoginRepository));
            if (userTokenRepository == null) throw new ArgumentException(nameof(userTokenRepository));

            #endregion

            // Default
            this.RoleRepository = roleRepository;
            this.UserRepository = userRepository;
            this.UserRoleRepository = userRoleRepository;
            this.UserLoginRepository = userLoginRepository;
            this.UserTokenRepository = userTokenRepository;
        }


        // Properties
        protected RoleRepository RoleRepository { get; private set; }

        protected UserRepository UserRepository { get; private set; }

        protected UserRoleRepository UserRoleRepository { get; private set; }

        protected UserLoginRepository UserLoginRepository { get; private set; }

        protected UserTokenRepository UserTokenRepository { get; private set; }
    }
}
