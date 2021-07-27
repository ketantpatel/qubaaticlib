using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
   public class HelperAdapter : MasterAdapter
    {
       public HelperAdapter(UserBean user)
       {
           this.AppUserBean = user;
       }

        public List<TeacherBean> FillFranchiseInstructors(bool isSelect,string frn_id)
        {
            string query = @"SELECT t.teach_id,t.first_name,t.last_name FROM teacher_franchise_allocation a inner join teachers t on t.teach_id=a.teach_id where a.frn_id="+frn_id+"";
            DLS db = new DLS(this.AppUserBean);
            List<TeacherBean> list = new List<TeacherBean>();
            if (isSelect)
            {
                TeacherBean d = new TeacherBean();
                d.teach_id = "";
                d.full_name = "--Select--";
                list.Add(d);
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TeacherBean d = new TeacherBean();
                    d.teach_id = dr["teach_id"].ToString();
                    d.full_name = dr["first_name"].ToString()+ " "+ dr["last_name"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
        public List<RoleBean> FillRoles(bool isSelect)
        {
            DLS db = new DLS(this.AppUserBean);
            List<RoleBean> list = new List<RoleBean>();
            if (isSelect)
            {
                RoleBean d = new RoleBean();
                d.role_id = "";
                d.name = "--Select--";
                list.Add(d);
            }
            string query = "select role_id,name from roles where status=1";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RoleBean d = new RoleBean();
                    d.role_id = dr["role_id"].ToString();
                    d.name = dr["name"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
        public List<RoleBean> FillCountryRole(bool isSelect,string role_id)
        {
            DLS db = new DLS(this.AppUserBean);
            List<RoleBean> list = new List<RoleBean>();
            if (isSelect)
            {
                RoleBean d = new RoleBean();
                d.role_id = "";
                d.name = "--Select--";
                list.Add(d);
            }
            string query = "select role_id,name from roles where status=1 and role_id in ("+role_id+")";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RoleBean d = new RoleBean();
                    d.role_id = dr["role_id"].ToString();
                    d.name = dr["name"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
        public List<UserTypeBean> FillUserTypes(bool isSelect,string user_type_id)
        {
            DLS db = new DLS(this.AppUserBean);
            List<UserTypeBean> list = new List<UserTypeBean>();
            if (isSelect)
            {
                UserTypeBean d = new UserTypeBean();
                d.user_type_id = "";
                d.user_type = "--Select--";
                list.Add(d);
            }
            string query = "select * from user_types where is_active=1 and ut_id != 1  and ut_id in (select user_type_create_id from users_create_permission where user_type_id=" + user_type_id + ")";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    UserTypeBean d = new UserTypeBean();
                    d.user_type_id = dr["ut_id"].ToString();
                    d.user_type = dr["user_type"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
        public List<RoleBean> FillStateRole(bool isSelect, string role_id)
        {
            DLS db = new DLS(this.AppUserBean);
            List<RoleBean> list = new List<RoleBean>();
            if (isSelect)
            {
                RoleBean d = new RoleBean();
                d.role_id = "";
                d.name = "--Select--";
                list.Add(d);
            }
            string query = "select role_id,name from roles where status=1 and role_id in (" + role_id + ")";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RoleBean d = new RoleBean();
                    d.role_id = dr["role_id"].ToString();
                    d.name = dr["name"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
        public List<RoleBean> FillContryStateRoles(bool isSelect)
        {
            DLS db = new DLS(this.AppUserBean);
            List<RoleBean> list = new List<RoleBean>();
            if (isSelect)
            {
                RoleBean d = new RoleBean();
                d.role_id = "";
                d.name = "--Select--";
                list.Add(d);
            }
            string query = "select role_id,name from roles where status=1 and role_id in (5)";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RoleBean d = new RoleBean();
                    d.role_id = dr["role_id"].ToString();
                    d.name = dr["name"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
        public List<CountryBean> FillCountry(bool isSelect,bool isSingleCountry,string country_id)
        {
            DLS db = new DLS(this.AppUserBean);
            List<CountryBean> list = new List<CountryBean>();
            if (isSelect)
            {
                CountryBean d = new CountryBean();
                d.country_id = "";
                d.country_name = "--Select--";
                list.Add(d);
            }
            string query = "select country_id,country_name from countries";
            if(isSingleCountry)
            {
                query = "select country_id,country_name from countries where country_id="+country_id;
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    CountryBean d = new CountryBean();
                    d.country_id = dr["country_id"].ToString();
                    d.country_name = dr["country_name"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
        public List<StateBean> FillStates(bool isSelect)
       {
           DLS db = new DLS(this.AppUserBean);
           List<StateBean> list = new List<StateBean>();
           if (isSelect)
           {
               StateBean d = new StateBean();
               d.state_id = "";
               d.state_name = "--Select--";
               list.Add(d);
           }
           string query = "select state_id,state_name from states";
           DataTable dt = db.GetDataTable(query);
           if (dt != null)
           {
               foreach (DataRow dr in dt.Rows)
               {
                   StateBean d = new StateBean();
                   d.state_id = dr["state_id"].ToString();
                   d.state_name = dr["state_name"].ToString();
                   list.Add(d);
               }
           }
           db.Dispose();
           return list;
       }

        public List<StateBean> FillStates(bool isSelect,string country_id)
        {
            DLS db = new DLS(this.AppUserBean);
            List<StateBean> list = new List<StateBean>();
            if (isSelect)
            {
                StateBean d = new StateBean();
                d.state_id = "";
                d.state_name = "--Select--";
                list.Add(d);
            }
            string query = "select state_id,state_name from states where country_id="+country_id+"";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    StateBean d = new StateBean();
                    d.state_id = dr["state_id"].ToString();
                    d.state_name = dr["state_name"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
        public List<StateBean> FillStateOnly(bool isSelect,string state_id)
        {
            DLS db = new DLS(this.AppUserBean);
            List<StateBean> list = new List<StateBean>();
            if (isSelect)
            {
                StateBean d = new StateBean();
                d.state_id = "";
                d.state_name = "--Select--";
                list.Add(d);
            }
            string query = "select state_id,state_name from states where state_id=" + state_id + "";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    StateBean d = new StateBean();
                    d.state_id = dr["state_id"].ToString();
                    d.state_name = dr["state_name"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
        public List<StateBean> FillSingleStates(bool isSelect, string state_id)
        {
            DLS db = new DLS(this.AppUserBean);
            List<StateBean> list = new List<StateBean>();
            if (isSelect)
            {
                StateBean d = new StateBean();
                d.state_id = "";
                d.state_name = "--Select--";
                list.Add(d);
            }
            string query = "select state_id,state_name from states where state_id=" + state_id + "";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    StateBean d = new StateBean();
                    d.state_id = dr["state_id"].ToString();
                    d.state_name = dr["state_name"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
        public List<DistrictBean> FillDistricts(string state_id, bool isSelect)
       {
           DLS db = new DLS(this.AppUserBean);
           List<DistrictBean> list = new List<DistrictBean>();
           if (isSelect)
           {
               DistrictBean d = new DistrictBean();
               d.dist_id = "-1";
               d.dist_name = "--Select--";
               list.Add(d);
           }
           string query = "select dist_id,dist_name from districts where state_id=" + state_id + "";
           DataTable dt = db.GetDataTable(query);
           if (dt != null)
           {
               foreach (DataRow dr in dt.Rows)
               {
                   DistrictBean d = new DistrictBean();
                   d.dist_id = dr["dist_id"].ToString();
                   d.dist_name = dr["dist_name"].ToString();
                   list.Add(d);
               }
           }
           db.Dispose();
           return list;
       }
       public bool ActivateRecord(string table,string pkName,string pkValue,string statusFieldName, string statusActiveOrNot)
       {
           DLS db = new DLS(this.AppUserBean);
           bool flag = false;
           db.AddParameters(statusFieldName, statusActiveOrNot, MyDBTypes.Int);
           string whereClaus = ""+pkName+"=" + pkValue;
           flag = db.Update(table, whereClaus);
           db.Dispose();
           return flag;
       }
       public bool DeleteRecord(string table, string pkName, string pkValue, string statusFieldName, string statusActiveOrNot)
       {
           DLS db = new DLS(this.AppUserBean);
           bool flag = false;
           db.AddParameters(statusFieldName, statusActiveOrNot, MyDBTypes.Int);
           string whereClaus = "" + pkName + "=" + pkValue;
           flag = db.Delete(table, whereClaus);
           db.Dispose();
           return flag;
       }

       //public static UserBean User { get { return this.AppUserBean; } }
       public   List<AskEducation> FillAskEducations()
       {
           DLS db = new DLS(this.AppUserBean);
           List<AskEducation> list = new List<AskEducation>();
           list.Clear();
           list.Add(new AskEducation("<--Select-->", "-1"));
           list.AddRange(
               from DataRow row in db.GetDataTable("ask_educations", "edu_title,edu_id", "is_marksheet=1", "", "edu_title").Rows
               select new AskEducation(row["edu_title"].ToString(), row["edu_id"].ToString()));

           db.Dispose();
           return list;
       }
       public List<SourceBean> FillSources()
       {
           DLS db = new DLS(this.AppUserBean);
           List<SourceBean> list = new List<SourceBean>();
           list.Clear();
           list.Add(new SourceBean("<--Select-->", ""));
           list.AddRange(
               from DataRow row in db.GetDataTable("sources", "title,source_id", "", "", "title").Rows
               select new SourceBean(row["title"].ToString(), row["source_id"].ToString()));

           db.Dispose();
           return list;
       }
       public List<AskEducation> FillAskCertificate()
       {
           DLS db = new DLS(this.AppUserBean);
           List<AskEducation> list = new List<AskEducation>();
           list.Clear();
           list.Add(new AskEducation("<--Select-->", "-1"));
           list.AddRange(
               from DataRow row in db.GetDataTable("ask_educations", "edu_title,edu_id", "is_certy=1", "", "edu_title").Rows
               select new AskEducation(row["edu_title"].ToString(), row["edu_id"].ToString()));

           db.Dispose();
           return list;
       }
       public List<AskEducation> FillSubmitedEducations(string stud_id)
       {
           DLS db = new DLS(this.AppUserBean);
           List<AskEducation> list = new List<AskEducation>();
           list.Clear();
           list.Add(new AskEducation("<--Select-->", "-1"));
           list.AddRange(
               from DataRow row in db.GetDataTable("select edu_title,edu_id from ask_educations a left join documents_edu d on d.course_id=a.edu_id where d.stud_id="+stud_id+" order by edu_id").Rows
               select new AskEducation(row["edu_title"].ToString(), row["edu_id"].ToString()));
           list.AddRange(
              from DataRow row in db.GetDataTable("select edu_title,edu_id from ask_educations a left join documents_certy d on d.certy_id=a.edu_id where d.stud_id=" + stud_id + " order by edu_id").Rows
              select new AskEducation(row["edu_title"].ToString(), row["edu_id"].ToString()));
           db.Dispose();
           return list;
       }
       public List<AskEducation> FillSubmitedDocuments(string form_id)
       {
           DLS db = new DLS(this.AppUserBean);
           List<AskEducation> list = new List<AskEducation>();
           list.Clear();
           list.Add(new AskEducation("<--Select-->", "-1"));
           list.AddRange(
               from DataRow row in db.GetDataTable("SELECT dt.doc_type_id,dt.title FROM document_types dt left join documents d on d.doc_type=dt.doc_type_id and d.form_id=" + form_id + " where d.frn_id is not null").Rows
               select new AskEducation(row["title"].ToString(), row["doc_type_id"].ToString()));
           //list.AddRange(
           //   from DataRow row in db.GetDataTable("select edu_title,edu_id from ask_educations a left join documents_certy d on d.certy_id=a.edu_id where d.stud_id=" + stud_id + " order by edu_id").Rows
           //   select new AskEducation(row["edu_title"].ToString(), row["edu_id"].ToString()));
           db.Dispose();
           return list;
       }
    }
}
