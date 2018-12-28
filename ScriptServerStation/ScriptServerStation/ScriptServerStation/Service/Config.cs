// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Collections.Generic;

namespace ScriptServerStation.Service
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResourceResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(), //必须要添加，否则报无效的scope错误
                new IdentityResources.Profile()
            };
        }
        // scopes define the API resources in your system
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("myApi", "My API")
            };
        }

        // clients want to access resources (aka scopes)
        public static IEnumerable<Client> GetClients()
        {
            // client credentials client
            return new List<Client>
            {
                new Client
                {
                    ClientId = "web",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    ClientSecrets = 
                    {
                        new Secret("MyWebSecret".Sha256())
                    },
                    AllowedScopes = { "myApi",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                  IdentityServerConstants.StandardScopes.Profile},
                    
                },

                // resource owner password grant client
                new Client
                {
                    ClientId = "wpf",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets = 
                    {
                        new Secret("MyWpfSecret".Sha256())
                    },
                    AllowedScopes = { "myApi",IdentityServerConstants.StandardScopes.OpenId, //必须要添加，否则报forbidden错误
                  IdentityServerConstants.StandardScopes.Profile }
                }
            };
        }
    }
}