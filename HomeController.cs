using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class HomeController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-37GV8N2\\SQLEXPRESS;Initial Catalog=LMS;Integrated Security=True;");
        // GET: Home load event ofindex

        public ActionResult Index()
        {
            //select batch and select categories
            string query = $"select * from tbl_batch order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ViewBag.table1 = dt;

            return View();
        }
        //save data of students in table on click of submit button of registration form

        public ActionResult saveuser(string txt_name, string txt_email,string txt_mobno,HttpPostedFileBase file_profile,string ddl_course,string ddl_year,int ddl_batch,string txt_password)
        {
            string query = $"insert into tbl_student values('{txt_name}','{txt_mobno}','{txt_email}','{txt_password}','{ddl_course}','{ddl_course}','{ddl_year}','{file_profile.FileName}',{ddl_batch},0,'{DateTime.Now.ToString("yyyy-MM-dd")}')";
            SqlCommand cmd = new SqlCommand(query,con);
            con.Open();
             int result= cmd.ExecuteNonQuery();
            con.Close();
            if (result > 0)
            {

                file_profile.SaveAs(Server.MapPath("/Content/profilepic/" + file_profile.FileName));
                return Content("<script>alert('you have registered successfully. You will recieve confirmation from admin very soon via email');location.href='/home/index'</script>");
            }
            else
            {
                return Content("<script>alert('Try again');location.href='/home/index'</script>");
            }
        }
        [HttpPost]

        public ActionResult login(string email, string password)
        {
            string query = $"select * from tbl_student where emailid='{email}' and password='{password}'";
            SqlDataAdapter sda = new SqlDataAdapter(query,con);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);

            if(dt1.Rows.Count>0)
            {
                if (Convert.ToInt32(dt1.Rows[0]["status"])==0)
                {
                    Session["name"] = dt1.Rows[0]["name"];
                    Session["email"] = dt1.Rows[0]["emailid"];
                    Session["batch"] = dt1.Rows[0]["batch"];
                    Session["picture"] = dt1.Rows[0]["picture"];
                    return Content("<script>alert('Welcome');location.href='/student/dashboard'</script>");
                    
                }
                else
                {

                    return Content("<script>alert('Welcome');location.href='student/dashboard'</script>");

                }
            }
            else
            {
                return Content("<script>alert('Invalid Id or Password');location.href='home/index'</script>");
            }
        }
        [HttpGet]
        public ActionResult SaveContact()
        {
            return View();
        }
        public ActionResult AdminLogin()
        {
            return View();
        }
        


    }
}