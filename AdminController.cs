using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using System.Web.WebPages;
using static System.Net.Mime.MediaTypeNames;
using System.Web.Mvc.Ajax;
namespace LMS.Controllers
{
    public class AdminController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-37GV8N2\\SQLEXPRESS;Initial Catalog=LMS;Integrated Security=True;");
        // GET: Admin
        public ActionResult Index()
        {

            return View();
        }
        [HttpGet]//
        public ActionResult AddBatch()
        {
            //selesct all data from tbl_batch
            string query = "select * from tbl_batch order by sr desc";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            //transfer data of Viewbag to Viewpage
            ViewBag.data = dt;
            return View();
        }
        [HttpPost]
        public ActionResult Savebatch(string txt_class)
        {

            string query = $"insert into tbl_batch values('{txt_class}',1)";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            if (result > 0)
            {
                return Content("<script>alert('Batch added Successfully');location.href='/admin/AddBatch'</script>");
            }
            else
            {
                return Content("<script>alert('Batch not added Successfully');location.href='/admin/AddBatch'</script>");
            }

        }
        public ActionResult AddVideoCategory()
        {
            //selesct all data from tbl_batch
            string query = "select * from tbl_category order by sr desc";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            adapter.Fill(dt);

            //transfer data of Viewbag to Viewpage
            ViewBag.data = dt;
            return View();
        }
        //on load event of addvideo form
        public ActionResult SubmitVideo()
        {
            //select batch and select categories
            string query = $"select * from tbl_batch order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            //select data from video csategory
            string command = $"select * from tbl_category order by sr desc";
            SqlDataAdapter adapter = new SqlDataAdapter(command, con);
            DataTable table2 = new DataTable();
            adapter.Fill(table2);


            ViewBag.table1 = dt;
            ViewBag.table2 = table2;

            return View();


        }

        public ActionResult ContactForm()
        {
            return View();
        }
        public ActionResult FIleUpload()
        {
            return View();
        }
        public ActionResult Assignment()
        {
            //select batch and select categories
            string query = $"select * from tbl_batch order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);

            //select data from tbl assignment

            string command = $"select * from tbl_assignment order by sr desc";
            SqlDataAdapter adapter1 = new SqlDataAdapter(command, con);
            DataTable dt2 = new DataTable();
            adapter1.Fill(dt2);






            ViewBag.table1 = dt1;
            ViewBag.table2 = dt2;


            return View();
        }
        //save video category into database
        [HttpPost]
        public ActionResult savevideocategory(string txt_category, HttpPostedFileBase file_thumbnail)
        {
            string query = $"insert into tbl_category values('{txt_category}','{file_thumbnail.FileName}','{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            if (result > 0)
            {
                file_thumbnail.SaveAs(Server.MapPath("/Content/catpici/" + file_thumbnail.FileName));
                return Content("<script>alert('Video added Successfully');location.href='/admin/addvideocategory'</script>");
            }
            else
            {
                return Content("<script>alert('Video not added Successfully');location.href='/admin/addvideocateory'</script>");
            }


        }
        //save video in database
        [HttpPost]
        public ActionResult savevideo(string txt_title, int ddl_batch, int ddl_category, string txt_desc, string txt_link, HttpPostedFileBase file_thumbnail)
        {
            string query = $"insert into tbl_video values('{txt_title}',{ddl_batch},{ddl_category},'{txt_desc}','{txt_link}','{file_thumbnail.FileName}','{DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")}')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int result = cmd.ExecuteNonQuery();

            con.Close();
            if (result > 0)
            {

                file_thumbnail.SaveAs(Server.MapPath("/Content/videopic/" + file_thumbnail.FileName));
                return Content("<script>alert('Video added Successfully');location.href='/admin/submitvideo'</script>");
            }
            else
            {
                return Content("<script>alert('Video not added Successfully');location.href='/admin/submitvideo'</script>");
            }
        }

        //save data of assignment in database on click of submit button
        [HttpPost]
        public ActionResult saveassignment(int ddl_batch, string txt_subject, DateTime date_lastdate, HttpPostedFileBase file_assignment, string txt_title, string txt_teacher, string txt_desc)

        {
            string query = $"insert into tbl_assignment values('{ddl_batch}','{txt_subject}','{txt_title}','{txt_desc}','{file_assignment.FileName}','{txt_teacher}','{date_lastdate.ToString("yyyy-MM-dd")}','{DateTime.Now.ToString("yyyy-MM-dd")}',1)";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            if (result > 0)
            {

                file_assignment.SaveAs(Server.MapPath("/Content/taskfile/" + file_assignment.FileName));
                return Content("<script>alert('Video added Successfully');location.href='/admin/assignment'</script>");
            }
            else
            {
                return Content("<script>alert('Video not added Successfully');location.href='/admin/assignment'</script>");
            }

        }
        public ActionResult ManageStudent()
        {
            string query = "select * from tbl_student left join tbl_batch on tbl_student.batch=tbl_batch.sr order by regdate";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            ViewBag.data = dt;


            return View();
        }
        public ActionResult updatestatus(string email, int? status)
        {
            if (!email.IsEmpty() && status.HasValue)
            {
                int s = status == 0 ? 1 : 0;
                string query = $"update tbl_student set status=[s] where emailid='{email}'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                int r = cmd.ExecuteNonQuery();
                con.Close();
                return Content("<script>alert('Status updated');location.href='/admin/managestudent'</script>");
            }
            else
            {
                return Content("<script>alert('Try again');location.href='/admin/managestudent'</script>");
            }
        }
        public ActionResult logout()
        {
            Session.Remove("admin");
            return Content("<script>alert('Logged out');location.href='/home/addlogin'</script>");
        }
        public ActionResult manageassignment()
        {
            string query = $"select * from tbl_submittedtask left join tbl_assignment on taskno=tbl_assignment.sr left join tbl_student on tbl_submittedtask.userid=tbl_student.emailid order by tbl_submittedtask.sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);

            ViewBag.assignment = dt;
            return View();
        }
        public ActionResult Notes()
        {
            //select batch and select categories
            string query = $"select * from tbl_batch order by sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt1 = new DataTable();
            sda.Fill(dt1);

            return View();
        }
        [HttpPost]

        public ActionResult savenotes(string txt_title, string txt_desc, string txt_teacher, int ddl_batch, HttpPostedFileBase file_notes)
        {
            string query = $"insert into tbl_notes values('{txt_title}','{txt_desc}','{txt_teacher}','{file_notes.FileName}',{ddl_batch},'{DateTime.Now.ToString("yyyy-MM-dd")}')";
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            int result = cmd.ExecuteNonQuery();
            con.Close();
            if (result > 0)
            {
                file_notes.SaveAs(Server.MapPath("/Content/notes/" + file_notes.FileName));
                return Content("<script>alert('notes added Successfully');location.href='/admin/notes'</script>");
            }
            else
            {
                return Content("<script>alert('notes not added S');location.href='/admin/notes'</script>");
            }
        }
        [HttpPost]
        public ActionResult adminsignin(string email, string password)
        {
            if (email.Equals("husainsaqib14@gmail.com") && password == "123")
            {
                Session["admin"] = email;
                return Content("<script>alert('Welcome'); location.href='/admin/addbatch'<script>");
            }
            else
            {
                return Content("<script>alert('invalid');location.href='/home/index'<script>");
            }

        }


    }
    
    
    

}
    

