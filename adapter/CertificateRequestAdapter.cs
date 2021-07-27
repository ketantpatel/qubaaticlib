using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class CertificateRequestAdapter : MasterAdapter
    {
        public CertificateRequestAdapter(UserBean ubean)
        {
            this.AppUserBean = ubean;
        }
        public FranchiseBean GetFranchisesDetails(string UserId)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select name,address1,frn_code from franchises  where user_id=" + UserId;
            DataRow row = db.GetSingleDataRow(query);
            FranchiseBean franchiseBean = new FranchiseBean();

            if (row != null)
            {
                franchiseBean.address1 = row["address1"].ToString();
                franchiseBean.name = row["name"].ToString();
                franchiseBean.frn_code = row["frn_code"].ToString();
            }
            db.Dispose();

            return franchiseBean;
        }

        public TeacherBean GetTeacherDetail(string frn_id, string teach_id)
        {
            string query = @"SELECT t.teach_id,t.first_name,t.last_name FROM teacher_franchise_allocation a inner join teachers t on t.teach_id=a.teach_id where a.frn_id=" + frn_id + " and a.teach_id =" + teach_id;
            DLS db = new DLS(this.AppUserBean);
            TeacherBean teacher = new TeacherBean();

            DataRow row = db.GetSingleDataRow(query);
            if (row != null)
            {
                teacher.teach_id = row["teach_id"].ToString();
                teacher.full_name = row["first_name"].ToString() + " " + row["last_name"].ToString();

            }
            db.Dispose();
            return teacher;
        }

        public bool SaveExamData(List<ExamMarksData> examMarks)
        {
            try
            {
                bool IsInserted = false;

                foreach (var item in examMarks)
                {
                    DLS db = new DLS(this.AppUserBean);
                    db.AddParameters("teach_id", item.teach_id, MyDBTypes.Int);
                    db.AddParameters("batch_id", item.batch_id, MyDBTypes.Int);
                    db.AddParameters("form_id", item.form_id, MyDBTypes.Int);
                    db.AddParameters("level_id", item.level_id, MyDBTypes.Int);
                    db.AddParameters("frn_id", item.frn_id, MyDBTypes.Int);
                    db.AddParameters("exam_date", item.exam_date, MyDBTypes.DateTime);
                    db.AddParameters("a", item.a, MyDBTypes.Numeric);
                    db.AddParameters("b", item.b, MyDBTypes.Numeric);
                    db.AddParameters("c", item.c, MyDBTypes.Numeric);
                    db.AddParameters("d", item.d, MyDBTypes.Numeric);
                    db.AddParameters("e", item.e, MyDBTypes.Numeric);
                    db.AddParameters("f", item.f, MyDBTypes.Numeric);
                    db.AddParameters("g", item.g, MyDBTypes.Numeric);
                    db.AddParameters("h", item.h, MyDBTypes.Numeric);
                    db.AddParameters("avg", item.avg, MyDBTypes.Numeric);
                    db.AddParameters("create_date", item.create_date, MyDBTypes.DateTime);
                    db.AddParameters("create_by", item.create_by, MyDBTypes.Int);

                    if (db.Insert("exam_marks_data"))
                    {
                        IsInserted = true;
                    }
                    else
                    {
                        IsInserted = false;
                        break;
                    }
                }
                return IsInserted;
            }
            catch (Exception ex)
            {

                throw;
            }
           

        }
    }
}
