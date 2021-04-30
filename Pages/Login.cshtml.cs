using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.AspNetCore.Http; 

namespace NetApp.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
            
        }
        
        public IActionResult OnPost(String login, String password){
            Console.WriteLine("Teste");

            Console.WriteLine("Login: " + login);
            Console.WriteLine("password: " + password);

            if (Login(login, password))
                return RedirectToPage("Calculate");
            else
                return Page();
        }

        public bool Login(String login, String password){
            String url="https://reqres.in/api/login";
            String log="eve.holt@reqres.in";
            String pass="cityslicka";

            String result;
            Int32 r=0;
            

            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
                
            //myRequest.Headers.Add("Authorization", string.Format("Basic {0}", credentials));
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.Method = WebRequestMethods.Http.Post;
            myRequest.AllowAutoRedirect = true;
            myRequest.Proxy = null;
             
            String dadosPOST = "email=" + log + "&password="+ pass;
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
                    r=(int)myResponse.StatusCode;
                    Console.WriteLine("po: " + r);
                    Console.WriteLine("res: " + result); 
                    
                    var g= JsonSerializer.Deserialize<Dictionary<String,String>>(result);
                    Console.WriteLine("TK: " +g["token"]);
                    HttpContext.Session.SetString("Token", g["token"]);
                    // if (myResponse.StatusCode == HttpStatusCode.OK){
                    if (r == 200){
                        Console.WriteLine("111");
                        return true;
                    }
                }
            }
                return false;
        }
    }
}
