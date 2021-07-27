using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class MarksheetAdapter : MasterAdapter
    {
        public MarksheetAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }

        public List<MarksheetBean> GetStudentMarksheetInfo(List<MarksheetBean> students)
        {
            DLS db = new DLS(this.AppUserBean);
            try
            {
                string prefix = "";
                 string serial="";
                 int serial_no = 1;
                 serial = db.GetSingleValue("select max(serial_no) from marksheet_ref");

                 if(serial=="")
                        serial="0";

                foreach (MarksheetBean m in students)
                {
                    m.student_name = db.GetSingleValue("select concat(CONCAT(UCASE(LEFT(first_name, 1)),LCASE(SUBSTRING(first_name, 2))),' ',CONCAT(UCASE(LEFT(middle_name, 1)),LCase(SUBSTRING(middle_name, 2))),' ',CONCAT(UCASE(LEFT(last_name, 1)),LCase(SUBSTRING(last_name, 2))) ) name from admission_forms where form_id="+m.form_id);
                    m.course_name = db.GetSingleValue("select course_name from courses where course_id in ("+m.course_id+")");
                    m.course_code = db.GetSingleValue("select UCASE(code) from courses where course_id in (" + m.course_id + ")");
                    DataRow frnRow = db.GetSingleDataRow("select name,frn_code from franchises where frn_id in(select frn_id from admission_forms where form_id="+m.form_id+")");
                    string ref_no = db.GetSingleValue("select ref_no from marksheet_ref where form_id="+m.form_id+"");
                    //DataRow drGrade = db.GetSingleDataRow("select round(percentage,0) percentage,marks_obtain,marks_total  from gradesheet where course_id in (select course_id from admission_forms where form_id=" + m.form_id + ") and form_id=" + m.form_id + "");

                    //if (drGrade != null)
                    //{
                    //    m.percentage = drGrade["percentage"].ToString();
                    //    m.total_marks = drGrade["marks_total"].ToString();
                    //    m.obtain_marks = drGrade["marks_obtain"].ToString();
                    //}

                    if(int.Parse(m.month)<10)
                        prefix = "0"+ m.month + "" + m.year.Substring(2, 2);
                    else
                        prefix=m.month+""+m.year.Substring(2,2);

                    serial_no = serial_no + int.Parse(serial);


                    m.id_no = prefix + "" + serial_no;


                    if (string.IsNullOrEmpty(ref_no))
                    {
                        db.AddParameters("ref_no", m.id_no, MyDBTypes.Varchar);
                        db.AddParameters("serial_no", serial_no, MyDBTypes.Int);
                        db.AddParameters("prefix", prefix, MyDBTypes.Varchar);
                        db.AddParameters("course_id", m.course_id, MyDBTypes.Int);
                        db.AddParameters("form_id", m.form_id, MyDBTypes.Int);
                        db.Insert("marksheet_ref");
                        serial_no += 1;
                    }
                    else
                    {
                        m.id_no = ref_no;
                    }

                   
                    
                    try
                    {
                        m.atc_code = frnRow["frn_code"].ToString();
                        m.atc_name = frnRow["name"].ToString();
                    }
                    catch (Exception exx)
                    {
                    }
                   // m.id_no = m.reg_no.Replace("/", "");
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
