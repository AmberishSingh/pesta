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
using System.Net;
using System.Web;
using System.Web.Mvc;
using Pesta.Engine.auth;
using Pesta.Engine.social.core.oauth;
using Pesta.Engine.social.oauth;

namespace pestaServer.ActionFilters
{
    public class AuthenticationFilter : ActionFilterAttribute
    {
        private readonly List<IAuthenticationHandler> handlers = new List<IAuthenticationHandler> 
                                                           { 
                                                               new UrlParameterAuthenticationHandler(),
                                                               new AnonymousAuthenticationHandler(),
                                                               new OAuthConsumerRequestAuthenticationHandler(new SampleContainerOAuthLookupService())
                                                           };
        // At some point change this to a container specific realm
        private const String realm = "shindig";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            HttpRequestBase request = filterContext.HttpContext.Request;
            HttpResponseBase response = filterContext.HttpContext.Response;
            try
            {

                foreach (IAuthenticationHandler handler in handlers)
                {
                    ISecurityToken token = handler.getSecurityTokenFromRequest(HttpContext.Current.Request);
                    if (token != null)
                    {
                        new AuthInfo(HttpContext.Current, request.RawUrl).setAuthType(handler.getName()).setSecurityToken(
                            token);
                        return;
                    }
                    String authHeader = handler.getWWWAuthenticateHeader(realm);
                    if (authHeader != null)
                    {
                        response.AddHeader("WWW-Authenticate", authHeader);
                    }
                }
            }
            catch (IAuthenticationHandler.InvalidAuthenticationException iae)
            {
                if (iae.getAdditionalHeaders() != null)
                {
                    foreach (var entry in iae.getAdditionalHeaders())
                    {
                        response.AddHeader(entry.Key, entry.Value);
                    }
                }
                if (iae.getRedirect() != null)
                {
                    response.Redirect(iae.getRedirect());
                }
                else
                {
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.StatusDescription = iae.Message;
                }
            }
        }
    }
}