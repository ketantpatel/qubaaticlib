using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class SubjectAdapter : MasterAdapter
    {
        public SubjectAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }
        public SubjectBean AddSubject(SubjectBean s)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(s.course_id))
                db.AddParameters("course_id", s.course_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.name))
                db.AddParameters("name", s.name, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.create_date))
                db.AddParameters("create_date", s.create_date, MyDBTypes.DateTime);
            s.is_opr_success = false;
            if (db.Insert("subjects"))
            {
                s.is_opr_success = true;
                s.subject_id = db.GetLastAutoID().ToString();
            }
            db.Dispose();

            return s;
        }
        public SubjectBean EditSubject(SubjectBean s)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(s.course_id))
                db.AddParameters("course_id", s.course_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.name))
                db.AddParameters("name", s.name, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.create_date))
                db.AddParameters("create_date", s.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(s.practical_marks))
                db.AddParameters("practical_marks", s.practical_marks, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.theory_marks))
                db.AddParameters("theory_marks", s.theory_marks, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.create_date))
                db.AddParameters("code", s.code, MyDBTypes.Varchar);
            s.is_opr_success = false;
            if (db.Update("subjects","subject_id="+s.subject_id+""))
            {
                s.is_opr_success = true;               
            }
            db.Dispose();

            return s;
        }

        public bool DeleteSubject(string subjectId)
        {
            bool flag = false;
            DLS db = new DLS(this.AppUserBean);
            db.Delete("subjects", "subject_id=" + subjectId + "");
            db.Dispose();
            return flag;
        }

        public List<SubjectBean> SubjectList(string course_id,bool isSelectOption)
        {
            string query = "select * from subjects where is_active=1 and course_id="+course_id;
            List<SubjectBean> list = new List<SubjectBean>();
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (isSelectOption)
            {
                SubjectBean s = new SubjectBean();
                s.name = "--Select--";
                s.course_id = "-1";
                list.Add(s);
            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SubjectBean s = new SubjectBean();
                    s.name = dr["name"].ToString();
                    s.course_id = dr["course_id"].ToString();
                    s.subject_id = dr["subject_id"].ToString();

                    s.practical_marks = dr["practical_marks"].ToString();
                    s.theory_marks = dr["theory_marks"].ToString();
                    s.code = dr["code"].ToString();
                    if (dr["code"] != null)
                    {
                        if (string.IsNullOrEmpty(dr["code"].ToString()))
                        {
                            s.code = "";
                        }
                    }
                    
                    
                    list.Add(s);
                }
            }
            db.Dispose();
            return list;
        }

        public SubjectBean GetSubject(string subjectId)
        {
            string query = "select * from subjects where is_active=1 and subject_id="+subjectId;
            List<SubjectBean> list = new List<SubjectBean>();
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SubjectBean s = new SubjectBean();
                    s.name = dr["name"].ToString();
                    s.course_id = dr["course_id"].ToString();
                    s.subject_id = dr["subject_id"].ToString();
                    list.Add(s);
                }
            }
            db.Dispose();
            if (list.Count == 0)
                new SubjectBean();
            return list[0];
        }
    }
}
