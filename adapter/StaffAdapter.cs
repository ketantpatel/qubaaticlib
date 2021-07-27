using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class StaffAdapter : MasterAdapter
    {
        public StaffAdapter(UserBean ub)
        {
            this.AppUserBean = ub;
        }
        public StaffBean SaveStaff(StaffBean ub)
        {
            if (ub.staff_id == "-1")
                ub = AddStaff(ub);
            else
                ub = EditStaff(ub);
            return ub;
        }
        public bool DeleteStaff(string staff_id)
        {
            DLS db = new DLS(this.AppUserBean);
            bool isFlag = db.Delete("frn_staff", "staff_id=" + staff_id);
           
            db.Dispose();
            return isFlag;
        }
            
        public bool UpdateParentId(string user_id, string parent_id)
        {
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("parent_id", parent_id, MyDBTypes.Int);
            bool flag = db.Update("users", "user_id=" + user_id);
            db.Dispose();
            return flag;
        }
        public bool UpdatePassowrd(string user_id, string password)
        {
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("password", password, MyDBTypes.Varchar);
            bool flag = db.Update("users", "user_id=" + user_id);
            db.Dispose();
            return flag;
        }
        public StaffBean AddStaff(StaffBean ub)
        {
            DLS db = new DLS(this.AppUserBean);
                   if (!string.IsNullOrEmpty(ub.first_name))
                       db.AddParameters("first_name", ub.first_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.last_name))
                db.AddParameters("last_name", ub.last_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.mobile))
                db.AddParameters("mobile", ub.mobile, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.qualification))
                db.AddParameters("qualification", ub.qualification, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.address))
                db.AddParameters("address", ub.address, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.city))
                db.AddParameters("city", ub.city, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.pincode))
                db.AddParameters("pincode", ub.pincode, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.experience))
                db.AddParameters("experience", ub.experience, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(ub.join_date))
                db.AddParameters("join_date", ub.join_date, MyDBTypes.DateTime);

            if (!string.IsNullOrEmpty(ub.job_profile))
                db.AddParameters("job_profile", ub.job_profile, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(ub.create_date))
                db.AddParameters("create_date", ub.create_date, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(ub.create_by))
                db.AddParameters("create_by", ub.create_by, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(ub.staff_type))
                db.AddParameters("staff_type", ub.staff_type, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(ub.user_id))
                db.AddParameters("user_id", ub.user_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(ub.frn_id))
                db.AddParameters("frn_id", ub.frn_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(ub.is_counsoler))
                db.AddParameters("is_counsoler", ub.is_counsoler, MyDBTypes.Bit);

            ub.is_opr_success = false;
            if (db.Insert("frn_staff"))
            {
                ub.staff_id = db.GetLastAutoID().ToString();
                ub.is_opr_success = true;
            }
            db.Dispose();
            return ub;
        }

        public StaffBean EditStaff(StaffBean ub)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(ub.first_name))
                db.AddParameters("first_name", ub.first_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.last_name))
                db.AddParameters("last_name", ub.last_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.mobile))
                db.AddParameters("mobile", ub.mobile, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.qualification))
                db.AddParameters("qualification", ub.qualification, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.address))
                db.AddParameters("address", ub.address, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.city))
                db.AddParameters("city", ub.city, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.pincode))
                db.AddParameters("pincode", ub.pincode, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.experience))
                db.AddParameters("experience", ub.experience, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(ub.join_date))
                db.AddParameters("join_date", ub.join_date, MyDBTypes.DateTime);

            if (!string.IsNullOrEmpty(ub.job_profile))
                db.AddParameters("job_profile", ub.job_profile, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(ub.create_date))
                db.AddParameters("create_date", ub.create_date, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(ub.create_by))
                db.AddParameters("create_by", ub.create_by, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(ub.staff_type))
                db.AddParameters("staff_type", ub.staff_type, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(ub.user_id))
                db.AddParameters("user_id", ub.user_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(ub.is_counsoler))
                db.AddParameters("is_counsoler", ub.is_counsoler, MyDBTypes.Bit);

            ub.is_opr_success = false;
            if (db.Update("frn_staff","staff_id="+ub.staff_id))
            {
                
                ub.is_opr_success = true;
            }
            db.Dispose();
            return ub;
        }

        public UserBean IsValidUser(UserBean user)
        {
            try
            {
                string query = "select * from users where username='" + user.username + "' and password='" + user.password + "'";
                MDACLib.DLS db = new MDACLib.DLS(user);
                DataTable dt = db.GetDataTable(query);
            //    user.message = "Starts";
                if (dt != null)
                {
                    if (dt.Rows.Count == 1)
                    {
                        user.message = "found";
                        user.role = dt.Rows[0]["role"].ToString();
                        user.user_id = dt.Rows[0]["user_id"].ToString();
                        user.user_type = dt.Rows[0]["user_type"].ToString();
                        user.is_active = dt.Rows[0]["is_active"].ToString();
                        user.first_name = dt.Rows[0]["first_name"].ToString();
                        user.last_name = dt.Rows[0]["last_name"].ToString();
                        user.username = dt.Rows[0]["username"].ToString();
                        user.parent_id = dt.Rows[0]["parent_id"].ToString();
                        user.is_valid_user = true;
                        user.message = "found end";
                    }
                    else
                    {
                        user.message = "No Record found";
                    }
                }
                else
                {
                    //user.message =s "DataTable null";
                }
            }
            catch (Exception ex)
            {
                user.message = ex.Message;
            }
            return user;
        }

        public bool IsUserNameExist(string username)
        {
            string query = "select count(*) from users where username='" + username + "'";
            MDACLib.DLS db = new MDACLib.DLS(this.AppUserBean);
            string value = db.GetSingleValue(query);
            db.Dispose();
            bool isExist = false;
            if (!string.IsNullOrEmpty(value))
            {
                if (int.Parse(value) > 0)
                {
                    isExist = true;
                }
            }
            return isExist;
        }

        public List<StaffBean> FillCounsoler(bool isWithSelect)
        {
            List<StaffBean> list = new List<StaffBean>();
            string query = "select fs.staff_id,fs.first_name,fs.last_name,st.title,DATE_FORMAT(join_date, '%d/%m/%Y') st_join_date  from frn_staff fs left join frn_staff_type st on st.staff_type_id=fs.staff_type where fs.is_counsoler=1";
            DLS db = new DLS(this.AppUserBean);
            StaffBean ub = new StaffBean();
            if (isWithSelect)
            {
                ub.staff_id = "";
                ub.first_name = "--Select--";
                list.Add(ub);
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                         ub = new StaffBean();
                        ub.staff_id = dr["staff_id"].ToString();
                       ub.first_name = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                     
                        list.Add(ub);
                    }
                }
            }
            return list;
        }

        public List<StaffBean> StaffList(string frn_id)
        {
            List<StaffBean> list = new List<StaffBean>();
            string query = "select fs.staff_id,fs.first_name,fs.last_name,st.title,DATE_FORMAT(join_date, '%d/%m/%Y') st_join_date  from frn_staff fs left join frn_staff_type st on st.staff_type_id=fs.staff_type where fs.frn_id="+frn_id+"";
            DLS db=new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StaffBean ub = new StaffBean();
                        ub.staff_id = dr["staff_id"].ToString();
                        ub.staff_type = dr["title"].ToString();
                        
                        ub.first_name = dr["first_name"].ToString();
                        ub.last_name = dr["last_name"].ToString();
                        ub.join_date = dr["st_join_date"].ToString();
                        
                        list.Add(ub);
                    }
                }
            }
            return list;
        }

        public StaffBean GetStaff(string staff_id)
        {
            List<StaffBean> list = new List<StaffBean>();
            string query = "select * from frn_staff where staff_id=" + staff_id;
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StaffBean sb = new StaffBean();
                        sb.first_name = dr["first_name"].ToString();
                        sb.last_name = dr["last_name"].ToString();
                        sb.experience = dr["experience"].ToString();
                        sb.qualification = dr["qualification"].ToString();
                        sb.staff_id = dr["staff_id"].ToString();
                        sb.frn_id = dr["frn_id"].ToString();
                        sb.address = dr["address"].ToString();
                        sb.city = dr["city"].ToString();
                        sb.pincode = dr["pincode"].ToString();
                        sb.mobile = dr["mobile"].ToString();
                        sb.job_profile = dr["job_profile"].ToString();
                        sb.join_date = dr["join_date"].ToString();
                        sb.create_by = dr["create_by"].ToString();
                        sb.create_date = dr["create_date"].ToString();
                        sb.user_id = dr["user_id"].ToString();
                        sb.staff_type = dr["staff_type"].ToString();
                        sb.is_counsoler = dr["is_counsoler"].ToString();
                        list.Add(sb);
                    }
                }
            }
            if (list.Count == 0)
                new UserBean();
            return list[0];
        }
    }
}
