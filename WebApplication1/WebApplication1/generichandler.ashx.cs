using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApplication1
{
    /// <summary>
    /// Summary description for generichandler
    /// </summary>
    public class generichandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");

            string headers = String.Empty;
            foreach (var key in context.Request.Headers.AllKeys)
                headers += key + "=" + context.Request.Headers[key] + Environment.NewLine;

            context.Response.Write(headers);

            Stream data = context.Request.InputStream;
            // convert stream to string
            StreamReader reader = new StreamReader(data);
            string text = reader.ReadToEnd();
            context.Response.Write(text);

            dynamic jsonResponse = JsonConvert.DeserializeObject(text);
            GetTransactionDetails.Run("89FVdsGYb7f", "6bdg4nR5997uFH8Z", jsonResponse.payload.id);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}