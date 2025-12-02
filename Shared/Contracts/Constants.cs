using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public static class Constants
    {

        public const string WEB_API_URL = @"https://e675415ed8b6.ngrok-free.app";


        //Account
        public const string API_AUTHENTICATION = WEB_API_URL + "api/Account/login";

        //Item
        public const string API_GET_CATEGORYS = WEB_API_URL + "api/item/GetCategories";
        public const string API_POST_CATEGORYS = WEB_API_URL + "api/item/SaveCategory";
        public const string API_DELETE_CATEGORYS = WEB_API_URL + "api/item/DeleteCategory/{id}";
        public const string API_GET_ITEMS = WEB_API_URL + "api/item/GetItems";
        public const string API_POST_ITEMS = WEB_API_URL + "api/item/SaveProduct";
        public const string API_DELETE_ITEMS = WEB_API_URL + "api/item/DeleteProduct/{id}";

        //customers
        public const string API_GET_CUSTOMERS = WEB_API_URL + "api/customer/GetCustomer";
        public const string API_POST_CUSTOMERS = WEB_API_URL + "api/customer/SaveCustomer";
        public const string API_DELETE_CUSTOMERS = WEB_API_URL + "api/customer/DeleteCustomer/{id}";


        //transaction
        public const string API_GET_TRANSACTION = WEB_API_URL + "api/transaction/GetTransaction";
        public const string API_GET_TRANSACTIONITEM = WEB_API_URL + "api/transaction/GetTransactionItem";
        public const string API_POST_TRANSACTION = WEB_API_URL + "api/transaction/SaveTransaction";

    }
}
