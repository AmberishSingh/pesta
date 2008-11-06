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
using System.Data;
using System.Configuration;
using System.Text;
using System.Web;

namespace Pesta
{
    /// <summary>
    /// Summary description for GadgetOAuthTokenStore
    /// </summary>
    /// <remarks>
    /// <para>
    ///  Apache Software License 2.0 2008 Shindig, ported to C# by Sean Lin M.T. (my6solutions.com)
    /// </para>
    /// </remarks>
    public class GadgetOAuthTokenStore
    {
        private OAuthStore store;
        private GadgetSpecFactory specFactory;

        /**
         * Public constructor.
         *
         * @param store an {@link OAuthStore} that can store and retrieve OAuth
         *              tokens, as well as information about service providers.
         */

        public GadgetOAuthTokenStore(OAuthStore store, GadgetSpecFactory specFactory)
        {
            this.store = store;
            this.specFactory = specFactory;
        }

        /**
         * Retrieve an AccessorInfo and OAuthAccessor that are ready for signing OAuthMessages.  To do
         * this, we need to figure out:
         * 
         * - what consumer key/secret to use for signing.
         * - if an access token should be used for the request, and if so what it is.   *   
         * - the OAuth request/authorization/access URLs.
         * - what HTTP method to use for request token and access token requests
         * - where the OAuth parameters are located.
         * 
         * Note that most of that work gets skipped for signed fetch, we just look up the consumer key
         * and secret for that.  Signed fetch always sticks the parameters in the query string.
         */
        public AccessorInfo getOAuthAccessor(SecurityToken securityToken,
                                                OAuthArguments arguments, OAuthClientState clientState)
        {
            AccessorInfoBuilder accessorBuilder = new AccessorInfoBuilder();

            // Does the gadget spec tell us any details about the service provider, like where to put the
            // OAuth parameters and what methods to use for their URLs?
            OAuthServiceProvider provider = null;
            if (arguments.MayUseToken())
            {
                provider = lookupSpecInfo(securityToken, arguments, accessorBuilder);
            }
            else
            {
                // This is plain old signed fetch.
                accessorBuilder.setParameterLocation(AccessorInfo.OAuthParamLocation.URI_QUERY);
            }

            // What consumer key/secret should we use?
            OAuthStore.ConsumerInfo consumer = store.getConsumerKeyAndSecret(
                                        securityToken, arguments.ServiceName, provider);
            accessorBuilder.setConsumer(consumer);

            // Should we use the OAuth access token?  We never do this unless the client allows it, and
            // if owner == viewer.
            if (arguments.MayUseToken()
                    && securityToken.getOwnerId() != null
                    && securityToken.getViewerId().Equals(securityToken.getOwnerId()))
            {
                lookupToken(securityToken, consumer, arguments, clientState, accessorBuilder);
            }

            return accessorBuilder.create();
        }

        /**
         * Lookup information contained in the gadget spec.
         */
        private OAuthServiceProvider lookupSpecInfo(SecurityToken securityToken, OAuthArguments arguments,
                                                        AccessorInfoBuilder accessorBuilder)
        {
            GadgetSpec spec = findSpec(securityToken, arguments);
            OAuthSpec oauthSpec = spec.getModulePrefs().getOAuthSpec();
            if (oauthSpec == null)
            {
                throw oauthNotFoundEx(securityToken);
            }
            OAuthService service = oauthSpec.getServices()[arguments.ServiceName];
            if (service == null)
            {
                throw serviceNotFoundEx(securityToken, oauthSpec, arguments.ServiceName);
            }
            // In theory some one could specify different parameter locations for request token and
            // access token requests, but that's probably not useful.  We just use the request token
            // rules for everything.
            accessorBuilder.setParameterLocation(getStoreLocation(service.getRequestUrl().location));
            accessorBuilder.setMethod(getStoreMethod(service.getRequestUrl().method));
            OAuthServiceProvider provider = new OAuthServiceProvider(
                                                    service.getRequestUrl().url.toASCIIString(),
                                                    service.getAuthorizationUrl().toASCIIString(),
                                                    service.getAccessUrl().url.toASCIIString());
            return provider;
        }

