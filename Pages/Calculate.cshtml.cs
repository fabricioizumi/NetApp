using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Http; 

namespace NetApp.Pages
{
    public class CalculateModel : PageModel
    {
        [BindProperty]
        public String Message {get;set;}

        public IActionResult OnGet()
        {
            Console.WriteLine("get calculate page");
            if (HttpContext.Session.GetString("Token") == null){
                return RedirectToPage("Login");

            }
            else{
                return Page();
            }
        }

        public IActionResult OnPost(String initial_value, String time){
            Console.WriteLine("Initial value: " + initial_value);
            Console.WriteLine("Time: " + time);

            String message = Calculate(initial_value, time);
            Message = message;

            return Page();
        }

        private String Calculate(String initial_value, String time){
            String url="http://127.0.0.1:8000/api/calc-interests-rate";
            

            String result;
            Int32 status=0;
            
            try
            {
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                
                //myRequest.Headers.Add("Authorization", string.Format("Basic {0}", credentials));
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.Method = WebRequestMethods.Http.Post;
                myRequest.AllowAutoRedirect = true;
                myRequest.Proxy = null;

                String dadosPOST = "initial_value=" + initial_value + "&time="+ time;
                var dados = Encoding.UTF8.GetBytes(dadosPOST);
                myRequest.ContentLength = dados.Length;
                using (var stream = myRequest.GetRequestStream())
                {
                    stream.Write(dados, 0, dados.Length);
                    stream.Close();
                } 
                using (HttpWebResponse myResponse = (HttpWebResponse) myRequest.GetResponse())
                {                    

                    using (StreamReader sr = new StreamReader(myResponse.GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        result = sr.ReadToEnd();

                        Console.WriteLine("result: " + result);
                        status=(int)myResponse.StatusCode;
                        Console.WriteLine("po: " + result);                                                

                        switch (status){
                            case 200:
                                Console.WriteLine("Calculate OK");                                
                                return "Result: " + result;                                
                            case 404:
                                Console.WriteLine("Resource Not Found");
                                return "Resource Not Found";
                        } 
                        
                    }
                }

            }
            catch(WebException wex){
                Console.WriteLine("Can't connect");
                return "Can't connect";
            }
            catch (System.Exception ex)
            {
                
                Console.WriteLine("Error: " + ex);
                return ex.Message;
            }
            
            return "fail to connect";
        }
    }
}
