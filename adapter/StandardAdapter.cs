using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class StandardAdapter : MasterAdapter
    {
        public StandardAdapter(UserBean ubean)
        {
            this.AppUserBean = ubean;
        }
        public List<StandardBean> FillStandard(bool isSelect)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from standards where status=1";
            List<StandardBean> list = new List<StandardBean>();
            DataTable dt = db.GetDataTable(query);

            if (isSelect)
            {
                StandardBean fb = new StandardBean();
                fb.std_id = "";
                fb.name = "--Select--";
                list.Add(fb);
            }

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        StandardBean fb = new StandardBean();
                        fb.std_id = dr["std_id"].ToString();
                        fb.name = dr["name"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
    }
}
