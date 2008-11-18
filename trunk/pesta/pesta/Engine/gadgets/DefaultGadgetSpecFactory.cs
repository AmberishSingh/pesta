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
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using URI = System.Uri;

namespace Pesta
{
    /// <summary>
    /// Summary description for DefaultGadgetSpecFactory
    /// </summary>
    /// <remarks>
    /// <para>
    ///  Apache Software License 2.0 2008 Shindig, ported to C# by Sean Lin M.T. (my6solutions.com)
    /// </para>
    /// </remarks>
    public class DefaultGadgetSpecFactory : GadgetSpecFactory
    {
        public static readonly String CACHE_NAME = "gadgetSpecs";
        static readonly String RAW_GADGETSPEC_XML_PARAM_NAME = "rawxml";
        static readonly Uri RAW_GADGET_URI = Uri.parse("http://localhost/raw.xml");
        static readonly String ERROR_SPEC = "<Module><ModulePrefs title='Error'/><Content/></Module>";
        static readonly String ERROR_KEY = "parse.exception";

        private readonly HttpFetcher fetcher;
        //private readonly SoftExpiringCache<Uri, GadgetSpec> cache;
        private readonly long refresh;

        public static readonly DefaultGadgetSpecFactory Instance = new DefaultGadgetSpecFactory();
        protected DefaultGadgetSpecFactory()
        {
            fetcher = BasicHttpFetcher.Instance;
            this.refresh = long.Parse(ContainerConfig.getConfigurationValue("gadget.cache.xml.refreshInterval"));

        }

        public GadgetSpec getGadgetSpec(GadgetContext context)
        {
            String rawxml = context.getParameter(RAW_GADGETSPEC_XML_PARAM_NAME);
            if (rawxml != null)
            {
                // Set URI to a fixed, safe value (localhost), preventing a gadget rendered
                // via raw XML (eg. via POST) to be rendered on a locked domain of any other
                // gadget whose spec is hosted non-locally.
                return new GadgetSpec(RAW_GADGET_URI, rawxml);
            }
            return getGadgetSpec(context.getUrl(), context.getIgnoreCache());
        }
        public GadgetSpec getGadgetSpec(URI gadgetUri, bool ignoreCache)
        {
            Uri uri = Uri.fromJavaUri(gadgetUri);
            if (ignoreCache)
            {
                return fetchObjectAndCache(uri, ignoreCache);
            }

            GadgetSpec cached = HttpRuntime.Cache[gadgetUri.ToString()] as GadgetSpec;

            GadgetSpec spec = null;
            if (cached == null)
            {
                try
                {
                    spec = fetchObjectAndCache(uri, ignoreCache);
                }
                catch (GadgetException e)
                {
                    // Enforce negative caching.
                    if (cached != null)
                    {
                        spec = cached;
                        Debug.Assert(spec != null);
                    }
                    else
                    {
                        // We create this dummy spec to avoid the cost of re-parsing when a remote site is out.
                        spec = new GadgetSpec(uri, ERROR_SPEC);
                        spec.setAttribute(ERROR_KEY, e);
                    }
                    HttpRuntime.Cache.Insert(uri.ToString(), spec, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromTicks(refresh));
                }
            }
            else
            {
                spec = cached;
            }

            GadgetException exception = (GadgetException)spec.getAttribute(ERROR_KEY);
            if (exception != null)
            {
                throw exception;
            }
            return spec;
        }

        private GadgetSpec fetchObjectAndCache(Uri url, bool ignoreCache)
        {
            sRequest request = new sRequest(url).setIgnoreCache(ignoreCache);
            // Since we don't allow any variance in cache time, we should just force the cache time
            // globally. This ensures propagation to shared caches when this is set.
            request.setCacheTtl((int)(refresh / 1000));

            sResponse response = fetcher.fetch(request);
            if (response.getHttpStatusCode() != (int)HttpStatusCode.OK)
            {
                throw new GadgetException(GadgetException.Code.FAILED_TO_RETRIEVE_CONTENT,
                                          "Unable to retrieve gadget xml. HTTP error " +
                                          response.getHttpStatusCode());
            }

            GadgetSpec spec = new GadgetSpec(url, response.responseString);
            HttpRuntime.Cache.Insert(url.ToString(), spec, null, System.Web.Caching.Cache.NoAbsoluteExpiration, TimeSpan.FromTicks(refresh));
            return spec;
        }
    } 
}