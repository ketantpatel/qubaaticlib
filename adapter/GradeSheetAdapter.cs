using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class GradeSheetAdapter : MasterAdapter
    {
        public GradeSheetAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }
        public string TotalMarks(string course_id)
        {
            string query = "select sum(practical_marks)+sum(theory_marks) total_marks  from subjects where course_id=" + course_id + "";
            DLS db = new DLS(AppUserBean);
            string marks = db.GetSingleValue(query);
            db.Dispose();
            return marks;
        }
        public List<GradeSheetBean> GetStudentGradSheet(string month, string year, string course_id, string frn_id,string parent_course_id)
        {
            if (month == "All")
                return null;
            List<GradeSheetBean> list = new List<GradeSheetBean>();
            DLS db = new DLS(this.AppUserBean);
          //  string query = "SELECT a.form_date,a.form_id,first_name,last_name,enroll_no,g.grade,g.marks_total,g.marks_obtain FROM admission_forms a left join gradesheet g on a.course_id=g.course_id and a.form_id=g.form_id where a.frn_id=" + frn_id + " and year(a.form_date)="+year+" and month(a.form_date)="+month+" and a.course_id=" + parent_course_id;
            string query = "select second1.form_date,second1.form_id,second1.first_name,second1.last_name,second1.enroll_no,first1.grade,first1.marks_total,first1.marks_obtain from (select * from gradesheet g where g.parent_course_id=" + parent_course_id + " " +
" and g.form_id in(select a.form_id from admission_forms a where  a.frn_id=" + frn_id + " and year(a.form_date)=" + year + " and month(a.form_date)=" + month + ") ) as first1 " +
" left join " +
" (select a.*  from admission_forms a where a.frn_id="+frn_id+" and year(a.form_date)="+year+" and month(a.form_date)="+month+") as second1 " +
" on first1.form_id=second1.form_id where first1.course_id="+course_id+"";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        GradeSheetBean fb = new GradeSheetBean();
                        fb.name = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                        fb.grade = dr["grade"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.marks_obtain = dr["marks_obtain"].ToString();
                        fb.marks_total = dr["marks_total"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.form_date = dr["form_date"].ToString();
                        list.Add(fb);
                    }
                }
            }
            return list;
        }

        public GradeSheetBean SaveGrade(GradeSheetBean g)
        {
            DLS db = new DLS(this.AppUserBean);
            try
            {
                string count = db.GetSingleValue("select count(*) from gradesheet where course_id=" + g.course_id + " and form_id=" + g.form_id + "");

                if (!string.IsNullOrEmpty(g.grade))
                    db.AddParameters("grade", g.grade, MyDBTypes.Varchar);
                if (!string.IsNullOrEmpty(g.marks_obtain))
                    db.AddParameters("marks_obtain", g.marks_obtain, MyDBTypes.Int);
                if (!string.IsNullOrEmpty(g.marks_total))
                    db.AddParameters("marks_total", g.marks_total, MyDBTypes.Int);
                if (!string.IsNullOrEmpty(g.course_id))
                    db.AddParameters("course_id", g.course_id, MyDBTypes.Varchar);
                if (!string.IsNullOrEmpty(g.form_id))
                    db.AddParameters("form_id", g.form_id, MyDBTypes.Int);
                if (!string.IsNullOrEmpty(g.percentage))
                    db.AddParameters("percentage", g.percentage, MyDBTypes.Int);
                if (!string.IsNullOrEmpty(g.parent_course_id))
                    db.AddParameters("parent_course_id", g.parent_course_id, MyDBTypes.Int);
               
                g.is_opr_success = false;
                if (int.Parse(count) == 0)
                {
                    if (!string.IsNullOrEmpty(g.create_date))
                        db.AddParameters("create_date", g.create_date, MyDBTypes.Varchar);
                    if (db.Insert("gradesheet"))
                    {
                        g.is_opr_success = true;
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(g.modify_date))
                        db.AddParameters("modify_date", g.modify_date, MyDBTypes.Varchar);
                    if (db.Update("gradesheet","course_id=" + g.course_id + " and form_id=" + g.form_id + ""))
                    {
                        g.is_opr_success = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                db.Dispose();
            }
            return g;
        }
    }
}
