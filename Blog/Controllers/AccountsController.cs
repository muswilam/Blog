using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Blog.Models;
using System.Security.Cryptography;
using System.Text;
using Blog.ViewModel;
using Blog.Common;
using System.Data.Entity;
using System.IO;

namespace Blog.Controllers
{
    public class AccountsController : Controller
    {
        private BlogContext context = new BlogContext();

        [HttpGet]
        public ActionResult Login(string hash)
        {
            AdminViewModel adminModel = new AdminViewModel();
            if (string.IsNullOrWhiteSpace(hash))
            {
                Random random = new Random(); //generate a random num
                byte[] randomData = new byte[sizeof(long)];
                random.NextBytes(randomData); //store it in arry of bytes
                string newNonce = BitConverter.ToUInt64(randomData, 0).ToString("X16"); //converting it to hexadecimal
                Session["Nonce"] = newNonce; //store it in session to can use it again 

                return View(adminModel); //model : because of preventing ambiguous of (string view name & string model)
            }

            return View();
        }

        [HttpPost]
        public ActionResult Login(AdminViewModel adminModel , string hash)
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

            AdminViewModel initAdminModel = new AdminViewModel();
            if(!ModelState.IsValid)
            {
                return View(initAdminModel);
            }
            
            var admin = context.Administrators.Where(a => a.UserName == adminModel.UserName).FirstOrDefault();
            string nonce = Session["Nonce"] as string; //get the current random num

            if (admin == null || string.IsNullOrWhiteSpace(nonce))
            {
                initAdminModel.AdminErrorMsg = "Invalid user name.";
                return View(initAdminModel);
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
            bool passwordMatch = (computeHash.ToLower() == hash.ToLower());
            if (passwordMatch)
                Session["IsAdmin"] = passwordMatch;
            else
            {
                initAdminModel.PassErrorMsg = "Invalid password.";
                return View(initAdminModel);
            }

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
            AboutAdminViewModel adminModel = new AboutAdminViewModel();

            adminModel.Administrator = context.Administrators.Include(a => a.Skills).Where(a => a.UserName.Equals("Admin")).First();

            DateTime dateTime = new DateTime(1900, 1, 1);
            adminModel.Administrator.Birthdate = adminModel.Administrator.Birthdate ?? dateTime;

            adminModel.SkillsTypes = context.Skills.Select(s => s.Type).Distinct().ToList();
            adminModel.Skill = new Skill();

            adminModel.IsAdmin = Session["IsAdmin"] != null && (bool)Session["IsAdmin"] == true;

            ViewBag.countries = CountriesList.Countries();

            return View(adminModel);
        }

        public JsonResult UploadImage()
        {
            JsonResult json = new JsonResult();

            var pic = Request.Files[0];

            var fileName = Guid.NewGuid() + Path.GetExtension(pic.FileName);
            var filePath = Server.MapPath("~/Images/Upload_Images/") + fileName;
            var fileUrl = "/Images/Upload_Images/" + fileName;

            pic.SaveAs(filePath);

            json.Data = fileUrl;

            return json;
        }

        [HttpPost]
        public JsonResult Edit(AboutAdminViewModel adminModel)
        {
            var adminFromDB = context.Administrators.Where(a => a.Id == adminModel.Id).First();

            bool result;

            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid Inputs." });
            }

            adminFromDB.Name = adminModel.Name;
            adminFromDB.Email = adminModel.Email;
            adminFromDB.Education = adminModel.Education;
            adminFromDB.Country = adminModel.Country;
            adminFromDB.Headline = adminModel.Headline;
            adminFromDB.Birthdate = adminModel.Birthdate;
            adminFromDB.Bio = adminModel.Bio;

            //get profile pic from db
            var profilePicFromDb = context.AdminsProfiles.Where(ap => ap.AdminProfileId == adminModel.Id).FirstOrDefault();
            profilePicFromDb.ProfileUrl = adminModel.ProfileImgUrl;
            context.Entry(profilePicFromDb).State = EntityState.Modified;

            context.Entry(adminFromDB).State = EntityState.Modified;
            result = context.SaveChanges() > 0;

            if (result)
                return Json(new { success = true });
            else
                return Json(new { success = false, message = "OPPS! Something went wrong." });
        }

        public JsonResult AddSkill(int adminId , SkillViewModel skillModel)
        {
            var admin = context.Administrators.Include(a => a.Skills).Where(a => a.Id == adminId).First();

            var skillDb = context.Skills.Where(s => s.Name.ToLower() == skillModel.Name.ToLower()).FirstOrDefault();

            bool result = false;

            if(!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Invalid skill name" });
            }

            Skill skill = new Skill();
            skill.Name = skillModel.Name;
            skill.Type = skillModel.Type;

            if(skillDb == null)
            {
                context.Skills.Add(skill);
                admin.Skills.Add(skill);

                result = context.SaveChanges() > 0;

                if (result)
                    return Json(new { success = true });
                else
                    return Json(new { success = false, message = "OPPS! Something went wrong." });
            }
            else
            {
                var skillExist = admin.Skills.Where(s => s.Name.ToLower() == skillDb.Name.ToLower()).FirstOrDefault();

                if (skillExist == null)
                {
                    admin.Skills.Add(skill);

                    result = context.SaveChanges() > 0;

                    if (result)
                        return Json(new { success = true });
                    else
                        return Json(new { success = false, message = "OPPS! Something went wrong." });
                }

                return Json(new { success = false, message = "Skill added before." });
            }
        }

        public JsonResult DeleteSkill(int id)
        {
            var skillDb = context.Skills.Where(s => s.Id == id).FirstOrDefault();

            if (skillDb != null)
            {
                context.Skills.Remove(skillDb);
                bool result = context.SaveChanges() > 0;

                if (result)
                    return Json(new { success = true });

                return Json(new { success = false, message = "OPPS! Something went wrong." });
            }

            return Json(new { success = false, message = "Skill not exists." } , JsonRequestBehavior.AllowGet);
        }
    }
}
