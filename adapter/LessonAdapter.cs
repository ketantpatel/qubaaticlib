using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class LessonAdapter:MasterAdapter
    {
        public LessonAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }

        public List<LessonBean> FillLesson(bool isSelect,string level_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from lessons where level_id="+level_id;
            List<LessonBean> list = new List<LessonBean>();

            LessonBean fb = new LessonBean();
            fb.lesson_id = "";
            fb.lesson_no = "--Select--";
            fb.lesson_name = "--Select--";
            list.Add(fb);

            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        fb = new LessonBean();
                        fb.lesson_id = dr["lesson_id"].ToString();
                        fb.lesson_no = dr["lesson_no"].ToString();
                        fb.lesson_name = dr["lesson_name"].ToString();
                        list.Add(fb);
                    }
                }
            }
            db.Dispose();
            return list;
        }
    }
}
