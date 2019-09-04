using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Security.Cryptography;
using System.Text;

namespace Blog.Controllers
{
    public class AccountsController : Controller
    {
        private BlogContext context = new BlogContext();

        public ActionResult Login(string userName, string hash)
        {
            if (string.IsNullOrWhiteSpace(hash))
            {
                Random random = new Random(); //generate a random num
                byte[] randomData = new byte[sizeof(long)];
                random.NextBytes(randomData); //store it in arry of bytes
                string newNonce = BitConverter.ToUInt64(randomData, 0).ToString("X16"); //converting it to hexadecimal
                Session["Nonce"] = newNonce; //store it in session to can use it again 

                return View(model: newNonce); //model : because of preventing ambiguous of (string view name & string model)
            }

            var admin = context.Administrators.Where(a => a.UserName == userName).FirstOrDefault();
            string nonce = Session["Nonce"] as string; //get the current random num

            if (admin == null || string.IsNullOrWhiteSpace(nonce))
            {
                return View("Index","Posts");
            }

            string computeHash; //generate the hash
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashInput = Encoding.ASCII.GetBytes(admin.Password + nonce);
                byte[] hashData = sha256.ComputeHash(hashInput);
                //convert byte array (hash) to string
                StringBuilder strBuilder = new StringBuilder();
                foreach (var value in hashData)
                {
                    strBuilder.AppendFormat("{0:X2}", value);
                }
                computeHash = strBuilder.ToString();
            }

            //the admin is loged in if the two hash match
            Session["IsAdmin"] = (computeHash.ToLower() == hash.ToLower());
            return RedirectToAction("Index", "Posts");
        }

        public ActionResult Logout()
        {
            Session["Nonce"] = null;
            Session["IsAdmin"] = null;
            return RedirectToAction("Index" , "Posts");
        }

        public ActionResult AboutAdmin()
        {
            var admin = context.Administrators.First();
          
            return View(admin);
        }
    }
}
