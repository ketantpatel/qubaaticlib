using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class ZoneAdapter : MasterAdapter
    {
        public ZoneAdapter(UserBean bean)
        {
            this.AppUserBean = bean;
        }

        public ZoneBean ZoneInfo(string zone_id)
        {
            ZoneBean z = new ZoneBean();
            DLS db = new DLS(this.AppUserBean);
            DataRow dr = db.GetSingleDataRow("select * from zones where zone_id="+zone_id+"");
            db.Dispose();
            if (dr != null)
            {
                z.zone_id = zone_id;
                z.zone_name = dr["zone_name"].ToString();
                z.zone_code = dr["zone_code"].ToString();
                z.state_id = dr["state_id"].ToString();

            }
            return z;
        }

        public List<ZoneBean> FillZones(string state_id, bool isSelect)
        {
            List<ZoneBean> list=new List<ZoneBean>();
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from zones where state_id="+state_id+"");
            db.Dispose();

            ZoneBean zb = new ZoneBean();
            if (isSelect)
            {

                zb.zone_id = "-1";
                zb.zone_name = "--Select--";
                list.Add(zb);

            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    zb = new ZoneBean();
                    zb.zone_id = dr["zone_id"].ToString();
                    zb.zone_name = dr["zone_name"].ToString();
                    list.Add(zb);

                }
            }
            return list;
        }

        public DataTable ListZones()
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from zones");
            db.Dispose();
            return dt;
        }

        public ZoneBean SaveZone(ZoneBean bean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (bean.zone_id == "-1")
            {
                db.AddParameters("state_id", bean.state_id, MyDBTypes.Varchar);
                db.AddParameters("zone_name", bean.zone_name, MyDBTypes.Varchar);
                db.AddParameters("zone_code", bean.zone_code, MyDBTypes.Varchar);
                if (db.Insert("zones"))
                {
                    bean.zone_id = db.GetLastAutoID().ToString();
                }
            }
            else
            {
                db.AddParameters("state_id", bean.state_id, MyDBTypes.Varchar);
                db.AddParameters("zone_name", bean.zone_name, MyDBTypes.Varchar);
                db.AddParameters("zone_code", bean.zone_code, MyDBTypes.Varchar);
                db.Update("zones", "zone_id=" + bean.zone_id);
            }
            db.Dispose();
            return bean;
        }
    }
}
