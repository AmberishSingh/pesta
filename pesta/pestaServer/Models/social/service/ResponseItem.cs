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
using System.Net;
using Pesta.Engine.social;

namespace Pesta.Engine.protocol
{
    /// <summary>
    /// Summary description for ResponseItem
    /// </summary>
    /// <remarks>
    /// <para>
    
    /// </para>
    /// </remarks>
    public class ResponseItem
    {
        /**
   * The error code associated with the item.
   */
        private readonly int errorCode;

        /**
         * The error message.
         */
        private readonly String errorMessage;

        /**
         * The response value.
         */
        private readonly Object response;

        /**
         * Create a ResponseItem specifying the ResponseError and error Message.
         * @param errorCode an RPC error code
         * @param errorMessage the Error Message
         */
        public ResponseItem(int errorCode, String errorMessage)
            : this(errorCode, errorMessage, null)
        {
        }

        /**
         * Create a ResponseItem specifying the ResponseError and error Message.
         * @param errorCode an RPC error code
         * @param errorMessage the Error Message
         * @param response the application specific value that will be sent as
         *     as part of "data" section of the error.
         */
        public ResponseItem(int errorCode, String errorMessage, Object response)
        {
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
            this.response = response;
        }

        /**
         * Create a ResponseItem specifying a value.
         */
        public ResponseItem(Object response)
        {
            this.errorCode = 200;
            this.errorMessage = null;
            this.response = response;
        }

        /**
         * Get the response value.
         */
        public Object getResponse()
        {
            return response;
        }

        /**
         * Get the error code associated with this ResponseItem.
         * @return the error code associated with this ResponseItem
         */
        public int getErrorCode()
        {
            return errorCode;
        }

        /**
         * Get the Error Message associated with this Response Item.
         * @return the Error Message
         */
        public String getErrorMessage()
        {
            return errorMessage;
        }


        public override bool Equals(Object o)
        {
            if (this == o)
            {
                return true;
            }

            if (!(o is ResponseItem))
            {
                return false;
            }

            ResponseItem that = (ResponseItem)o;
            return (errorCode == that.errorCode)
                   && Object.Equals(errorMessage, that.errorMessage)
                   && Object.Equals(response, that.response);
        }

        public override int GetHashCode()
        {
            int result = errorCode.GetHashCode();
            result = 31 * result + (errorMessage != null ? errorMessage.GetHashCode() : 0);
            result = 31 * result + (response != null ? response.GetHashCode() : 0);
            return result;
        }
    }
}