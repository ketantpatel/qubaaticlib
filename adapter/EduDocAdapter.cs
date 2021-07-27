using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class EduDocAdapter : MasterAdapter
    {
        public EduDocAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }
        public EduDoc SaveDoc(EduDoc edoc)
        {
            if (edoc.doc_id == "-1")
            {
                AddDoc(edoc);
            }
            else
            {
                EditDoc(edoc);
            }
            return edoc;
        }
        public EduDoc AddDoc(EduDoc edoc)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(edoc.stud_id))
                db.AddParameters("stud_id", edoc.stud_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(edoc.course_id))
                db.AddParameters("course_id", edoc.course_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(edoc.grade_marks))
                db.AddParameters("grade_marks", edoc.grade_marks, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(edoc.passing_year))
                db.AddParameters("passing_year", edoc.passing_year, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(edoc.school))
                db.AddParameters("school", edoc.school, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(edoc.create_date))
                db.AddParameters("create_date", edoc.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(edoc.modify_date))
                db.AddParameters("modify_date", edoc.modify_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(edoc.created_by))
                db.AddParameters("created_by", edoc.created_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(edoc.modify_by))
                db.AddParameters("modify_by", edoc.modify_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(edoc.university))
                db.AddParameters("university", edoc.university, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(edoc.file_name))
                db.AddParameters("file_name", edoc.file_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(edoc.frn_id))
                db.AddParameters("frn_id", edoc.frn_id, MyDBTypes.Int);
            edoc.is_opr_success = false;
            if (db.Insert("documents_edu"))
            {
                edoc.doc_id = db.GetLastAutoID().ToString();
                edoc.is_opr_success = true;
            }
            db.Dispose();
            return edoc;
        }
        public EduDoc EditDoc(EduDoc edoc)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(edoc.stud_id))
                db.AddParameters("stud_id", edoc.stud_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(edoc.course_id))
                db.AddParameters("course_id", edoc.course_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(edoc.grade_marks))
                db.AddParameters("grade_marks", edoc.grade_marks, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(edoc.passing_year))
                db.AddParameters("passing_year", edoc.passing_year, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(edoc.school))
                db.AddParameters("school", edoc.school, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(edoc.create_date))
                db.AddParameters("create_date", edoc.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(edoc.modify_date))
                db.AddParameters("modify_date", edoc.modify_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(edoc.created_by))
                db.AddParameters("created_by", edoc.created_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(edoc.modify_by))
                db.AddParameters("modify_by", edoc.modify_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(edoc.university))
                db.AddParameters("university", edoc.university, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(edoc.file_name))
                db.AddParameters("file_name", edoc.file_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(edoc.frn_id))
                db.AddParameters("frn_id", edoc.frn_id, MyDBTypes.Int);
            edoc.is_opr_success = false;

            if (db.Update("documents_edu", "doc_id=" + edoc.doc_id))
            {
               // edoc.doc_id = db.GetLastAutoID().ToString();
                edoc.is_opr_success = true;
            }
            db.Dispose();
            return edoc;
        }
        public bool ChangeDocumentStatus(string stud_id,string course_id,string status)
        {
            DLS db = new DLS(this.AppUserBean);
            bool flag=false;
            db.AddParameters("status", status, MyDBTypes.Varchar);
            if (db.Update("documents_edu", "stud_id="+stud_id+" and course_id="+course_id+""))
            {
                flag = true;
            }
            db.Dispose();
            return flag;
        }

        public bool ChangeDocumentStatus(string form_id,int status,string date,string user_id)
        {
            DLS db = new DLS(this.AppUserBean);
            bool flag = false;
            db.AddParameters("is_verify", status, MyDBTypes.Int);
            db.AddParameters("verify_date", date, MyDBTypes.DateTime);
            db.AddParameters("verify_by", user_id, MyDBTypes.Int);
            if (db.Update("admission_forms", "form_id=" + form_id + ""))
            {
                flag = true;
            }
            db.Dispose();
            return flag;
        }

        public List<EduDoc> ListDocuments(string stud_id)
        {
            List<EduDoc> list = new List<EduDoc>();
            string query = "select d.*,a.edu_title from documents_edu d left join ask_educations a on a.edu_id=d.course_id where d.stud_id="+stud_id;
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EduDoc ed = new EduDoc();
                        ed.doc_id = dr["doc_id"].ToString();
                        ed.stud_id = dr["stud_id"].ToString();
                        ed.course_id = dr["course_id"].ToString();
                        ed.grade_marks = dr["grade_marks"].ToString();
                        ed.passing_year = dr["passing_year"].ToString();
                        ed.school = dr["school"].ToString();
                        ed.create_date = dr["create_date"].ToString();
                        ed.modify_date = dr["modify_date"].ToString();
                        ed.created_by = dr["created_by"].ToString();
                        ed.modify_by = dr["modify_by"].ToString();
                        ed.university = dr["university"].ToString();
                        ed.course_name = dr["edu_title"].ToString();
                        list.Add(ed);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public EduDoc GetDocumentInfo(string docId)
        {
            List<EduDoc> list = new List<EduDoc>();
            string query = "select * from documents_edu where doc_id=" + docId;
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EduDoc ed = new EduDoc();
                        ed.doc_id = dr["doc_id"].ToString();
                        ed.stud_id = dr["stud_id"].ToString();
                        ed.course_id = dr["course_id"].ToString();
                        ed.grade_marks = dr["grade_marks"].ToString();
                        ed.passing_year = dr["passing_year"].ToString();
                        ed.school = dr["school"].ToString();
                        ed.create_date = dr["create_date"].ToString();
                        ed.modify_date = dr["modify_date"].ToString();
                        ed.created_by = dr["created_by"].ToString();
                        ed.modify_by = dr["modify_by"].ToString();
                        ed.university = dr["university"].ToString();
                        list.Add(ed);
                    }
                }
            }
            db.Dispose();
            if (list.Count == 0)
                return new EduDoc();
            return list[0];
        }


        public EduDoc GetDoucument(string stud_id, string course_id)                   
        {
            List<EduDoc> list = new List<EduDoc>();
            string query = "select * from documents_edu where stud_id=" + stud_id+" and course_id="+course_id;
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        EduDoc ed = new EduDoc();
                        ed.doc_id = dr["doc_id"].ToString();
                        ed.stud_id = dr["stud_id"].ToString();
                        ed.course_id = dr["course_id"].ToString();
                        ed.grade_marks = dr["grade_marks"].ToString();
                        ed.passing_year = dr["passing_year"].ToString();
                        ed.school = dr["school"].ToString();
                        ed.create_date = dr["create_date"].ToString();
                        ed.modify_date = dr["modify_date"].ToString();
                        ed.created_by = dr["created_by"].ToString();
                        ed.modify_by = dr["modify_by"].ToString();
                        ed.university = dr["university"].ToString();
                        ed.file_name = dr["file_name"].ToString();
                        ed.status = dr["status"].ToString();
                        list.Add(ed);
                    }
                }
            }
            db.Dispose();
            if (list.Count == 0)
                return new EduDoc();
            return list[0];        
        }
    }
}
