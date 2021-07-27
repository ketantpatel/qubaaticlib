using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class CertyAdapter : MasterAdapter
    {
        public CertyAdapter(UserBean ubean)
        {
            this.AppUserBean = ubean;
        }
        public CertyBean SaveCerty(CertyBean fbean)
        {
            if (fbean.doc_id == "-1")
            {
                fbean = AddCerty(fbean);
            }
            else
            {
                fbean = EditCerty(fbean);
            }

            return fbean;
        }
        public CertyBean AddCerty(CertyBean fbean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(fbean.certy_id))
                db.AddParameters("certy_id", fbean.certy_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.file_name))
                db.AddParameters("file_name", fbean.file_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.stud_id))
                db.AddParameters("stud_id", fbean.stud_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.frn_id))
                db.AddParameters("frn_id", fbean.frn_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.create_date))
                db.AddParameters("create_date", fbean.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(fbean.modify_date))
                db.AddParameters("modify_date", fbean.modify_date, MyDBTypes.DateTime);

            fbean.is_opr_success = false;
            if (db.Insert("documents_certy"))
            {
                fbean.is_opr_success = true;
            }
            db.Dispose();
            return fbean;
        }
        public CertyBean EditCerty(CertyBean fbean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(fbean.certy_id))
                db.AddParameters("certy_id", fbean.certy_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.file_name))
                db.AddParameters("file_name", fbean.file_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.stud_id))
                db.AddParameters("stud_id", fbean.stud_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.frn_id))
                db.AddParameters("frn_id", fbean.frn_id, MyDBTypes.Int);

            fbean.is_opr_success = false;
            if (db.Update("documents_certy", "doc_id=" + fbean.doc_id))
            {
                fbean.is_opr_success = true;
            }
            db.Dispose();
            return fbean;
        }

        public List<CertyBean> ListCerty(string stud_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from documents_certy d left join ask_educations e on e.edu_id=d.certy_id and d.stud_id=" + stud_id;
            List<CertyBean> list = new List<CertyBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CertyBean fb = new CertyBean();
                        fb.doc_id = dr["doc_id"].ToString();
                        fb.certy_name = dr["edu_title"].ToString();
                        fb.certy_id = dr["certy_id"].ToString();
                        fb.file_name = dr["file_name"].ToString();
                        fb.stud_id = dr["stud_id"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public CertyBean GetCerty(string stud_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from documents_certy d left join ask_educations e on e.edu_id=d.certy_id and d.stud_id="+stud_id;
            List<CertyBean> list = new List<CertyBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CertyBean fb = new CertyBean();
                        fb.doc_id = dr["doc_id"].ToString();
                        fb.certy_name = dr["edu_title"].ToString();
                        fb.certy_id = dr["certy_id"].ToString();
                        fb.file_name = dr["file_name"].ToString();
                        fb.stud_id = dr["stud_id"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            if (list.Count > 0)
                return list[0];
            return new CertyBean();
        }

        public bool DeleteCerty(string doc_id)
        {
            bool flag = false;
            DLS db = new DLS(this.AppUserBean);
            flag=db.Delete("documents_certy", "doc_id=" + doc_id + "");
            db.Dispose();
            return flag;
        }
        public CertyBean GetCertyDetail(string doc_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from documents_certy where doc_id=" + doc_id;
            List<CertyBean> list = new List<CertyBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        CertyBean fb = new CertyBean();
                        fb.doc_id = dr["doc_id"].ToString();
                        fb.certy_id = dr["certy_id"].ToString();
                        fb.file_name = dr["file_name"].ToString();
                        fb.stud_id = dr["stud_id"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            if (list.Count > 0)
                return list[0];
            return new CertyBean();
        }
    }
}
