﻿#region License, Terms and Conditions
/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements. See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership. The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License. You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied. See the License for the
 * specific language governing permissions and limitations under the License.
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using Pesta.Engine.auth;
using Pesta.Engine.common.crypto;
using Pesta.Utilities;

namespace pesta.Auth
{
    public class BasicSecurityToken : ISecurityToken
    {
        /** serialized form of the token */
        private readonly String token;

        /** data from the token */
        private readonly Dictionary<String, String> tokenData;

        /** tool to use for signing and encrypting the token */
        private readonly BlobCrypter crypter = new BasicBlobCrypter(Encoding.UTF8.GetBytes(PestaSettings.TokenMasterKey));

        private const string OWNER_KEY = "o";
        private const string APP_KEY = "a";
        private const string VIEWER_KEY = "v";
        private const string DOMAIN_KEY = "d";
        private const string APPURL_KEY = "u";
        private const string MODULE_KEY = "m";
        private const String CONTAINER_KEY = "c";

        /**
        * {@inheritDoc}
        */
        public String toSerialForm()
        {
            return token;
        }

        /**
        * Generates a token from an input string
        * @param token String form of token
        * @param maxAge max age of the token (in seconds)
        * @throws BlobCrypterException
        */
        public BasicSecurityToken(String token, int maxAge)
        {
            this.token = token;
            tokenData = crypter.unwrap(token, maxAge);
        }

        public BasicSecurityToken(String owner, String viewer, String app,
                                  String domain, String appUrl, String moduleId, String container)
        {
            tokenData = new Dictionary<String, String>();
            putNullSafe(OWNER_KEY, owner);
            putNullSafe(VIEWER_KEY, viewer);
            putNullSafe(APP_KEY, app);
            putNullSafe(DOMAIN_KEY, domain);
            putNullSafe(APPURL_KEY, appUrl);
            putNullSafe(MODULE_KEY, moduleId);
            putNullSafe(CONTAINER_KEY, container);
            token = crypter.wrap(tokenData);
        }

        static public BasicSecurityToken createFromToken(string token, int maxAge) 
        {
            return new BasicSecurityToken(token, maxAge);
        }

        /**
        * Generates a token from an input array of values
        * @param owner owner of this gadget
        * @param viewer viewer of this gadget
        * @param app application id
        * @param domain domain of the container
        * @param appUrl url where the application lives
        * @param moduleId module id of this gadget 
        * @throws BlobCrypterException 
        */
        static public BasicSecurityToken createFromValues(string owner, string viewer, string app, string domain, string appUrl, string moduleId, string container) 
        {
            return new BasicSecurityToken(owner, viewer, app, domain, appUrl, moduleId, container);
        }

        private void putNullSafe(String key, String value)
        {
            if (value != null)
            {
                tokenData.Add(key, value);
            }
        }

        /**
        * {@inheritDoc}
        */
        public String getAppId()
        {
            return tokenData[APP_KEY];
        }

        /**
        * {@inheritDoc}
        */
        public String getDomain()
        {
            return tokenData[DOMAIN_KEY];
        }

        /**
         * {@inheritDoc}
         */
        public String getContainer()
        {
            return tokenData[CONTAINER_KEY];
        }

        /**
        * {@inheritDoc}
        */
        public String getOwnerId()
        {
            return tokenData[OWNER_KEY];
        }

        /**
        * {@inheritDoc}
        */
        public String getViewerId()
        {
            return tokenData[VIEWER_KEY];
        }

        /**
        * {@inheritDoc}
        */
        public String getAppUrl()
        {
            return tokenData[APPURL_KEY];
        }

        /**
        * {@inheritDoc}
        */
        public long getModuleId()
        {
            long modid;
            if (!long.TryParse(tokenData[MODULE_KEY], out modid))
            {
                modid = Convert.ToInt64(tokenData[MODULE_KEY], 16);
            }
            return modid;
        }

        /**
        * {@inheritDoc}
        */
        public String getUpdatedToken()
        {
            return null;
        }

        /**
        * {@inheritDoc}
        */
        public String getTrustedJson()
        {
            return null;
        }

        /**
        * {@inheritDoc}
        */
        public bool isAnonymous()
        {
            return false;
        }
    }
}