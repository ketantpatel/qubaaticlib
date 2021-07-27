using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class AdmissionFormAdapter : MasterAdapter
    {
        public AdmissionFormAdapter(UserBean ubean)
        {
            this.AppUserBean = ubean;
        }
        public List<SelectBean> getMonths(string franchise_id)
        {
            List<SelectBean> list = new List<SelectBean>();
            SelectBean s = new SelectBean();
            s.id = "All";
            s.text = "--All--";
            list.Add(s);
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct monthname(form_date) as month_name,month(form_date) month_no from admission_forms where frn_id=" + franchise_id + " order by month(form_date)";
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
        public List<SelectBean> getMonths(string franchise_id, string course_id)
        {
            List<SelectBean> list = new List<SelectBean>();
            SelectBean s = new SelectBean();
            s.id = "All";
            s.text = "--All--";
            list.Add(s);
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct monthname(form_date) as month_name,month(form_date) month_no from admission_forms where frn_id=" + franchise_id + "  and course_id=" + course_id + " order by month(form_date)";
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
        public List<SelectBean> getYearsForFranchise(string state_id, string franchise_id)
        {
            List<SelectBean> list = new List<SelectBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct year(form_date) year from admission_forms where frn_id=" + franchise_id + " order by year(form_date)";
            if (franchise_id == "0")
            {
                query = "select distinct year(form_date) year from admission_forms where frn_id in (select frn_id from franchises where state1=" + state_id + ") order by year(form_date)";
            }
            SelectBean s = new SelectBean();
            s.id = "";
            s.text = "--Select--";
            list.Add(s);

            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        s = new SelectBean();
                        s.id = dr["year"].ToString();
                        s.text = dr["year"].ToString();
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
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct year(form_date) year from admission_forms where frn_id=" + franchise_id + " order by year(form_date)";
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

        public List<SelectBean> getYears(string franchise_id, string course_id)
        {
            List<SelectBean> list = new List<SelectBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct year(form_date) year from admission_forms where frn_id=" + franchise_id + " and course_id=" + course_id + " order by year(form_date)";
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

        public AdmissionFormBean SaveForm(AdmissionFormBean iBean)
        {
            if (iBean.form_id == "-1")
            {
                iBean = AddInquiry(iBean);
            }
            else
            {
                iBean = EditInquiry(iBean);
            }

            return iBean;
        }
        public bool isFormExist(string inquiry_id)
        {
            bool isFlag = false;
            DLS db = new DLS(this.AppUserBean);
            string existCount = db.GetSingleValue("select count(*) from admission_forms where inquiry_id=" + inquiry_id);
            if (int.Parse(existCount) > 0)
                isFlag = true;
            db.Dispose();
            return isFlag;
        }
        public bool DeleteCourse(string course_id)
        {
            DLS db = new DLS(this.AppUserBean);
            bool isFlag = db.Delete("courses", "course_id=" + course_id);
            db.Dispose();
            return isFlag;
        }
        public bool VerifyApplication(AdmissionFormBean iBean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(iBean.first_name))
                db.AddParameters("inquiry_id", iBean.inquiry_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(iBean.first_name))
                db.AddParameters("inquiry_id", iBean.inquiry_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(iBean.first_name))
                db.AddParameters("inquiry_id", iBean.inquiry_id, MyDBTypes.Int);
            iBean.is_opr_success = false;
            if (db.Update("admission_forms", "form_id=" + iBean.form_id))
            {
                iBean.is_opr_success = true;
            }
            db.Dispose();
            return iBean.is_opr_success;
        }
        public AdmissionFormBean AddInquiry(AdmissionFormBean iBean)
        {
            DLS db = new DLS(this.AppUserBean);

            if (!string.IsNullOrEmpty(iBean.inquiry_id))
                db.AddParameters("inquiry_id", iBean.inquiry_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.std_id))
                db.AddParameters("std_id", iBean.std_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.batch_id))
                db.AddParameters("batch_id", iBean.batch_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.first_name))
                db.AddParameters("first_name", iBean.first_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.middle_name))
                db.AddParameters("middle_name", iBean.middle_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.last_name))
                db.AddParameters("last_name", iBean.last_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.address))
                db.AddParameters("address", iBean.address, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.city))
                db.AddParameters("city", iBean.city, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.pincode))
                db.AddParameters("pincode", iBean.pincode, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.email))
                db.AddParameters("email", iBean.email, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.mobile))
                db.AddParameters("mobile", iBean.mobile, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.inquirer))
                db.AddParameters("inquirer", iBean.inquirer, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.course_id))
                db.AddParameters("course_id", iBean.course_id, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.form_date))
                db.AddParameters("form_date", iBean.form_date, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.remark))
                db.AddParameters("remark", iBean.remark, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.created_by))
                db.AddParameters("created_by", iBean.created_by, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.remark))
                db.AddParameters("create_date", iBean.create_date, MyDBTypes.DateTime);

            if (!string.IsNullOrEmpty(iBean.remark))
                db.AddParameters("modify_by", iBean.modify_by, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.remark))
                db.AddParameters("modify_date", iBean.modify_date, MyDBTypes.DateTime);

            if (!string.IsNullOrEmpty(iBean.dob))
                db.AddParameters("dob", iBean.dob, MyDBTypes.DateTime);

            if (!string.IsNullOrEmpty(iBean.res_no))
                db.AddParameters("res_no", iBean.res_no, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.discount))
                db.AddParameters("discount", iBean.discount, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.other_fees))
                db.AddParameters("other_fees", iBean.other_fees, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.fees))
                db.AddParameters("fees", iBean.fees, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.frn_id))
                db.AddParameters("frn_id", iBean.frn_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.gender))
                db.AddParameters("gender", iBean.gender, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.father_name))
                db.AddParameters("father_name", iBean.father_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(iBean.mother_name))
                db.AddParameters("mother_name", iBean.mother_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(iBean.sister_name))
                db.AddParameters("sister_name", iBean.sister_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(iBean.brother_name))
                db.AddParameters("brother_name", iBean.brother_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.stud_age))
                db.AddParameters("stud_age", iBean.stud_age, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(iBean.brother_age))
                db.AddParameters("brother_age", iBean.brother_age, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(iBean.sister_age))
                db.AddParameters("sister_age", iBean.sister_age, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.mother_tonge))
                db.AddParameters("mother_tonge", iBean.mother_tonge, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.office_address))
                db.AddParameters("office_address", iBean.office_address, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(iBean.office_mobile))
                db.AddParameters("office_mobile", iBean.office_mobile, MyDBTypes.Varchar);


            iBean.is_opr_success = false;
            if (db.Insert("admission_forms"))
            {
                iBean.is_opr_success = true;
                iBean.form_id = db.GetLastAutoID().ToString();
            }
            db.Dispose();
            return iBean;
        }
        public void SaveEnrollment1(AdmissionFormBean iBean, int year, bool isForceToUpdate)
        {
            DLS db = new DLS(this.AppUserBean);
            string entrollment_no = db.GetSingleValue("select enroll_no from admission_forms where form_id=" + iBean.form_id);
            string prefix = "";

            string count = "1";
            int serial = 0;
            if (string.IsNullOrEmpty(entrollment_no))
            {
                bool isNotExist = false;
                count = db.GetSingleValue("select start_from from enroll_setup where frn_id=" + iBean.frn_id + " and year=" + year + "");
                if (string.IsNullOrEmpty(count))
                {
                    isNotExist = true;
                    count = "0";
                }
                if (count == "0")
                {
                    serial++;
                }
                else
                {
                    serial = int.Parse(count) + 1;
                }
                if (serial.ToString().Length == 1)
                    prefix = "000";
                if (serial.ToString().Length == 2)
                    prefix = "00";
                if (serial.ToString().Length == 3)
                    prefix = "0";
                string enrollment = prefix + serial + "/" + this.AppUserBean.frn_code + "/" + year;
                //     string enrollment = frn_code + "/" + iBean.form_date.Substring(2, 2) + "/" + frn_code.Substring(4, 2) + "" + prefix + "" + count;
                //string enrollment = frn_code + "/" + iBean.form_date.Substring(2, 2) + "" + prefix + "" + count;
                iBean.enroll_no = enrollment;
                iBean.is_opr_success = false;
                db.AddParameters("enroll_no", iBean.enroll_no, MyDBTypes.Varchar);
                if (db.Update("admission_forms", "form_id=" + iBean.form_id))
                {
                    iBean.is_opr_success = true;
                    if (isNotExist)
                    {
                        db.AddParameters("frn_id", iBean.frn_id, MyDBTypes.Int);
                        db.AddParameters("year", year, MyDBTypes.Int);
                        db.AddParameters("start_from", serial, MyDBTypes.Int);
                        db.Insert("enroll_setup");
                    }
                    else
                    {
                        db.AddParameters("start_from", serial, MyDBTypes.Int);
                        db.Update("enroll_setup", "frn_id=" + iBean.frn_id);
                    }
                }
            }


            db.Dispose();
        }
        public void SaveEnrollment(AdmissionFormBean iBean, int year, bool isForceToUpdate, string frn_code)
        {
            DLS db = new DLS(this.AppUserBean);
            string entrollment_no = db.GetSingleValue("select enroll_no from admission_forms where form_id=" + iBean.form_id);
            string prefix = "";

            string count = "1";
            int serial = 0;
            if (string.IsNullOrEmpty(entrollment_no))
            {
                bool isNotExist = false;
                count = db.GetSingleValue("select start_from from enroll_setup where frn_id=" + iBean.frn_id + " and year=" + year + "");
                if (string.IsNullOrEmpty(count))
                {
                    isNotExist = true;
                    count = "0";
                }
                if (count == "0")
                {
                    serial++;
                }
                else
                {
                    serial = int.Parse(count) + 1;
                }
                if (serial.ToString().Length == 1)
                    prefix = "00";
                if (serial.ToString().Length == 2)
                    prefix = "0";

                //  string enrollment = prefix + serial + "/" + this.AppUserBean.frn_code + "/" + year;
                //string enrollment = frn_code + "/" + iBean.form_date.Substring(2, 2) + frn_code.Substring(frn_code.Length - 2, 2) + "" + prefix + "" + serial;
                string enrollment = iBean.form_date.Substring(2, 2) + "" + prefix + "" + serial;
                iBean.enroll_no = enrollment;
                iBean.is_opr_success = false;
                db.AddParameters("enroll_no", iBean.enroll_no, MyDBTypes.Varchar);
                if (db.Update("admission_forms", "form_id=" + iBean.form_id))
                {
                    iBean.is_opr_success = true;
                    if (isNotExist)
                    {
                        db.AddParameters("frn_id", iBean.frn_id, MyDBTypes.Int);
                        db.AddParameters("year", year, MyDBTypes.Int);
                        db.AddParameters("start_from", serial, MyDBTypes.Int);
                        db.Insert("enroll_setup");
                    }
                    else
                    {
                        db.AddParameters("start_from", serial, MyDBTypes.Int);
                        db.Update("enroll_setup", "frn_id=" + iBean.frn_id);
                    }
                }
            }


            db.Dispose();
        }
        public AdmissionFormBean EditInquiry(AdmissionFormBean iBean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(iBean.first_name))
                db.AddParameters("first_name", iBean.first_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.std_id))
                db.AddParameters("std_id", iBean.std_id, MyDBTypes.Int);


            if (!string.IsNullOrEmpty(iBean.batch_id))
                db.AddParameters("batch_id", iBean.batch_id, MyDBTypes.Int);


            if (!string.IsNullOrEmpty(iBean.middle_name))
                db.AddParameters("middle_name", iBean.middle_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.last_name))
                db.AddParameters("last_name", iBean.last_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.address))
                db.AddParameters("address", iBean.address, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.city))
                db.AddParameters("city", iBean.city, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.pincode))
                db.AddParameters("pincode", iBean.pincode, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.email))
                db.AddParameters("email", iBean.email, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.mobile))
                db.AddParameters("mobile", iBean.mobile, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.inquirer))
                db.AddParameters("inquirer", iBean.inquirer, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.course_id))
                db.AddParameters("course_id", iBean.course_id, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.form_date))
                db.AddParameters("form_date", iBean.form_date, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.remark))
                db.AddParameters("remark", iBean.remark, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.dob))
                db.AddParameters("dob", iBean.dob, MyDBTypes.DateTime);

            if (!string.IsNullOrEmpty(iBean.remark))
                db.AddParameters("modify_by", iBean.modify_by, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.remark))
                db.AddParameters("modify_date", iBean.modify_date, MyDBTypes.DateTime);

            if (!string.IsNullOrEmpty(iBean.other_fees))
                db.AddParameters("other_fees", iBean.other_fees, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.res_no))
                db.AddParameters("res_no", iBean.res_no, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.discount))
                db.AddParameters("discount", iBean.discount, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.fees))
                db.AddParameters("fees", iBean.fees, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.frn_id))
                db.AddParameters("frn_id", iBean.frn_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.gender))
                db.AddParameters("gender", iBean.gender, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.father_name))
                db.AddParameters("father_name", iBean.father_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(iBean.mother_name))
                db.AddParameters("mother_name", iBean.mother_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(iBean.sister_name))
                db.AddParameters("sister_name", iBean.sister_name, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(iBean.brother_name))
                db.AddParameters("brother_name", iBean.brother_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.stud_age))
                db.AddParameters("stud_age", iBean.stud_age, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(iBean.brother_age))
                db.AddParameters("brother_age", iBean.brother_age, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(iBean.sister_age))
                db.AddParameters("sister_age", iBean.sister_age, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.mother_tonge))
                db.AddParameters("mother_tonge", iBean.mother_tonge, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.office_address))
                db.AddParameters("office_address", iBean.office_address, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(iBean.office_mobile))
                db.AddParameters("office_mobile", iBean.office_mobile, MyDBTypes.Varchar);

            db.AddParameters("isdropout", iBean.isdropout, MyDBTypes.Int);
            db.AddParameters("dropoutreason", iBean.dropoutreason, MyDBTypes.Varchar);

            if (iBean.isdropout == 1)
            {
                db.AddParameters("dropoutdate", iBean.dropoutdate, MyDBTypes.DateTime);
            }

            iBean.is_opr_success = false;
            if (db.Update("admission_forms", "form_id=" + iBean.form_id))
            {
                iBean.is_opr_success = true;
            }
            db.Dispose();
            return iBean;
        }

        public List<AdmissionFormBean> ListForm(string month, string year, string frn_id, string status)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.code from admission_forms a left join courses c on c.course_id=a.course_id where a.is_active=1 and frn_id=" + frn_id + " and year(form_date)=" + year + " and is_verify=" + status + " order by form_id desc";
            if (month != "All")
                query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.code from admission_forms a left join courses c on c.course_id=a.course_id where a.is_active=1 and frn_id=" + frn_id + " and month(form_date)=" + month + " and year(form_date)=" + year + " and is_verify=" + status + " order by form_id desc";
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.gender = dr["gender"].ToString();
                        fb.course_code = dr["code"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.full_name = fb.first_name + " " + fb.last_name;
                        if (!string.IsNullOrEmpty(fb.middle_name))
                        {
                            if (fb.middle_name.Length > 0)
                            {
                                fb.full_name = fb.first_name + " " + fb.middle_name + " " + fb.last_name;
                            }
                        }
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<AdmissionFormBean> ListPaidStudents(bool monthWise, string year, string month, string frn_id)
        {
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = @"SELECT p.amount,p.fee_type_id,f.frn_code,a.form_id,first_name,middle_name,last_name,enroll_no,form_date,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,
DATE_FORMAT(payment_date, '%d/%m/%Y') receipt_date,p.pay_id FROM payments p inner join admission_forms a on p.form_id=a.form_id inner join franchises f on f.frn_id=a.frn_id  where p.is_cancel=0
 and year(p.payment_date)=" + year + " and month(p.payment_date)=" + month + " and (a.frn_id in (select f.frn_id from franchises f where f.parent_id=" + frn_id + ") or a.frn_id=" + frn_id + ")";
            //if (monthWise)
            //     query = "select form_id,f.frn_code,first_name,middle_name,last_name,enroll_no,form_date,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date from admission_forms a inner join franchises f on f.frn_id=a.frn_id where  year(form_date)=" + year + " and month(form_date)=" + month + " and batch_id in (select batch_id from batches where a.is_active=1 and frn_id in (select frn_id from franchises where parent_id=" + frn_id + ") or frn_id=" + frn_id + ")";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.form_date = dr["receipt_date"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.full_name = fb.first_name + " " + fb.last_name;
                        if (!string.IsNullOrEmpty(fb.middle_name))
                        {
                            if (fb.middle_name.Length > 0)
                            {
                                fb.full_name = fb.first_name + " " + fb.middle_name + " " + fb.last_name;
                            }
                        }
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<RoyaltiFormBean> ListPaidStudentOfFranchise(bool monthWise, string year, string month, string frn_id)
        {
            string query = @"SELECT f.tier_id,f.frn_id,f.name,f.frn_code,count(distinct form_id) nos,sum(amount) paid,sum(tax_amount) tax FROM franchises f
left join payments p on p.frn_id=f.frn_id and year(p.payment_date)=" + year + " and month(p.payment_date)=" + month + "  group by f.frn_id,f.name,f.frn_code";
            List<RoyaltiFormBean> list = new List<RoyaltiFormBean>();
            DLS db = new DLS(this.AppUserBean);
            //            string query = @"SELECT p.amount,p.fee_type_id,f.frn_code,a.form_id,first_name,middle_name,last_name,enroll_no,form_date,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,
            //DATE_FORMAT(payment_date, '%d/%m/%Y') receipt_date,p.pay_id FROM payments p inner join admission_forms a on p.form_id=a.form_id inner join franchises f on f.frn_id=a.frn_id  where p.is_cancel=0
            // and year(p.payment_date)=" + year + " and month(p.payment_date)=" + month + " and (a.frn_id in (select f.frn_id from franchises f where f.parent_id=" + frn_id + ") or a.frn_id=" + frn_id + ")";
            //if (monthWise)
            //     query = "select form_id,f.frn_code,first_name,middle_name,last_name,enroll_no,form_date,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date from admission_forms a inner join franchises f on f.frn_id=a.frn_id where  year(form_date)=" + year + " and month(form_date)=" + month + " and batch_id in (select batch_id from batches where a.is_active=1 and frn_id in (select frn_id from franchises where parent_id=" + frn_id + ") or frn_id=" + frn_id + ")";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        RoyaltiFormBean fb = new RoyaltiFormBean();
                        //  fb.form_id = dr["form_id"].ToString();
                        // fb.first_name = dr["first_name"].ToString();
                        //  fb.middle_name = dr["middle_name"].ToString();
                        //  fb.last_name = dr["last_name"].ToString();
                        //  fb.form_date = dr["receipt_date"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.frn_name = dr["name"].ToString();
                        //  fb.enroll_no = dr["enroll_no"].ToString();
                        //  fb.full_name = fb.first_name + " " + fb.last_name;
                        fb.fee_amount = dr["paid"].ToString();
                        fb.tier_id = dr["tier_id"].ToString();
                        fb.tax_amount = dr["tax"].ToString();
                        fb.no_of_students = dr["nos"].ToString();
                        fb.frn_id = dr["frn_id"].ToString();
                        if (!string.IsNullOrEmpty(fb.middle_name))
                        {
                            if (fb.middle_name.Length > 0)
                            {
                                fb.full_name = fb.first_name + " " + fb.middle_name + " " + fb.last_name;
                            }
                        }
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<RoyaltiFormBean> ListPaidStudentForRoyalti(string year, string month, string frn_id, string royalti_field)
        {
            List<RoyaltiFormBean> list = new List<RoyaltiFormBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = @"select f.frn_code,a.form_id,a.first_name,a.last_name,a.middle_name,a.enroll_no,DATE_FORMAT(a.form_date, '%d/%m/%Y') f_form_date,DATE_FORMAT(p.payment_date, '%d/%m/%Y') payment_date,
p.amount,p.discount,p.pay_id,p.fee_type_id,fm.fee_type,fm.fee_type_mst_id,ft.royalti_state,ft.royalti_country,ft.royalti_company,p.tax_amount,p.fee_receivable,(p.fee_type_amount-p.discount) fee_type_amount
 from payments p inner join admission_forms a on a.form_id=p.form_id
inner join fee_types ft on ft.fee_type_id=p.fee_type_id
inner join fee_type_master fm on fm.fee_type_mst_id=ft.fee_type_mst_id inner join franchises f on f.frn_id=p.frn_id
  where month(p.payment_date)=" + month + " and year(p.payment_date)=" + year + " and f.frn_id=" + frn_id + "";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    float paidAmount = 0;
                    float totalAmount = 0;
                    foreach (DataRow dr in dt.Rows)
                    {
                        paidAmount = 0;
                        totalAmount = 0;
                        RoyaltiFormBean fb = new RoyaltiFormBean();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.tax_amount = dr["tax_amount"].ToString();
                        fb.fee_type = dr["fee_type"].ToString();
                        fb.fee_amount = dr["amount"].ToString();
                        fb.receipt_id = dr["pay_id"].ToString();
                        fb.fee_receivable = dr["fee_receivable"].ToString();
                        fb.fee_type_amount = dr["fee_type_amount"].ToString();
                        fb.full_name = fb.first_name + " " + fb.last_name;
                        fb.royalti = dr[royalti_field].ToString();
                        fb.payment_date = dr["payment_date"].ToString();
                        float.TryParse(fb.fee_amount, out paidAmount);
                        float.TryParse(fb.fee_receivable, out totalAmount);
                        fb.fee_pending = (totalAmount - paidAmount).ToString();
                        if (!string.IsNullOrEmpty(fb.middle_name))
                        {
                            if (fb.middle_name.Length > 0)
                            {
                                fb.full_name = fb.first_name + " " + fb.middle_name + " " + fb.last_name;
                            }
                        }
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<AdmissionFormBean> ListActiveStudents(bool monthWise, string year, string month, string frn_id)
        {
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select a.form_id,first_name,middle_name,last_name,enroll_no,form_date,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date from admission_forms a inner join payments p on p.form_id=a.form_id and month(p.payment_date)=6 and year(p.payment_date)=2020 and p.is_Cancel=0 and batch_id in (select batch_id from batches where is_active=1 and frn_id=" + frn_id + ")";
            if (monthWise)
                query = "select form_id,f.frn_code,first_name,middle_name,last_name,enroll_no,form_date,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date from admission_forms a inner join franchises f on f.frn_id=a.frn_id where  year(form_date)=" + year + " and month(form_date)=" + month + " and batch_id in (select batch_id from batches where a.is_active=1 and frn_id in (select frn_id from franchises where parent_id=" + frn_id + ") or frn_id=" + frn_id + ")";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.frn_code = dr["frn_code"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.full_name = fb.first_name + " " + fb.last_name;
                        if (!string.IsNullOrEmpty(fb.middle_name))
                        {
                            if (fb.middle_name.Length > 0)
                            {
                                fb.full_name = fb.first_name + " " + fb.middle_name + " " + fb.last_name;
                            }
                        }
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<AdmissionFormBean> ListForm(BatchBean batch, string frn_id, string status)
        {
            DLS db = new DLS(this.AppUserBean);
            DataRow drBatch = db.GetSingleDataRow("select start_date , end_date from batches where batch_id=" + batch.batch_id);
            if (drBatch == null)
            {
                db.Dispose();
                return null;
            }
            string query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.code from admission_forms a left join courses c on c.course_id=a.course_id where a.is_active=1 and frn_id=" + batch.frn_id + " and a.batch_id=" + batch.batch_id + " and is_verify=" + status + " order by form_id desc";
            //if (status != "All")
            //    query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.code from admission_forms a left join courses c on c.course_id=a.course_id where a.is_active=1 and frn_id=" + frn_id + " and month(form_date) between '" + batch.db_start_date + "' and '" + batch.db_end_date + "' and is_verify=" + status + " order by form_id desc";
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.gender = dr["gender"].ToString();
                        fb.course_code = dr["code"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.full_name = fb.first_name + " " + fb.last_name;
                        if (!string.IsNullOrEmpty(fb.middle_name))
                        {
                            if (fb.middle_name.Length > 0)
                            {
                                fb.full_name = fb.first_name + " " + fb.middle_name + " " + fb.last_name;
                            }
                        }
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }

        public List<AdmissionFormBean> ListAdmissionForm(BatchBean batch, string frn_id, string status)
        {
            DLS db = new DLS(this.AppUserBean);
            //DataRow drBatch = db.GetSingleDataRow("select start_date , end_date from batches where batch_id=" + batch.batch_id);
            //if (drBatch == null)
            // {
            //   db.Dispose();
            // return null;
            //}
            string query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.code from admission_forms a left join courses c on c.course_id=a.course_id where a.is_active=1 and frn_id=" + frn_id + " and a.batch_id=" + batch.batch_id + " order by form_id desc";
            if (status != "All")
                query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.code from admission_forms a left join courses c on c.course_id=a.course_id where a.is_active=1 and frn_id=" + frn_id + " and  is_verify=" + status + " and a.batch_id=" + batch.batch_id + " order by form_id desc";
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.gender = dr["gender"].ToString();
                        fb.course_code = dr["code"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.full_name = fb.first_name + " " + fb.last_name;
                        if (!string.IsNullOrEmpty(fb.middle_name))
                        {
                            if (fb.middle_name.Length > 0)
                            {
                                fb.full_name = fb.first_name + " " + fb.middle_name + " " + fb.last_name;
                            }
                        }
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<AdmissionFormBean> ListFranchBatchAdmissionForm(BatchBean batch, string status)
        {
            DLS db = new DLS(this.AppUserBean);
            //DataRow drBatch = db.GetSingleDataRow("select start_date , end_date from batches where batch_id=" + batch.batch_id);
            //if (drBatch == null)
            // {
            //   db.Dispose();
            // return null;
            //}
            string query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.code from admission_forms a left join courses c on c.course_id=a.course_id where a.is_active=1 and frn_id=" + batch.frn_id + " and a.batch_id=" + batch.batch_id + " and a.is_verify=" + status + " order by form_id desc";
            if (status != "All")
                query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.code from admission_forms a left join courses c on c.course_id=a.course_id where a.is_active=1 and frn_id=" + batch.frn_id + " and  is_verify=" + status + " and a.batch_id=" + batch.batch_id + " order by form_id desc";
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.gender = dr["gender"].ToString();
                        fb.course_code = dr["code"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.full_name = fb.first_name + " " + fb.last_name;
                        if (!string.IsNullOrEmpty(fb.middle_name))
                        {
                            if (fb.middle_name.Length > 0)
                            {
                                fb.full_name = fb.first_name + " " + fb.middle_name + " " + fb.last_name;
                            }
                        }
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }

        public List<AdmissionFormBean> ListFormTrash(string frn_id, string status)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.code from admission_forms a left join courses c on c.course_id=a.course_id where a.is_active=" + status + " and frn_id=" + frn_id + "  order by form_id desc";
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.form_date = dr["form_date"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.gender = dr["gender"].ToString();
                        fb.course_code = dr["code"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.full_name = fb.first_name + " " + fb.last_name;
                        if (!string.IsNullOrEmpty(fb.middle_name))
                        {
                            if (fb.middle_name.Length > 0)
                            {
                                fb.full_name = fb.first_name + " " + fb.middle_name + " " + fb.last_name;
                            }
                        }
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public bool ActivateOnlineUser(List<AdmissionFormBean> forms)
        {
            if (forms.Count > 0)
            {
                DLS db = new DLS(this.AppUserBean);
                foreach (AdmissionFormBean form in forms)
                {
                    if (form.status == "1" || form.status == "0")
                    {
                        db.AddParameters("password", clsFunctions.CreateRandomPassword(3), MyDBTypes.Varchar);
                        db.AddParameters("status", form.status, MyDBTypes.Int);
                        db.Update("admission_forms", "form_id=" + form.form_id);
                    }
                }
                db.Dispose();
            }
            return true;
        }
        public List<AdmissionFormBean> ListQubaaticForm(string state, string year, string frn_id, string status)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date from admission_forms where frn_id=" + frn_id + " and year(form_date)=" + year + "  order by form_id desc";
            if (frn_id == "0")
                query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date from admission_forms where year(form_date)=" + year + " and frn_id in (select frn_id from franchises where state1=" + state + ") and year(form_date)=" + year + " order by form_id desc";
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.gender = dr["gender"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<AdmissionFormBean> ListForm(string month, string year, string frn_id, string status, string course_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date from admission_forms where frn_id=" + frn_id + " and is_verify=" + status + " and course_id=" + course_id + " order by form_id desc";
            if (month != "All")
                query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date from admission_forms where frn_id=" + frn_id + " and month(form_date)=" + month + " and year(form_date)=" + year + " and course_id=" + course_id + " and is_verify=" + status + " order by form_id desc";
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.gender = dr["gender"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<AdmissionFormBean> ListStudent(string month, string year, string frn_id, string status)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name from admission_forms f left join courses c on c.course_id=f.course_id where frn_id=" + frn_id + " and is_verify=" + status + " order by form_date desc";
            if (month != "All")
                query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name  from admission_forms f left join courses c on c.course_id=f.course_id  where frn_id=" + frn_id + " and month(form_date)=" + month + " and year(form_date)=" + year + " and is_verify=" + status;
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.gender = dr["gender"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }



        public List<AdmissionFormBean> ListBatchStudent(string batch_id, string frn_id, string status)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name from admission_forms f left join courses c on c.course_id=f.course_id where frn_id=" + frn_id + " and f.batch_id=" + batch_id + " and is_verify=" + status;
            // if (month != "All")
            //   query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name  from admission_forms f left join courses c on c.course_id=f.course_id  where frn_id=" + frn_id + " and f.batch_id="+batch_id+" and is_verify=" + status;
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.gender = dr["gender"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<ExamMarkRowBean> ListCourseStudent(string month, string year, string course_id, string frn_id, string status, string parent_course_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select form_id,inquiry_id,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,first_name,last_name,middle_name,enroll_no," +
" (select round(percentage,2) from gradesheet g where g.form_id=a.form_id and g.course_id=" + course_id + " and g.parent_course_id=" + parent_course_id + " ) percentage " +
 " from admission_forms a where course_id=" + parent_course_id + " and frn_id=" + frn_id + "";
            if (month != "All")
            {
                //query = "select form_id,inquiry_id,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,first_name,last_name,middle_name,enroll_no,(select round(percentage,2) from gradesheet g where g.form_id=a.form_id) percentage from admission_forms a where course_id=4 and frn_id=6 and month(a.form_date)=" + month + " and year(a.form_date)=" + year + " and is_verify=" + status + " ";
                //query = "select form_id,inquiry_id,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,first_name,last_name,middle_name,enroll_no,(select round(percentage,2) from gradesheet g where g.form_id=a.form_id  and g.course_id=a.course_id ) percentage from admission_forms a where course_id=" + course_id + " and frn_id=" + frn_id + " and month(a.form_date)=" + month + " and year(a.form_date)=" + year + " and is_verify=" + status + " ";
                query = "select form_id,inquiry_id,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,first_name,last_name,middle_name,enroll_no," +
" (select round(percentage,2) from gradesheet g where g.form_id=a.form_id and g.course_id=" + course_id + " and g.parent_course_id=" + parent_course_id + " ) percentage " +
 " from admission_forms a where course_id=" + parent_course_id + " and frn_id=" + frn_id + "  and month(a.form_date)=" + month + " and year(a.form_date)=" + year + " and is_verify=" + status + "";
            }

            List<ExamMarkRowBean> list = new List<ExamMarkRowBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ExamMarkRowBean fb = new ExamMarkRowBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();

                        fb.form_date = dr["f_form_date"].ToString();

                        // fb.course_name = dr["course_name"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.marks = dr["percentage"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<ExamMarkRowBean> ListCourseStudent(string month, string year, string course_id, string frn_id, string status, string subject_id, string exam_type)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select f.*,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name,m.marks from admission_forms f left join courses c on c.course_id=f.course_id left join exam_marks m on m.course_id=c.course_id and m.stud_id=f.form_id and m.subject_id=" + subject_id + " and m.exam_type=" + exam_type + " where f.frn_id=" + frn_id + " and is_verify=" + status + " and c.course_id=" + course_id + "";
            if (month != "All")
                query = "select f.*,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name,m.marks  from admission_forms f left join courses c on c.course_id=f.course_id left join exam_marks m on m.course_id=c.course_id and m.stud_id=f.form_id  and m.subject_id=" + subject_id + "  and m.exam_type=" + exam_type + "   where f.frn_id=" + frn_id + " and month(f.form_date)=" + month + " and year(f.form_date)=" + year + " and is_verify=" + status + "  and c.course_id=" + course_id + "";
            List<ExamMarkRowBean> list = new List<ExamMarkRowBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ExamMarkRowBean fb = new ExamMarkRowBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();

                        fb.form_date = dr["f_form_date"].ToString();

                        fb.course_name = dr["course_name"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.marks = dr["marks"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<ExamMarkRowBean> ListCourseStudentMarks(string month, string year, string course_id, string frn_id, string status, string subject_id, bool isSum, string parent_course_id)
        {

            DLS db = new DLS(this.AppUserBean);
            string query = "";
            string markStr = "marks";
            if (isSum)
            {
                markStr = "sum(marks)";
                query = @"select f.*,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name, 
(select " + markStr + " from exam_marks m where m.stud_id=f.form_id and m.course_id=" + course_id + "  and m.exam_type=1) theory_marks," +
   @"(select " + markStr + " from exam_marks m where m.stud_id=f.form_id and  m.course_id=" + course_id + "  and m.exam_type=2) practical_marks " +
   @"from admission_forms f
left join courses c on c.course_id=f.course_id
where f.frn_id=" + frn_id + " and is_verify=" + status + " and c.course_id=" + parent_course_id + "";

                if (month != "All")
                {
                    query = @"select f.*,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name, 
(select " + markStr + " from exam_marks m where m.stud_id=f.form_id and  m.course_id=" + course_id + "  and m.exam_type=1 and m.parent_course_id=" + parent_course_id + ") theory_marks," +
    @"(select " + markStr + " from exam_marks m where m.stud_id=f.form_id  and m.course_id=" + course_id + "  and m.exam_type=2 and m.parent_course_id=" + parent_course_id + ") practical_marks " +
    @"from admission_forms f
left join courses c on c.course_id=f.course_id
where f.frn_id=" + frn_id + " and month(f.form_date)=" + month + " and year(f.form_date)=" + year + " and is_verify=" + status + " and c.course_id=" + parent_course_id + "";
                }
            }
            else
            {
                query = @"select f.*,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name, 
(select " + markStr + " from exam_marks m where m.stud_id=f.form_id and m.exam_type=1 and m.course_id=" + course_id + " and m.subject_id=" + subject_id + " and m.parent_course_id=" + parent_course_id + ") theory_marks," +
   @"(select " + markStr + " from exam_marks m where m.stud_id=f.form_id and m.exam_type=2 and m.course_id=" + course_id + " and and m.subject_id=" + subject_id + " and m.parent_course_id=" + parent_course_id + ") practical_marks " +
   @"from admission_forms f
left join courses c on c.course_id=f.course_id
where f.frn_id=" + frn_id + " and is_verify=" + status + " and c.course_id=" + parent_course_id + "";

                if (month != "All")
                {
                    query = @"select f.*,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name, 
(select " + markStr + " from exam_marks m where m.stud_id=f.form_id and m.exam_type=1 and m.course_id=" + course_id + " and m.subject_id=" + subject_id + "  and m.parent_course_id=" + parent_course_id + ") theory_marks," +
    @"(select " + markStr + " from exam_marks m where m.stud_id=f.form_id and m.exam_type=2 and m.course_id=" + course_id + " and m.subject_id=" + subject_id + "  and m.parent_course_id=" + parent_course_id + ") practical_marks " +
    @"from admission_forms f
left join courses c on c.course_id=f.course_id
where f.frn_id=" + frn_id + " and month(f.form_date)=" + month + " and year(f.form_date)=" + year + " and is_verify=" + status + " and c.course_id=" + parent_course_id + "";
                }
            }

            List<ExamMarkRowBean> list = new List<ExamMarkRowBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ExamMarkRowBean fb = new ExamMarkRowBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();

                        fb.form_date = dr["f_form_date"].ToString();

                        fb.course_name = dr["course_name"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.theory_marks = dr["theory_marks"].ToString();
                        fb.practical_marks = dr["practical_marks"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<AdmissionFormBean> TopListStudent(string frn_id, int top)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select *,DATE_FORMAT(form_date, '%d/%m/%Y') f_form_date,c.course_name course_name from admission_forms f left join courses c on c.course_id=f.course_id where frn_id=" + frn_id + " order by form_id desc limit 0," + top + "";
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.form_date = dr["f_form_date"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.course_name = dr["course_name"].ToString();
                        fb.gender = dr["gender"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public AdmissionFormBean GetFormDetail(string fid)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select a.*,c.course_name,c.code from admission_forms a left join courses c on c.course_id=a.course_id where form_id=" + fid;
            List<AdmissionFormBean> list = new List<AdmissionFormBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        AdmissionFormBean fb = new AdmissionFormBean();
                        fb.course_id = dr["course_id"].ToString();
                        fb.dropoutreason = dr["dropoutreason"].ToString();
                        fb.isdropout = dr["isdropout"] == null ? 0: 1;
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.form_id = dr["form_id"].ToString();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.inquiry_id = dr["inquiry_id"].ToString();
                        fb.address = dr["address"].ToString();
                        fb.city = dr["city"].ToString();
                        fb.pincode = dr["pincode"].ToString();
                        fb.email = dr["email"].ToString();
                        fb.mobile = dr["mobile"].ToString();
                        fb.inquirer = dr["inquirer"].ToString();
                        fb.course_id = dr["course_id"].ToString();
                        fb.form_date = dr["form_date"].ToString();
                        fb.remark = dr["remark"].ToString();
                        fb.create_date = dr["create_date"].ToString();
                        fb.res_no = dr["res_no"].ToString();
                        fb.modify_date = dr["modify_date"].ToString();
                        fb.modify_by = dr["modify_by"].ToString();
                        fb.created_by = dr["created_by"].ToString();
                        fb.gender = dr["gender"].ToString();
                        fb.fees = dr["fees"].ToString();
                        fb.discount = dr["discount"].ToString();
                        fb.is_verify = dr["is_verify"].ToString();
                        fb.enroll_no = dr["enroll_no"].ToString();
                        fb.other_fees = dr["other_fees"].ToString();
                        fb.dob = dr["dob"].ToString();
                        fb.std_id = dr["std_id"].ToString();
                        fb.course_code = dr["code"].ToString();
                        fb.course_name = dr["course_name"].ToString();

                        fb.father_name = dr["father_name"].ToString();
                        fb.mother_name = dr["mother_name"].ToString();
                        fb.brother_name = dr["brother_name"].ToString();
                        fb.sister_name = dr["sister_name"].ToString();

                        fb.sister_age = dr["sister_age"].ToString();
                        fb.brother_age = dr["brother_age"].ToString();
                        fb.stud_age = dr["stud_age"].ToString();

                        fb.mother_tonge = dr["mother_tonge"].ToString();

                        fb.office_address = dr["office_address"].ToString();
                        fb.office_mobile = dr["office_mobile"].ToString();


                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            if (list.Count > 0)
                return list[0];
            return new AdmissionFormBean();
        }


        public AdmissionFormBean GetFromList(string user_id)
        {
            DLS db = new DLS(this.AppUserBean);

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select a.first_name as fn,a.middle_name as mn,a.last_name as ln,a.enroll_no,a.create_date,f.frn_code,f.name as ffn,b.name as bn,tc.first_name tcfn,tc.last_name tcln from admission_forms a ");
            strQuery.AppendLine(" inner join batches b on b.batch_id =a.batch_id ");
            strQuery.AppendLine(" inner join franchises f on b.frn_id = f.frn_id ");
            strQuery.AppendLine(" inner join teacher_franchise_allocation t on t.frn_id = f.frn_id ");
            strQuery.AppendLine(" inner join teachers tc on tc.teach_id = t.teach_id ");
            strQuery.AppendLine(" inner join users u on u.user_id = f.user_id ");
            strQuery.AppendLine(" where f.user_id = " + user_id);
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
            if (list.Count > 0)
                return list[0];
            return new AdmissionFormBean();
        }
    }
}
