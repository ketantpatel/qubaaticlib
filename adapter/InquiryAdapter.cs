using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class InquiryAdapter : MasterAdapter
    {
        public InquiryAdapter(UserBean ubean)
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
            string query = "select distinct monthname(create_date) as month_name,month(create_date) month_no from enquiry where frn_id=" + franchise_id + " order by month(create_date)";
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
            DLS db = new DLS(this.AppUserBean);
            string query = "select distinct year(create_date) year from enquiry where frn_id=" + franchise_id + " order by year(create_date)";
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
        public InquiryBean SaveInquiry(InquiryBean iBean)
        {
            if (iBean.enquiry_id == "-1")
            {
                iBean = AddInquiry(iBean);
            }
            else
            {
                iBean = EditInquiry(iBean);
            }

            return iBean;
        }
        public bool DeleteCourse(string course_id)
        {
            DLS db = new DLS(this.AppUserBean);
            bool isFlag=db.Delete("courses", "course_id=" + course_id);
            db.Dispose();
            return isFlag;
        }

        public bool DeleteEnquiry(string enq_id)
        {
            DLS db = new DLS(this.AppUserBean);
            bool isFlag = db.Delete("enquiry", "enquiry_id=" + enq_id);
            db.Dispose();
            return isFlag;
        }
        public InquiryBean AddInquiry(InquiryBean iBean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(iBean.first_name))
                db.AddParameters("first_name", iBean.first_name, MyDBTypes.Varchar);
        
            if (!string.IsNullOrEmpty(iBean.middle_name))
                db.AddParameters("middle_name", iBean.middle_name, MyDBTypes.Varchar);
           
            if (!string.IsNullOrEmpty(iBean.last_name))
                db.AddParameters("last_name", iBean.last_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.school_name))
                db.AddParameters("school_name", iBean.school_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.age))
                db.AddParameters("age", iBean.age, MyDBTypes.Varchar);
            
            if (!string.IsNullOrEmpty(iBean.p_first_name))
                db.AddParameters("p_first_name", iBean.p_first_name, MyDBTypes.Varchar);
            
            if (!string.IsNullOrEmpty(iBean.p_middle_name))
                db.AddParameters("p_middle_name", iBean.p_middle_name, MyDBTypes.Varchar);
            
            if (!string.IsNullOrEmpty(iBean.p_last_name))
                db.AddParameters("p_last_name", iBean.p_last_name, MyDBTypes.Varchar);
            
            if (!string.IsNullOrEmpty(iBean.contactNumber))
                db.AddParameters("contact_number", iBean.contactNumber, MyDBTypes.Varchar);
            
            if (!string.IsNullOrEmpty(iBean.product))
                db.AddParameters("product", iBean.product, MyDBTypes.Varchar);
            
            if (!string.IsNullOrEmpty(iBean.foll1))
                db.AddParameters("foll1", iBean.foll1, MyDBTypes.Varchar);
            
            if (!string.IsNullOrEmpty(iBean.foll2))
                db.AddParameters("foll2", iBean.foll2, MyDBTypes.Varchar);
            
            if (!string.IsNullOrEmpty(iBean.foll3))
                db.AddParameters("foll3", iBean.foll3, MyDBTypes.Varchar);
            
            if (!string.IsNullOrEmpty(iBean.remark))
                db.AddParameters("remark", iBean.remark, MyDBTypes.Varchar);
             
            if (!string.IsNullOrEmpty(iBean.date))
                db.AddParameters("date", iBean.date, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.created_by))
                db.AddParameters("created_by", iBean.created_by, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.frn_id))
                db.AddParameters("frn_id", iBean.frn_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.create_date))
                db.AddParameters("create_date", iBean.create_date, MyDBTypes.DateTime);

            if (!string.IsNullOrEmpty(iBean.modify_by))
                db.AddParameters("modify_by", iBean.modify_by, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.modify_date))
                db.AddParameters("modify_date", iBean.modify_date, MyDBTypes.DateTime);

          
            iBean.is_opr_success = false;
            if (db.Insert("enquiry"))
            {
                iBean.is_opr_success = true;
                iBean.enquiry_id = db.GetLastAutoID().ToString();
            }
            db.Dispose();
            return iBean;
        }
        public InquiryBean EditInquiry(InquiryBean iBean)
        {
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(iBean.first_name))
                db.AddParameters("first_name", iBean.first_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.middle_name))
                db.AddParameters("middle_name", iBean.middle_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.last_name))
                db.AddParameters("last_name", iBean.last_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.school_name))
                db.AddParameters("school_name", iBean.school_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.age))
                db.AddParameters("age", iBean.age, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.p_first_name))
                db.AddParameters("p_first_name", iBean.p_first_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.p_middle_name))
                db.AddParameters("p_middle_name", iBean.p_middle_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.p_last_name))
                db.AddParameters("p_last_name", iBean.p_last_name, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.contactNumber))
                db.AddParameters("contact_number", iBean.contactNumber, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.product))
                db.AddParameters("product", iBean.product, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.foll1))
                db.AddParameters("foll1", iBean.foll1, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.foll2))
                db.AddParameters("foll2", iBean.foll2, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.foll3))
                db.AddParameters("foll3", iBean.foll3, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.remark))
                db.AddParameters("remark", iBean.remark, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.date))
                db.AddParameters("date", iBean.date, MyDBTypes.Varchar);

            if (!string.IsNullOrEmpty(iBean.created_by))
                db.AddParameters("created_by", iBean.created_by, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.frn_id))
                db.AddParameters("frn_id", iBean.frn_id, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.create_date))
                db.AddParameters("create_date", iBean.create_date, MyDBTypes.DateTime);

            if (!string.IsNullOrEmpty(iBean.modify_by))
                db.AddParameters("modify_by", iBean.modify_by, MyDBTypes.Int);

            if (!string.IsNullOrEmpty(iBean.modify_date))
                db.AddParameters("modify_date", iBean.modify_date, MyDBTypes.DateTime);


            iBean.is_opr_success = false;
            if (db.Update("enquiry", "enquiry_id=" + iBean.enquiry_id))
            {
                iBean.is_opr_success = true;               
            }
            db.Dispose();
            return iBean;
        }

        public List<InquiryBean> ListInquiry(string month,string year,string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select *,DATE_FORMAT(date, '%d/%m/%Y') date from enquiry where frn_id="+frn_id;
            if(month !="All")
                query = "select *,DATE_FORMAT(date, '%d/%m/%Y') date from enquiry where frn_id=" + frn_id+" and month(create_date)="+month+" and year(create_date)="+year+"";
            List<InquiryBean> list = new List<InquiryBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        InquiryBean fb = new InquiryBean();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.enquiry_id = dr["enquiry_id"].ToString();
                        fb.age = dr["age"].ToString();
                        fb.p_first_name = dr["p_first_name"].ToString();
                        fb.p_middle_name = dr["p_middle_name"].ToString();
                        fb.p_last_name = dr["p_last_name"].ToString();
                        fb.contactNumber = dr["contact_number"].ToString();
                        fb.product = dr["product"].ToString();
                        fb.foll1 = dr["foll1"].ToString();
                        fb.foll2 = dr["foll2"].ToString();
                        fb.foll3 = dr["foll3"].ToString();
                        fb.remark = dr["remark"].ToString();
                        //fb.date = dr["date"].ToString();
                        fb.date = Convert.ToDateTime(dr["date"].ToString()).ToShortDateString() ;
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.modify_date = dr["modify_date"].ToString();
                        fb.modify_by = dr["modify_by"].ToString();
                        fb.created_by = dr["created_by"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }

        //public InquiryBean GetInquiryDetail(string fid)
        //{
        //    DLS db = new DLS(this.AppUserBean);
        //    string query = "select * from inquiry where inquiry_id=" + fid;
        //    List<InquiryBean> list = new List<InquiryBean>();
        //    DataTable dt = db.GetDataTable(query);
        //    if (dt != null)
        //    {
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                InquiryBean fb = new InquiryBean();
        //                fb.course_id = dr["course_id"].ToString();
        //                fb.first_name = dr["first_name"].ToString();
        //                fb.middle_name = dr["middle_name"].ToString();
        //                fb.last_name = dr["last_name"].ToString();
        //                fb.inquiry_id = dr["inquiry_id"].ToString();
        //                 fb.address = dr["address"].ToString();
        //                fb.city = dr["city"].ToString();
        //                fb.pincode = dr["pincode"].ToString();
        //                fb.email = dr["email"].ToString();
        //                fb.mobile = dr["mobile"].ToString();
        //                fb.inquirer = dr["inquirer"].ToString();
        //                fb.course_id = dr["course_id"].ToString();
        //                fb.other_course = dr["other_course"].ToString();
        //                fb.counselor = dr["counselor"].ToString();
        //                fb.source_id = dr["source_id"].ToString();
        //                fb.exp_join_date = dr["exp_join_date"].ToString();
        //                fb.remark = dr["remark"].ToString();
        //                fb.create_date = dr["create_date"].ToString();
        //                fb.qualification = dr["qualification"].ToString();
        //                fb.modify_date = dr["modify_date"].ToString();
        //                fb.modify_by = dr["modify_by"].ToString();
        //                fb.created_by = dr["created_by"].ToString();
        //                list.Add(fb);
        //            }
        //        }
        //    }
        //    db.Dispose();
        //    if (list.Count > 0)
        //        return list[0];
        //    return new InquiryBean();
        //}

        public InquiryBean GetInquiryDetail(string fid,string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from enquiry where enquiry_id=" + fid + " and frn_id =" + frn_id;
            List<InquiryBean> list = new List<InquiryBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        InquiryBean fb = new InquiryBean();
                        fb.first_name = dr["first_name"].ToString();
                        fb.middle_name = dr["middle_name"].ToString();
                        fb.last_name = dr["last_name"].ToString();
                        fb.school_name = dr["school_name"].ToString();
                        fb.enquiry_id = dr["enquiry_id"].ToString();
                        fb.age = dr["age"].ToString();
                        fb.p_first_name = dr["p_first_name"].ToString();
                        fb.p_middle_name = dr["p_middle_name"].ToString();
                        fb.p_last_name = dr["p_last_name"].ToString();
                        fb.contactNumber = dr["contact_number"].ToString();
                        fb.product = dr["product"].ToString();
                        fb.foll1 = dr["foll1"].ToString();
                        fb.foll2 = dr["foll2"].ToString();
                        fb.foll3 = dr["foll3"].ToString();
                        fb.remark = dr["remark"].ToString();
                        fb.date = dr["date"].ToString();
                        fb.frn_id = dr["frn_id"].ToString();
                        fb.modify_date = dr["modify_date"].ToString();
                        fb.modify_by = dr["modify_by"].ToString();
                        fb.created_by = dr["created_by"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            if (list.Count > 0)
                return list[0];
            return new InquiryBean();
        }
    }
}
