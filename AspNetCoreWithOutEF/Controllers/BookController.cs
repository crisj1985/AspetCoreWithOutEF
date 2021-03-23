using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
using AspNetCoreWithOutEF.Data;
using AspNetCoreWithOutEF.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AspNetCoreWithOutEF.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;

        public BookController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        // GET: Book
        public IActionResult Index()
        {
            DataTable dtResult = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewAll", sqlCon);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtResult);
            }
            return View(dtResult);
        }

        
        // GET: Book/Edit/5
        public IActionResult AddOrEdit(int id)
        {
            BookModelView bookModelView = new BookModelView();
            if (id > 0)
                bookModelView = FetchBookById(id);

            return View(bookModelView);
        }

        // POST: Book/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("BookId,Title,Author,Price")] BookModelView bookModelView)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection sqlCon = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
                {
                    sqlCon.Open();
                    SqlCommand sqlCommand = new SqlCommand("BookAddOrEdit", sqlCon);
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("BookId", bookModelView.BookId);
                    sqlCommand.Parameters.AddWithValue("Title", bookModelView.Title);
                    sqlCommand.Parameters.AddWithValue("Author", bookModelView.Author);
                    sqlCommand.Parameters.AddWithValue("Price", bookModelView.Price);
                    sqlCommand.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookModelView);
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int id)
        {
            BookModelView objBook = new BookModelView();
            objBook = FetchBookById(id);
            return View(objBook);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection sqlCon = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlCon.Open();
                SqlCommand sqlCo = new SqlCommand("DeleteById", sqlCon);
                sqlCo.Parameters.AddWithValue("BookId", id);
                sqlCo.CommandType = CommandType.StoredProcedure;
                sqlCo.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        public BookModelView FetchBookById(int id)
        {
            BookModelView objBook = new BookModelView();
            DataTable dtResult = new DataTable();
            using (SqlConnection sqlCon = new SqlConnection(_configuration.GetConnectionString("DevConnection")))
            {
                sqlCon.Open();
                SqlDataAdapter sqlDa = new SqlDataAdapter("BookViewById", sqlCon);
                sqlDa.SelectCommand.Parameters.AddWithValue("BookId", id);
                sqlDa.SelectCommand.CommandType = CommandType.StoredProcedure;
                sqlDa.Fill(dtResult);
            }

            if (dtResult.Rows.Count == 1)
            {
                objBook.BookId = Convert.ToInt32(dtResult.Rows[0]["BookId"].ToString());
                objBook.Title = dtResult.Rows[0]["Title"].ToString();
                objBook.Author = dtResult.Rows[0]["Author"].ToString();
                objBook.Price = Convert.ToInt32(dtResult.Rows[0]["Price"].ToString());
            }

            return objBook;
        }

    }
}
