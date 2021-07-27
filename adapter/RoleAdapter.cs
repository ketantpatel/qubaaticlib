using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class RoleAdapter : MasterAdapter
    {
        public UserBean user;
        public RoleAdapter(UserBean user)
        {
            this.user = user;
        }
        public List<RoleBean> FillRoles(bool isSelect)
        {
            List<RoleBean> list = new List<RoleBean>();
            string query = "select role_id,name from roles where status=1";
            DLS db = new DLS(user);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RoleBean role = new RoleBean();
                    role.name = dr["name"].ToString();
                    role.role_id = dr["role_id"].ToString();
                    list.Add(role);
                }
            }
            db.Dispose();
            return list;
        }

        public List<RoleBean> FillUserRoles(bool isSelect)
        {
            List<RoleBean> list = new List<RoleBean>();
            string query = "select role_id,name from roles where status=1 and role_id=2";
            DLS db = new DLS(user);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    RoleBean role = new RoleBean();
                    role.name = dr["name"].ToString();
                    role.role_id = dr["role_id"].ToString();
                    list.Add(role);
                }
            }
            db.Dispose();
            return list;
        }
    }
}
