using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class CourseAdapter : MasterAdapter
    {
        public CourseAdapter(UserBean ubean)
        {
            this.AppUserBean = ubean;
        }
        public CourseBean SaveCourse(CourseBean fbean)
        {
            if (fbean.course_id == "-1")
            {
                fbean.is_active = "1";
                fbean = AddCourse(fbean);
                fbean.parent_course_id = fbean.course_id;

                fbean = AddCoureMap(fbean);
            }
            else
            {
                fbean = EditCourse(fbean);
            }

            return fbean;
        }
        public string GetFees(string course_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string fees = db.GetSingleValue("select fees from courses where course_id=" + course_id);
            db.Dispose();
            return fees;
        }
        public bool DeleteCourse(string course_id)
        {
            DLS db = new DLS(this.AppUserBean);
            bool isFlag=db.Delete("courses", "course_id=" + course_id);
            db.Dispose();
            return isFlag;
        }
        public CourseBean AddCourse(CourseBean fbean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(fbean.course_name))
                db.AddParameters("course_name", fbean.course_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.duration_months))
                db.AddParameters("duration_months", fbean.duration_months, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.is_active))
                db.AddParameters("is_active", fbean.is_active, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.description))
                db.AddParameters("description", fbean.description, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.course_type))
                db.AddParameters("course_type", fbean.course_type, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(fbean.fees))
                db.AddParameters("fees", fbean.fees, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.royalty))
                db.AddParameters("royalty", fbean.royalty, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.theory_hrs))
                db.AddParameters("theory_hrs", fbean.theory_hrs, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.practical_hrs))
                db.AddParameters("practical_hrs", fbean.practical_hrs, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.code))
                db.AddParameters("code", fbean.code, MyDBTypes.Varchar);


            fbean.is_opr_success = false;
            if (db.Insert("courses"))
            {
                fbean.is_opr_success = true;
                fbean.course_id = db.GetLastAutoID().ToString();

            }
            db.Dispose();
            return fbean;
        }
        public CourseBean DeleteCoourseMap(CourseBean c)
        {
            DLS db = new DLS(this.AppUserBean);
            string existCourse = db.GetSingleValue("select count(*) from course_map where course_id=" + c.course_id + " and parent_course_id=" + c.parent_course_id + "");
            int count = 0;
            int.TryParse(existCourse, out count);
            c.is_opr_success = false;
            if (count > 0)
            {
                if (db.Delete("course_map", "course_id=" + c.course_id + " and parent_course_id=" + c.parent_course_id + ""))
                {
                   
                    c.is_opr_success = true;
                }
            }

            db.Dispose();
            return c;
        }
        public CourseBean AddCoureMap(CourseBean c)
        {
            DLS db = new DLS(this.AppUserBean);
            string existCourse = db.GetSingleValue("select count(*) from course_map where course_id=" + c.course_id + " and parent_course_id=" + c.parent_course_id + "");
            int count = 0;
            int.TryParse(existCourse, out count);
            
            db.AddParameters("course_id", c.course_id, MyDBTypes.Int);
            db.AddParameters("parent_course_id", c.parent_course_id, MyDBTypes.Int);
            db.AddParameters("is_active", c.is_active, MyDBTypes.Bit);
            c.is_opr_success = false;
            if (count == 0)
            {
                if (db.Insert("course_map"))
                {
                    c.course_map_id = db.GetLastAutoID().ToString();
                    c.is_opr_success = true;
                }
            }
          
            db.Dispose();
            return c;
        }
        public CourseBean EditCourse(CourseBean fbean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(fbean.course_name))
                db.AddParameters("course_name", fbean.course_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.duration_months))
                db.AddParameters("duration_months", fbean.duration_months, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.is_active))
                db.AddParameters("is_active", fbean.is_active, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.description))
                db.AddParameters("description", fbean.description, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.course_type))
                db.AddParameters("course_type", fbean.course_type, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(fbean.fees))
                db.AddParameters("fees", fbean.fees, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.royalty))
                db.AddParameters("royalty", fbean.royalty, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.theory_hrs))
                db.AddParameters("theory_hrs", fbean.theory_hrs, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.practical_hrs))
                db.AddParameters("practical_hrs", fbean.practical_hrs, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.code))
                db.AddParameters("code", fbean.code, MyDBTypes.Varchar);

            fbean.is_opr_success = false;
            if (db.Update("courses", "course_id=" + fbean.course_id))
            {
                fbean.is_opr_success = true;
            }
            db.Dispose();
            return fbean;
        }

        public List<CourseBean> ListCourses()
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from courses where is_active=1";
            List<CourseBean> list = new List<CourseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CourseBean fb = new CourseBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        fb.duration_months = dr["duration_months"].ToString();
                        fb.description = dr["description"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }

        public List<CourseBean> ListCertificateCourses(string course_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from courses where is_active=1 and course_id in (select course_id from course_map where parent_course_id="+course_id+")";
            List<CourseBean> list = new List<CourseBean>();
            DataTable dt = db.GetDataTable(query);

            CourseBean fb = new CourseBean();
            fb.course_id = "";
            fb.course_name = "--Select--";
            list.Add(fb);

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fb = new CourseBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        fb.duration_months = dr["duration_months"].ToString();
                        fb.description = dr["description"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }

        public List<CourseBean> ListMapCourses(string course_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select c.course_id,c.code,c.course_name,(select count(*) from course_map m where m.course_id=c.course_id and m.parent_course_id="+course_id+") found from courses c where is_active=1";
            List<CourseBean> list = new List<CourseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CourseBean fb = new CourseBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.course_map_id = dr["found"].ToString();
                        fb.code = dr["code"].ToString();
                        fb.parent_course_id = course_id;
                        fb.course_name = dr["course_name"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<CourseBean> ListCoursesDeactivated()
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from courses where is_active=0";
            List<CourseBean> list = new List<CourseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CourseBean fb = new CourseBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.code = dr["code"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        fb.duration_months = dr["duration_months"].ToString();
                        fb.description = dr["description"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }

        public List<CourseBean> ListFranchiseCourses(string month, string year, string frn_id, string status)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from courses where course_id in (select distinct course_id from admission_forms where frn_id=" + frn_id + " and month(form_date)=" + month + " and year(form_date)=" + year + " and is_verify=" + status + " )";
            List<CourseBean> list = new List<CourseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CourseBean fb = new CourseBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        fb.duration_months = dr["duration_months"].ToString();
                        fb.description = dr["description"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<CourseBean> ListFranchiseCourses(string month, string year, string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from courses where course_id in (select distinct course_id from admission_forms where frn_id=" + frn_id + " and month(form_date)=" + month + " and year(form_date)=" + year + ")";
            List<CourseBean> list = new List<CourseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CourseBean fb = new CourseBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        fb.duration_months = dr["duration_months"].ToString();
                        fb.description = dr["description"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }

        public List<CourseBean> FillCourse(bool isWithSelect)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from courses";
            List<CourseBean> list = new List<CourseBean>();
            CourseBean fb = new CourseBean();
            fb.course_id = "";
            fb.course_name = "--Select--";
            list.Add(fb);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fb = new CourseBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        fb.duration_months = dr["duration_months"].ToString();
                        fb.description = dr["description"].ToString();
                        fb.fees = dr["fees"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<CourseBean> FillMonthYearCourse(bool isWithSelect,string month,string year,string frnd_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from courses where course_id in (select course_id from admission_forms where frn_id=" + frnd_id + " and month(form_date)=" + month + " and year(form_date)=" + year + ")";
            List<CourseBean> list = new List<CourseBean>();
            CourseBean fb = new CourseBean();
            fb.course_id = "";
            fb.course_name = "--Select--";
            list.Add(fb);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fb = new CourseBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        fb.duration_months = dr["duration_months"].ToString();
                        fb.description = dr["description"].ToString();
                        fb.fees = dr["fees"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<CourseBean> FillCourse(bool isWithSelect,string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from courses where course_id in (select distinct course_id from admission_forms where frn_id="+frn_id+")";
            List<CourseBean> list = new List<CourseBean>();
            CourseBean fb = new CourseBean();
            fb.course_id = "";
            fb.course_name = "--Select--";
            list.Add(fb);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fb = new CourseBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        fb.duration_months = dr["duration_months"].ToString();
                        fb.description = dr["description"].ToString();
                        fb.fees = dr["fees"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public CourseBean GetCourseDetail(string fid)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from courses where course_id=" + fid;
            List<CourseBean> list = new List<CourseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CourseBean fb = new CourseBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        fb.duration_months = dr["duration_months"].ToString();
                        fb.description = dr["description"].ToString();

                        fb.fees = dr["fees"].ToString();
                        fb.royalty = dr["royalty"].ToString();
                        fb.theory_hrs = dr["theory_hrs"].ToString();
                        fb.practical_hrs = dr["practical_hrs"].ToString();
                        fb.code = dr["code"].ToString();

                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            if (list.Count > 0)
                return list[0];
            return new CourseBean();
        }
    }
}
