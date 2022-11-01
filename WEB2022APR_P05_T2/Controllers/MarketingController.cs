using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2022APR_P05_T2.DAL;
using WEB2022APR_P05_T2.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace WEB2022APR_P05_T2.Controllers
{
    public class MarketingController : Controller
    {
        private FeedbackPageDAL feedbackContext = new FeedbackPageDAL();
        private UserTransactionDAL transactionContext = new UserTransactionDAL();
        private List<string> year = new List<string> { "2017", "2018", "2019", "2020" };
        private List<string> month = new List<string> { "1", "2", "3", "4", "5","6","7","8","9","10","11","12"};
        private List<SelectListItem> SelectYear = new List<SelectListItem>();
        private List<SelectListItem> SelectMonth = new List<SelectListItem>();
        private List<string> voucher = new List<string> { "20", "40", "80", "160" };
        private List<SelectListItem> SelectVoucher = new List<SelectListItem>();

        public MarketingController(){
            foreach (var yr in year)
            {
                SelectYear.Add(
                    new SelectListItem
                    {
                        Value = yr,
                        Text = yr
                    });
            }
            foreach (var mnth in month)
            {
                SelectMonth.Add(
                    new SelectListItem
                    {
                        Value = mnth,
                        Text = mnth
                    });
            }
            foreach (var v in voucher)
            {
                SelectVoucher.Add(
                    new SelectListItem
                    {
                        Value = v,
                        Text = v
                    });
            }
        }
        // GET: MarketingController
        public ActionResult Index()
        {
            HttpContext.Session.GetString("Role");
            if (HttpContext.Session.GetString("Role") == null || HttpContext.Session.GetString("Role") != "Marketing"){
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        public ActionResult Feedback()
        {
            List<Feedback> feedbackList = feedbackContext.GetFeedback();
            return View(feedbackList);
        }

        public ActionResult Respond(int id)
        {
            Feedback chosenFeedback = new Feedback();
            chosenFeedback = feedbackContext.getFeedbackDetail(id);
            Response newResponse = new Response();
            newResponse.MemberID = chosenFeedback.MemberID;
            newResponse.UserFeedback = chosenFeedback.UserFeedback;
            newResponse.UserSubject = chosenFeedback.UserSubject;
            return View(newResponse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Respond(Response response, int id)
        {
            Response newResponse = new Response();
            newResponse.DateTimePosted = DateTime.Now;
            newResponse.FeedbackID = id;
            newResponse.StaffID = "Marketing";
            newResponse.MemberID = null;
            newResponse.Text = response.Text;
            
            if (ModelState.IsValid)
            {
                feedbackContext.addResponse(newResponse);
                return RedirectToAction("Index");
            }
            
            else
            {
                return View(response);
            }
            
        }
        public ActionResult Details(int id)
        {
            return View();
        }
        
        public ActionResult ViewMonthlySpending()
        {
            ViewData["ShowResult"] = false;
            ViewData["SelectYear"] = SelectYear;
            ViewData["SelectMonth"] = SelectMonth;
           
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ViewMonthlySpending(MonthlySpendingViewModel ms)
        {
            ViewData["ShowResult"] = true;
            ViewData["SelectYear"] = SelectYear;
            ViewData["SelectMonth"] = SelectMonth;
            ViewData["SelectVoucher"] = SelectVoucher;
            List<MonthlySpending> monthlySpendings = transactionContext.GetMonthlySpendingsByMonth(ms.selMonth, ms.selYear);
            List<CashVoucher> cashVouchers = transactionContext.GetCashVouchersbyMonth(ms.selMonth, ms.selYear);
            List<MonthlySpending> eachCustomer = new List<MonthlySpending>();
            for(int i = 0; i<monthlySpendings.Count; i++)
            {
                bool MemberExist = false;
                if (i == 0)
                {
                    MemberExist = true;
                    eachCustomer.Add(
                    new MonthlySpending
                    {
                        MemberID = monthlySpendings[i].MemberID,
                        noTransactions = 1,
                        TotalAmtSpent = monthlySpendings[i].TotalAmtSpent,
                        Voucher = 0,
                        VoucherAssigned = false
                    });
                    continue;
                }
                foreach(MonthlySpending c in eachCustomer)
                {
                    if(monthlySpendings[i].MemberID == c.MemberID)
                    {
                        MemberExist = true;
                    }
                }
                if (MemberExist == false)
                {
                    eachCustomer.Add(
                        new MonthlySpending
                        {
                            MemberID = monthlySpendings[i].MemberID,
                            noTransactions = 1,
                            TotalAmtSpent = monthlySpendings[i].TotalAmtSpent,
                            Voucher = 0,
                            VoucherAssigned = false
                        });
                }
                else
                {
                    foreach(MonthlySpending cust in eachCustomer)
                    {
                        if (cust.MemberID == monthlySpendings[i].MemberID)
                        {
                            cust.noTransactions = cust.noTransactions + 1;
                            cust.TotalAmtSpent = cust.TotalAmtSpent + monthlySpendings[i].TotalAmtSpent;
                            continue;
                        }
                    }
                }
            }
            foreach(CashVoucher voucher in cashVouchers)
            {
                foreach(MonthlySpending cust in eachCustomer)
                {
                    if (voucher.MemberID == cust.MemberID)
                    {
                        cust.Voucher = cust.Voucher + voucher.Amount;
                        cust.VoucherAssigned = true;
                    }
                }
            }
            List<MonthlySpending> sortedMonthlySpending = eachCustomer.OrderBy(o => o.noTransactions).ToList();

            MonthlySpendingViewModel mSVM = new MonthlySpendingViewModel();
            mSVM.monthlySpendingList = sortedMonthlySpending;
            mSVM.voucherSelect = getVouchers();
            ViewData["SelectVoucher"] = getVouchers();
            return View(mSVM);
        }
        
        public List<SelectListItem> getVouchers()
        {
            return new List<SelectListItem>
            {
                new SelectListItem
                {
                    Value = "20",
                    Text = "20"
                },
                new SelectListItem
                {
                    Value = "40",
                    Text= "40"
                },
                new SelectListItem
                {
                    Value = "80",
                    Text = "80"
                },
                new SelectListItem
                {
                    Value = "160",
                    Text = "160"
                }
            };
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AssignVoucher(string memId, IFormCollection collection)
        {
            decimal voucher = Convert.ToDecimal(collection["selectedVoucher"]);
            Console.WriteLine(voucher);
            for (int i = 0; i < voucher/20; i++)
            {
                CashVoucher newVoucher = new CashVoucher();
                newVoucher.MemberID = memId;
                newVoucher.Amount = 20;
                //newVoucher.MonthIssuedFor = DateTime.Now.Month - 1;
                newVoucher.MonthIssuedFor = 5;
                //newVoucher.YearIssuedFor = DateTime.Now.Year;
                newVoucher.YearIssuedFor = 2017;
                newVoucher.DateTimeIssued = DateTime.Now;
                newVoucher.VoucherSN = null;
                newVoucher.Status = '0';
                newVoucher.DateTimeRedeemed = null;
                transactionContext.assignVoucher(newVoucher);
                Console.WriteLine("konnichiwar");
            }
            return RedirectToAction("ViewMonthlySpending");
        }

        // GET: MarketingController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: MarketingController/Edit/5
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

        // GET: MarketingController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: MarketingController/Delete/5
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
