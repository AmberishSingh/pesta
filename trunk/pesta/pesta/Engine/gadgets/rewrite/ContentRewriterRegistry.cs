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
using System.Web;
using System.Net;

namespace Pesta
{
    /// <summary>
    /// Summary description for ContentRewriterRegistry
    /// </summary>
    /// <remarks>
    /// <para>
    ///  Apache Software License 2.0 2008 Shindig, ported to C# by Sean Lin M.T. (my6solutions.com)
    /// </para>
    /// </remarks>
    public interface ContentRewriterRegistry
    {
        /**
        * Rewrites a {@code Gadget} object given the registered rewriters.
        * @param gadget Gadget object to use as a rewriting context.
        * @param currentView The gadget view to rewrite
        * @return The rewritten content.
        * @throws GadgetException Potentially passed through from rewriters
        */
        String rewriteGadget(Gadget gadget, View currentView);

        /**
        * Rewrites a {@code Gadget} object given the registered rewriters.
        * @param gadget Gadget object to use as a rewriting context.
        * @param content The content to be rewritten.
        * @return The rewritten content.
        * @throws GadgetException Potentially passed through from rewriters
        */
        String rewriteGadget(Gadget gadget, String content);

        /**
        * Rewrites an {@code HttpResponse} object with the given request as context,
        * using the registered rewriters.
        * @param req Request object for context.
        * @param resp Original response object.
        * @return Rewritten response object, or resp if not modified.
        */
        sResponse rewriteHttpResponse(sRequest req, sResponse resp);
    } 
}
