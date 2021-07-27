using iTextSharp.text.pdf;
using MDACLib.adapter;
using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace MDACLib
{
    public class MarksheetGenerator
    {
        public List<MarksheetBean> students;
        public UserBean userBean;
        public MarksheetGenerator(List<MarksheetBean> students, UserBean user)
        {
            this.students = students;
            this.userBean = user;
        }
        string[] oneToTen = { "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten" };
        string[] tenToHundred = { "Ten", "Twenty", "Thirty", "Fourty", "Fifty", "Sixty", "Seventy", "Eightty", "Ninety", "Hundred" };
        string[] elevenRow = { "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen", "Twenty" };
        public string getTwoDigitWord(string number)
        {
          
            if (int.Parse(number) > 10 && int.Parse(number) < 20)
            {
                return elevenRow[int.Parse(number) - 11];
            }
            else
            {
                int firstIndex = int.Parse(number.Substring(0, 1));
                int secondIndex = int.Parse(number.Substring(1, 1));
                if (int.Parse(number) <= 10)
                {
                    if (int.Parse(number) == 0)
                    {
                        return "";
                    }
                    return oneToTen[int.Parse(number) - 1];
                }
                if (secondIndex > 0)
                {
                    return tenToHundred[firstIndex - 1] + " " + oneToTen[secondIndex - 1];
                }
                else
                {
                    return tenToHundred[firstIndex - 1];
                }
            }

            return "";
        }
        public string getDigitToWord(string number)
        {
            

            if (int.Parse(number) <= 10)
            {
                return oneToTen[int.Parse(number) - 1];
            }
            else if (int.Parse(number) < 100)
            {
                return getTwoDigitWord(number);
            }
            else
            {
                int firstIndex = int.Parse(number.Substring(0, 1));
                int lastTwoNumber = int.Parse(number.Substring(1, 2));
                string lastTwoNumberWord = getTwoDigitWord(number.Substring(1, 2));
                if (lastTwoNumberWord.Length == 0)
                {
                    // lastTwoNumberWord = tenToHundred[9];
                }
                return oneToTen[firstIndex - 1] + " " + tenToHundred[9] + " " + lastTwoNumberWord;
            }
            return "";
        }
        public void GeneratMarksheet(string bastPath)
        {
            string filePath = bastPath + "\\files\\marksheet\\CCC.pdf";


            DLS db = new DLS(this.userBean);
            try
            {
                string course_id = "-1";
                DataTable dtExamTypes = null;
                DataTable dtMarks = null;
                DataTable dtSubjects = null;
                DataTable dt = null;
                MarksheetBean s1;
                if (students.Count > 0)
                {
                    s1 = students[0];
                    course_id = s1.course_id;
                    dtExamTypes = db.GetDataTable("SELECT exam_type FROM exam_marks e where stud_id=" + s1.form_id + " group by exam_type");
                    dtSubjects = db.GetDataTable("select subject_id,code,practical_marks,theory_marks,name from subjects where course_id=" + course_id);
                }
                foreach (MarksheetBean s in students)
                {
                    
                   
                    dtMarks = db.GetDataTable("select * from exam_marks where stud_id=" + s.form_id);
                   
                    dt = db.GetDataTable("select * from exam_marks where stud_id=" + s.form_id + " and course_id=" + course_id + "");

                    PdfReader rdr = new PdfReader(filePath);
                    PdfReader reader = new PdfReader(rdr);
                    string enroll_no = "";
                    enroll_no = s.reg_no;
                    s.reg_no = s.reg_no.Replace("/", "");
                    string newFileName = bastPath + "\\files\\marksheet\\download\\" + s.reg_no + ".pdf";
                    decimal percentage = 0;
                    using (FileStream fs = new FileStream(newFileName, FileMode.Create))
                    {
                        PdfStamper stamper = new PdfStamper(reader, fs);
                        var a = stamper.AcroFields;
                        a.SetField("STUDEN_NAME", s.student_name);

                        a.SetField("ID_NO", s.id_no);
                        a.SetField("REG_NO", enroll_no);
                        a.SetField("ATC_NAME", s.atc_name);
                        a.SetField("ATC_CODE", s.atc_code);
                        a.SetField("TRAINING_PERIOD", s.training_period);
                        a.SetField("COURSE_CODE", s.course_code);
                        a.SetField("COURSE_NAME", s.course_name);
                        a.SetField("TRAINING_PERIOD", s.training_period);
                        a.SetField("EXAM_DATE", s.exam_date);

                        if (dtSubjects != null)
                        {
                            if (dtSubjects.Rows.Count > 1)
                            {
                                string subName1 = dtSubjects.Rows[0]["name"].ToString();
                                string subName2 = dtSubjects.Rows[1]["name"].ToString();
                                a.SetField("SUBJECT1", subName1);
                                a.SetField("SUBJECT2", subName2);
                            }
                            if (dtExamTypes != null)
                            {
                                int totalPractical = 0;
                                int totalTheory = 0;
                                int totalPracticalFrom = 0;
                                int totalTheoryFrom = 0;
                                foreach (DataRow drSub in dtSubjects.Rows)
                                {
                                    string subjectId = drSub["subject_id"].ToString();
                                    string practical_marks = drSub["practical_marks"].ToString();
                                    string theory_marks = drSub["theory_marks"].ToString();
                                    string code = drSub["code"].ToString();
                                    foreach (DataRow drExamType in dtExamTypes.Rows)
                                    {
                                        string examType = drExamType["exam_type"].ToString();

                                        DataRow[] markRow = dtMarks.Select("course_id=" + course_id + " and exam_type=" + examType + " and stud_id=" + s.form_id + " and subject_id=" + subjectId + "");
                                        if (markRow != null)
                                        {
                                            if (markRow.Length == 0)
                                                continue;
                                            string marks = markRow[0]["marks"].ToString();
                                            int temp_marks=0;
                                            int.TryParse(marks,out temp_marks);
                                            if (examType == "1")
                                            {
                                                int temp_theory_form=0;
                                                int.TryParse(theory_marks, out temp_theory_form);
                                                totalTheoryFrom += temp_theory_form;
                                                totalTheory += temp_marks;
                                                a.SetField(code + "_THEORY_MARKS", marks);
                                            }
                                            else if (examType == "2")
                                            {
                                                int temp_practical_form = 0;
                                                int.TryParse(practical_marks, out temp_practical_form);
                                                totalPracticalFrom += temp_practical_form;
                                                totalPractical += temp_marks;
                                                a.SetField(code + "_PRACTICAL_MARKS", marks);
                                            }
                                        }
                                        a.SetField(code + "_THEORY_MARKS_FROM", theory_marks);
                                        a.SetField(code + "_PRACTICAL_MARKS_FROM", practical_marks);
                                    }
                                    a.SetField("S_THEORY_MARKS_FROM", totalTheoryFrom.ToString());
                                    a.SetField("S_PRACTICAL_MARKS_FROM", totalPracticalFrom.ToString());

                                    a.SetField("S_THEORY_MARKS", totalTheory.ToString());
                                    a.SetField("S_PRACTICAL_MARKS",totalPractical.ToString());
                                    a.SetField("GRAND_TOTAL", (totalPractical + totalTheory).ToString() + "/" + (totalTheoryFrom + totalPracticalFrom));
                                    decimal totalGain = (totalPractical + totalTheory);
                                    decimal totalFrom = (totalTheoryFrom + totalPracticalFrom);
                                    percentage = Math.Round((totalGain / totalFrom) * 100,0,MidpointRounding.AwayFromZero);
                                }
                                string grad = db.GetSingleValue("select grade from gradesheet where course_id=" + course_id + " and form_id=" + s.form_id + "");
                                a.SetField("GRAND_TOTAL_IN_WORDS", getDigitToWord((totalPractical + totalTheory).ToString()));
                                a.SetField("PERCENTAGE", percentage + "%");
                                a.SetField("GRADE", grad);
                                a.SetField("CERTY_DATE", s.marksheet_date);
                            }
                        }
                      
                        stamper.Close();
                        stamper.Dispose();
                        fs.Close();
                        fs.Dispose();
                    }
                    rdr.Dispose();
                    rdr.Close();
                    reader.Dispose();
                    rdr.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
