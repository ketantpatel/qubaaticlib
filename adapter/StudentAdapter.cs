using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class StudentAdapter : MasterAdapter
    {
        public StudentAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }

       

        public List<SelectBean> getMonths()
        {
            List<SelectBean> list = new List<SelectBean>();
            SelectBean s = new SelectBean();
            s.id = "All";
            s.text = "--All--";
            list.Add(s);
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct monthname(create_date) as month_name,month(create_date) month_no from students order by month(create_date)";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        s = new SelectBean();
                        s.id = dr["month_no"].ToString();
                        s.text = dr["month_name"].ToString();
                        list.Add(s);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<SelectBean> getMonths(string franchise_id)
        {
            List<SelectBean> list = new List<SelectBean>();
            SelectBean s = new SelectBean();
            s.id = "All";
            s.text = "--All--";
            list.Add(s);
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct CONVERT(varchar(3), create_date, 100) as month_name,month(create_date) month_no from students where frn_id=" + franchise_id + " order by month(create_date)";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        s = new SelectBean();
                        s.id = dr["month_no"].ToString();
                        s.text = dr["month_name"].ToString();
                        list.Add(s);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<SelectBean> getYears()
        {
            List<SelectBean> list = new List<SelectBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct year(create_date) year from students order by year(create_date)";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SelectBean s = new SelectBean();
                        s.id = dr["year"].ToString();
                        s.text = dr["year"].ToString();
                        list.Add(s);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<SelectBean> getYears(string franchise_id)
        {
            List<SelectBean> list = new List<SelectBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct year(create_date) year from students where frn_id=" + franchise_id + " order by year(create_date)";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SelectBean s = new SelectBean();
                        s.id = dr["year"].ToString();
                        s.text = dr["year"].ToString();
                        list.Add(s);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public StudentBean SaveStudent(StudentBean sbean)
        {
            if (sbean.stud_id == "-1")
            {
                AddStudent(sbean);
            }
            else
            {
                EditStudent(sbean);
            }
            return sbean;
        }
        public StudentBean AddStudent(StudentBean sbean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(sbean.first_name))
                db.AddParameters("first_name", sbean.first_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.middle_name))
                db.AddParameters("middle_name", sbean.middle_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.last_name))
                db.AddParameters("last_name", sbean.last_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.dob))
                db.AddParameters("dob", sbean.dob, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(sbean.address1))
                db.AddParameters("address1", sbean.address1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.address2))
                db.AddParameters("address2", sbean.address2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.city1))
                db.AddParameters("city1", sbean.city1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.city2))
                db.AddParameters("city2", sbean.city2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.state1))
                db.AddParameters("state1", sbean.state1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.state2))
                db.AddParameters("state2", sbean.state2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.pincode1))
                db.AddParameters("pincode1", sbean.pincode1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.pincode2))
                db.AddParameters("pincode2", sbean.pincode2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.stud_mobile))
                db.AddParameters("stud_mobile", sbean.stud_mobile, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.parent_mobile))
                db.AddParameters("parent_mobile", sbean.parent_mobile, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.emgcy_mobile))
                db.AddParameters("emgcy_mobile", sbean.emgcy_mobile, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.stud_email))
                db.AddParameters("stud_email", sbean.stud_email, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.is_active))
                db.AddParameters("is_active", sbean.is_active, MyDBTypes.Bit);
            if (!string.IsNullOrEmpty(sbean.create_date))
                db.AddParameters("create_date", sbean.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(sbean.modify_date))
                db.AddParameters("modify_date", sbean.modify_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(sbean.create_by))
                db.AddParameters("create_by", sbean.create_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(sbean.modify_by))
                db.AddParameters("modify_by", sbean.modify_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(sbean.is_widthraw))
                db.AddParameters("is_widthraw", sbean.is_widthraw, MyDBTypes.Bit);
            if (!string.IsNullOrEmpty(sbean.is_passout))
                db.AddParameters("is_passout", sbean.is_passout, MyDBTypes.Bit);
            if (!string.IsNullOrEmpty(sbean.batch_id))
                db.AddParameters("batch_id", sbean.batch_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(sbean.course_id))
                db.AddParameters("course_id", sbean.course_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(sbean.frn_id))
                db.AddParameters("frn_id", sbean.frn_id, MyDBTypes.Int);
            sbean.is_opr_success = false;
            if (db.Insert("students"))
            {
                sbean.stud_id = db.GetLastAutoID().ToString();
                sbean.is_opr_success = true;
            }
            db.Dispose();
            return sbean;
        }
        public StudentBean EditStudent(StudentBean sbean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(sbean.first_name))
                db.AddParameters("first_name", sbean.first_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.middle_name))
                db.AddParameters("middle_name", sbean.middle_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.last_name))
                db.AddParameters("last_name", sbean.last_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.dob))
                db.AddParameters("dob", sbean.dob, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(sbean.address1))
                db.AddParameters("address1", sbean.address1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.address2))
                db.AddParameters("address2", sbean.address2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.city1))
                db.AddParameters("city1", sbean.city1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.city2))
                db.AddParameters("city2", sbean.city2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.state1))
                db.AddParameters("state1", sbean.state1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.state2))
                db.AddParameters("state2", sbean.state2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.pincode1))
                db.AddParameters("pincode1", sbean.pincode1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.pincode2))
                db.AddParameters("pincode2", sbean.pincode2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.stud_mobile))
                db.AddParameters("stud_mobile", sbean.stud_mobile, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.parent_mobile))
                db.AddParameters("parent_mobile", sbean.parent_mobile, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.emgcy_mobile))
                db.AddParameters("emgcy_mobile", sbean.emgcy_mobile, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.stud_email))
                db.AddParameters("stud_email", sbean.stud_email, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(sbean.is_active))
                db.AddParameters("is_active", sbean.is_active, MyDBTypes.Bit);
            if (!string.IsNullOrEmpty(sbean.create_date))
                db.AddParameters("create_date", sbean.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(sbean.modify_date))
                db.AddParameters("modify_date", sbean.modify_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(sbean.create_by))
                db.AddParameters("create_by", sbean.create_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(sbean.modify_by))
                db.AddParameters("modify_by", sbean.modify_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(sbean.is_widthraw))
                db.AddParameters("is_widthraw", sbean.is_widthraw, MyDBTypes.Bit);
            if (!string.IsNullOrEmpty(sbean.is_passout))
                db.AddParameters("is_passout", sbean.is_passout, MyDBTypes.Bit);
            if (!string.IsNullOrEmpty(sbean.batch_id))
                db.AddParameters("batch_id", sbean.batch_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(sbean.course_id))
                db.AddParameters("course_id", sbean.course_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(sbean.frn_id))
                db.AddParameters("frn_id", sbean.frn_id, MyDBTypes.Int);
            sbean.is_opr_success = false;
            if (db.Update("students","stud_id="+sbean.stud_id))
            {
                sbean.is_opr_success = true;
            }
            db.Dispose();
            return sbean;
        }
        public List<StudentBean> ListStudents()
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id ";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.franchise_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<StudentBean> ListFranchiseStudents(string month,string year,string franchise_id)
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);

            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id where f.frn_id=" + franchise_id + " and month(s.form_date)=" + month + " and year(s.form_date)=" + year + "";
            if (month == "All")
            {
                query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id where f.frn_id=" + franchise_id + " and year(s.form_date)=" + year + "";
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.franchise_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<StudentBean> FillBatchStudents(string frn_id,string batch_id)
        {
            List<StudentBean> list = new List<StudentBean>();
            StudentBean sb = new StudentBean();
            sb.stud_id = "-1";
            sb.full_name = "--Select--";
            list.Add(sb);

            DLS db = new DLS(this.AppUserBean);

            string query = "select * from admission_forms f  where  f.frn_id="+frn_id+" and f.batch_id="+batch_id;
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                         sb = new StudentBean();
                        sb.stud_id = dr["form_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.full_name = sb.first_name + " " + sb.middle_name + " " + sb.last_name;
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<StudentBean> ListFranchiseStudents(string month, string year, string franchise_id,string course_id)
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);

            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id where f.frn_id=" + franchise_id + " and month(s.create_date)=" + month + " and year(s.create_date)=" + year + " and c.course_id="+course_id;
           
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.franchise_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<StudentBean> ListFranchiseStudents(string month, string year)
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);

            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id where  month(s.form_date)=" + month + " and year(s.form_date)=" + year + "";
            if (month == "All")
            {
                query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id where year(s.form_date)=" + year + "";
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.franchise_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        
        public List<StudentBean> ListStudents(string top,string frn_id)
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select top " + top + " s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id  where f.frn_id="+frn_id+" order by s.stud_id desc";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.course_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<StudentBean> ListStudentByStatus(string status,string franchise_id)
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id  where f.frn_id="+franchise_id+" and s.stud_id in(select stud_id from documents_edu d where d.status='" + status + "')";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.franchise_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<StudentBean> ListStudentByStatus(string status)
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id  where s.stud_id in(select stud_id from documents_edu d where d.status='"+status+"')";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.course_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        
        public List<StudentBean> ListStudentVerifed(string status)
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id  where s.stud_id in(select stud_id from documents_edu d where s.stud_id in(select stud_id  from ("+
" select stud_id,first_name,middle_name,last_name,(select count(*) from documents_edu d where d.stud_id=s.stud_id) doc_count,"+
" (select count(*) from documents_edu d where d.stud_id=s.stud_id and status='verified') verified_doc_count"+
 " from students s) as a where doc_count=verified_doc_count and doc_count >0))";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.course_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<StudentBean> ListStudentVerifed(string status,string franchise_id)
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id  where f.frn_id="+franchise_id+" and s.stud_id in(select stud_id from documents_edu d where s.stud_id in(select stud_id  from (" +
" select stud_id,first_name,middle_name,last_name,(select count(*) from documents_edu d where d.stud_id=s.stud_id) doc_count," +
" (select count(*) from documents_edu d where d.stud_id=s.stud_id and status='verified') verified_doc_count" +
 " from students s) as a where doc_count=verified_doc_count and doc_count >0))";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.franchise_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<StudentBean> ListStudentNoDoc()
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id where stud_id not in (select stud_id from documents_edu)";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.franchise_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<StudentBean> ListStudentNoDoc(string franchise_id)
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id where f.frn_id="+franchise_id+" and stud_id not in (select stud_id from documents_edu)";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.franchise_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public StudentBean GetStudentInfo(string student_id)
        {
            List<StudentBean> list = new List<StudentBean>();
            DLS db = new DLS(this.AppUserBean);
            
            string query = "select s.*,f.name franchise_name,c.course_name from students s left join franchises f on f.frn_id=s.frn_id left join courses c on c.course_id=s.course_id where s.stud_id="+student_id;
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StudentBean sb = new StudentBean();
                        sb.stud_id = dr["stud_id"].ToString();
                        sb.first_name = dr["first_name"].ToString();
                        sb.middle_name = dr["middle_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.dob = dr["dob"].ToString();
                        sb.address1 = dr["address1"].ToString();
                        sb.address2 = dr["address2"].ToString();
                        sb.city1 = dr["city1"].ToString();
                        sb.city2 = dr["city2"].ToString();
                        sb.state1 = dr["state1"].ToString();
                        sb.state2 = dr["state2"].ToString();
                        sb.pincode1 = dr["pincode1"].ToString();
                        sb.pincode2 = dr["pincode2"].ToString();
                        sb.stud_mobile = dr["stud_mobile"].ToString();
                        sb.parent_mobile = dr["parent_mobile"].ToString();
                        sb.emgcy_mobile = dr["emgcy_mobile"].ToString();
                        sb.stud_email = dr["stud_email"].ToString();
                        sb.is_active = dr["is_active"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.modify_date = dr["modify_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.modify_by = dr["modify_by"].ToString();
                        sb.is_widthraw = dr["is_widthraw"].ToString();
                        sb.is_passout = dr["is_passout"].ToString();
                        sb.batch_id = dr["batch_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.course_id = dr["course_id"].ToString();
                        sb.course_name = dr["course_name"].ToString();
                        sb.course_name = dr["franchise_name"].ToString();
                        list.Add(sb);
                    }
                }
            }
            db.Dispose();
            if (list.Count == 0)
                return new StudentBean();
            return list[0];
        }
    }
}

