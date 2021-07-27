using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class VerificationAdapter : MasterAdapter
    {
        public VerificationAdapter(UserBean userBean)
        {
            this.AppUserBean = userBean;
        }

        public VerificationBean GetStudent(string enrollmentNo)
        {
            VerificationBean v = new VerificationBean();
            DLS db = new DLS(this.AppUserBean);

            string query = "select a.*,c.course_name,c.code,c.duration_months from admission_forms a left join courses c on c.course_id=a.course_id where enroll_no='" + enrollmentNo + "'";
            DataTable dt = db.GetDataTable(query);
            int recordCount = 0;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        v.StudentName = dr["first_name"].ToString();
                        if (dr["middle_name"] != null)
                        {
                            v.StudentName = v.StudentName + " " + dr["middle_name"].ToString() + " " + dr["last_name"].ToString();
                        }
                        v.JoinDate = dr["form_date"].ToString();
                        v.Grade = db.GetSingleValue("select grade from gradesheet where form_id=" + dr["form_id"].ToString() + "");                            
                        v.EnrollNo = enrollmentNo;
                        v.Duration = dr["duration_months"].ToString() +" Month[s]";

                        v.CourseName = dr["course_name"].ToString();
                        recordCount++;
                    }
                }
            }
            db.Dispose();
            if (recordCount == 0)
            {
                v.isFail = true;
                v.message = "Sorry ! : Record not found.";
            }
            else
            {
                if (recordCount > 1)
                {
                    v.isFail = true;
                    v.message = "Dual entry found for searched record. please contact to admistration";
                }
            }
            return v;
        }
    }
}
