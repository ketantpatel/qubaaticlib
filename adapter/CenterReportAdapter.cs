using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class CenterReportAdapter
    {
        public UserBean user;
        public CenterReportAdapter(UserBean user)
        {
            this.user = user;
        }
        public CenterReportEntryBean SaveCenterReport(CenterReportEntryBean b)
        {
            DLS db = new DLS(user);
            db.AddParameters("batch_no", b.batch_no, MyDBTypes.Varchar);
            db.AddParameters("day1", b.day1, MyDBTypes.DateTime);
            db.AddParameters("day1_time", b.day1_time, MyDBTypes.DateTime);
            db.AddParameters("day2", b.day2, MyDBTypes.DateTime);
            db.AddParameters("day2_time", b.day2_time, MyDBTypes.DateTime);
            db.AddParameters("frn_id", b.frn_id, MyDBTypes.DateTime);
            db.AddParameters("totalStudents", b.totalStudents, MyDBTypes.Varchar);
            db.AddParameters("currentlevel", b.currentlevel, MyDBTypes.Varchar);
            db.AddParameters("start_date", b.start_date, MyDBTypes.DateTime);
            db.AddParameters("compeledlession", b.compeledlession, MyDBTypes.Varchar);
            db.AddParameters("teach_id", b.teach_id, MyDBTypes.Varchar);
            db.AddParameters("create_date", DateTime.Now, MyDBTypes.DateTime);
            db.AddParameters("created_by", b.created_by, MyDBTypes.Int);
            //if(!string.IsNullOrEmpty(b.id))
            //{
            //    db.AddParameters("id", b.id, MyDBTypes.Int);

            //}
            //if (b.id == "-1")
            //{
            b.isSuccess = db.Insert("center_report_entry");
            b.id = db.GetLastAutoID().ToString();
            //}
            //else
            //{
            //    b.issuccess = db.update("batches", "batch_id=" + b.batch_id);
            //}
            db.Dispose();
            return b;
        }


        public List<CenterReportEntryBean> GetCenterReport(string frn_id, string month = null, string year = null)
        {
            DLS db = new DLS(this.user);

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select c.day1,c.day1_time,c.day2,c.day2_time,c.totalStudents,c.start_date,c.compeledlession,tc.first_name,tc.last_name,b.name as batchName,l.level_title from center_report_entry c");
            strQuery.AppendLine(" inner join levels l on c.currentlevel = l.level_id ");
            strQuery.AppendLine(" inner join batches b on b.batch_id = c.batch_no ");
            strQuery.AppendLine(" inner join franchises f on c.frn_id = f.frn_id ");
            strQuery.AppendLine(" inner join teacher_franchise_allocation t on t.frn_id = f.frn_id ");
            strQuery.AppendLine(" inner join teachers tc on tc.teach_id = t.teach_id ");
            strQuery.AppendLine(" inner join users u on u.user_id = f.user_id ");
            strQuery.AppendLine(" where f.frn_id = " + frn_id);



            if (!string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(month))
            {
                if (year.ToLower() != "all")
                {
                    strQuery.AppendLine(" and YEAR(c.start_date)=" + year);
                }

                if (month.ToLower() != "all")
                {
                    strQuery.AppendLine(" and MONTH(c.start_date) =" + month);
                }
            }

            strQuery.AppendLine(" order by c.start_date");

            string query = strQuery.ToString();
            List<CenterReportEntryBean> list = new List<CenterReportEntryBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        CenterReportEntryBean cr = new CenterReportEntryBean();
                        cr.batch_name = dr["batchName"].ToString();
                        cr.day1 = dr["day1"].ToString();
                        cr.day1_time = dr["day1_time"].ToString();
                        cr.day2 = dr["day2"].ToString();
                        cr.day2_time = dr["day2_time"].ToString();
                        cr.totalStudents = dr["totalStudents"].ToString();
                        cr.level_name = dr["level_title"].ToString();
                        cr.start_date = Convert.ToDateTime(dr["start_date"].ToString()).ToString("yyyy-MM-dd");
                        cr.compeledlession = dr["compeledlession"].ToString();
                        cr.teacher_name = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                        list.Add(cr);
                    }
                }
            }
            db.Dispose();
            //if (list.Count > 0)
            //    return list[0];
            // return new AdmissionFormBean();
            return list;
        }

        public List<SelectBean> getMonths(string franchise_id)
        {
            List<SelectBean> list = new List<SelectBean>();
            SelectBean s = new SelectBean();
            s.id = "All";
            s.text = "--All--";
            list.Add(s);
            DLS db = new DLS(this.user);
            string query = "select distinct monthname(start_date) as month_name,month(start_date) month_no from center_report_entry where frn_id=" + franchise_id + " order by month(start_date)";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        s = new SelectBean();
                        s.id = dr["month_no"].ToString();
                        s.text = dr["month_name"].ToString();
                        list.Add(s);
                    }
                }
            }
            db.Dispose();
            return list;
        }

        public List<SelectBean> getYears(string franchise_id)
        {
            List<SelectBean> list = new List<SelectBean>();
            DLS db = new DLS(this.user);
            SelectBean s1 = new SelectBean();
            s1.id = "All";
            s1.text = "--All--";
            list.Add(s1);
            string query = "select distinct year(start_date) year from center_report_entry where frn_id=" + franchise_id + " order by year(start_date)";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SelectBean s = new SelectBean();
                        s.id = dr["year"].ToString();
                        s.text = dr["year"].ToString();
                        list.Add(s);
                    }
                }
            }
            db.Dispose();
            return list;
        }

        public string GetTotalEnquiries(string frn_id, string month = null, string year = null)
        {
            DLS db = new DLS(this.user);

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select count(*) from enquiry f");
            strQuery.AppendLine(" where f.frn_id = " + frn_id);



            if (!string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(month))
            {
                if (year.ToLower() != "all")
                {
                    strQuery.AppendLine(" and YEAR(f.create_date)=" + year);
                }

                if (month.ToLower() != "all")
                {
                    strQuery.AppendLine(" and MONTH(f.create_date) =" + month);
                }
            }


            string query = strQuery.ToString();
            string count = db.GetSingleValue(query);

            db.Dispose();
            return count;
        }

        public string GetTotalAdmission(string frn_id, string month = null, string year = null)
        {
            DLS db = new DLS(this.user);

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select count(*) from admission_forms a");
            strQuery.AppendLine(" where a.frn_id = " + frn_id);



            if (!string.IsNullOrEmpty(year) && !string.IsNullOrEmpty(month))
            {
                if (year.ToLower() != "all")
                {
                    strQuery.AppendLine(" and YEAR(a.create_date)=" + year);
                }

                if (month.ToLower() != "all")
                {
                    strQuery.AppendLine(" and MONTH(a.create_date) =" + month);
                }
            }

            string query = strQuery.ToString();
            string count = db.GetSingleValue(query);

            db.Dispose();
            return count;
        }

        public string GetMarketingActivities(string frn_id, string year, string month)
        {
            DLS db = new DLS(this.user);

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select title from marketting_activities f");
            strQuery.AppendLine(" where f.frn_id = " + frn_id);

            if (!string.IsNullOrEmpty(year) || !string.IsNullOrEmpty(month))
            {
                if (!string.IsNullOrEmpty(year) && year.ToLower() != "all")
                {
                    strQuery.AppendLine(" and f.year=" + year);
                }

                if (!string.IsNullOrEmpty(month) && month.ToLower() != "all")
                {
                    strQuery.AppendLine(" and f.month =" + month);
                }
            }


            string query = strQuery.ToString();
            DataTable dt = db.GetDataTable(query);

            string marketingActivies = "";

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (marketingActivies == "")
                    {
                        marketingActivies = dt.Rows[i]["title"].ToString();
                    }
                    else
                    {
                        marketingActivies = marketingActivies + "," + dt.Rows[i]["title"];
                    }
                }

            }

            db.Dispose();
            return marketingActivies;
        }

        public MarketingActiviesBean SaveMarketingActiviesBatch(MarketingActiviesBean b)
        {
            DLS db = new DLS(user);
            db.AddParameters("title", b.title, MyDBTypes.Varchar);
            db.AddParameters("year", b.year, MyDBTypes.Int);
            db.AddParameters("month", b.month, MyDBTypes.Int);
            db.AddParameters("frn_id", b.frn_id, MyDBTypes.Int);
            db.AddParameters("usr_id", b.usr_id, MyDBTypes.Int);

            if (b.mk_id == "-1")
            {
                db.AddParameters("create_date", b.create_date, MyDBTypes.DateTime);
                b.isSuccess = db.Insert("marketting_activities");
            }
            else
            {
                db.AddParameters("modify_date", b.modify_date, MyDBTypes.DateTime);
                b.isSuccess = db.Update("marketting_activities", "mk_id=" + b.mk_id);
            }
            db.Dispose();
            return b;
        }

        public List<MarketingActiviesBean> getMarkettingActivies(string frn_id)
        {
            DLS db = new DLS(this.user);

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select mk_id,title,year,month from marketting_activities m");
            strQuery.AppendLine(" where m.frn_id = " + frn_id);

            string query = strQuery.ToString();
            List<MarketingActiviesBean> list = new List<MarketingActiviesBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        MarketingActiviesBean cr = new MarketingActiviesBean();

                        cr.title = dr["title"].ToString();
                        cr.mk_id = dr["mk_id"].ToString();
                        cr.year = dr["year"].ToString();
                        cr.month = dr["month"].ToString();

                        list.Add(cr);
                    }
                }
            }
            db.Dispose();

            return list;
        }


        public MarketingActiviesBean getMarkettingActiviyById(string id)
        {
            DLS db = new DLS(this.user);
            MarketingActiviesBean cr = new MarketingActiviesBean();

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select mk_id,title,year,month from marketting_activities m");
            strQuery.AppendLine(" where m.mk_id = " + id);

            string query = strQuery.ToString();
            DataRow dr = db.GetSingleDataRow(query);
            if (dr != null)
            {
                cr.title = dr["title"].ToString();
                cr.mk_id = dr["mk_id"].ToString();
                cr.year = dr["year"].ToString();
                cr.month = dr["month"].ToString();
            }

            db.Dispose();

            return cr;
        }

        public bool DeleteMarketingActivity(string id)
        {
            DLS db = new DLS(this.user);
            bool isFlag = db.Delete("marketting_activities", "mk_id=" + id);
            db.Dispose();
            return isFlag;
        }
    }
}
