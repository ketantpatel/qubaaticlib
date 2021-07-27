using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class ExamLevelAdapter : MasterAdapter
    {
        public ExamLevelAdapter(UserBean ubean)
        {
            this.AppUserBean = ubean;
        }
       

        public List<ExamLevel> FillLevels(bool isSelect)
        {
            DLS db = new DLS(this.AppUserBean);
            List<ExamLevel> list = new List<ExamLevel>();
            if (isSelect)
            {
                ExamLevel d = new ExamLevel();
                d.level_id = "";
                d.title = "--Select--";
                list.Add(d);
            }
            string query = "select level_id,title from exam_level where is_active=1";
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ExamLevel d = new ExamLevel();
                    d.level_id = dr["level_id"].ToString();
                    d.title = dr["title"].ToString();
                    list.Add(d);
                }
            }
            db.Dispose();
            return list;
        }
    }
}
