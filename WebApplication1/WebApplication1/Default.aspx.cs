using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (IsPostBack) {
                string xx = "check the postback data";
            }

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            processTransaction(this);

        }

        void processTransaction(Page p) {
            // These are default transaction keys.
            // You can create your own keys in seconds by signing up for a sandbox account here: https://developer.authorize.net/sandbox/
            const string apiLoginId = "89FVdsGYb7f";
            const string transactionKey = "6bdg4nR5997uFH8Z";

            const string shippingAddressId = "1223213";
            const decimal amount = 12.34m;
            const string subscriptionId = "1223213";
            const short day = 45;
            const string emailId = "test@test.com";
            string token = "";

            var retValue = GetAnAcceptPaymentPage.Run(apiLoginId, transactionKey, 500.89m, p, out token);

            var descriptor = "COMMON.ACCEPT.INAPP.PAYMENT";

            postData(token, descriptor, token);

        }

        void postData(string dataValue, string dataDescriptor, string token) {
            NameValueCollection collections = new NameValueCollection();
            collections.Add("dataValue", dataValue);
            collections.Add("dataDescriptor", dataDescriptor);
            collections.Add("token", token);
            //string remoteUrl = "https://accept.authorize.net/payment/payment";
            string remoteUrl = "https://test.authorize.net/payment/payment";

            string html = "<html><head>";
            html += "</head><body onload='document.forms[0].submit()'>";
            html += string.Format("<form name='PostForm' method='POST' action='{0}'>", remoteUrl);
            foreach (string key in collections.Keys)
            {
                html += string.Format("<input name='{0}' type='hidden' value='{1}'>", key, collections[key]);
            }
            html += "</form></body></html>";
            Response.Clear();
            Response.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");
            Response.HeaderEncoding = Encoding.GetEncoding("ISO-8859-1");
            Response.Charset = "ISO-8859-1";
            Response.Write(html);
            Response.End();
        }
    }
}