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
using System;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using pestaServer.ActionFilters;
using pestaServer.Models.gadgets;
using pestaServer.Models.gadgets.servlet;
using HttpRequestWrapper=pestaServer.Models.gadgets.http.HttpRequestWrapper;
using HttpResponseWrapper=pestaServer.Models.gadgets.http.HttpResponseWrapper;

namespace pestaServer.Controllers
{
    public class concatController : Controller
    {
        private ProxyHandler proxyHandler = ProxyHandler.Instance;

        [CompressFilter]
        public void Index()
        {
            HttpRequest request = System.Web.HttpContext.Current.Request;
            HttpResponse response = System.Web.HttpContext.Current.Response;
            if (request.Headers["If-Modified-Since"] != null)
            {
                response.StatusCode = (int)HttpStatusCode.NotModified;
                return;
            }
            if (request.Params[ProxyBase.REWRITE_MIME_TYPE_PARAM] != null)
            {
                response.ContentType = request.Params[ProxyBase.REWRITE_MIME_TYPE_PARAM];
            }
            if (request.Params[ProxyBase.REFRESH_PARAM] != null)
            {
                int ttl = 0;
                int.TryParse(request.Params[ProxyBase.REFRESH_PARAM], out ttl);
                HttpUtil.SetCachingHeaders(response, ttl);
            }
            response.AddHeader("Content-Disposition", "attachment;filename=p.txt");
            HttpResponseWrapper wrapper = new HttpResponseWrapper(response);
            for (int i = 1; i < int.MaxValue; i++)
            {
                string url = request.Params[i.ToString()];
                if (url == null)
                    break;

                try
                {
                    wrapper.Write(Encoding.UTF8.GetBytes("/* ---- Start " + url + " ---- */"));
                    proxyHandler.Fetch(new RequestWrapper(System.Web.HttpContext.Current, url, true), wrapper);
                    if (wrapper.getStatus() != (int)HttpStatusCode.OK)
                    {
                        wrapper.Write(Encoding.UTF8.GetBytes(
                            formatHttpError(wrapper.getStatus(), wrapper.getErrorMessage())));
                    }

                    wrapper.Write(Encoding.UTF8.GetBytes("/* ---- End " + url + " ---- */"));
                }
                catch (GadgetException ge)
                {
                    if (ge.getCode() != GadgetException.Code.FAILED_TO_RETRIEVE_CONTENT)
                    {
                        outputError(ge, url, response);
                        return;
                    }
                    else
                    {
                        wrapper.Write(Encoding.UTF8.GetBytes("/* ---- End " + url + " 404 ---- */"));
                    }

                }
            }
            response.End();
        }

        private String formatHttpError(int status, String errorMessage)
        {
            StringBuilder err = new StringBuilder();
            err.Append("/* ---- Error ");
            err.Append(status);
            if (errorMessage != null)
            {
                err.Append(", ");
                err.Append(errorMessage);
            }

            err.Append(" ---- */");
            return err.ToString();
        }


        private class RequestWrapper : HttpRequestWrapper
        {
            protected String url;
            public RequestWrapper(HttpContext context, String url, bool isConcat)
                : base(context, isConcat)
            {
                this.url = url;
            }

            public override String getParameter(String paramName)
            {
                if (ProxyHandler.URL_PARAM.Equals(paramName))
                {
                    return url;
                }
                return base.getParameter(paramName);
            }
        }

        private void outputError(Exception excep, String url, HttpResponse resp)
        {
            StringBuilder err = new StringBuilder();
            err.Append(excep.Source);
            err.Append(" concat(");
            err.Append(url);
            err.Append(") ");
            err.Append(excep.Message);

            // Log the errors here for now. We might want different severity levels
            // for different error codes.
            resp.StatusCode = (int)HttpStatusCode.BadRequest;
            resp.StatusDescription = err.ToString();
        }
    }
}
