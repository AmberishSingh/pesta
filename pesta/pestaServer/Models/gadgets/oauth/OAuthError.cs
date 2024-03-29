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

namespace pestaServer.Models.gadgets.oauth
{
    /// <remarks>
    /// <para>
    
    /// </para>
    /// </remarks>

    /**
     * Error strings to be returned to gadgets as "oauthError" data.
     */
    public enum OAuthError
    {
        /**
         * The request cannot be completed because the OAuth configuration for
         * the gadget is incorrect.
         */
        BAD_OAUTH_CONFIGURATION,

        /**
         * The request cannot be completed for an unspecified reason.
         */
        UNKNOWN_PROBLEM,

        /**
         * The user is not authenticated.
         */
        UNAUTHENTICATED,

        /**
         * The user is not the owner of the page.
         */
        NOT_OWNER,

        /**
        * The request cannot be completed because the request options were invalid.
        */
        INVALID_REQUEST
    }
}