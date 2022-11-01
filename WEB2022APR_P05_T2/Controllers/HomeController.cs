using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WEB2022APR_P05_T2.Models;
using WEB2022APR_P05_T2.DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using WEB2022APR_P05_T2.DAL;
using WEB2022APR_P05_T2.Models;

namespace WEB2022APR_P05_T2.Controllers
{
    
    public class HomeController : Controller
    {
        private ProductDAL productContext = new ProductDAL();
        private readonly ILogger<HomeController> _logger;
        private UserDAL userContext = new UserDAL();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Product> productList = productContext.GetProduct();
            return View(productList);
        }
        [HttpPost]
        public ActionResult UserLogin(IFormCollection formData)
        {
            List<User> userList = userContext.GetUsers();

            // Read inputs from textboxes
            string username = formData["uname"].ToString();
            string password = formData["upass"].ToString();

            bool userExist = false;

            for (int i = 0; i < userList.Count; i++)
            {

                if (username == userList[i].Username)
                {
                    if (password == userList[i].UPassword)
                    {
                        userExist = true;
                        if (userList[i].URole == "Customer" && userExist)
                        {
                            HttpContext.Session.SetString("Username", username);
                            HttpContext.Session.SetString("Role", "Customer");

                            return RedirectToAction("Index", "Customer");
                        }
                        else if (userList[i].URole == "Sales Personnel" && userExist)
                        {
                            HttpContext.Session.SetString("Username", username);
                            HttpContext.Session.SetString("Role", "Sales");

                            return RedirectToAction("Index", "SalesPersonnel");
                        }
                        else if (userList[i].URole == "Product Manager" && userExist)
                        {

                            HttpContext.Session.SetString("Username", username);
                            HttpContext.Session.SetString("Role", "Product");

                            return RedirectToAction("Index", "Product");
                        }
                        else if (userList[i].URole == "Marketing Personnel" && userExist)
                        {
                            HttpContext.Session.SetString("Username", username);
                            HttpContext.Session.SetString("Role", "Marketing");

                            return RedirectToAction("Index", "Marketing");
                        }
                        else
                        {
                            TempData["Message"] = "Invalid Password!";
                            return RedirectToAction("Index");
                        }
                    }
                }

            }
            if (userExist == false)
            {
                TempData["Message"] = "User does not exist!";
                return RedirectToAction("Index");
            }
            return View(userList);
        }
       
        public IActionResult Privacy()
        {
            return View();
        }

        public ActionResult LogOut()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
