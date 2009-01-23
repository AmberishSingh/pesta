﻿/*
 * Copyright 2007 Netflix, Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;

namespace Pesta.Interop.oauth
{
    /// <summary>
    /// Summary description for OAuthException
    /// </summary>
    /// <remarks>
    /// <para>
    ///  Apache Software License 2.0 2008 Shindig ported to Pesta by Sean Lin M.T. (my6solutions.com)
    /// </para>
    /// </remarks>
    public class OAuthException : Exception
    {
        /**
         * For subclasses only.
         */
        protected OAuthException()
        {
        }

        /**
         * @param message
         */
        public OAuthException(String message)
            : base(message)
        {

        }

        /**
         * @param cause
         */
        public OAuthException(Exception cause)
            : base("", cause)
        {

        }

        /**
         * @param message
         * @param cause
         */
        public OAuthException(String message, Exception cause)
            : base(message, cause)
        {

        }
    }
}