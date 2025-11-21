using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LMS.Controllers
{
    public class StudentController : Controller
    {
        SqlConnection con = new SqlConnection("Data Source=DESKTOP-37GV8N2\\SQLEXPRESS;Initial Catalog=LMS;Integrated Security=True;");
        // GET: Student
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FileUpload()
        {
            return View();
        }
        public ActionResult Dashboard()
        {
            return View();
        }
        public ActionResult Courses()
        {
            string query = "select * from tbl_category order by sr desc";
            SqlDataAdapter sda= new SqlDataAdapter(query,con);
            DataTable dt=new DataTable();
            sda.Fill(dt);

            ViewBag.cat = dt;
            return View();
        }
        public ActionResult SoftwareKit()
        {
            
            return View();
        }
        public ActionResult Task()
        {
            int batchid = Convert.ToInt32(Session["batch"]);
            string query = $"select * from tbl_assignment left join tbl_submittedtask on tbl_assignment.sr=taskno where batch={batchid} order by tbl_assignment.sr desc";
            SqlDataAdapter sda = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            ViewBag.task = dt;
            return View();
        }
        public ActionResult Assignment()
        {
            return View();
        }
        public ActionResult Videolecture(int? catid)
        {
            if(catid.HasValue)
            {
                int batchid = Convert.ToInt32(Session["batch"]);
                string query = $"select * from tbl_video where batch={batchid} and category={catid} order by sr";
                SqlDataAdapter sda = new SqlDataAdapter(query,con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                ViewBag.video = dt;
                return View();
            }
            else
            {
                return Content("<script>alert('Please select video Category');location.href='/student/courses'</script>");
            }
            return View();
        }
        public ActionResult notes()
        {
            return View();
        }
        public ActionResult logout()
        {
            Session.RemoveAll();
            return Content("<script>location.href='home/index'</script>");
        }
        [HttpPost]
        public ActionResult submittask(int? taskno, string email,HttpPostedFileBase taskfile)
        {
            string query = $"insert into tbl_submittedtask values({taskno},'{email}','{taskfile.FileName}','{DateTime.Now.ToString("yyyy-MM-dd")}',0,'')";
            SqlCommand cmd= new SqlCommand(query,con);
            con.Open();
            int result=cmd.ExecuteNonQuery();
            con.Close();
            if (result > 0)
            {

                taskfile.SaveAs(Server.MapPath("/Content/answerfile/" + taskfile.FileName));
                return Content("<script>alert('you have answer successfully. You will recieve confirmation from admin very soon via email');location.href='/student/task'</script>");
            }
            else
            {
                return Content("<script>alert('not submitted');location.href='/student/task'</script>");
            }


            //return Content(taskno+email+taskfile);
        }
        [HttpPost]
        public ActionResult changepassword(string opasswd, string npasswd, string cpasswd)
        {
            if (npasswd.Equals(cpasswd))
            {
                string email = Session["email"].ToString();
                string query = $"update tbl_student set password='{npasswd}' where emailid='{email}' and password='{opasswd}'";
                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                int result = cmd.ExecuteNonQuery();
                con.Close();
                if (result > 0)
                {
                    Session.RemoveAll();
                    return Content("<script>alert('Password updated');location.href='/home/index'</script>");
                }
                else
                {
                    return Content("<script>alert('Not change, old password not matched');location.href='/student/changepassword'</script>");
                }

            }
            else
            {
                return Content("<script>alert('New password and confirm password should match');location.href='/student/changepassword'</script>");
            }
        }
    }
}