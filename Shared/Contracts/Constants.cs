using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Contracts
{
    public static class Constants
    {
       
        public const string WEB_API_URL = @"http://localhost:5000/";
        public const string API_AUTHENTICATION = WEB_API_URL + "api/Account/";

        //Item
        public const string API_GET_CATEGORYS = WEB_API_URL + "api/item/GetCategories";
        public const string API_GET_ITEMS = WEB_API_URL + "api/item/GetItems";

        //customers
        public const string API_GET_CUSTOMERS = WEB_API_URL + "api/customer/GetCustomer";

    }
}
