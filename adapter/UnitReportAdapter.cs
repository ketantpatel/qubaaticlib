using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class UnitReportAdapter : MasterAdapter
    {
        public UnitReportAdapter(UserBean ubean)
        {
            this.AppUserBean = ubean;
        }
        public List<AdmissionFormBean> GetAdmissionList(string user_id, string month = null, string year = null, string country_id = null, string state_id = null)
        {
            DLS db = new DLS(this.AppUserBean);

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select f.address1,a.first_name as fn,a.middle_name as mn,a.last_name as ln,a.enroll_no,a.create_date,f.frn_code,f.name as ffn,b.name as bn,tc.first_name tcfn,tc.last_name tcln from admission_forms a ");
           
            
            strQuery.AppendLine(" inner join batches b on b.batch_id =a.batch_id ");
            strQuery.AppendLine(" inner join franchises f on b.frn_id = f.frn_id  ");
            strQuery.AppendLine(" inner join teacher_franchise_allocation t on t.frn_id = f.frn_id ");
            strQuery.AppendLine(" inner join teachers tc on tc.teach_id = t.teach_id ");
            strQuery.AppendLine(" inner join users u on u.user_id = f.user_id ");
            strQuery.AppendLine(" where f.user_id in ( " + user_id + ")");

            string state = "";

            if (!string.IsNullOrEmpty(country_id))
            {
                strQuery.AppendLine(" and f.country_id =" + country_id);
            }

            if (!string.IsNullOrEmpty(state_id))
            {
                //strQuery.AppendLine(" and f.state_id =" + state_id);
                if (!string.IsNullOrEmpty(country_id))
                {
                     state = db.GetSingleValue(" select state_name from states where state_id =" + state_id);
                }
                else
                {
                    state = db.GetSingleValue(" select state_name from states where state_id =" + state_id  + " and country_id = "+ country_id);
                }
            }


            if (!string.IsNullOrEmpty(year) || !string.IsNullOrEmpty(month))
            {
                if (!string.IsNullOrEmpty(year) && year.ToLower() != "all")
                {
                    strQuery.AppendLine(" and YEAR(a.create_date)=" + year);
                }

                if (!string.IsNullOrEmpty(month) && month.ToLower() != "all")
                {
                    strQuery.AppendLine(" and MONTH(a.create_date) =" + month);
                }
            }

            strQuery.AppendLine(" order by a.create_date");

            string query = strQuery.ToString();
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.address = dr["address1"].ToString();
                        fb.state_name = state;
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.frn_name = dr["ffn"].ToString();
                        fb.create_date = dr["create_date"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.full_name = dr["fn"].ToString() + dr["mn"].ToString() + dr["ln"].ToString();
                        fb.batch_name = dr["bn"].ToString();
                        fb.teacher_name = dr["tcfn"].ToString() + dr["tcln"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            //if (list.Count > 0)
            //    return list[0];
            // return new AdmissionFormBean();
            return list;
        }




        public List<AdmissionFormBean> GetDropoutStudents(string user_id, string month = null, string year = null, string country_id = null, string state_id = null)

        {
            DLS db = new DLS(this.AppUserBean);

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select tc.level_id,f.address1,a.first_name as fn,a.dropoutreason,a.middle_name as mn,a.last_name as ln,a.enroll_no,a.create_date,f.frn_code,f.name as ffn,b.name as bn,tc.first_name tcfn,tc.last_name tcln from admission_forms a ");
            strQuery.AppendLine(" inner join batches b on b.batch_id =a.batch_id ");
            strQuery.AppendLine(" inner join franchises f on b.frn_id = f.frn_id ");
            strQuery.AppendLine(" inner join teacher_franchise_allocation t on t.frn_id = f.frn_id ");
            strQuery.AppendLine(" inner join teachers tc on tc.teach_id = t.teach_id ");
            strQuery.AppendLine(" inner join users u on u.user_id = f.user_id ");
            strQuery.AppendLine(" where f.user_id in ( " + user_id + ") and a.isdropout = 1");

            if (!string.IsNullOrEmpty(country_id))
            {
                strQuery.AppendLine(" and f.country_id =" + country_id);
            }

            //if (!string.IsNullOrEmpty(state_id))
            //{
            //    strQuery.AppendLine(" and f.state_id =" + state_id);
            //}


            //if (!string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(month))
            //{
            //    if (year.ToLower() != "all")
            //    {
            //        strQuery.AppendLine(" and YEAR(a.dropoutdate)=" + year);
            //    }

            //    if (month.ToLower() != "all")
            //    {
            //        strQuery.AppendLine(" and MONTH(a.dropoutdate) =" + month);
            //    }
            //}

            if (!string.IsNullOrEmpty(year) || !string.IsNullOrEmpty(month))
            {
                if (!string.IsNullOrEmpty(year) && year.ToLower() != "all")
                {
                    strQuery.AppendLine(" and YEAR(a.dropoutdate)=" + year);
                }

                if (!string.IsNullOrEmpty(month) && month.ToLower() != "all")
                {
                    strQuery.AppendLine(" and MONTH(a.dropoutdate) =" + month);
                }
            }

            strQuery.AppendLine(" order by a.dropoutdate");

            LevelAdapter levelAdapter = new LevelAdapter(this.AppUserBean);

            string query = strQuery.ToString();
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.address = dr["address1"].ToString();
                        fb.level_name = dr["level_id"].ToString() != "" ? levelAdapter.FillLevel(dr["level_id"].ToString()).level_title  : "" ;
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.frn_name = dr["ffn"].ToString();
                        fb.create_date = dr["create_date"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.full_name = dr["fn"].ToString() + "    " + dr["mn"].ToString() + "    " + dr["ln"].ToString();
                        fb.batch_name = dr["bn"].ToString();
                        fb.teacher_name = dr["tcfn"].ToString() + dr["tcln"].ToString();
                        fb.dropoutreason = dr["dropoutreason"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            //if (list.Count > 0)
            //    return list[0];
            // return new AdmissionFormBean();
            return list;
        }
    }
}
