using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class UserAdapter : MasterAdapter
    {
        public UserAdapter(UserBean ub)
        {
            this.AppUserBean = ub;
        }
        public UserBean SaveUser(UserBean ub)
        {
            if (ub.user_id == "-1")
                ub = AddUser(ub);
            else
                ub = EditUser(ub);
            return ub;
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
        public bool isUserNameExist1(string user_name)
        {
            DLS db = new DLS(this.AppUserBean);
            string value = db.GetSingleValue("select count(*) from users where username='"+user_name+"'");
            db.Dispose();
            int found = 0;
            int.TryParse(value, out found);
            if (found > 0)
                return true;
            else
                return false;
        }
        public UserBean AddUser(UserBean ub)
        {
            DLS db = new DLS(this.AppUserBean);
                   if (!string.IsNullOrEmpty(ub.username))
            db.AddParameters("username", ub.username, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.password))
            db.AddParameters("password", ub.password, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.user_type))
            db.AddParameters("user_type", ub.user_type, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(ub.role))
            db.AddParameters("role", ub.role, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(ub.is_active))
            db.AddParameters("is_active", ub.is_active, MyDBTypes.Bit);
            if (!string.IsNullOrEmpty(ub.create_date))
            db.AddParameters("create_date", ub.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(ub.modify_date))
            db.AddParameters("modify_date", ub.modify_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(ub.first_name))
            db.AddParameters("first_name", ub.first_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.last_name))
            db.AddParameters("last_name", ub.last_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.state_id))
             db.AddParameters("state_id", ub.state_id, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.country_id))
                db.AddParameters("country_id", ub.country_id, MyDBTypes.Varchar);
            ub.is_opr_success = false;
            if (db.Insert("users"))
            {
                ub.user_id = db.GetLastAutoID().ToString();
                ub.is_opr_success = true;
            }
            db.Dispose();
            return ub;
        }

        public UserBean EditUser(UserBean ub)
        {
            DLS db = new DLS(this.AppUserBean);
                 if (!string.IsNullOrEmpty(ub.username))
                db.AddParameters("username", ub.username, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.password))
                db.AddParameters("password", ub.password, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.user_type))
                db.AddParameters("user_type", ub.user_type, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(ub.role))
                db.AddParameters("role", ub.role, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(ub.is_active))
                db.AddParameters("is_active", ub.is_active, MyDBTypes.Bit);
            if (!string.IsNullOrEmpty(ub.create_date))
                db.AddParameters("create_date", ub.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(ub.modify_date))
                db.AddParameters("modify_date", ub.modify_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(ub.first_name))
                db.AddParameters("first_name", ub.first_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.last_name))
                db.AddParameters("last_name", ub.last_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.state_id))
                db.AddParameters("state_id", ub.state_id, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(ub.country_id))
                db.AddParameters("country_id", ub.country_id, MyDBTypes.Varchar);
            ub.is_opr_success = false;
            if (db.Update("users","user_id="+ub.user_id))
            {
                ub.user_id = db.GetLastAutoID().ToString();
                ub.is_opr_success = true;
            }
            db.Dispose();
            return ub;
        }

        public UserBean IsValidUser(UserBean user)
        {
            try
            {
                string query = "select u.*,t.user_type user_type_name from users u inner join user_types t on u.user_type=t.ut_id where username='" + user.username + "' and password='" + user.password + "'";
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
                        user.state_id = dt.Rows[0]["state_id"].ToString();
                        user.country_id = dt.Rows[0]["country_id"].ToString();
                        user.user_type_name = dt.Rows[0]["user_type_name"].ToString();
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

        public UserBean IsValidStudentUser(UserBean user)
        {
            try
            {
                string query = "select * from admission_forms where enroll_no='" + user.username + "' and password='" + user.password + "'";
                MDACLib.DLS db = new MDACLib.DLS(user);
                DataTable dt = db.GetDataTable(query);
                //    user.message = "Starts";
                if (dt != null)
                {
                    if (dt.Rows.Count == 1)
                    {
                        user.message = "found";
                        user.role = "5";
                        user.user_id = dt.Rows[0]["form_id"].ToString();
                        user.user_type = "7";
                        user.is_active = dt.Rows[0]["is_active"].ToString();
                        user.first_name = dt.Rows[0]["first_name"].ToString();
                        user.last_name = dt.Rows[0]["last_name"].ToString();
                        user.username = dt.Rows[0]["enroll_no"].ToString();
                        user.parent_id = "-1";
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

        public UserTypeBean UserTypeInfo(string user_type_id)
        {
            UserTypeBean ub = new UserTypeBean();
            string query = "select * from user_types where ut_id="+user_type_id;
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {

                        ub.user_type_id = dr["ut_id"].ToString();
                        ub.user_type = dr["user_type"].ToString();
                    //    ub.USER_T= dr["user_type"].ToString();
                        ub.role_id = dr["role_id"].ToString();
                        ub.code = dr["code"].ToString();
                        ub.code_level = dr["code_level"].ToString();
                    }
                }
            }
            return ub;
        }

        public List<UserBean> UserList()
        {
            List<UserBean> list = new List<UserBean>();
            string query = "select u.*,t.user_type user_type_name from users u left join user_types t on t.ut_id=u.user_type";
            DLS db=new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        UserBean ub = new UserBean();
                        ub.user_id = dr["user_id"].ToString();
                        ub.user_type = dr["user_type"].ToString();
                        ub.is_active = dr["is_active"].ToString();
                        ub.first_name = dr["first_name"].ToString();
                        ub.last_name = dr["last_name"].ToString();
                        ub.username = dr["username"].ToString();
                        ub.user_type_name = dr["user_type_name"].ToString();
                        list.Add(ub);
                    }
                }
            }
            return list;
        }

        public List<UserBean> UserTypeList(string userType,string country_id)
        {
            List<UserBean> list = new List<UserBean>();
            string query = "select u.*,t.user_type user_type_name from users u left join user_types t on t.ut_id=u.user_type where u.user_type = "+ userType + " and country_id ="+ country_id;
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        UserBean ub = new UserBean();
                        ub.user_id = dr["user_id"].ToString();
                        ub.user_type = dr["user_type"].ToString();
                        ub.is_active = dr["is_active"].ToString();
                        ub.first_name = dr["first_name"].ToString();
                        ub.last_name = dr["last_name"].ToString();
                        ub.username = dr["username"].ToString();
                        ub.user_type_name = dr["user_type_name"].ToString();
                        list.Add(ub);
                    }
                }
            }
            return list;
        }

        public List<UserBean> UserList(bool isContryUser,bool isStateUser)
        {
            List<UserBean> list = new List<UserBean>();
            string query = "select u.*,t.user_type user_type_name,r.name role_name from users u left join user_types t on t.ut_id=u.user_type inner join roles r on r.role_id=u.role";
            if(isContryUser)
                query = "select u.*,t.user_type user_type_name,r.name role_name from users u left join user_types t on t.ut_id=u.user_type inner join roles r on r.role_id=u.role where u.user_id not in(" + this.AppUserBean.user_id + ") and  u.country_id=" + this.AppUserBean.country_id;
            if(isStateUser)
                query = "select u.*,t.user_type user_type_name,r.name role_name from users u left join user_types t on t.ut_id=u.user_type inner join roles r on r.role_id=u.role where u.user_id not in("+this.AppUserBean.user_id+") and u.state_id=" + this.AppUserBean.state_id;
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        UserBean ub = new UserBean();
                        ub.user_id = dr["user_id"].ToString();
                        ub.user_type = dr["user_type"].ToString();
                        ub.is_active = dr["is_active"].ToString();
                        ub.first_name = dr["first_name"].ToString();
                        ub.last_name = dr["last_name"].ToString();
                        ub.username = dr["username"].ToString();
                        ub.role_name = dr["role_name"].ToString();
                        ub.user_type_name = dr["user_type_name"].ToString();
                        list.Add(ub);
                    }
                }
            }
            return list;
        }
        public UserBean GetUser(string user_id)
        {
            List<UserBean> list = new List<UserBean>();
            string query = "select u.*,t.user_type user_type_name from users u left join user_types t on t.ut_id=u.user_type where u.user_id="+user_id;
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        UserBean ub = new UserBean();
                        ub.user_type = dr["user_type"].ToString();
                        ub.is_active = dr["is_active"].ToString();
                        ub.first_name = dr["first_name"].ToString();
                        ub.last_name = dr["last_name"].ToString();
                        ub.role = dr["role"].ToString();
                        ub.username = dr["username"].ToString();
                        ub.user_type_name = dr["user_type_name"].ToString();
                        ub.country_id = dr["country_id"].ToString();
                        ub.state_id = dr["state_id"].ToString();
                        list.Add(ub);
                    }
                }
            }
            if (list.Count == 0)
                return new UserBean();

            
            return list[0];
        }
        public UserBean GetContryUserInfo(string country_id)
        {
            List<UserBean> list = new List<UserBean>();
            string query = "select u.*,t.user_type user_type_name from users u left join user_types t on t.ut_id=u.user_type where u.country_id="+country_id+" and u.role=5";
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        UserBean ub = new UserBean();
                        ub.user_id = dr["user_id"].ToString();
                        ub.user_type = dr["user_type"].ToString();
                        ub.is_active = dr["is_active"].ToString();
                        ub.first_name = dr["first_name"].ToString();
                        ub.last_name = dr["last_name"].ToString();
                        ub.role = dr["role"].ToString();
                        ub.username = dr["username"].ToString();
                        ub.user_type_name = dr["user_type_name"].ToString();
                        list.Add(ub);
                    }
                }
            }
            if (list.Count == 0)
                return new UserBean();


            return list[0];
        }
        public UserBean GetStateUserInfo(string state_id)
        {
            List<UserBean> list = new List<UserBean>();
            string query = "select u.*,t.user_type user_type_name from users u left join user_types t on t.ut_id=u.user_type where u.state_id="+state_id+" and u.role=6";
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        UserBean ub = new UserBean();
                        ub.user_id = dr["user_id"].ToString();
                        ub.user_type = dr["user_type"].ToString();
                        ub.is_active = dr["is_active"].ToString();
                        ub.first_name = dr["first_name"].ToString();
                        ub.last_name = dr["last_name"].ToString();
                        ub.role = dr["role"].ToString();
                        ub.username = dr["username"].ToString();
                        ub.user_type_name = dr["user_type_name"].ToString();
                        list.Add(ub);
                    }
                }
            }
            if (list.Count == 0)
                return new UserBean();


            return list[0];
        }
    }
}
