using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB2022APR_P05_T2.Models;
using WEB2022APR_P05_T2.DAL;
using System.Net.Http;
using Newtonsoft.Json;
using System.Diagnostics;

namespace WEB2022APR_P05_T2.Controllers
{
    public class SalesPersonnelController : Controller
    {
        private UserDAL userDAL = new UserDAL();
        

        // List that stores the options for Gender
        private List<char> gender = new List<char> { 'M', 'F' };
        // List that stores the options for all the countries in the world
        private List<string> countriesList = new List<string>();

        // A list for populating drop-down list
        private List<SelectListItem> countryList2 = new List<SelectListItem>();

        // Constructor for the SalesPersonnelController
        //public SalesPersonnelController()
        //{
        //}

        public ActionResult Account()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Sales"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                int num = 1;
                while (true)
                {
                    if (HttpContext.Session.GetString(num.ToString()) != null)
                    {
                        countriesList.Add(HttpContext.Session.GetString(num.ToString()));
                        num++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = 0; i < countriesList.Count; i++)
                {
                    countryList2.Add(new SelectListItem
                    {
                        Value = countriesList[i],
                        Text = countriesList[i]
                    });
                }

                ViewData["ShowResult"] = false;
                ViewData["MGender"] = gender;
                ViewData["MCountry"] = countryList2;

                Customer customer = new Customer
                {
                    MemberID = "",
                    MName = "",
                    MGender = gender[0],
                    MBirthDate = DateTime.Now,
                    MAddress = "",
                    MCountry = Convert.ToString(countryList2[0].Value),
                    MTelNo = "",
                    MEmailAddr = ""
                };

                return View(customer);
            }
        }

        // POST: SalesPersonnelController/Account
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Account(Customer customer)
        {
            // Get gender list & country list for drop-down list in case of the need to return to Create.cshtml view
            ViewData["MGender"] = gender;
            ViewData["MCountry"] = countryList2;

            if (ModelState.IsValid)
            {
                // Add staff record to database
                customer.MemberID = userDAL.Add(customer);
                // Redirect user to SalesPersonnel/Index
                return RedirectToAction("Index");
            }
            else
            {
                int num = 1;
                while (true)
                {
                    if (HttpContext.Session.GetString(num.ToString()) != null)
                    {
                        countriesList.Add(HttpContext.Session.GetString(num.ToString()));
                        num++;
                    }
                    else
                    {
                        break;
                    }
                }

                for (int i = 0; i < countriesList.Count; i++)
                {
                    countryList2.Add(new SelectListItem
                    {
                        Value = countriesList[i],
                        Text = countriesList[i]
                    });
                }
                // Input validation fails, return to the Account view to display error message
                return View(customer);
            }
        }

        public ActionResult Records()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Sales"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<CashVoucher> cvoucherList = userDAL.GetAllCollectible();
                ViewData["Collectible Vouchers"] = cvoucherList;
                List<CashVoucher> rvoucherList = userDAL.GetAllRedeemable();
                ViewData["Redeemable Vouchers"] = rvoucherList;
                List<Customer> customerList = userDAL.GetAllCustomer();
                return View(customerList);
            }
        }

        public ActionResult Collect()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Sales"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<CashVoucher> voucherList = userDAL.GetAllCollectible();
                return View(voucherList);
            }
        }

        // GET: SalesPersonnelController
        public async Task<ActionResult> Index()
        {
            if ((HttpContext.Session.GetString("Role") == null) ||
                (HttpContext.Session.GetString("Role") != "Sales"))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                List<Customer> customerList = userDAL.GetAllCustomer();
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("https://restcountries.com");
                HttpResponseMessage response = await client.GetAsync("/v3.1/all");
                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();
                    Country.Class1[] countryList = JsonConvert.DeserializeObject<Country.Class1[]>(data);
                    List<string> countryNameList = new List<string>();
                    List<string> aCountryList = new List<string>();
                    List<string> bCountryList = new List<string>();
                    List<string> cCountryList = new List<string>();
                    List<string> dCountryList = new List<string>();
                    List<string> eCountryList = new List<string>();
                    List<string> fCountryList = new List<string>();
                    List<string> gCountryList = new List<string>();
                    List<string> hCountryList = new List<string>();
                    List<string> iCountryList = new List<string>();
                    List<string> jCountryList = new List<string>();
                    List<string> kCountryList = new List<string>();
                    List<string> lCountryList = new List<string>();
                    List<string> mCountryList = new List<string>();
                    List<string> nCountryList = new List<string>();
                    List<string> oCountryList = new List<string>();
                    List<string> pCountryList = new List<string>();
                    List<string> qCountryList = new List<string>();
                    List<string> rCountryList = new List<string>();
                    List<string> sCountryList = new List<string>();
                    List<string> tCountryList = new List<string>();
                    List<string> uCountryList = new List<string>();
                    List<string> vCountryList = new List<string>();
                    List<string> wCountryList = new List<string>();
                    List<string> xCountryList = new List<string>();
                    List<string> yCountryList = new List<string>();
                    List<string> zCountryList = new List<string>();

                    for (int i = 0; i < countryList.Length; i++)
                    {
                        if (countryList[i].name.common.StartsWith("A"))
                        {
                            aCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("B"))
                        {
                            bCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("C"))
                        {
                            cCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("D"))
                        {
                            dCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("E"))
                        {
                            eCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("F"))
                        {
                            fCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("G"))
                        {
                            gCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("H"))
                        {
                            hCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("I"))
                        {
                            iCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("J"))
                        {
                            jCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("K"))
                        {
                            kCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("L"))
                        {
                            lCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("M"))
                        {
                            mCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("N"))
                        {
                            nCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("O"))
                        {
                            oCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("P"))
                        {
                            pCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("Q"))
                        {
                            qCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("R"))
                        {
                            rCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("S"))
                        {
                            sCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("T"))
                        {
                            tCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("U"))
                        {
                            uCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("V"))
                        {
                            vCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("W"))
                        {
                            wCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("X"))
                        {
                            xCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("Y"))
                        {
                            yCountryList.Add(countryList[i].name.common);
                        }
                        else if (countryList[i].name.common.StartsWith("Z"))
                        {
                            zCountryList.Add(countryList[i].name.common);
                        }
                    }

                    foreach (string s in aCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in bCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in cCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in dCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in eCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in fCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in gCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in hCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in iCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in jCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in kCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in lCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in mCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in nCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in oCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in pCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in qCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in rCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in sCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in tCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in uCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in vCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in wCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in xCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in yCountryList)
                    {
                        countryNameList.Add(s);
                    }
                    foreach (string s in zCountryList)
                    {
                        countryNameList.Add(s);
                    }

                    int count = 1;
                    foreach(string s in countryNameList)
                    {
                        HttpContext.Session.SetString(count.ToString(), s);
                        count++;
                    }
                }
                ViewData["UserName"] = HttpContext.Session.GetString("Username");
                ViewData["NumOfMember"] = customerList.Count;
                return View();
            }
        }

        // GET: SalesPersonnelController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SalesPersonnelController/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: SalesPersonnelController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: SalesPersonnelController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: SalesPersonnelController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: SalesPersonnelController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
