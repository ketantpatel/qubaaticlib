using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class StateAdapter : MasterAdapter
    {
        public StateAdapter(UserBean bean)
        {
            this.AppUserBean = bean;
        }

        public StateBean StateInfo(string state_id)
        {
            StateBean z = new StateBean();
            DLS db = new DLS(this.AppUserBean);
            DataRow dr = db.GetSingleDataRow("select * from states where state_id=" + state_id + "");
            db.Dispose();
            if (dr != null)
            {
                z.state_id = state_id;
                z.state_name = dr["state_name"].ToString();
                z.code = dr["code"].ToString();
                z.state_id = dr["state_id"].ToString();
                z.country_id = dr["country_id"].ToString();

            }
            return z;
        }
        public AreaBean AreaInfo(string area_id)
        {
            AreaBean z = new AreaBean();
            DLS db = new DLS(this.AppUserBean);
            DataRow dr = db.GetSingleDataRow("select * from areas where area_id=" + area_id + "");
            db.Dispose();
            if (dr != null)
            {
                z.area_id = area_id;
                z.area_name = dr["area_name"].ToString();
                z.area_code = dr["area_code"].ToString();
                z.state_id = dr["state_id"].ToString();

            }
            return z;
        }

        public List<StateBean> FillStates(string state_id, bool isSelect)
        {
            List<StateBean> list = new List<StateBean>();
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from states where state_id=" + state_id + "");
            db.Dispose();

            StateBean zb = new StateBean();
            if (isSelect)
            {

                zb.state_id = "-1";
                zb.state_name = "--Select--";
                list.Add(zb);

            }
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    zb = new StateBean();
                    zb.state_id = dr["state_id"].ToString();
                    zb.state_name = dr["state_name"].ToString();
                    list.Add(zb);

                }
            }
            return list;
        }

        public DataTable ListStates()
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from states");
            db.Dispose();
            return dt;
        }

        public DataTable ListStates(string country_id)
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from states where country_id="+country_id);
            db.Dispose();
            return dt;
        }

        public DataTable ListAreas(string state_id,string frn_type = "",string frn_id = "")
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = new DataTable();
            if (frn_type == "6")
            {
                string area_ids = db.GetSingleValue(" select area_ids FROM franchises where frn_id= " + frn_id);
                if (!string.IsNullOrEmpty(area_ids))
                {
                    dt = db.GetDataTable("select * from areas where area_id in (" + area_ids + ")");
                }
            }
            else
            {
                dt = db.GetDataTable("select * from areas where state_id=" + state_id);
            }
            db.Dispose();
            return dt;
        }
     


        public StateBean SaveState(StateBean bean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (bean.state_id == "-1")
            {
               // db.AddParameters("state_id", bean.state_id, MyDBTypes.Varchar);
                db.AddParameters("state_name", bean.state_name, MyDBTypes.Varchar);
                db.AddParameters("code", bean.code, MyDBTypes.Varchar);
                db.AddParameters("country_id", bean.country_id, MyDBTypes.Varchar);
                if (db.Insert("states"))
                {
                    bean.state_id = db.GetLastAutoID().ToString();
                    bean.isSuccess = true;
                }
            }
            else
            {
              //  db.AddParameters("state_id", bean.state_id, MyDBTypes.Varchar);
                db.AddParameters("state_name", bean.state_name, MyDBTypes.Varchar);
                db.AddParameters("code", bean.code, MyDBTypes.Varchar);
                db.AddParameters("country_id", bean.country_id, MyDBTypes.Varchar);

                if (db.Update("states", "state_id=" + bean.state_id))
                {
                    bean.isSuccess = true;
                }
            }
            db.Dispose();
            return bean;
        }
        public AreaBean SaveArea(AreaBean bean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (bean.area_id == "-1")
            {
                // db.AddParameters("state_id", bean.state_id, MyDBTypes.Varchar);
                db.AddParameters("area_name", bean.area_name, MyDBTypes.Varchar);
                if(!String.IsNullOrEmpty(bean.area_code))
                db.AddParameters("area_code", bean.area_code, MyDBTypes.Varchar);
                else
                    db.AddParameters("area_code", string.Empty, MyDBTypes.Varchar);

                db.AddParameters("state_id", bean.state_id, MyDBTypes.Int);
                if (db.Insert("areas"))
                {
                    bean.area_id = db.GetLastAutoID().ToString();
                    bean.isSuccess = true;
                }
            }
            else
            {
                //  db.AddParameters("state_id", bean.state_id, MyDBTypes.Varchar);
                db.AddParameters("area_name", bean.area_name, MyDBTypes.Varchar);
                db.AddParameters("area_code", bean.area_code, MyDBTypes.Varchar);
                db.AddParameters("state_id", bean.state_id, MyDBTypes.Int);

                if (db.Update("areas", "area_id=" + bean.area_id))
                {
                    bean.isSuccess = true;
                }
            }
            db.Dispose();
            return bean;
        }
    }
}

