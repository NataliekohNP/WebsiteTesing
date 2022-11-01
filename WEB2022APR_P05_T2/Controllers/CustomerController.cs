using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;
using WEB2022APR_P05_T2.DAL;
using WEB2022APR_P05_T2.Models;

namespace WEB2022APR_P05_T2.Controllers
{
    public class CustomerController : Controller
    {

        private UserDAL userContext = new UserDAL();
        private CustomerFeedbackDAL feedbackContext = new CustomerFeedbackDAL();

        // GET: CustomerController
        public ActionResult Index()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }

            String User = HttpContext.Session.GetString("Username");
            CustomerEdit customer = userContext.GetCustomerDetails(User);

            return View(customer);
        }

        /*Implementation of new pages*/

        // GET: CustomerController/ViewVoucher
        public ActionResult ViewVoucher()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }

            String User = HttpContext.Session.GetString("Username");
            List<CashVoucher> cashVoucherList = userContext.GetCashVouchersbyMemberID(User);
            return View(cashVoucherList);
        }

        // GET: CustomerController/CreateFeedback
        public ActionResult CreateFeedback()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        public ActionResult Feedback()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }

            String User = HttpContext.Session.GetString("Username");
            List<Feedback> feedbackList = feedbackContext.GetFeedbackbyID(User);
            ViewData["name"] = User;
            return View(feedbackList);
        }

        public ActionResult ViewResponse(string? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            String User = HttpContext.Session.GetString("Username");
            List<Feedback> feedbackList = feedbackContext.GetFeedbackbyID(User);
            for (int i = 0; i < feedbackList.Count; i++)
            {
                if (feedbackList[i].FeedbackID.ToString() == id)
                {
                    ViewData["feedbackID"] = feedbackList[i].FeedbackID;
                    ViewData["feedbackText"] = feedbackList[i].UserFeedback;
                    ViewData["user"] = User;
                    break;
                }
            }

            List<Response> responseList = feedbackContext.GetResponsebyFeedbackID(id);
            if (responseList == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(responseList);
        }

        public ActionResult CreateResponse(int? id)
        {

            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            Feedback chosenFeedback = new Feedback();
            chosenFeedback = feedbackContext.getFeedbackDetail((int)id);
            Response newResponse = new Response();
            newResponse.MemberID = chosenFeedback.MemberID;
            newResponse.UserFeedback = chosenFeedback.UserFeedback;
            newResponse.UserSubject = chosenFeedback.UserSubject;
            return View(newResponse);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateResponse(Response response, int? id)
        {

            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { 
              //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            } if (response == null)
            {
                //Query string parameter not provided
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            String User = HttpContext.Session.GetString("Username");

            Response newResponse = new Response();
            newResponse.DateTimePosted = DateTime.Now;
            newResponse.FeedbackID = (int)id;
            newResponse.StaffID = null;
            newResponse.MemberID = User;
            newResponse.Text = response.Text;

            if (ModelState.IsValid)
            {
                feedbackContext.addResponse(newResponse);
                return RedirectToAction("Index");
            }

            else
            {
                ViewData["message"] = "Do fill up the response";
                return View(response);
            }

        }

        // POST: CustomerController/CreateFeedback
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateFeedback(Feedback feedback)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (feedback == null)
            {
                //Query string parameter not provided
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            String User = HttpContext.Session.GetString("Username");

            Feedback newFeedback = new Feedback();
            newFeedback.MemberID = User;
            newFeedback.DateTimePosted = DateTime.Now;
            newFeedback.UserSubject = feedback.UserSubject;
            newFeedback.UserFeedback = feedback.UserFeedback;
            newFeedback.fileToUpload = feedback.fileToUpload;

            if (ModelState.IsValid)
            {
                if (newFeedback.fileToUpload != null)
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(newFeedback.fileToUpload.FileName);
                    // Rename the uploaded file with the Customer’s name.
                    newFeedback.ImageFileName = newFeedback.UserSubject.ToLower() + fileExt;
                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", newFeedback.ImageFileName);
                    // Upload the file to server
                    using (var fileSteam = new FileStream(savePath, FileMode.Create))
                    {
                        await newFeedback.fileToUpload.CopyToAsync(fileSteam);
                    }
                    feedbackContext.addFeedback(newFeedback);
                    return RedirectToAction("Index");
                }
                else
                {
                    feedbackContext.addFeedback(newFeedback);
                    return RedirectToAction("Index");
                }
            }

            else
            {
                return View();
            }
        }

        // GET: CustomerController/Profile
        [HttpGet]
        public ActionResult Profile()
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }

            string User = HttpContext.Session.GetString("Username");
            List<Customer> customerList = userContext.GetCustomer(User);
            return View(customerList);
        }

        // GET: CustomerController/Edit/id
        public ActionResult Edit(string? id)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Staff" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            { //Query string parameter not provided
              //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            CustomerEdit customer = userContext.GetCustomerDetails(id);
            if (customer == null)
            {
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CustomerEdit customer)
        {
            // Stop accessing the action if not logged in
            // or account not in the "Customer" role
            if ((HttpContext.Session.GetString("Role") == null) ||
            (HttpContext.Session.GetString("Role") != "Customer"))
            {
                return RedirectToAction("Index", "Home");
            }
            if (customer == null)
            {
                //Query string parameter not provided
                //Return to listing page, not allowed to edit
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                if (customer.MEmailAddr != null)
                {
                    if (userContext.IsEmailExist(customer.MEmailAddr))
                    {
                        if (customer.MTelNo != null)
                        {
                            if (userContext.IsTelNoExist(customer.MTelNo))
                            {
                                return RedirectToAction("Profile");
                            }
                            else
                            {
                                //Update customer record to database
                                userContext.Update(customer);
                                return RedirectToAction("Profile");
                            }
                        }
                        else
                        {
                            //Update customer record to database
                            userContext.Update(customer);
                            return RedirectToAction("Profile");
                        }
                    }
                    else
                    {
                        //Update customer record to database
                        userContext.Update(customer);
                        return RedirectToAction("Profile");
                    }
                }
                else
                {
                    //Update customer record to database
                    userContext.Update(customer);
                    return RedirectToAction("Profile");
                }
            }
            else
            {
                //Input validation fails, return to the view
                //to display error message
                return View(customer);
            }

        }

        public async Task<IActionResult> UploadPhoto(Feedback feedback)
        {
            if (feedback.fileToUpload != null && feedback.fileToUpload.Length > 0)
            {
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(feedback.fileToUpload.FileName);
                    // Rename the uploaded file with the staff’s name.
                    string uploadedFile = feedback.FeedbackID + fileExt;
                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", uploadedFile);
                    // Upload the file to server
                    using (var fileSteam = new FileStream(savePath, FileMode.Create))
                    {
                        await feedback.fileToUpload.CopyToAsync(fileSteam);
                    }

                    feedback.ImageFileName = uploadedFile;
                    ViewData["Message"] = "File uploaded successfully.";
                }

                catch (IOException)
                {
                    //File IO error, could be due to access rights denied
                    ViewData["Message"] = "File uploading fail!";
                }
                catch (Exception ex) //Other type of error
                {
                    ViewData["Message"] = ex.Message;
                }
            }
            return View(feedback);
        }
        
        /*-------------------------------------------------------------------------------------------------------------*/
    }
}
