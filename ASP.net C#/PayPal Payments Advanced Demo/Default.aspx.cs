using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Net;

namespace PayPal_Payments_Advanced_Demo
{
    public partial class _Default : System.Web.UI.Page
    {
        string environment = "test"; // Change to "live" to process real transactions.
        // (For a live transaction, you must use a real, valid CC and billing address.)

        //current absolute URI minus any GET string
        string url = HttpContext.Current.Request.Url.AbsoluteUri.Split(new[] { '?' })[0];

        protected NameValueCollection httpRequestVariables()
        {
            var post = Request.Form;       // $_POST
            var get = Request.QueryString; // $_GET
            return Merge(post, get);
        }

        protected void Page_PreInit()
        {
            if (httpRequestVariables()["RESULT"] != null)
            {
                Page.MasterPageFile = "Blank.Master";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string output = "";
            ////////
            ////
            ////  First, handle any return/responses
            ////

            //Check if we just returned inside the iframe.  If so, store payflow response and redirect parent window with javascript.
            if (httpRequestVariables()["RESULT"] != null)
            {
                HttpContext.Current.Session["payflowresponse"] = httpRequestVariables();
                output += "<script type=\"text/javascript\">window.top.location.href = \"" + url + "\";</script>";
                BodyContentDiv.InnerHtml = output;
                return;
            }

            //Check whether we stored a server response.  If so, print it out.
            var payflowresponse = HttpContext.Current.Session["payflowresponse"] as NameValueCollection;
            if (payflowresponse != null)
            {
                HttpContext.Current.Session["payflowresponse"] = null;

                bool success = payflowresponse["RESULT"] == "0";
                if (success)
                {
                    output += "<span style='font-family:sans-serif;font-weight:bold;'>Transaction approved! Thank you for your order.</span>";
                }
                else
                {
                    output += "<span style='font-family:sans-serif;'>Transaction failed! Please try again with another payment method.</span>";
                }

                output += "<p>(server response follows)</p>\n";
                output += print_r(payflowresponse);

                AdvancedDemoContent.InnerHtml = output;
                return;
            };

            /////////
            ////
            //// Otherwise, begin hosted checkout pages flow
            ////
            NameValueCollection requestArray = new NameValueCollection()
            {
                {"PARTNER", "PayPal"},                         // You'll want to change these 4
                {"VENDOR", "palexanderpayflowtest"},           // To use your own credentials
                {"USER", "palexanderpayflowtestapionly"},
                {"PWD", "demopass123"},
                {"TRXTYPE", "A"},
                {"AMT", "1.00"},
                {"CURRENCY", "USD"},
                {"CREATESECURETOKEN", "Y"},
                {"SECURETOKENID", genId()},  //This should be generated and unique, never used before
                {"RETURNURL", url},  //Note how this simple example merely returns back to itself, rather than having a seperate Return.aspx
                {"CANCELURL", url},
                {"ERRORURL", url},

                // In practice you'd collect billing and shipping information with your own form,
                // then afterwards be doing this request for a secure token to display the payment iframe.
                // (For visuals, see page 7 of https://cms.paypal.com/cms_content/US/en_US/files/developer/Embedded_Checkout_Design_Guide.pdf )
                // This example uses hardcoded address values for simplicity.
                {"BILLTOFIRSTNAME", "John"},
                {"BILLTOLASTNAME", "Doe"},
                {"BILLTOSTREET", "123 Main St."},
                {"BILLTOCITY", "San Jose"},
                {"BILLTOSTATE", "CA"},
                {"BILLTOZIP", "95101"},
                {"BILLTOCOUNTRY", "US"},
                {"SHIPTOFIRSTNAME", "Jane"},
                {"SHIPTOLASTNAME", "Smith"},
                {"SHIPTOSTREET", "1234 Park Ave"},
                {"SHIPTOCITY", "San Jose"},
                {"SHIPTOSTATE", "CA"},
                {"SHIPTOZIP", "95101"},
                {"SHIPTOCOUNTRY", "US"},
            };

            NameValueCollection resp = run_payflow_call(requestArray);
            if (resp["RESULT"] != "0")
            {
                output += "Payflow call failed";
            }
            else
            {
                string mode;
                if (environment == "pilot" || environment == "test" || environment == "sandbox") mode = "TEST"; else mode = "LIVE";

                output += "<div style=\"border: 1px dashed; width:492px; height:567px;\">";
                output += "<iframe src='https://payflowlink.paypal.com?SECURETOKEN=" + resp["SECURETOKEN"] + "&SECURETOKENID=" + resp["SECURETOKENID"] + "&MODE=" + mode + "' width='490' height='565' border='0' frameborder='0' scrolling='no' allowtransparency='true'>\n</iframe>";
                output += "</div><p style='margin-left:40px;font-size:120%;font-family:monospace;'>(end of hosted iframe, marked with dashed line)</p>";
            };

            AdvancedDemoContent.InnerHtml = output;  //end of hosted checkout pages flow
        }

        // Run Payflow request and return an array with the response
        protected NameValueCollection run_payflow_call(NameValueCollection requestArray)
        {
            String nvpstring = "";
            foreach (string key in requestArray)
            {
                //format:  "PARAMETERNAME[lengthofvalue]=VALUE&".  Never URL encode.
                var val = requestArray[key];
                nvpstring += key + "[ " + val.Length + "]=" + val + "&";
            }

            string urlEndpoint;
            if (environment == "pilot" || environment == "test" || environment == "sandbox")
            {
                urlEndpoint = "https://pilot-payflowpro.paypal.com/";
            }
            else
            {
                urlEndpoint = "https://payflowpro.paypal.com";
            }

            //send request to Payflow
            HttpWebRequest payReq = (HttpWebRequest)WebRequest.Create(urlEndpoint);
            payReq.Method = "POST";
            payReq.ContentLength = nvpstring.Length;
            payReq.ContentType = "application/x-www-form-urlencoded";

            StreamWriter sw = new StreamWriter(payReq.GetRequestStream());
            sw.Write(nvpstring);
            sw.Close();

            //get Payflow response
            HttpWebResponse payResp = (HttpWebResponse)payReq.GetResponse();
            StreamReader sr = new StreamReader(payResp.GetResponseStream());
            string response = sr.ReadToEnd();
            sr.Close();

            //parse string into array and return
            NameValueCollection dict = new NameValueCollection();
            foreach (string nvp in response.Split('&'))
            {
                string[] keys = nvp.Split('=');
                dict.Add(keys[0], keys[1]);
            }
            return dict;
        }

        // generates a random unique ID for use in the initial CREATESECURETOKEN=Y request
        protected string genId()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 16)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return "MySecTokenID-" + result; //add a prefix to avoid confusion with the "SECURETOKEN"
        }

        //human-readable representation of an NVC
        public string print_r(Object obj)
        {
            string output = "<pre>\n";
            if (obj is NameValueCollection)
            {
                NameValueCollection nvc = obj as NameValueCollection;
                foreach (var key in nvc)
                {
                    output += key + "=" + nvc[key.ToString()] + "\n";
                };
            }
            else
            {
                output += "UNKNOWN TYPE";
            }
            output += "</pre>";
            return output;
        }

        // merges two NVCs
        public static NameValueCollection Merge(NameValueCollection first, NameValueCollection second)
        {
            if (first == null && second == null)
                return null;
            else if (first != null && second == null)
                return new NameValueCollection(first);
            else if (first == null && second != null)
                return new NameValueCollection(second);

            NameValueCollection result = new NameValueCollection(first);
            for (int i = 0; i < second.Count; i++)
                result.Set(second.GetKey(i), second.Get(i));
            return result;
        }
    }
}