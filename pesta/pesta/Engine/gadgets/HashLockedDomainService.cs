#region License, Terms and Conditions
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
using System.Collections.Generic;
using System;
using System.Text;
using System.Security.Cryptography;
using Pesta.Engine.common;
using Pesta.Engine.gadgets.spec;

namespace Pesta.Engine.gadgets
{
    /// <summary>
    /// Locked domain implementation based on sha1.
    /// The generated domain takes the form:
    /// base32(sha1(gadget url)).
    /// Other domain locking schemes are possible as well.
    /// </summary>
    /// <remarks>
    /// <para>
    ///  Apache Software License 2.0 2008 Shindig
    /// </para>
    /// </remarks>
    public class HashLockedDomainService : LockedDomainService
    {
        private readonly bool enabled;
        private readonly Dictionary<String, String> lockedSuffixes;
        private readonly Dictionary<String, Boolean> required;

        public static readonly String LOCKED_DOMAIN_REQUIRED_KEY = "gadgets.lockedDomainRequired";
        public static readonly String LOCKED_DOMAIN_SUFFIX_KEY = "gadgets.lockedDomainSuffix";


        public static readonly HashLockedDomainService Instance = new HashLockedDomainService(JsonContainerConfig.Instance, false);
        /**
        * Create a LockedDomainService
        * @param config per-container configuration
        * @param embedHost host name to use for embedded content
        * @param enabled whether this service should do anything at all.
        */
        public HashLockedDomainService(ContainerConfig config, bool enabled)
        {
            this.enabled = enabled;
            lockedSuffixes = new Dictionary<string,string>();
            required = new Dictionary<string,bool>();
            ICollection<String> containers = config.getContainers();
            if (enabled) 
            {
                foreach(String container in containers) 
                {
                    String suffix = config.get(container, LOCKED_DOMAIN_SUFFIX_KEY);
                    if (suffix == null)
                    {

                    } 
                    else 
                    {
                        lockedSuffixes.Add(container, suffix);
                    }
                    String require = config.get(container, LOCKED_DOMAIN_REQUIRED_KEY);
                    required.Add(container, "true".Equals(require));
                }
            }
        }

        public bool isEnabled()
        {
            return enabled;
        }

        public bool isSafeForOpenProxy(String host)
        {
            if (enabled)
            {
                return !hostRequiresLockedDomain(host);
            }
            return true;
        }

        public bool gadgetCanRender(String host, GadgetSpec gadget, String container)
        {
            container = normalizeContainer(container);
            if (enabled)
            {
                if (gadgetWantsLockedDomain(gadget) ||
                    hostRequiresLockedDomain(host) ||
                    containerRequiresLockedDomain(container))
                {
                    String neededHost = getLockedDomain(gadget, container);
                    return host.Equals(neededHost);
                }
            }
            return true;
        }

        public String getLockedDomainForGadget(GadgetSpec gadget, String container)
        {
            container = normalizeContainer(container);
            if (enabled)
            {
                if (gadgetWantsLockedDomain(gadget) ||
                    containerRequiresLockedDomain(container))
                {
                    return getLockedDomain(gadget, container);
                }
            }
            return null;
        }

        private String getLockedDomain(GadgetSpec gadget, String container)
        {
            String suffix;
            if (!lockedSuffixes.TryGetValue(container, out suffix))
            {
                return null;
            }
            String hash = SHA1.Create().ComputeHash(Encoding.Default.GetBytes(gadget.getUrl().ToString())).ToString();
            return hash + suffix;
        }

        private bool gadgetWantsLockedDomain(GadgetSpec gadget)
        {
            return gadget.getModulePrefs().getFeatures().ContainsKey("locked-domain");
        }

        private bool hostRequiresLockedDomain(String host) 
        {
            foreach(String suffix in lockedSuffixes.Values) 
            {
                if (host.EndsWith(suffix))
                {
                    return true;
                }
            }
            return false;
        }

        private bool containerRequiresLockedDomain(String container)
        {
            bool dummy;
            return required.TryGetValue(container, out dummy);
        }

        private String normalizeContainer(String container)
        {
            if (required.ContainsKey(container))
            {
                return container;
            }
            return ContainerConfig.DEFAULT_CONTAINER;
        }
    }
}