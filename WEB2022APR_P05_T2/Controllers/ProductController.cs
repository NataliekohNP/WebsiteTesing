using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using WEB2022APR_P05_T2.DAL;
using WEB2022APR_P05_T2.Models;
using System.IO;

namespace WEB2022APR_P05_T2.Controllers
{
    public class ProductController : Controller
    {
        private ProductDAL productContext = new ProductDAL();
        // GET: ProductController
        public ActionResult Index()
        {
            List<Product> productList = productContext.GetProduct();
            return View(productList);
        }
        
        // GET: ProductController/Details/5
        public ActionResult ProductDetail(int id)
        {
            Product product = productContext.GetProductDetail(id);
            Product productvm = MapToProductVM(product);
            return View(productvm);
        }
        public Product MapToProductVM(Product product)
        {
            Product productvm = new Product
            {
                ProductId = product.ProductId,
                ProductTitle = product.ProductTitle,
                ProductImage = product.ProductImage,
                Price = product.Price,
                EffectiveDate = product.EffectiveDate,
                Obsolete = product.Obsolete
            };
            return productvm;
              
        }
        // GET: ProductController/Create
        public ActionResult CreateProducts()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateProductsAsync(Product product)
        {   
            
                try
                {
                    // Find the filename extension of the file to be uploaded.
                    string fileExt = Path.GetExtension(
                     product.filetoupload.FileName);
                    
                    // Rename the uploaded file with the staff’s name.
                    string uploadedFile = product.ProductTitle + fileExt;
                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot\\image\\Products", uploadedFile);
                    // Upload the file to server
                    using (var fileSteam = new FileStream(
                     savePath, FileMode.Create))
                    {
                        await product.filetoupload.CopyToAsync(fileSteam);
                    }
                    ViewData["Message"] = "File uploaded successfully.";
                    product.ProductId = productContext.Add(product);
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
            
            return RedirectToAction("Index");
 
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int? id)
        {
            Product product = productContext.GetProductDetail(id.Value);
            return View(product);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditAsync(Product product)
        {
            
                // Find the filename extension of the file to be uploaded.

                if (product.filetoupload == null)
                {
                    productContext.Update2(product);
                    return RedirectToAction(nameof(Index));
                }
                else {
                    string fileExt = Path.GetExtension(
                 product.filetoupload.FileName);

                    // Rename the uploaded file with the staff’s name.
                    string uploadedFile = product.ProductTitle + fileExt;
                    // Get the complete path to the images folder in server
                    string savePath = Path.Combine(
                     Directory.GetCurrentDirectory(),
                     "wwwroot\\image\\Products", uploadedFile);
                    // Upload the file to server
                    using (var fileSteam = new FileStream(
                     savePath, FileMode.Create))
                    {
                        await product.filetoupload.CopyToAsync(fileSteam);
                    }
                    ViewData["Message"] = "File uploaded successfully.";
                    productContext.Update(product);
                    return RedirectToAction(nameof(Index));
                }
                    
         
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int? id)
        {
            Product product = productContext.GetProductDetail(id.Value);
            return View(product);
        }
       

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Product product)
        {
            try
            {
                productContext.ObsoleteProduct(product);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
    }
}
