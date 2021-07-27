using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class LessonDataAdapter : MasterAdapter
    {
        public LessonDataAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }

        public void SaveLesson(List<LessonDataBean> list)
        {
            DLS db = new DLS(this.AppUserBean);
            string data_id = "0";
            bool flag = false;
            foreach (LessonDataBean b in list)
            {
                flag = false;
                data_id = db.GetSingleValue("select data_id from lessons_data where level_id="+b.level_id+" and teach_id=" + b.teach_id + " and batch_id=" + b.batch_id + " and stud_id=" + b.stud_id + " and lesson_id=" + b.lesson_id + "");
                if (!string.IsNullOrEmpty(b.data_value))
                    db.AddParameters("data_value", b.data_value, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.sw_1_minute))
                    db.AddParameters("sw_1_minute", b.sw_1_minute, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.level_id))
                    db.AddParameters("level_id", b.level_id, MyDBTypes.Int);

                if (!string.IsNullOrEmpty(b.jd_called))
                    db.AddParameters("jd_called", b.jd_called, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.jd_scored))
                    db.AddParameters("jd_scored", b.jd_scored, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.fc_showed))
                    db.AddParameters("fc_showed", b.fc_showed, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.fc_scored))
                    db.AddParameters("fc_scored", b.fc_scored, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.fmp_total))
                    db.AddParameters("fmp_total", b.fmp_total, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.fmp_correct))
                    db.AddParameters("fmp_correct", b.fmp_correct, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.os_called))
                    db.AddParameters("os_called", b.os_called, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.os_correct))
                    db.AddParameters("os_correct", b.os_correct, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.vs_total))
                    db.AddParameters("vs_total", b.vs_total, MyDBTypes.Varchar);

                if (!string.IsNullOrEmpty(b.vs_correct))
                    db.AddParameters("vs_correct", b.vs_correct, MyDBTypes.Varchar);


                if (data_id == "" || data_id == "0")
                {
                    db.AddParameters("lesson_id", b.lesson_id, MyDBTypes.Int);

                    if (!string.IsNullOrEmpty(b.pattern_id))
                    db.AddParameters("pattern_id", b.pattern_id, MyDBTypes.Int);

                    if (!string.IsNullOrEmpty(b.label_id))
                        db.AddParameters("label_id", b.label_id, MyDBTypes.Int);
                    
                    db.AddParameters("create_date", b.create_date, MyDBTypes.DateTime);
                    db.AddParameters("modify_date", b.modify_date, MyDBTypes.DateTime);
                    db.AddParameters("teach_id", b.teach_id, MyDBTypes.Int);
                    db.AddParameters("batch_id", b.batch_id, MyDBTypes.Int);
                    db.AddParameters("stud_id", b.stud_id, MyDBTypes.Int);
                    if(db.Insert("lessons_data"))
                    {
                        
                        b.is_success = true;                        
                    }
                }
                else
                {
                    db.AddParameters("modify_date", b.modify_date, MyDBTypes.DateTime);
                    flag=db.Update("lessons_data", " level_id=" + b.level_id + " and teach_id=" + b.teach_id+" and batch_id=" + b.batch_id + " and stud_id=" + b.stud_id + " and lesson_id=" + b.lesson_id + "");
                    b.is_success = flag;
                }
            }
            db.Dispose();

        }
    }
}
