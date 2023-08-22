using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyanmarCurrencyName.Models;

namespace MyanmarCurrencyName.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        [HttpPost]
        public string GetCurrency(AmountNumber number)
        {
            string currency = ChangeCurrencyData(number);
            return currency;
        }

        private string ChangeCurrencyData(AmountNumber number)
        {
            string num = "";
            if (!string.IsNullOrEmpty(number.Number))
            {
                int len = number.Number.Length;
                int l = len;
                if (number.Number.StartsWith("0"))
                {
                    return "Not start with zero";
                }
                for (int i= 0; i < len; i++)
                {
                    if (l > 5)
                    {
                        int al = l - 5;
                        if (number.Number.Substring(i, 1).ToString() != "0")
                        {
                            num += ChangeNumber(number.Number.Substring(i, 1));
                            num += ChangeAmount(al.ToString()); 
                        }
                        if (al == 1)
                        {
                            num += ChangeAmount("6");
                        }
                    }
                    else
                    {
                        if (number.Number.Substring(i, 1).ToString() != "0")
                        {
                            num += ChangeNumber(number.Number.Substring(i, 1));
                            num += ChangeAmount(l.ToString());
                        }
                    }
                    
                    l--;
                }
                return num+"ကျပ်";
            }
            return "Pls insert data";
        }

        private string ChangeNumber(string number)
        {
            string num;
            switch (number)
            {
                case "1": num = "တစ်"; break;
                case "2": num = "နှစ်"; break;
                case "3": num = "သုံး"; break;
                case "4": num = "လေး"; break;
                case "5": num = "ငါး"; break;
                case "6": num = "ခြောက်"; break;
                case "7": num = "ခုနှစ်"; break;
                case "8": num = "ရှစ်"; break;
                case "9": num = "ကိုး"; break;
                default: num = ""; break;  
            }
            return num;
        }

        private string ChangeAmount(string number)
        {
            string num;
            switch(number)
            {
                case "6": num = "သိန်း"; break;
                case "5": num = "သောင်း"; break;
                case "4": num = "ထောင်"; break;
                case "3": num = "ရာ"; break;
                case "2": num = "ဆယ်"; break;
                case "1": num = ""; break;
                default: num = "";break;
            }
            return num;
        }
    }
}
