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

namespace pestaServer.Models.social.service
{
    /// <summary>
    /// Dispatcher for DataRequestHandler requests.  The default implementation
    /// registers the three standard Shindig handlers.
    /// Add a custom binding of this interface to customize request handling.
    /// </summary>
    /// <remarks>
    /// <para>
    
    /// </para>
    /// </remarks>
    public abstract class IHandlerDispatcher
    {
        public const String PEOPLE_ROUTE = "people";
        public const String ACTIVITY_ROUTE = "activities";
        public const String APPDATA_ROUTE = "appdata";
        public const String MESSAGE_ROUTE = "messages";

        /**
        * @param service a service name
        * @return the handler, or null if no handler is registered for that service
        */
        public abstract DataRequestHandler getHandler(String service);
    }
}