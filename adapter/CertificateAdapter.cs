using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class CertificateAdapter : MasterAdapter
    {
        public CertificateAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }

        public List<CertificateBean> GetStudentCertificateInfo(List<CertificateBean> students, string dirPath)
        {
            DLS db = new DLS(this.AppUserBean);
            try
            {
                string serial_no = db.GetSingleValue("select max(serial_no) from gradesheet");
                int sr_no = 0;
                int max_ser_no = 0;
                foreach (CertificateBean m in students)
                {
                    m.student_name = db.GetSingleValue("select UCASE(concat(CONCAT(UCASE(LEFT(first_name, 1)),LCASE(SUBSTRING(first_name, 2))),' ',CONCAT(UCASE(LEFT(middle_name, 1)),LCase(SUBSTRING(middle_name, 2))),' ',CONCAT(UCASE(LEFT(last_name, 1)),LCase(SUBSTRING(last_name, 2))) )) name from admission_forms where form_id=" + m.form_id);
                    DataRow frnRow = db.GetSingleDataRow("select name,frn_code,UCASE(city1) city1 from franchises where frn_id in(select frn_id from admission_forms where form_id=" + m.form_id + ")");
                    try
                    {
                        m.fran_city = "";
                        if (frnRow["city1"] != null)
                            m.fran_city = frnRow["city1"].ToString();
                        m.reg_no = db.GetSingleValue("select enroll_no from admission_forms where form_id=" + m.form_id);
                        m.course_name = db.GetSingleValue("select UCASE(course_name) from courses where course_id in ("+m.course_id+")");
                        m.gaurdian = db.GetSingleValue("select UCASE(middle_name) from admission_forms where form_id=" + m.form_id + "");
                        m.grade = db.GetSingleValue("select grade from gradesheet where course_id in (" + m.course_id + ") and form_id=" + m.form_id + "");
                        m.serial_no = db.GetSingleValue("select serial_no from gradesheet where course_id in (" + m.course_id + ") and form_id=" + m.form_id + "");
                        //m.percentage = db.GetSingleValue("select round(percentage,0) percentage  from gradesheet where course_id in (select course_id from admission_forms where form_id=" + m.form_id + ") and form_id=" + m.form_id + "");
                        m.photoPath = dirPath + db.GetSingleValue("select filePath from documents where form_id=" + m.form_id + " and doc_type=5");

                        bool isNewSerial = false;
                        if(m.serial_no == null)
                            isNewSerial=true;
                        if (m.serial_no != null)
                        {
                            if (m.serial_no == "0")
                            {
                                isNewSerial = true;
                            }
                        }

                        if (isNewSerial)
                        {
                            int.TryParse(db.GetSingleValue("select max(serial_no) from gradesheet"), out max_ser_no);

                            max_ser_no = max_ser_no + 1;
                            m.serial_no = max_ser_no.ToString();
                            db.AddParameters("serial_no", m.serial_no, MyDBTypes.Int);
                            db.Update("gradesheet", "form_id=" + m.form_id);
                        }

                       



                    }
                    catch (Exception exx)
                    {
                    }

                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                db.Dispose();
            }
            return students;
        }
    }
}
