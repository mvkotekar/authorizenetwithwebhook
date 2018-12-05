using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using AuthorizeNet.Api.Controllers;
using AuthorizeNet.Api.Contracts.V1;
using AuthorizeNet.Api.Controllers.Bases;
using System.Web.UI;

namespace WebApplication1
{
    public class CreateAnAcceptPaymentTransaction
    {
        //Not used anywhere
        public static ANetApiResponse Run(String ApiLoginID, String ApiTransactionKey, decimal amount, string descriptor, string value, Page p)
        {
            p.Response.Write("Create an Accept Payment Transaction Sample");

            ApiOperationBase<ANetApiRequest, ANetApiResponse>.RunEnvironment = AuthorizeNet.Environment.SANDBOX;

            // define the merchant information (authentication / transaction id)
            ApiOperationBase<ANetApiRequest, ANetApiResponse>.MerchantAuthentication = new merchantAuthenticationType()
            {
                name = ApiLoginID,
                ItemElementName = ItemChoiceType.transactionKey,
                Item = ApiTransactionKey,
            };

            var opaqueData = new opaqueDataType
            {
                dataDescriptor = descriptor,
                dataValue = value

            };

            var billingAddress = new customerAddressType
            {
                firstName = "John",
                lastName = "Doe",
                address = "123 My St",
                city = "OurTown",
                zip = "98004"
            };

            //standard api call to retrieve response
            var paymentType = new paymentType { Item = opaqueData };

            // Add line Items
            var lineItems = new lineItemType[2];
            lineItems[0] = new lineItemType { itemId = "1", name = "t-shirt", quantity = 2, unitPrice = new Decimal(15.00) };
            lineItems[1] = new lineItemType { itemId = "2", name = "snowboard", quantity = 1, unitPrice = new Decimal(450.00) };

            var transactionRequest = new transactionRequestType
            {
                transactionType = transactionTypeEnum.authCaptureTransaction.ToString(),    // charge the card
                amount = amount,
                payment = paymentType,
                billTo = billingAddress,
                lineItems = lineItems,
            };

            var request = new createTransactionRequest { transactionRequest = transactionRequest, refId="9900640898" };

            // instantiate the controller that will call the service
            var controller = new createTransactionController(request);
            controller.Execute();

            // get the response from the service (errors contained if any)
            var response = controller.GetApiResponse();

            // validate response
            if (response != null)
            {
                if (response.messages.resultCode == messageTypeEnum.Ok)
                {
                    if (response.transactionResponse.messages != null)
                    {
                        p.Response.Write("Successfully created transaction with Transaction ID: " + response.transactionResponse.transId);
                        p.Response.Write("Response Code: " + response.transactionResponse.responseCode);
                        p.Response.Write("Message Code: " + response.transactionResponse.messages[0].code);
                        p.Response.Write("Description: " + response.transactionResponse.messages[0].description);
                        p.Response.Write("Success, Auth Code : " + response.transactionResponse.authCode);
                    }
                    else
                    {
                        p.Response.Write("Failed Transaction.");
                        if (response.transactionResponse.errors != null)
                        {
                            p.Response.Write("Error Code: " + response.transactionResponse.errors[0].errorCode);
                            p.Response.Write("Error message: " + response.transactionResponse.errors[0].errorText);
                        }
                    }
                }
                else
                {
                    p.Response.Write("Failed Transaction.");
                    if (response.transactionResponse != null && response.transactionResponse.errors != null)
                    {
                        p.Response.Write("Error Code: " + response.transactionResponse.errors[0].errorCode);
                        p.Response.Write("Error message: " + response.transactionResponse.errors[0].errorText);
                    }
                    else
                    {
                        p.Response.Write("Error Code: " + response.messages.message[0].code);
                        p.Response.Write("Error message: " + response.messages.message[0].text);
                    }
                }
            }
            else
            {
                p.Response.Write("Null Response.");
            }

            return response;
        }
    }
}