        /**
         * Figure out the OAuth token that should be used with this request.  We check for this in three
         * places.  In order of priority:
         * 
         * 1) From information we cached on the client.
         *    We encrypt the token and cache on the client for performance.
         *    
         * 2) From information we have in our persistent state.
         *    We persist the token server-side so we can look it up if necessary.
         *    
         * 3) From information the gadget developer tells us to use (a preapproved request token.)
         *    Gadgets can be initialized with preapproved request tokens.  If the user tells the service
         *    provider they want to add a gadget to a gadget container site, the service provider can
         *    create a preapproved request token for that site and pass it to the gadget as a user
         *    preference.
         * @throws GadgetException 
         */
        private void lookupToken(SecurityToken securityToken, OAuthStore.ConsumerInfo consumerInfo,
                OAuthArguments arguments, OAuthClientState clientState, AccessorInfoBuilder accessorBuilder)
        {
            if (clientState.getRequestToken() != null)
            {
                // We cached the request token on the client.
                accessorBuilder.setRequestToken(clientState.getRequestToken());
                accessorBuilder.setTokenSecret(clientState.getRequestTokenSecret());
            }
            else if (clientState.getAccessToken() != null)
            {
                // We cached the access token on the client
                accessorBuilder.setAccessToken(clientState.getAccessToken());
                accessorBuilder.setTokenSecret(clientState.getAccessTokenSecret());
            }
            else
            {
                // No useful client-side state, check persistent storage
                OAuthStore.TokenInfo tokenInfo = store.getTokenInfo(securityToken, consumerInfo,
                arguments.ServiceName, arguments.TokenName);
                if (tokenInfo != null && tokenInfo.getAccessToken() != null)
                {
                    // We have an access token in persistent storage, use that.
                    accessorBuilder.setAccessToken(tokenInfo.getAccessToken());
                    accessorBuilder.setTokenSecret(tokenInfo.getTokenSecret());
                }
                else
                {
                    // We don't have an access token yet, but the client sent us a (hopefully) preapproved
                    // request token.
                    accessorBuilder.setRequestToken(arguments.RequestToken);
                    accessorBuilder.setTokenSecret(arguments.RequestTokenSecret);
                }
            }
        }

        private AccessorInfo.OAuthParamLocation getStoreLocation(OAuthService.Location location)
        {
            if (location == OAuthService.Location.HEADER)
            {
                return AccessorInfo.OAuthParamLocation.AUTH_HEADER;
            }
            else if (location == OAuthService.Location.URL)
            {
                return AccessorInfo.OAuthParamLocation.URI_QUERY;
            }
            else if (location == OAuthService.Location.BODY)
            {
                return AccessorInfo.OAuthParamLocation.POST_BODY;
            }
            throw new GadgetException(GadgetException.Code.INTERNAL_SERVER_ERROR,
                            "Unknown parameter location " + location);
        }

        private AccessorInfo.HttpMethod getStoreMethod(OAuthService.Method method)
        {
            if (method == OAuthService.Method.GET)
            {
                return AccessorInfo.HttpMethod.GET;
            }
            else if (method == OAuthService.Method.POST)
            {
                return AccessorInfo.HttpMethod.POST;
            }
            throw new GadgetException(GadgetException.Code.INTERNAL_SERVER_ERROR,
                            "Unknown method " + method);
        }

        private GadgetSpec findSpec(SecurityToken securityToken, OAuthArguments arguments)
        {
            try
            {
                return specFactory.getGadgetSpec(new java.net.URI(securityToken.getAppUrl()),
                                    arguments.BypassSpecCache);
            }
            catch (UriFormatException e)
            {
                throw new UserVisibleOAuthException("could not fetch gadget spec, gadget URI invalid", e);
            }
        }

        private GadgetException serviceNotFoundEx(SecurityToken securityToken, OAuthSpec oauthSpec, String serviceName)
        {
            StringBuilder message = new StringBuilder()
                        .Append("Spec for gadget ")
                        .Append(securityToken.getAppUrl())
                        .Append(" does not contain OAuth service ")
                        .Append(serviceName)
                        .Append(".  Known services: ");
            string[] temp = new string[oauthSpec.getServices().Keys.Count];
            oauthSpec.getServices().Keys.CopyTo(temp, 0);
            message.Append(String.Join(",", temp));
            return new UserVisibleOAuthException(message.ToString());
        }

        private GadgetException oauthNotFoundEx(SecurityToken securityToken)
        {
            StringBuilder message = new StringBuilder()
                        .Append("Spec for gadget ")
                        .Append(securityToken.getAppUrl())
                        .Append(" does not contain OAuth element.");
            return new UserVisibleOAuthException(message.ToString());
        }

        /**
         * Store an access token for the given user/gadget/service/token name
         */
        public void storeTokenKeyAndSecret(SecurityToken securityToken, OAuthStore.ConsumerInfo consumerInfo,
                                    OAuthArguments arguments, OAuthStore.TokenInfo tokenInfo)
        {
            store.setTokenInfo(securityToken, consumerInfo, arguments.ServiceName,
                                arguments.TokenName, tokenInfo);
        }

        /**
         * Remove an access token for the given user/gadget/service/token name
         */
        public void removeToken(SecurityToken securityToken, OAuthStore.ConsumerInfo consumerInfo, OAuthArguments arguments)
        {
            store.removeToken(securityToken, consumerInfo, arguments.ServiceName, arguments.TokenName);
        }
    } 
}
