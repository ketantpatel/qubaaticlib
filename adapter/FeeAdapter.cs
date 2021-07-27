using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class FeeAdapter : MasterAdapter
    {
        public FeeAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }
        public List<FeeBean> SelectFranchiseFeeTypes(bool isSelect,string frn_id)
        {
            List<FeeBean> list = new List<FeeBean>();
            FeeBean f = new FeeBean();
            if (isSelect)
            {
                f = new FeeBean();
                f.fee_type = "--Select--";
                f.fee_id = "-1";
                list.Add(f);
            }
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select ft.*,f.amount,f.amount_min,f.amount_max,f.fee_id,f.tax,f.is_active fee_active from fee_types ft left join fee_type_fee f on f.fee_type_id=ft.fee_type_id and f.frn_id=" + frn_id + " where ft.is_active=1 and f.is_active=1 and ft.tier_id in (select tier_id from franchises f where f.frn_id="+frn_id+")");
            db.Dispose();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    f = new FeeBean();
                    f.fee_amount = dr["amount"].ToString();
                    f.fee_amount_min = dr["amount_min"].ToString();
                    f.fee_amount_max = dr["amount_max"].ToString();
                    f.fee_min = dr["fee_min"].ToString();
                    f.fee_max = dr["fee_max"].ToString();
                    f.fee_id = dr["fee_id"].ToString();
                    f.fee_type = dr["fee_type"].ToString();
                    f.fee_type_id = dr["fee_type_id"].ToString();
                    f.is_active = dr["fee_active"].ToString();
                    f.tax = dr["tax"].ToString();
                    list.Add(f);
                }
            }
            return list;
        }
        public List<FeeBean> ListFranchiseFeeTypes(string frn_id,string frn_user_type_id)
        {
            List<FeeBean> list = new List<FeeBean>();
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select ft.*,f.amount,f.amount_min,f.amount_max,f.fee_id,f.tax,f.is_active fee_active from fee_types ft left join fee_type_fee f on f.fee_type_id=ft.fee_type_id and f.frn_id=" + frn_id + " and f.is_active=1 where ft.is_active=1 and ft.tier_id in (select tier_id from franchises f where f.frn_id=" + frn_id + ") and ft.frn_user_type_id="+frn_user_type_id+"");
            db.Dispose();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    FeeBean f = new FeeBean();
                    f.fee_amount = dr["amount"].ToString();
                    f.fee_amount_min = dr["amount_min"].ToString();
                    f.fee_amount_max = dr["amount_max"].ToString();
                    f.fee_min = dr["fee_min"].ToString();
                    f.fee_max = dr["fee_max"].ToString();
                    f.fee_id = dr["fee_id"].ToString();
                    f.fee_type = dr["fee_type"].ToString();
                    f.fee_type_id = dr["fee_type_id"].ToString();
                    f.is_active = dr["fee_active"].ToString();
                    f.tax = dr["tax"].ToString();                    
                    list.Add(f);
                }
            }
            return list;
        }

        public FeeBean GetFranchiseFeeType(string frn_id, string fee_type_id)
        {
            FeeBean f = new FeeBean();
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select ft.*,f.amount,f.amount_min,f.amount_max,f.fee_id,f.tax,f.is_active fee_active from fee_types ft left join fee_type_fee f on f.fee_type_id=ft.fee_type_id and f.frn_id=" + frn_id + " where ft.is_active=1 and f.is_active=1 and f.fee_type_id="+fee_type_id);
            db.Dispose();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    f.fee_amount = dr["amount"].ToString();
                    f.fee_amount_min = dr["amount_min"].ToString();
                    f.fee_amount_max = dr["amount_max"].ToString();
                    f.fee_min = dr["fee_min"].ToString();
                    f.fee_max = dr["fee_max"].ToString();
                    f.fee_id = dr["fee_id"].ToString();
                    f.fee_type = dr["fee_type"].ToString();
                    f.fee_type_id = dr["fee_type_id"].ToString();
                    f.is_active = dr["fee_active"].ToString();
                    f.tax = dr["tax"].ToString();


                }
            }
            return f;
        }
        public FeeBean SaveFee(FeeBean f)
        {
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("amount", f.fee_amount, MyDBTypes.Int);
            db.AddParameters("amount_min", f.fee_amount_min, MyDBTypes.Int);
            db.AddParameters("amount_max", f.fee_amount_max, MyDBTypes.Int);
            db.AddParameters("fee_type_id", f.fee_type_id, MyDBTypes.Int);
            db.AddParameters("frn_id", f.frn_id, MyDBTypes.Int);
            db.AddParameters("tax", f.tax, MyDBTypes.Int);
            
            f.is_db_success = false;
            if (string.IsNullOrEmpty(f.fee_id))
            {
                db.AddParameters("is_active", "1", MyDBTypes.Int);
                if (db.Insert("fee_type_fee"))
                {
                   
                    f.fee_id = db.GetLastAutoID().ToString();
                    f.is_db_success = true;
                }
            }
            else
            {
                float oldAmount = 0;
                float.TryParse(f.fee_old_amout, out oldAmount);
                float newAmount = 0;
                float.TryParse(f.fee_amount, out newAmount);

                float oldTax = 0;
                float.TryParse(f.tax_old, out oldTax);
                float newTax = 0;
                float.TryParse(f.tax, out newTax);

                if (oldAmount != newAmount)
                {
                    if (db.Insert("fee_type_fee"))
                    {
                        db.AddParameters("is_active", "1", MyDBTypes.Int);
                        f.fee_id = db.GetLastAutoID().ToString();
                        f.is_db_success = true;
                    }
                }
                else if(oldTax != newTax)
                {
                    if (db.Insert("fee_type_fee"))
                    {
                        db.AddParameters("is_active", "1", MyDBTypes.Int);
                        f.fee_id = db.GetLastAutoID().ToString();
                        f.is_db_success = true;
                    }
                }
                
                   
                
            }

            db.AddParameters("is_active", "0", MyDBTypes.Int);
            db.Update("fee_type_fee", "fee_type_id=" + f.fee_type_id + " and frn_id=" + f.frn_id + " and fee_id!=" + f.fee_id + "");
            f.is_db_success = true;

            db.Dispose();
            return f;
        }

        public List<FeeBean> ListFeeTypes(string frn_id,string frn_user_type_id)
        {
            List<FeeBean> list = new List<FeeBean>();

            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from fee_types t left join fee_type_fee f on f.fee_type_id=t.fee_type_id and f.frn_id="+frn_id+" and f.is_active=1 where t.tier_id in (select tier_id from franchises f where f.frn_id="+frn_id+") and t.frn_user_type_id="+frn_user_type_id+"");
            db.Dispose();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    FeeBean f = new FeeBean();
                    f.fee_type = dr["fee_type"].ToString();
                    f.fee_type_id = dr["fee_type_id"].ToString();
                    f.fee_amount = dr["amount"].ToString();

                    list.Add(f);
                }
            }
            return list;
        }
        public List<FeeBean> ListFeeMasterTypes(string frn_id, string frn_user_type_id)
        {
            List<FeeBean> list = new List<FeeBean>();

            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from fee_type_master");
            db.Dispose();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    FeeBean f = new FeeBean();
                    f.fee_type = dr["fee_type"].ToString();
                    f.fee_type_id = dr["fee_type_mst_id"].ToString();
                    list.Add(f);
                }
            }
            return list;
        }

        public DataTable GetRoyaltiTable(string frn_id,string frn_user_type_id)
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dr = db.GetDataTable("select * from fee_types ft where  frn_user_type_id="+frn_user_type_id+" and ft.tier_id in (select tier_id from franchises f where f.frn_id="+frn_id+")");
            db.Dispose();
            return dr;
        }
        public DataTable GetTierRoyaltiTable()
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dr = db.GetDataTable("select * from fee_types");
            db.Dispose();
            return dr;
        }

        public DataTable ListTiers()
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dr = db.GetDataTable("select * from fee_tier");
            db.Dispose();
            return dr;
        }
    }
}
