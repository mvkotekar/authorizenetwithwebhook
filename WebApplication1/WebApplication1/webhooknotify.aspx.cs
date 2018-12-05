using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class webhooknotify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string headers = String.Empty;
            foreach (var key in Request.Headers.AllKeys)
                headers += key + "=" + Request.Headers[key] + Environment.NewLine;

            this.Response.Write(headers);


            Stream data = Request.InputStream;
            // convert stream to string
            StreamReader reader = new StreamReader(data);
            string text = reader.ReadToEnd();
            this.Response.Write(text);



        }
    }
}