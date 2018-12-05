using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Controllers.Bases;

namespace WebApplication1
{
    public class GetAnAcceptPaymentPage
    {
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, decimal amount, Page p, out string token)
        {
            p.Response.Write("GetAnAcceptPaymentPage Sample\n");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            settingType[] settings = new settingType[3];

            settings[0] = new settingType();
            settings[0].settingName = settingNameEnum.hostedPaymentButtonOptions.ToString();
            settings[0].settingValue = "{\"text\": \"Pay\"}";

            settings[1] = new settingType();
            settings[1].settingName = settingNameEnum.hostedPaymentOrderOptions.ToString();
            settings[1].settingValue = "{\"show\": true}";

            settings[2] = new settingType();
            settings[2].settingName = settingNameEnum.hostedPaymentReturnOptions.ToString();
            settings[2].settingValue = "{\"showReceipt\": true, \"url\": \"http://localhost:57853/Default\",\"cancelUrl\": \"http://localhost:57853/Default\"}";



            // Add line Items
            var lineItems = new lineItemType[2];
            lineItems[0] = new lineItemType { itemId = "1", name = "t-shirt", quantity = 2, unitPrice = new Decimal(15.00) };
            lineItems[1] = new lineItemType { itemId = "2", name = "snowboard", quantity = 1, unitPrice = new Decimal(450.00) };

            var billingAddress = new customerAddressType
            {
                firstName = "Ellen",
                lastName = "Johnson",
                address = "14 Main Street",
                city = "Pecan Springs",
                state = "TX",
                phoneNumber = "+16061234123",
                zip = "44628",
                company = "Souveniropolis",
                country = "USA"
            };

            var customer = new customerDataType
            {
                email = "test@test.com",
                taxId = "123"
            };

            var order = new orderType();
            order.invoiceNumber = "123123";
            order.description = "234234";

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // authorize capture only
                amount = amount,
                lineItems = lineItems,
                billTo = billingAddress,
                customerIP = "192.1.1.1",
                order=order
            };

            var request = new getHostedPaymentPageRequest();

            request.transactionRequest = transactionRequest;
            request.hostedPaymentSettings = settings;

            request.refId = "11111111";
             

            // instantiate the controller that will call the service
            var controller = new getHostedPaymentPageController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            // validate response
            if (response != null && response.messages.resultCode == messageTypeEnum.Ok)
            {
                p.Response.Write("Message code : " + response.messages.message[0].code);
                p.Response.Write("Message text : " + response.messages.message[0].text);
                //p.Response.Write("Token : " + response.token);
            }
            else if (response != null)
            {
                p.Response.Write("Error: " + response.messages.message[0].code + "  " + response.messages.message[0].text);
                p.Response.Write("Failed to get hosted payment page");
            }

            token = response.token;


            return response;
        }
    }
}