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

namespace pestaServer.Models.social.service
{
    /// <summary>
    /// Default implementation of HandlerDispatcher.  Provides default
    /// bindings for the person, activity, and app data handlers.
    /// </summary>
    /// <remarks>
    /// <para>
    
    /// </para>
    /// </remarks>
    public class StandardHandlerDispatcher : IHandlerDispatcher
    {
        private readonly Dictionary<String, DataRequestHandler> handlers;

        /**
        * Creates a dispatcher with the standard handlers.
        * @param personHandlerProvider provider for the person handler
        * @param activityHandlerProvider provider for the activity handler
        * @param appDataHandlerProvider provider for the app data handler
        */


        public StandardHandlerDispatcher(PersonHandler personHandlerProvider,
                                         ActivityHandler activityHandlerProvider, AppDataHandler appDataHandlerProvider,
                                         MessageHandler messageHandlerProvider)
            : this(new Dictionary<string, DataRequestHandler>() 
                       { 
                           {PEOPLE_ROUTE, personHandlerProvider},
                           {ACTIVITY_ROUTE, activityHandlerProvider},
                           {APPDATA_ROUTE, appDataHandlerProvider},
                           {MESSAGE_ROUTE, messageHandlerProvider}
                       }
                )
        {
            this.handlers.Add("samplecontainer", new SampleContainerHandler());
        }

        /**
        * Creates a dispatcher with a custom list of handlers.
        * @param handlers a map of handlers by service name
        */

        private StandardHandlerDispatcher(Dictionary<String, DataRequestHandler> handlers)
        {
            this.handlers = handlers;
        }

        /**
        * Gets a handler by service name.
        */
        public override DataRequestHandler getHandler(String service) 
        {
            DataRequestHandler provider = null;
            if (!handlers.TryGetValue(service, out provider))
            {
                return null;
            }

            return provider;
        }

        /**
        * Adds a custom handler.
        */
        public void addHandler(String service, DataRequestHandler handler) 
        {
            handlers.Add(service, handler);
        }
    }
}