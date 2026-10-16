using CURDEx.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CURDEx.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        CompanyContext cc;
        public ProductAPIController(CompanyContext cc)
        {
            this.cc = cc;
        }

        /// <summary>
        /// Method to retrive All Products
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        public IActionResult GetProducts()
        {
            var res = this.cc.Products.ToList();
            if (res == null)
                return NotFound("No Record Found!");
            return Ok(res);
        }


        /// <summary>
        /// Method to retrivethe product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpGet("{id}")]
        public IActionResult GetProducts(Int64 id)
        {
            var res = this.cc.Products.SingleOrDefault(p=>p.ProductID==id);
            if (res == null)
                return NotFound("No Record Found!");

            return Ok(res);
        }

        /// <summary>
        /// method to insert or add product
        /// </summary>
        /// <param name="prod"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Insert(Product prod)
        {
            try
            {
                this.cc.Products.Add(prod);
                this.cc.SaveChanges();
                //return Created();
                return Created("/productapi/insert",prod);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///  method to update the record 
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="prod"></param>
        /// <returns></returns>

        [HttpPut("{prodid}")]
        public IActionResult Update(Int64 prodid, Product prod)
        {
            var oldrec = this.cc.Products.SingleOrDefault(p=>p.ProductID==prodid);
            if (oldrec == null)
                return NotFound();
            oldrec.ProductName=prod.ProductName;
            oldrec.MfgName = prod.MfgName;
            oldrec.Price = prod.Price;
            this.cc.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// method to patch the product 
        /// </summary>
        /// <param name="prodid"></param>
        /// <param name="patch"></param>
        /// <returns></returns>

        [HttpPatch("{prodid}")]
        public IActionResult PatchProduct(Int64 prodid, JsonPatchDocument<Product> patch)
        {
            var oldrec = this.cc.Products.SingleOrDefault(p => p.ProductID == prodid);
            if (oldrec == null)
                return NotFound();

            patch.ApplyTo(oldrec);

            this.cc.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Method to delete product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        [HttpDelete("{id}")]
        public IActionResult Delete(Int64 id)
        {
            if (id == 0)
                return BadRequest("Invalid Product ID");
            try
            {
                var rec = this.cc.Products.SingleOrDefault(p=>p.ProductID==id);
                if (rec == null)
                    return NotFound();
                        
                this.cc.Products.Remove(rec);
                this.cc.SaveChanges();
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
