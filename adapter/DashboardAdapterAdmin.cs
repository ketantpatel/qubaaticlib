using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDACLib.adapter
{
    public class DashboardAdapterAdmin : MasterAdapter
    {
        public DashboardAdapterAdmin(UserBean user)
        {
            this.AppUserBean = user;
        }
        public DashboardAdminBean GetFranchiseDashboardInfo(string frn_id)
        {
            DashboardAdminBean bean = new DashboardAdminBean();
            string totalStudent = "select count(*) from students where frn_id="+frn_id;
            bean.TotalUsers = "1";
            DLS db = new DLS(this.AppUserBean);
            bean.TotalStudent = db.GetSingleValue(totalStudent);
            db.Dispose();
            return bean;
        }
        public DashboardAdminBean GetTeachDashboard(string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            DashboardAdminBean b = new DashboardAdminBean();
            b.TotalFranchises = db.GetSingleValue("SELECT count(*) found FROM teacher_franchise_allocation where teach_id="+ frn_id);
            b.TotalBatches = db.GetSingleValue("SELECT count(*) found FROM batches where teach_id=" + frn_id);
            db.Dispose();
            return b;
        }
        public DashboardAdminBean GetDashboardInfoFranchise(string frn_id)
        {
            DashboardAdminBean bean = new DashboardAdminBean();
            string totalStudent = "select count(*) from admission_forms where frn_id=" + frn_id+" and is_verify=1";
            bean.TotalUsers = "1";
            DLS db = new DLS(this.AppUserBean);
            bean.TotalStudent = db.GetSingleValue(totalStudent);
            db.Dispose();
            return bean;
        }
        public DashboardAdminBean GetDashboardInfo(string franchise_id)
        {
            DashboardAdminBean bean = new DashboardAdminBean();
            //            string totalVerified = @"select count(*) verify_count from (
            //select stud_id,first_name,middle_name,last_name,(select count(*) from documents_edu d where d.stud_id=s.stud_id) doc_count,
            //(select count(*) from documents_edu d where d.stud_id=s.stud_id and status='verified') verified_doc_count
            // from students s) as a where doc_count=verified_doc_count and doc_count >0";

            string totalVerified = @"select count(*) from admission_forms where is_verify=1 and frn_id=" + franchise_id + " or frn_id in (select frn_id from franchises where parent_id=" + franchise_id + ")";

            //string totalPending = @"select count(*) pending from (select stud_id,count(*) a from documents_edu where status='pending' group by stud_id) as a";
            string totalPending = @"select count(*) from admission_forms where is_verify=0  and frn_id=" + franchise_id + " or frn_id in (select frn_id from franchises where parent_id=" + franchise_id + ")";

            //    string totalRejected = @"select count(*) pending from (select stud_id,count(*) a from documents_edu where status='rejected' group by stud_id) as a";
            string totalRejected = @"select count(*) from admission_forms where is_verify=2  and frn_id=" + franchise_id + " or frn_id in (select frn_id from franchises where parent_id=" + franchise_id + ")";

            //   string totalNoDocs= @"select count(*) from students where stud_id not in(select stud_id from documents_edu)";

            string totalNoDocs = @"select count(*) from admission_forms where form_id not in(select form_id from documents)  and frn_id=" + franchise_id + " or frn_id in (select frn_id from franchises where parent_id=" + franchise_id + ")";

            string totalStudent = "select count(*) from admission_forms  where frn_id=" + franchise_id + " or frn_id in (select frn_id from franchises where parent_id=" + franchise_id + ")";
            string totalFranchise = "select count(*) from franchises where frn_id=" + franchise_id + " or parent_id=" + franchise_id + "";
            string totalUser = "select count(*) from users where user_id in (select user_id from franchises where frn_id=" + franchise_id + " or parent_id=" + franchise_id + ")";
            DLS db = new DLS(this.AppUserBean);
            bean.TotalStudent = db.GetSingleValue(totalStudent);
            bean.TotalFranchises = db.GetSingleValue(totalFranchise);
            bean.TotalVerifiedStudents = db.GetSingleValue(totalVerified);
            bean.TotalPendingStudents = db.GetSingleValue(totalPending);
            bean.TotalRejectedStudents = db.GetSingleValue(totalRejected);
            bean.TotalNoDocsStudents = db.GetSingleValue(totalNoDocs);
            bean.TotalUsers = db.GetSingleValue(totalUser);
            db.Dispose();
            return bean;
        }

        public DashboardAdminBean GetDashboardInfo(bool isCountryAccount,bool isStateAccount,string user_id)
        {
            DashboardAdminBean bean = new DashboardAdminBean();
            //            string totalVerified = @"select count(*) verify_count from (
            //select stud_id,first_name,middle_name,last_name,(select count(*) from documents_edu d where d.stud_id=s.stud_id) doc_count,
            //(select count(*) from documents_edu d where d.stud_id=s.stud_id and status='verified') verified_doc_count
            // from students s) as a where doc_count=verified_doc_count and doc_count >0";

            string totalVerified ="select 0";

            //string totalPending = @"select count(*) pending from (select stud_id,count(*) a from documents_edu where status='pending' group by stud_id) as a";
            string totalPending = "select 0";

            //    string totalRejected = @"select count(*) pending from (select stud_id,count(*) a from documents_edu where status='rejected' group by stud_id) as a";
            string totalRejected = "select 0";

            //   string totalNoDocs= @"select count(*) from students where stud_id not in(select stud_id from documents_edu)";

            string totalNoDocs = "select 0";

            
            string totalStudent = @"select count(*) From admission_forms
where frn_id in (
select frn_id from franchises where create_by in (
select user_id From users where country_id in (select country_id from users where user_id="+user_id+")))";
            if (isStateAccount)
            {
                totalStudent = @"select count(*) From admission_forms
where frn_id in (
select frn_id from franchises where create_by in (
select user_id From users where state_id in (select state_id from users where user_id=" + user_id + ")))";
            }
            string totalFranchise = @"select count(*) from franchises where create_by in (
select user_id From users where country_id in (select country_id from users where user_id="+user_id+"))";

            if (isStateAccount)
            {
                totalFranchise = @"select count(*) from franchises where create_by in (
select user_id From users where state_id in (select state_id from users where user_id="+user_id+"))";
            }
            string totalUser = "select 0";
            DLS db = new DLS(this.AppUserBean);
            bean.TotalStudent = db.GetSingleValue(totalStudent);
            bean.TotalFranchises = db.GetSingleValue(totalFranchise);
            bean.TotalVerifiedStudents = db.GetSingleValue(totalVerified);
            bean.TotalPendingStudents = db.GetSingleValue(totalPending);
            bean.TotalRejectedStudents = db.GetSingleValue(totalRejected);
            bean.TotalNoDocsStudents = db.GetSingleValue(totalNoDocs);
            bean.TotalUsers = db.GetSingleValue(totalUser);
            db.Dispose();
            return bean;
        }

        public DashboardAdminBean GetDashboardInfo()
        {
            DashboardAdminBean bean = new DashboardAdminBean();
//            string totalVerified = @"select count(*) verify_count from (
//select stud_id,first_name,middle_name,last_name,(select count(*) from documents_edu d where d.stud_id=s.stud_id) doc_count,
//(select count(*) from documents_edu d where d.stud_id=s.stud_id and status='verified') verified_doc_count
// from students s) as a where doc_count=verified_doc_count and doc_count >0";

            string totalVerified = @"select count(*) from admission_forms where is_verify=1";

            //string totalPending = @"select count(*) pending from (select stud_id,count(*) a from documents_edu where status='pending' group by stud_id) as a";
            string totalPending = @"select count(*) from admission_forms where is_verify=0";

        //    string totalRejected = @"select count(*) pending from (select stud_id,count(*) a from documents_edu where status='rejected' group by stud_id) as a";
            string totalRejected = @"select count(*) from admission_forms where is_verify=2";

         //   string totalNoDocs= @"select count(*) from students where stud_id not in(select stud_id from documents_edu)";

            string totalNoDocs = @"select count(*) from admission_forms where form_id not in(select form_id from documents)";

            string totalStudent = "select count(*) from admission_forms";
            string totalFranchise = "select count(*) from franchises";
            string totalUser = "select count(*) from users";
            DLS db = new DLS(this.AppUserBean);
            bean.TotalStudent = db.GetSingleValue(totalStudent);
            bean.TotalFranchises = db.GetSingleValue(totalFranchise);
            bean.TotalVerifiedStudents = db.GetSingleValue(totalVerified);
            bean.TotalPendingStudents = db.GetSingleValue(totalPending);
            bean.TotalRejectedStudents = db.GetSingleValue(totalRejected);
            bean.TotalNoDocsStudents = db.GetSingleValue(totalNoDocs);
            bean.TotalUsers = db.GetSingleValue(totalUser);
            db.Dispose();
            return bean;
        }
        public DataTable ListFranchise(string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select f.frn_id,f.name,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 and (f.frn_id="+frn_id+" or f.parent_id="+frn_id+") group by f.name,f.frn_id";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["verified_count"] = db.GetSingleValue(@"select count(*) from admission_forms f where frn_id=" + dr["frn_id"].ToString() + " and is_verify=1");
                    }
                }
            }
            db.Dispose();
            return dt;
        }
        public DataTable ListFranchise()
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select f.frn_id,f.name,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 group by f.name,f.frn_id";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["verified_count"] = db.GetSingleValue(@"select count(*) from admission_forms f where frn_id=" + dr["frn_id"].ToString() + " and is_verify=1");
                    }
                }
            }
            db.Dispose();
            return dt;
        }
        public DataTable ListFranchise(bool isCountryAccount,bool isStateAccount)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select f.frn_id,f.name,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 group by f.name,f.frn_id";
            if (isCountryAccount)
            {
                query = "select f.frn_id,f.name,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 and f.country_id=" + this.AppUserBean.country_id + " group by f.name,f.frn_id";
            }
            if (isStateAccount)
                query = "select f.frn_id,f.name,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 and f.state_id=" + this.AppUserBean.state_id + " group by f.name,f.frn_id";

            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["verified_count"] = db.GetSingleValue(@"select count(*) from admission_forms f where frn_id=" + dr["frn_id"].ToString() + " and is_verify=1");
                    }
                }
            }
            db.Dispose();
            return dt;
        }
        public DataTable ListFranchise(bool isCountryAccount, bool isStateAccount,string user_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select f.frn_id,f.name,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 group by f.name,f.frn_id";
            if (isCountryAccount)
            {
                query = @"select f.frn_id,f.name,c.country_name,st.state_name,sum(case when s.frn_id is null then 0 else 1 end) student_count,'0' verified_count from franchises f
 left join admission_forms s on s.frn_id=f.frn_id
left join countries c on c.country_id=f.country_id
left join states st on st.state_id=f.state_id
  where f.is_active=1 and f.country_id=1
 and (f.create_by in (select user_id from users where parent_id="+user_id+") or f.create_by="+user_id+") group by f.name,f.frn_id";
                //query = "SELECT * FROM franchises where create_by in (select user_id from users where parent_id="+user_id+")";
            }
            if (isStateAccount)
            {
                query = "select f.frn_id,f.name,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 and f.state_id=" + this.AppUserBean.state_id + " group by f.name,f.frn_id";
                query = @"select f.frn_id,f.name,c.country_name,st.state_name,sum(case when s.frn_id is null then 0 else 1 end) student_count,'0' verified_count from franchises f
 left join admission_forms s on s.frn_id=f.frn_id
left join countries c on c.country_id=f.country_id
left join states st on st.state_id=f.state_id
  where f.is_active=1 and f.country_id=1
 and (f.create_by in (select user_id from users where parent_id=" + user_id + ") or f.create_by=" + user_id + ") group by f.name,f.frn_id";
            }

            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        dr["verified_count"] = db.GetSingleValue(@"select count(*) from admission_forms f where frn_id=" + dr["frn_id"].ToString() + " and is_verify=1");
                    }
                }
            }
            db.Dispose();
            return dt;
        }

        public DataTable ListRegisteredAccounts(bool IsAdmin,bool isCountryAccount, bool isStateAccount,bool isAreaAccount, string user_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select f.frn_id,f.name,count(*) student_count,'0' verified_count,'0' frnchise_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 group by f.name,f.frn_id";
            if (isCountryAccount)
            {
                query = @"select f.frn_id,f.name,c.country_name,st.state_name,sum(case when s.frn_id is null then 0 else 1 end) student_count,'0' verified_count,sum(case when f.frn_id is null then 0 else 1 end) frnchise_count from franchises f
 left join admission_forms s on s.frn_id=f.frn_id
left join countries c on c.country_id=f.country_id
left join states st on st.state_id=f.state_id
  where f.is_active=1 and f.country_id=1
 and (f.create_by in (select user_id from users where parent_id=" + user_id + ") or f.create_by=" + user_id + ") group by c.country_name,st.state_name";
                query = @"select concat(state_name,', ',country_name) location,c.country_name,st.state_name,sum(case when s.frn_id is null then 0 else 1 end) student_count,
count(distinct f.frn_id) frnchise_count from franchises f
left join admission_forms s on s.frn_id=f.frn_id
left join countries c on c.country_id=f.country_id
left join states st on st.state_id=f.state_id
  where f.is_active=1 and f.country_id in (select country_id from users where user_id=" +user_id+") group by c.country_name,st.state_name";
                //query = "SELECT * FROM franchises where create_by in (select user_id from users where parent_id="+user_id+")";
            }
            if (isStateAccount || isAreaAccount)
            {
                query = "select f.frn_id,f.name,count(*) student_count,'0' verified_count from franchises f left join admission_forms s on s.frn_id=f.frn_id where f.is_active=1 and f.state_id=" + this.AppUserBean.state_id + " group by f.name,f.frn_id";
                query = @"select f.frn_id,f.name,c.country_name,st.state_name,sum(case when s.frn_id is null then 0 else 1 end) student_count,'0' verified_count,
sum(case when f.frn_id is null then 0 else 1 end) frnchise_count,'0' verified_count from franchises f
 left join admission_forms s on s.frn_id=f.frn_id
left join countries c on c.country_id=f.country_id
left join states st on st.state_id=f.state_id
  where f.is_active=1 and f.country_id=1
 and (f.create_by in (select user_id from users where parent_id=" + user_id + ") or f.create_by=" + user_id + ") group by c.country_name,st.state_name";
                query = @"select
concat(case when length(f.city)>0 then f.city else 'None' end,', ',state_name,', ',country_name,' (',r.name,')') location,
r.name role_name,case when length(f.city)>0 then f.city else 'None' end city,c.country_name,st.state_name,sum(case when s.frn_id is null then 0 else 1 end) student_count,
count(distinct f.frn_id) frnchise_count,'0' verified_count from franchises f
left join admission_forms s on s.frn_id=f.frn_id
left join countries c on c.country_id=f.country_id
left join states st on st.state_id=f.state_id
inner join users u on f.create_by=u.user_id
inner join roles r on r.role_id=u.role
  where f.is_active=1 and st.state_id in (select state_id from users where user_id="+user_id+") group by f.city";
            }
            if (IsAdmin)
            {
                query = @"select concat(state_name,', ',country_name) location,c.country_name,st.state_name,sum(case when s.frn_id is null then 0 else 1 end) student_count,
count(distinct f.frn_id) frnchise_count from franchises f
left join admission_forms s on s.frn_id=f.frn_id
left join countries c on c.country_id=f.country_id
left join states st on st.state_id=f.state_id
  where f.is_active=1 group by c.country_name,st.state_name";
            }
            DataTable dt = db.GetDataTable(query);

            if (IsAdmin || isCountryAccount || isStateAccount || isAreaAccount)
            {
            }
            else{
                if (dt != null)
                {
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["verified_count"] = db.GetSingleValue(@"select count(*) from admission_forms f where frn_id=" + dr["frn_id"].ToString() + " and is_verify=1");
                        }
                    }
                }
            }

            db.Dispose();
            return dt;
        }
        public DataTable ListTeacherFranchise(string teach_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "SELECT t.frn_id,f.name,f.contact_person,f.phone1,f.address1,f.city1,f.pincode1 FROM franchises f inner join teacher_franchise_allocation t on t.frn_id=f.frn_id where t.teach_id="+teach_id;

            DataTable dt = db.GetDataTable(query);
            //if (dt != null)
            //{
            //    if (dt.Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            dr["verified_count"] = db.GetSingleValue(@"select count(*) from admission_forms f where frn_id=" + dr["frn_id"].ToString() + " and is_verify=1");
            //        }
            //    }
            //}
            db.Dispose();
            return dt;
        }
    }
}
