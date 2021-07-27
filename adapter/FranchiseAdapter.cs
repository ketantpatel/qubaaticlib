using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class FranchiseAdapter : MasterAdapter
    {
        public FranchiseAdapter(UserBean ubean)
        {
            this.AppUserBean = ubean;
        }
        public string GetStateCode(string state_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select code from states where state_id=" + state_id + "";
            string value = db.GetSingleValue(query);
            int count = 0;
            int.TryParse(value, out count);
            count = count + 1;
            db.Dispose();
            return value;
        }
        public string GetCountryCode(string country_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select country_code from countries where country_id=" + country_id + "";
            string value = db.GetSingleValue(query);
            db.Dispose();
            return value;
        }
        public string MasterZonalFranchiseCode(string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select count(*) from franchises where parent_id in ( select frn_id from franchises where frn_id=" + frn_id + " and frn_type='Master') and frn_type='Zonal'";
            string value = db.GetSingleValue(query);
            int count = 0;
            int.TryParse(value, out count);
            count = count + 1;
            db.Dispose();
            //if (value.Length == 1)
            //    value = "00" + count;
            //if (value.Length == 2)
            //    value = "0" + count;
            return count.ToString();
        }
        public string GetFranchiseCode(string type, string state_id, string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select max(f.serial_no) from franchises f inner join users u on u.user_id=f.user_id where u.user_type=" + type + " and f.parent_id=" + frn_id + "";
            string value = db.GetSingleValue(query);
            db.Dispose();
            int count = 0;
            int.TryParse(value, out count);
            count = count + 1;
            string newcode = "";
            if (count.ToString().Length == 1)
                newcode = "0" + count.ToString();
            else
                newcode = count.ToString();
            //else if (count.ToString().Length == 2)
            //    newcode = "0" + count;
            return newcode;
        }
        public string ZonalFranchiseUniteCode(string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select count(*) from franchises where parent_id in ( select frn_id from franchises where frn_id=" + frn_id + " and frn_type='Zonal') and frn_type='Franchise'";
            string value = db.GetSingleValue(query);
            db.Dispose();
            int count = 0;
            int.TryParse(value, out count);
            count = count + 1;
            if (value.Length == 1)
                value = "0" + count;
            //if (value.Length == 2)
            //  value = "0" + count;
            return value;
        }
        public string ZonalFranchiseSchoolCode(string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select count(*) from franchises where parent_id in ( select frn_id from franchises where frn_id=" + frn_id + " and frn_type='Zonal') and frn_type='School'";
            string value = db.GetSingleValue(query);
            db.Dispose();
            int count = 0;
            int.TryParse(value, out count);
            count = count + 1;
            if (value.Length == 1)
                value = "0" + count;
            //if (value.Length == 2)
            //  value = "0" + count;
            return value;
        }
        public FranchiseBean SaveFranchise(FranchiseBean fbean)
        {
            if (fbean.frn_id == "-1")
            {
                fbean.is_active = "1";
                fbean = AddFranchise(fbean);
            }
            else
            {
                fbean = EditFranchise(fbean);
            }

            return fbean;
        }
        public bool UpdateUserId(string user_id, string fr_id)
        {
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("user_id", user_id, MyDBTypes.Int);
            bool flag = db.Update("franchises", "frn_id=" + fr_id);
            db.Dispose();
            return flag;
        }
        public FranchiseBean AddFranchise(FranchiseBean fbean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(fbean.name))
                db.AddParameters("name", fbean.name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.alt_contact1))
                db.AddParameters("alt_contact1", fbean.alt_contact1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.role))
                db.AddParameters("role", fbean.role, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.alt_contat2))
                db.AddParameters("alt_contact2", fbean.alt_contat2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.contact_person))
                db.AddParameters("contact_person", fbean.contact_person, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.address1))
                db.AddParameters("address1", fbean.address1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.city1))
                db.AddParameters("city1", fbean.city1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.pincode1))
                db.AddParameters("pincode1", fbean.pincode1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.state1))
                db.AddParameters("state1", fbean.state1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.address2))
                db.AddParameters("address2", fbean.address2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.city2))
                db.AddParameters("city2", fbean.city2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.pincode2))
                db.AddParameters("pincode2", fbean.pincode2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.state2))
                db.AddParameters("state2", fbean.state2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.phone1))
                db.AddParameters("phone1", fbean.phone1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.phone2))
                db.AddParameters("phone2", fbean.phone2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.phone3))
                db.AddParameters("phone3", fbean.phone3, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.email_id))
                db.AddParameters("email_id", fbean.email_id, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.is_active))
                db.AddParameters("is_active", fbean.is_active, MyDBTypes.Bit);
            if (!string.IsNullOrEmpty(fbean.create_date))
                db.AddParameters("create_date", fbean.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(fbean.modify_date))
                db.AddParameters("modify_date", fbean.modify_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(fbean.create_by))
                db.AddParameters("create_by", fbean.create_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.modify_by))
                db.AddParameters("modify_by", fbean.modify_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.join_date))
                db.AddParameters("join_date", fbean.join_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(fbean.job_profile))
                db.AddParameters("job_profile", fbean.job_profile, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.exp))
                db.AddParameters("exp", fbean.exp, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.frn_code))
                db.AddParameters("frn_code", fbean.frn_code, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.parent_id))
                db.AddParameters("parent_id", fbean.parent_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.dist_id))
                db.AddParameters("dist_id", fbean.dist_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.zone_id))
                db.AddParameters("zone_id", fbean.zone_id, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.frn_type))
                db.AddParameters("frn_type", fbean.frn_type, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.marketing_activies))
                db.AddParameters("marketing_activity", fbean.marketing_activies, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.state_id))
                db.AddParameters("state_id", fbean.state_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.country_id))
                db.AddParameters("country_id", fbean.country_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.city1))
                db.AddParameters("city", fbean.city1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.tier_id))
                db.AddParameters("tier_id", fbean.tier_id, MyDBTypes.Int);
            else
                db.AddParameters("tier_id", "0", MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.area_ids))
                db.AddParameters("area_ids", fbean.area_ids, MyDBTypes.Varchar);
            else
                db.AddParameters("area_ids", string.Empty, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.serial_no))
                db.AddParameters("serial_no", fbean.serial_no, MyDBTypes.Varchar);

            db.AddParameters("IsClose", fbean.IsClose, MyDBTypes.Bit);

            if (!string.IsNullOrEmpty(fbean.validateMonths))
                db.AddParameters("validateMonths", fbean.validateMonths, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.dropout_reason))
                db.AddParameters("dropout_reason", fbean.dropout_reason, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.validateDate))
                db.AddParameters("validateDate", fbean.validateDate, MyDBTypes.DateTime);

            fbean.is_opr_success = false;
            if (db.Insert("franchises"))
            {
                fbean.is_opr_success = true;
                fbean.frn_id = db.GetLastAutoID().ToString();
            }
            db.Dispose();
            return fbean;
        }
        public FranchiseBean EditFranchise(FranchiseBean fbean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(fbean.role))
                db.AddParameters("role", fbean.role, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.parent_id))
                db.AddParameters("parent_id", fbean.parent_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.name))
                db.AddParameters("name", fbean.name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.alt_contact1))
                db.AddParameters("alt_contact1", fbean.alt_contact1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.alt_contat2))
                db.AddParameters("alt_contact2", fbean.alt_contat2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.contact_person))
                db.AddParameters("contact_person", fbean.contact_person, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.address1))
                db.AddParameters("address1", fbean.address1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.city1))
                db.AddParameters("city1", fbean.city1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.pincode1))
                db.AddParameters("pincode1", fbean.pincode1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.state1))
                db.AddParameters("state1", fbean.state1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.address2))
                db.AddParameters("address2", fbean.address2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.city2))
                db.AddParameters("city2", fbean.city2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.pincode2))
                db.AddParameters("pincode2", fbean.pincode2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.state2))
                db.AddParameters("state2", fbean.state2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.phone1))
                db.AddParameters("phone1", fbean.phone1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.phone2))
                db.AddParameters("phone2", fbean.phone2, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.phone3))
                db.AddParameters("phone3", fbean.phone3, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.email_id))
                db.AddParameters("email_id", fbean.email_id, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.is_active))
                db.AddParameters("is_active", fbean.is_active, MyDBTypes.Bit);
            if (!string.IsNullOrEmpty(fbean.create_date))
                db.AddParameters("create_date", fbean.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(fbean.modify_date))
                db.AddParameters("modify_date", fbean.modify_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(fbean.create_by))
                db.AddParameters("create_by", fbean.create_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.modify_by))
                db.AddParameters("modify_by", fbean.modify_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.join_date))
                db.AddParameters("join_date", fbean.join_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(fbean.job_profile))
                db.AddParameters("job_profile", fbean.job_profile, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.exp))
                db.AddParameters("exp", fbean.exp, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(fbean.frn_code))
                db.AddParameters("frn_code", fbean.frn_code, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.state_id))
                db.AddParameters("state_id", fbean.state_id, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.country_id))
                db.AddParameters("country_id", fbean.country_id, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.city1))
                db.AddParameters("city", fbean.city1, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.marketing_activies))
                db.AddParameters("marketing_activity", fbean.marketing_activies, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(fbean.tier_id))
                db.AddParameters("tier_id", fbean.tier_id, MyDBTypes.Int);
            else
                db.AddParameters("tier_id", "0", MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.area_ids))
                db.AddParameters("area_ids", fbean.area_ids, MyDBTypes.Varchar);
            else
                db.AddParameters("area_ids", string.Empty, MyDBTypes.Varchar);

            db.AddParameters("IsClose", fbean.IsClose, MyDBTypes.Bit);

            if (!string.IsNullOrEmpty(fbean.validateMonths))
                db.AddParameters("validateMonths", fbean.validateMonths, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(fbean.dropout_reason))
                db.AddParameters("dropout_reason", fbean.dropout_reason, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(fbean.validateDate))
                db.AddParameters("validateDate", fbean.validateDate, MyDBTypes.DateTime);

            fbean.is_opr_success = false;
            if (db.Update("franchises", "frn_id=" + fbean.frn_id))
            {
                fbean.is_opr_success = true;
            }
            db.Dispose();
            return fbean;
        }

        public List<FranchiseBean> ListFranchise(string frn_id)
        {
            List<FranchiseBean> list = new List<FranchiseBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select f.frn_code,f.frn_id,f.name,f.contact_person,f.phone1,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 and (f.frn_id=" + frn_id + " or f.parent_id=" + frn_id + ") group by f.name,f.frn_id";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }

            db.Dispose();
            return list;
        }
        public List<FranchiseBean> ListQubaaticResultFranchise(string frn_id)
        {
            List<FranchiseBean> list = new List<FranchiseBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select f.frn_code,f.frn_id,f.name,f.contact_person,f.phone1,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 and (f.frn_id=" + frn_id + " or f.parent_id=" + frn_id + ") group by f.name,f.frn_id";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.total_students = db.GetSingleValue(@"select count(*) from admission_forms f where frn_id=" + dr["frn_id"].ToString() + "");

                        fb.total_student_in_examhall = db.GetSingleValue("select count(*) from paper_sessions where form_id in(select form_id from admission_forms where frn_id=" + frn_id + ")");
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }

            db.Dispose();
            return list;
        }
        public List<FranchiseBean> ListQubaaticResultSingleFranchise(string frn_id)
        {
            List<FranchiseBean> list = new List<FranchiseBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select f.frn_code,f.frn_id,f.name,f.contact_person,f.phone1,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 and (f.frn_id=" + frn_id + " or f.parent_id=" + frn_id + ") group by f.name,f.frn_id";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.total_students = db.GetSingleValue(@"select count(*) from admission_forms f where frn_id=" + dr["frn_id"].ToString() + "");

                        fb.total_student_in_examhall = db.GetSingleValue("select count(*) from paper_sessions where form_id in(select form_id from admission_forms where frn_id=" + frn_id + ")");
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }

            db.Dispose();
            return list;
        }
        public List<FranchiseBean> ListUnderMasterFranchise(string frn_id)
        {
            List<FranchiseBean> list = new List<FranchiseBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises where is_active=1 and parent_id=" + frn_id;
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }

            db.Dispose();
            return list;
        }
        //ApplicationUser.user_id
        public List<FranchiseBean> ListRegisterFranchises(bool isCountryAccount, bool isStateAccount, string country_id, string state_id, string userid, string month = null, string year = null)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises f inner join users u on u.user_id =f.user_id inner join states s on f.state1 = s.state_id  where f.is_active=1 and u.user_id in ( " + userid + ")";

            if (!string.IsNullOrEmpty(year))
            {
                if (year.ToLower() != "all")
                {
                    query += " and YEAR(join_date)=" + year;
                }
            }
            if (!string.IsNullOrEmpty(month))
            {
                if (month.ToLower() != "all")
                {
                    query += " and MONTH(join_date) =" + month;
                }
            }


            query += " order by join_date";
            string frn_code = "";
            string frn_name = "";

            DataRow dataRow = db.GetSingleDataRow(" select frn_code,name from franchises f inner join users u on u.user_id =f.user_id  where u.user_id =" + this.AppUserBean.user_id);

            if (dataRow != null)
            {
                frn_code = dataRow["frn_code"].ToString();
                frn_name = dataRow["name"].ToString();
            }

            List<FranchiseBean> list = new List<FranchiseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.join_date = Convert.ToDateTime(dr["join_date"].ToString()).ToShortDateString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.address1 = dr["address1"].ToString();
                        fb.state1 = dr["state_name"].ToString();
                        fb.frn_fees = "";// dr["name"].ToString();
                        fb.frn_coShare = ""; //dr["name"].ToString();
                        fb.frn_master_code = frn_code;
                        fb.frn_master_name = frn_name;
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }

        public List<FranchiseBean> ListFranchises(bool isCountryAccount, bool isStateAccount, string country_id, string state_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises where is_active=1";
            if (isCountryAccount)
                query = "select * from franchises where is_active=1 and country_id=" + country_id;
            if (isStateAccount)
                query = "select * from franchises where is_active=1 and state_id=" + state_id;
            List<FranchiseBean> list = new List<FranchiseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<FranchiseBean> ListFranchises()
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises where is_active=1";
            List<FranchiseBean> list = new List<FranchiseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<TeacherFacultyBean> ListFranchises(string teach_id, string country_id, string state_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "SELECT f.*,t.frn_id t_frn_id,t.teach_id,t.status t_status_id FROM franchises f left join teacher_franchise_allocation  t on t.frn_id=f.frn_id and t.teach_id=" + teach_id + " where f.country_id=" + country_id + " and f.state_id=" + state_id + "";
            List<TeacherFacultyBean> list = new List<TeacherFacultyBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        TeacherFacultyBean fb = new TeacherFacultyBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.frn_name = dr["name"].ToString();
                        fb.status = dr["t_status_id"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<FranchiseBean> ListQubaaticResultFranchises(string exam_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises where is_active=1";
            List<FranchiseBean> list = new List<FranchiseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.total_students = db.GetSingleValue(@"select count(*) from admission_forms f where frn_id=" + dr["frn_id"].ToString() + "");
                        fb.total_student_in_examhall = db.GetSingleValue("select count(*) from paper_sessions where form_id in(select form_id from admission_forms where frn_id=" + dr["frn_id"].ToString() + ") and exam_id=" + exam_id + "");
                        fb.name = dr["name"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<FranchiseBean> ListFranchisesDeactivated()
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises where is_active=0";
            List<FranchiseBean> list = new List<FranchiseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<FranchiseBean> FillFranchises(bool isWithSelect)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises where is_active=1";
            List<FranchiseBean> list = new List<FranchiseBean>();

            FranchiseBean fb = new FranchiseBean();
            fb.frn_id = "";
            fb.name = "--Select--";
            list.Add(fb);

            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<FranchiseBean> FillTeacherFranchises(bool isSelect, string teach_id)
        {
            DLS db = new DLS(this.AppUserBean);
            //string query = "select * from franchises where is_active=1";
            string query = "SELECT t.frn_id,f.name,f.contact_person,f.phone1,f.address1,f.city1,f.pincode1 FROM franchises f inner join teacher_franchise_allocation t on t.frn_id=f.frn_id where t.teach_id=" + teach_id;

            List<FranchiseBean> list = new List<FranchiseBean>();

            FranchiseBean fb = new FranchiseBean();
            fb.frn_id = "";
            fb.name = "--Select--";
            list.Add(fb);

            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<FranchiseBean> FillFranchises(bool isWithSelect, bool isWithAll, string state_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises where is_active=1 and state1=" + state_id + "";
            List<FranchiseBean> list = new List<FranchiseBean>();

            FranchiseBean fb = new FranchiseBean();
            fb.frn_id = "";
            fb.name = "--Select--";
            list.Add(fb);
            if (isWithAll)
            {
                fb = new FranchiseBean();
                fb.frn_id = "0";
                fb.name = "-All--";
                list.Add(fb);
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<FranchiseBean> FillChildFranchises(bool isWithSelect, string parent_frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises where is_active=1 and parent_id=" + parent_frn_id + "";
            List<FranchiseBean> list = new List<FranchiseBean>();
            FranchiseBean fb = new FranchiseBean();

            if (isWithSelect)
            {
                fb.frn_id = "";
                fb.name = "--Select--";
                list.Add(fb);
            }


            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<FranchiseBean> FillChildFranchises(bool isWithSelect, bool isWithAll, string parent_frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises where is_active=1 and (frn_id =" + parent_frn_id + " or parent_id=" + parent_frn_id + ")";
            List<FranchiseBean> list = new List<FranchiseBean>();


            FranchiseBean fb = null;
            if (isWithAll)
            {
                fb = new FranchiseBean();
                fb.frn_id = "All";
                fb.name = "All";
                list.Add(fb);
            }
            else
            {
                fb = new FranchiseBean();
                fb.frn_id = "";
                fb.name = "--Select--";
                list.Add(fb);
            }

            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public FranchiseBean GetUserFranchiseDetail(string parent_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from franchises where user_id=" + parent_id;
            List<FranchiseBean> list = new List<FranchiseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.role = dr["role"].ToString();
                        fb.parent_id = dr["parent_id"].ToString();

                        fb.name = dr["name"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();


                        fb.alt_contact1 = InToStr(dr["alt_contact1"]);
                        fb.alt_contat2 = InToStr(dr["alt_contact2"]);
                        fb.contact_person = InToStr(dr["contact_person"]);
                        fb.address1 = InToStr(dr["address1"]);
                        fb.city1 = InToStr(dr["city1"]);
                        fb.pincode1 = InToStr(dr["pincode1"]);
                        fb.state1 = InToStr(dr["state1"]);
                        fb.address2 = InToStr(dr["address2"]);
                        fb.city2 = InToStr(dr["city2"]);
                        fb.pincode2 = InToStr(dr["pincode2"]);
                        fb.state2 = InToStr(dr["state2"]);
                        fb.phone1 = InToStr(dr["phone1"]);
                        fb.phone2 = InToStr(dr["phone2"]);
                        fb.phone3 = InToStr(dr["phone3"]);
                        fb.email_id = InToStr(dr["email_id"]);
                        fb.is_active = InToStr(dr["is_active"]);
                        fb.create_date = InToStr(dr["create_date"]);
                        fb.modify_date = InToStr(dr["modify_date"]);
                        fb.create_by = InToStr(dr["create_by"]);
                        fb.modify_by = InToStr(dr["modify_by"]);
                        fb.user_id = InToStr(dr["user_id"]);
                        fb.frn_code = InToStr(dr["frn_code"]);
                        fb.frn_type = InToStr(dr["frn_type"]);
                        fb.zone_id = InToStr(dr["zone_id"]);
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            if (list.Count > 0)
                return list[0];
            return new FranchiseBean();
        }

        public FranchiseBean GetFranchiseDetail(string fid)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select f.*,u.user_type from franchises f inner join users u on u.user_id=f.user_id where frn_id=" + fid;
            List<FranchiseBean> list = new List<FranchiseBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.user_type = dr["user_type"].ToString();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.role = dr["role"].ToString();
                        fb.country_id = dr["country_id"].ToString();
                        fb.user_id = dr["user_id"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.phone1 = dr["phone1"].ToString();


                        fb.alt_contact1 = dr["alt_contact1"].ToString();
                        fb.alt_contat2 = dr["alt_contact2"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.address1 = dr["address1"].ToString();
                        fb.city1 = dr["city1"].ToString();
                        fb.pincode1 = dr["pincode1"].ToString();
                        fb.state1 = dr["state1"].ToString();
                        fb.address2 = dr["address2"].ToString();
                        fb.city2 = dr["city2"].ToString();
                        fb.pincode2 = dr["pincode2"].ToString();
                        fb.state2 = dr["state2"].ToString();
                        fb.phone1 = dr["phone1"].ToString();
                        fb.phone2 = dr["phone2"].ToString();
                        fb.phone3 = dr["phone3"].ToString();
                        fb.email_id = dr["email_id"].ToString();
                        fb.is_active = dr["is_active"].ToString();
                        fb.create_date = dr["create_date"].ToString();
                        fb.modify_date = dr["modify_date"].ToString();
                        fb.create_by = dr["create_by"].ToString();
                        fb.modify_by = dr["modify_by"].ToString();
                        fb.user_id = dr["user_id"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.parent_id = dr["parent_id"].ToString();
                        fb.job_profile = dr["job_profile"].ToString();
                        fb.tier_id = dr["tier_id"].ToString();
                        fb.exp = dr["exp"].ToString();
                        fb.area_ids = dr["area_ids"].ToString();
                        fb.marketing_activies = dr["marketing_activity"].ToString();

                        fb.join_date = dr["join_date"].ToString();
                        fb.IsClose = dr["IsClose"].ToString() == "1" ? true : false;

                        if (dr["zone_id"] != null)
                        {
                            if (!string.IsNullOrEmpty(dr["zone_id"].ToString()))
                                fb.zone_id = dr["zone_id"].ToString();
                        }
                        if (dr["frn_type"] != null)
                        {
                            if (!string.IsNullOrEmpty(dr["frn_type"].ToString()))
                                fb.frn_type = dr["frn_type"].ToString();
                        }
                        if (dr["dist_id"] != null)
                        {
                            if (!string.IsNullOrEmpty(dr["dist_id"].ToString()))
                                fb.dist_id = dr["dist_id"].ToString();
                        }
                        fb.frn_type = dr["frn_type"].ToString();

                        if (dr["validateMonths"] != null)
                        {
                            if (!string.IsNullOrEmpty(dr["validateMonths"].ToString()))
                                fb.validateMonths = dr["validateMonths"].ToString();
                        }

                        if (dr["validateDate"] != null)
                        {
                            if (!string.IsNullOrEmpty(dr["validateDate"].ToString()))
                                fb.validateDate = dr["validateDate"].ToString();
                        }
                        if (dr["dropout_reason"] != null)
                        {
                            if (!string.IsNullOrEmpty(dr["dropout_reason"].ToString()))
                                fb.dropout_reason = dr["dropout_reason"].ToString();
                        }
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            if (list.Count > 0)
                return list[0];
            return new FranchiseBean();
        }


        public List<SelectBean> getMonths()
        {
            List<SelectBean> list = new List<SelectBean>();

            DLS db = new DLS(this.AppUserBean);

            #region Months

            SelectBean s = new SelectBean();
            s.id = "All";
            s.text = "--All--";
            list.Add(s);

            DataTable dt = new DataTable();
            dt.Columns.Add("month_no", typeof(string));
            dt.Columns.Add("month_name", typeof(string));

            DataRow dataRow1 = dt.NewRow();
            dataRow1["month_no"] = "1";
            dataRow1["month_name"] = "January";
            dt.Rows.Add(dataRow1);

            DataRow dataRow2 = dt.NewRow();
            dataRow2["month_no"] = "2";
            dataRow2["month_name"] = "February";
            dt.Rows.Add(dataRow2);

            DataRow dataRow3 = dt.NewRow();
            dataRow3["month_no"] = "3";
            dataRow3["month_name"] = "March";
            dt.Rows.Add(dataRow3);

            DataRow dataRow4 = dt.NewRow();
            dataRow4["month_no"] = "4";
            dataRow4["month_name"] = "April";
            dt.Rows.Add(dataRow4);

            DataRow dataRow5 = dt.NewRow();
            dataRow5["month_no"] = "5";
            dataRow5["month_name"] = "May";
            dt.Rows.Add(dataRow5);

            DataRow dataRow6 = dt.NewRow();
            dataRow6["month_no"] = "6";
            dataRow6["month_name"] = "June";
            dt.Rows.Add(dataRow6);

            DataRow dataRow7 = dt.NewRow();
            dataRow7["month_no"] = "7";
            dataRow7["month_name"] = "July";
            dt.Rows.Add(dataRow7);


            DataRow dataRow8 = dt.NewRow();
            dataRow8["month_no"] = "8";
            dataRow8["month_name"] = "August";
            dt.Rows.Add(dataRow8);

            DataRow dataRow9 = dt.NewRow();
            dataRow9["month_no"] = "9";
            dataRow9["month_name"] = "September";
            dt.Rows.Add(dataRow9);


            DataRow dataRow10 = dt.NewRow();
            dataRow10["month_no"] = "10";
            dataRow10["month_name"] = "October";
            dt.Rows.Add(dataRow10);


            DataRow dataRow11 = dt.NewRow();
            dataRow11["month_no"] = "11";
            dataRow11["month_name"] = "November";
            dt.Rows.Add(dataRow11);


            DataRow dataRow12 = dt.NewRow();
            dataRow12["month_no"] = "12";
            dataRow12["month_name"] = "December";
            dt.Rows.Add(dataRow12);

            #endregion

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

        public List<SelectBean> getYears()
        {
            List<SelectBean> list = new List<SelectBean>();
            DLS db = new DLS(this.AppUserBean);

            #region Year

            SelectBean s = new SelectBean();
            s.id = "All";
            s.text = "--All--";
            list.Add(s);

            DataTable dt = new DataTable();
            dt.Columns.Add("yearno", typeof(string));
            dt.Columns.Add("year", typeof(string));

            DataRow dataRow1 = dt.NewRow();
            dataRow1["yearno"] = "2009";
            dataRow1["year"] = "2009";
            dt.Rows.Add(dataRow1);

            DataRow dataRow2 = dt.NewRow();
            dataRow2["yearno"] = "2010";
            dataRow2["year"] = "2010";
            dt.Rows.Add(dataRow2);

            DataRow dataRow3 = dt.NewRow();
            dataRow3["yearno"] = "2011";
            dataRow3["year"] = "2011";
            dt.Rows.Add(dataRow3);

            DataRow dataRow4 = dt.NewRow();
            dataRow4["yearno"] = "2012";
            dataRow4["year"] = "2012";
            dt.Rows.Add(dataRow4);

            DataRow dataRow5 = dt.NewRow();
            dataRow5["yearno"] = "2013";
            dataRow5["year"] = "2013";
            dt.Rows.Add(dataRow5);

            DataRow dataRow6 = dt.NewRow();
            dataRow6["yearno"] = "2014";
            dataRow6["year"] = "2014";
            dt.Rows.Add(dataRow6);

            DataRow dataRow7 = dt.NewRow();
            dataRow7["yearno"] = "2015";
            dataRow7["year"] = "2015";
            dt.Rows.Add(dataRow7);


            DataRow dataRow8 = dt.NewRow();
            dataRow8["yearno"] = "2016";
            dataRow8["year"] = "2016";
            dt.Rows.Add(dataRow8);

            DataRow dataRow9 = dt.NewRow();
            dataRow9["yearno"] = "2017";
            dataRow9["year"] = "2017";
            dt.Rows.Add(dataRow9);


            DataRow dataRow10 = dt.NewRow();
            dataRow10["yearno"] = "2018";
            dataRow10["year"] = "2018";
            dt.Rows.Add(dataRow10);


            DataRow dataRow11 = dt.NewRow();
            dataRow11["yearno"] = "2019";
            dataRow11["year"] = "2019";
            dt.Rows.Add(dataRow11);


            DataRow dataRow12 = dt.NewRow();
            dataRow12["yearno"] = "2020";
            dataRow12["year"] = "2020";
            dt.Rows.Add(dataRow12);

            DataRow dataRow13 = dt.NewRow();
            dataRow13["yearno"] = "2021";
            dataRow13["year"] = "2021";
            dt.Rows.Add(dataRow13);

            #endregion

            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SelectBean s1 = new SelectBean();
                        s1.id = dr["yearno"].ToString();
                        s1.text = dr["year"].ToString();
                        list.Add(s1);
                    }
                }
            }
            db.Dispose();
            return list;
        }


        public List<FranchiseBean> GetRenealFranchiseDetail(string user_id, string month = null, string year = null, string country_id = null, string state_id = null)
        {
            DLS db = new DLS(this.AppUserBean);

            string frn_code = "";
            string frn_name = "";

            DataRow dataRow = db.GetSingleDataRow(" select frn_code,name from franchises f inner join users u on u.user_id =f.user_id  where u.user_id =" + this.AppUserBean.user_id);

            if (dataRow != null)
            {
                frn_code = dataRow["frn_code"].ToString();
                frn_name = dataRow["name"].ToString();
            }

            StringBuilder strQuery = new StringBuilder();

            strQuery.AppendLine(" select f.*,s.state_name from franchises f ");
            strQuery.AppendLine(" inner join states s on s.state_id = f.state_id ");
            strQuery.AppendLine(" where f.user_id in (" + user_id + ") and f.IsClose = 0 and f.is_active = 1");


            if (!string.IsNullOrEmpty(country_id))
            {
                strQuery.AppendLine(" and f.country_id =" + country_id);
            }


            if (!string.IsNullOrEmpty(year) || !string.IsNullOrEmpty(month))
            {
                if (!string.IsNullOrEmpty(year) && year.ToLower() != "all")
                {
                    strQuery.AppendLine(" and YEAR(f.validateDate)=" + year);
                }

                if (!string.IsNullOrEmpty(month) && month.ToLower() != "all")
                {
                    strQuery.AppendLine(" and MONTH(f.validateDate) =" + month);
                }
            }


            strQuery.AppendLine(" order by f.validateDate");

            List<FranchiseBean> list = new List<FranchiseBean>();
            DataTable dt = db.GetDataTable(strQuery.ToString());
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.name = dr["name"].ToString();
                        fb.user_id = dr["user_id"].ToString();
                        fb.state1 = dr["state_name"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.frn_master_code = frn_code;
                        fb.frn_master_name = frn_name;

                        if (dr["validateDate"] != null)
                        {
                            if (!string.IsNullOrEmpty(dr["validateDate"].ToString()))
                                fb.validateDate = Convert.ToDateTime(dr["validateDate"].ToString()).ToShortDateString();
                        }
                       
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
           
            return list;
        }


        public List<FranchiseBean> GetNotActiveFranchiseDetail(string user_id, string month = null, string year = null, string country_id = null, string state_id = null)
        {
            DLS db = new DLS(this.AppUserBean);

            string frn_code = "";
            string frn_name = "";

            DataRow dataRow = db.GetSingleDataRow(" select frn_code,name from franchises f inner join users u on u.user_id =f.user_id  where u.user_id =" + this.AppUserBean.user_id);

            if (dataRow != null)
            {
                frn_code = dataRow["frn_code"].ToString();
                frn_name = dataRow["name"].ToString();
            }

            StringBuilder strQuery = new StringBuilder();

            strQuery.AppendLine(" select f.*,s.state_name from franchises f ");
            strQuery.AppendLine(" inner join states s on s.state_id = f.state_id ");
            strQuery.AppendLine(" where f.user_id in (" + user_id + ") and f.IsClose = 1");


            if (!string.IsNullOrEmpty(country_id))
            {
                strQuery.AppendLine(" and f.country_id =" + country_id);
            }


            if (!string.IsNullOrEmpty(year) || !string.IsNullOrEmpty(month))
            {
                if (!string.IsNullOrEmpty(year) && year.ToLower() != "all")
                {
                    strQuery.AppendLine(" and YEAR(f.modify_date)=" + year);
                }

                if (!string.IsNullOrEmpty(month) && month.ToLower() != "all")
                {
                    strQuery.AppendLine(" and MONTH(f.modify_date) =" + month);
                }
            }


            strQuery.AppendLine(" order by f.modify_date");

            List<FranchiseBean> list = new List<FranchiseBean>();
            DataTable dt = db.GetDataTable(strQuery.ToString());


            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    DashboardAdapterAdmin fa = new DashboardAdapterAdmin(this.AppUserBean);

                    TeacherAdapter ta = new TeacherAdapter(this.AppUserBean);
                    
                    foreach (DataRow dr in dt.Rows)
                    {
                        FranchiseBean fb = new FranchiseBean();
                        fb.frn_id = dr["frn_id"].ToString();

                        DashboardAdminBean bean = new DashboardAdminBean();
                        bean = fa.GetDashboardInfo(fb.frn_id);

                        List<TeacherBean> teachers = ta.FillFranchTeachers(fb.frn_id);
                        fb.name = dr["name"].ToString();
                        fb.contact_person = dr["contact_person"].ToString();
                        fb.dropout_reason = dr["dropout_reason"].ToString();
                        fb.user_id = dr["user_id"].ToString();
                        fb.state1 = dr["state_name"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.frn_master_code = frn_code;
                        fb.frn_master_name = frn_name;
                        fb.total_students = bean.TotalStudent;
                        fb.total_instructors = (teachers.Count - 1).ToString();

                        if (dr["modify_date"] != null)
                        {
                            if (!string.IsNullOrEmpty(dr["modify_date"].ToString()))
                                fb.modify_date = Convert.ToDateTime(dr["modify_date"].ToString()).ToShortDateString();
                        }

                        list.Add(fb);
                    }
                }
            }
            db.Dispose();

            return list;
        }


    }
}
