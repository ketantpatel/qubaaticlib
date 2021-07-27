using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class PaymentAdapter : MasterAdapter
    {
        public PaymentAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }
       
        public PaymentBean AddPayment(PaymentBean s)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(s.amount))
                db.AddParameters("amount", s.amount, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.crdr))
                db.AddParameters("crdr", s.crdr, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(s.create_date))
                db.AddParameters("create_date", s.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(s.created_by))
                db.AddParameters("created_by", s.created_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.payment_date))
                db.AddParameters("payment_date", s.payment_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(s.form_id))
                db.AddParameters("form_id", s.form_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.frn_id))
                db.AddParameters("frn_id", s.frn_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.payment_type))
                db.AddParameters("payment_type", s.payment_type, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(s.fee_type_id))
                db.AddParameters("fee_type_id", s.fee_type_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.discount))
                db.AddParameters("discount", s.discount, MyDBTypes.Int);
            else
                db.AddParameters("discount", "0", MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.tax_amount))
                db.AddParameters("tax_amount", s.tax_amount, MyDBTypes.Int);
            else
                db.AddParameters("tax_amount", "0", MyDBTypes.Int);
            db.AddParameters("fee_type_amount", s.fee_type_amount, MyDBTypes.Int);
            db.AddParameters("fee_receivable", s.fee_receivable, MyDBTypes.Int);
            db.AddParameters("txn_id", DateTime.Now.Ticks.ToString(), MyDBTypes.Varchar);
            s.is_opr_success = false;
            if (db.Insert("payments"))
            {
                s.is_opr_success = true;
                s.pay_id = db.GetLastAutoID().ToString();
            }
            db.Dispose();

            return s;
        }
        public PaymentBean EditPayment(PaymentBean s)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(s.amount))
                db.AddParameters("amount", s.amount, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.crdr))
                db.AddParameters("crdr", s.crdr, MyDBTypes.Varchar);
            if (!string.IsNullOrEmpty(s.create_date))
                db.AddParameters("create_date", s.create_date, MyDBTypes.DateTime);
            if (!string.IsNullOrEmpty(s.created_by))
                db.AddParameters("created_by", s.created_by, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.form_id))
                db.AddParameters("form_id", s.form_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.frn_id))
                db.AddParameters("frn_id", s.frn_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.fee_type_id))
                db.AddParameters("fee_type_id", s.fee_type_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(s.payment_type))
                db.AddParameters("payment_type", s.payment_type, MyDBTypes.Varchar);
            s.is_opr_success = false;
            if (db.Update("payments","pay_id="+s.pay_id))
            {
                s.is_opr_success = true;
                s.pay_id = db.GetLastAutoID().ToString();
            }
            db.Dispose();

            return s;
        }
        public float PaidAmount(string form_id,string fee_type_id)
        {
            float flag = 0.00f;
            DLS db = new DLS(this.AppUserBean);
            string paid = db.GetSingleValue("select sum(amount) from payments where fee_type_id=" + fee_type_id + " and form_id=" + form_id);
            db.Dispose();
            float.TryParse(paid, out flag);
            return flag;
        }
        public bool DeletePayment(string pay_id)
        {
            bool flag = false;
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("is_cancel", "1", MyDBTypes.Int);
            db.Update("payments", "pay_id=" + pay_id + "");
            db.Dispose();
            return flag;
        }

        public List<PaymentBean> PaymentList(string form_id)
        {
            string query = "select p.*,ft.fee_type from payments p inner join fee_types ft on ft.fee_type_id=p.fee_type_id where form_id=" + form_id;
            List<PaymentBean> list = new List<PaymentBean>();
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PaymentBean s = new PaymentBean();
                    s.amount = dr["amount"].ToString();
                    s.pay_id = dr["pay_id"].ToString();
                    s.is_cancel = dr["is_cancel"].ToString();
                    s.fee_type_title = dr["fee_type"].ToString();
                    s.payment_date = dr["payment_date"].ToString();
                    list.Add(s);
                }
            }
            db.Dispose();
            return list;
        }
        public List<PaymentBean> PaymentListbyType(string form_id,string fee_type_id)
        {
            string query = "select p.*,ft.fee_type from payments p inner join fee_types ft on ft.fee_type_id=p.fee_type_id where p.fee_type_id="+fee_type_id+" and form_id=" + form_id;
            List<PaymentBean> list = new List<PaymentBean>();
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PaymentBean s = new PaymentBean();
                    s.amount = dr["amount"].ToString();
                    s.pay_id = dr["pay_id"].ToString();
                    s.fee_type_title = dr["fee_type"].ToString();
                    s.payment_date = dr["payment_date"].ToString();
                    list.Add(s);
                }
            }
            db.Dispose();
            return list;
        }

        public PaymentBean GetPayment(string pay_id)
        {
            string query = "select * from payments where pay_id=" + pay_id;
            List<PaymentBean> list = new List<PaymentBean>();
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    PaymentBean s = new PaymentBean();
                    s.amount = dr["amount"].ToString();
                    s.pay_id = dr["pay_id"].ToString();
                    s.txn_id = dr["txn_id"].ToString();
                    s.payment_date = DateTime.Parse(dr["payment_date"].ToString()).ToString("dd/MM/yyyy");
                    list.Add(s);
                }
            }
            db.Dispose();
            if (list.Count == 0)
                new PaymentBean();
            return list[0];
        }
        public DataTable GetFranchiseStudentPayment(string frn_id,string year,string month)
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(@"select n.form_id,f.first_name,f.last_name,f.middle_name,n.paid,ftf.amount,ftf.tax,ft.fee_type,ft.fee_type_id,frn.tier_id from (
select m.form_id,p.fee_type_id,sum(ifnull(p.amount,0)) paid,m.frn_id from (
select a.form_id,a.frn_id from admission_forms a inner join payments p on p.form_id=a.form_id
where year(p.payment_date)="+year+" and month(p.payment_date)="+month+" and p.is_cancel=0 and batch_id in (select batch_id from batches where is_active=1 and frn_id in (select frn_id from franchises where parent_id=" + frn_id + ") or frn_id=" + frn_id + ")" +
@") as m
left join payments p on p.form_id=m.form_id group by m.form_id,p.fee_type_id,m.frn_id)
as n inner join admission_forms f on f.form_id=n.form_id left join fee_types ft on ft.fee_type_id=n.fee_type_id
left join fee_type_fee ftf on ftf.fee_type_id=n.fee_type_id and ftf.is_Active=1 and ftf.frn_id=n.frn_id inner join franchises frn on frn.frn_id=n.frn_id where n.frn_id in (select frn_id from franchises where parent_id=" + frn_id + ") or n.frn_id=" + frn_id + "");
            db.Dispose();
            return dt;
        }
        public DataTable GetAllFranchiseStudentPayment(string frn_id, string year, string month)
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(@"select frn_id,count(distinct form_id) nos,p.fee_type_id,ftm.fee_type_mst_id,sum(amount) paid,sum(tax_amount) tax from payments p
inner join fee_types ft on ft.fee_type_id=p.fee_type_id inner join fee_type_master ftm on ftm.fee_type_mst_id=ft.fee_type_mst_id
 where year(p.payment_date)=" + year + " and month(p.payment_date)=" + month + " and p.frn_id in (select frn_id from franchises f where f.parent_id=" + frn_id + " or p.frn_id=" + frn_id + ") group by frn_id,p.fee_type_id,ftm.fee_type_mst_id");
            db.Dispose();
            return dt;
        }
        public DataTable GetFeeTypeAmount()
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(@"select * From fee_types");
            db.Dispose();
            return dt;
        }

        public List<SelectBean> getMonths()
        {
            List<SelectBean> list = new List<SelectBean>();
            SelectBean s = new SelectBean();
            s.id = "All";
            s.text = "--All--";
            list.Add(s);
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct monthname(create_date) as month_name,month(create_date) month_no from payments order by month(create_date)";
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

        public string getStateName(string state_id) {
            DLS db = new DLS(this.AppUserBean);

            string state = db.GetSingleValue(" select state_name from states where state_id =" + state_id);
            db.Dispose();
            return state;
        }

        public List<SelectBean> getYears()
        {
            List<SelectBean> list = new List<SelectBean>();
            DLS db = new DLS(this.AppUserBean);
            SelectBean s1 = new SelectBean();
            s1.id = "All";
            s1.text = "--All--";
            list.Add(s1);
            string query = "select distinct year(create_date) year from payments order by year(create_date)";
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

        public List<RoyaltyReportBean> GetRoyaltyList(string frn_id, string month = null, string year = null, string country_id = null, string state_id = null)
        {
            DLS db = new DLS(this.AppUserBean);

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select distinct pay_id,payment_date,a.first_name,a.middle_name,a.last_name,a.enroll_no,p.amount,c.course_name,p.payment_type,p.tax_amount,r.amount as royalti,fr.name,fr.frn_code from payments p ");
            strQuery.AppendLine(" inner join franchises fr on fr.frn_id = p.frn_id ");
            strQuery.AppendLine(" left join admission_forms a on a.form_id = p.form_id ");
            strQuery.AppendLine(" left join courses c on c.course_id = a.course_id ");
            strQuery.AppendLine(" left join fee_type_fee f on f.fee_type_id = p.fee_type_id ");
            strQuery.AppendLine(" left join fee_royalti r on r.fee_type_id = p.fee_type_id ");
            strQuery.AppendLine(" where p.frn_id in ( " + frn_id + ")");

            string state = string.Empty;
            if (!string.IsNullOrEmpty(year) || !string.IsNullOrEmpty(month))
            {
                if (!string.IsNullOrEmpty(year) && year.ToLower() != "all")
                {
                    strQuery.AppendLine(" and YEAR(p.create_date)=" + year);
                }

                if (!string.IsNullOrEmpty(month) && month.ToLower() != "all")
                {
                    strQuery.AppendLine(" and MONTH(p.create_date) =" + month);
                    if (!string.IsNullOrEmpty(country_id))
                    {
                        state = db.GetSingleValue(" select state_name from states where state_id =" + state_id);
                    }
                    else
                    {
                        state = db.GetSingleValue(" select state_name from states where state_id =" + state_id + " and country_id = " + country_id);
                    }
                }
            }

            strQuery.AppendLine(" order by p.create_date");

            string query = strQuery.ToString();
            List<RoyaltyReportBean> list = new List<RoyaltyReportBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        RoyaltyReportBean reportBean = new RoyaltyReportBean();
                        reportBean.frn_code = dr["frn_code"].ToString();
                        reportBean.frn_name = dr["name"].ToString();
                        reportBean.state = state;
                        reportBean.Receipt_NO = dr["pay_id"].ToString();
                        reportBean.Receipt_Date = dr["payment_date"].ToString();
                        reportBean.Student_Code = dr["enroll_no"].ToString();
                        reportBean.Student_Name = dr["first_name"].ToString() + dr["middle_name"].ToString() + dr["last_name"].ToString();

                        if (!string.IsNullOrEmpty(dr["payment_type"].ToString()))
                        {
                            if (Convert.ToInt32(dr["payment_type"].ToString()) == 1)
                            {
                                reportBean.Module_Fees = dr["amount"].ToString();
                            }
                            else if (Convert.ToInt32(dr["payment_type"].ToString()) == 2)
                            {
                                reportBean.Other_fee = dr["amount"].ToString();
                            }
                            else if (Convert.ToInt32(dr["payment_type"].ToString()) == 3)
                            {
                                reportBean.Kit_Fees = dr["amount"].ToString();
                            }
                            else if (Convert.ToInt32(dr["payment_type"].ToString()) == 4)
                            {
                                reportBean.Other_fee = dr["amount"].ToString();
                            }
                        }
                        reportBean.GST = dr["tax_amount"].ToString();
                        reportBean.Total_amount= dr["amount"].ToString();
                        reportBean.Royalti_payable= dr["royalti"].ToString();
                        reportBean.Mention_module = dr["course_name"].ToString();
                        list.Add(reportBean);
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
