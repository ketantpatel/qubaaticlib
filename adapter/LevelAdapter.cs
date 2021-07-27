using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class LevelAdapter : MasterAdapter
    {
        public LevelAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }

        public List<LevelBean> FillLevels(bool isSelect)
        {
            DLS db = new DLS(this.AppUserBean);
            List<LevelBean> list = new List<LevelBean>();
            if (isSelect)
            {
                LevelBean d = new LevelBean();
                d.level_id = "";
                d.level_title = "--Select--";
                list.Add(d);
            }
            string query = "select level_id,level_title from levels where is_active=1";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    LevelBean d = new LevelBean();
                    d.level_id = dr["level_id"].ToString();
                    d.level_title = dr["level_title"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }


        public LevelBean FillLevel(string id)
        {
            DLS db = new DLS(this.AppUserBean);
            LevelBean d = new LevelBean();

            string query = "select level_id,level_title from levels where is_active=1 and level_id =" + id;
            DataRow dr = db.GetSingleDataRow(query);
            if (dr != null)
            {
                d.level_id = dr["level_id"].ToString();
                d.level_title = dr["level_title"].ToString();
            }
            db.Dispose();
            return d;
        }
    }
}
