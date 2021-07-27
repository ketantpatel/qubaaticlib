using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class TopicAdapter : MasterAdapter
    {
        public TopicAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }
        public DataTable ListTopic(string level_id)
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from topics where level_id=" + level_id + "");
            db.Dispose();
            return dt;
        }
        public TopicBean SaveTopic(TopicBean t)
        {
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("topic", t.topic, MyDBTypes.Varchar);
            db.AddParameters("status", t.status, MyDBTypes.Int);
            db.AddParameters("level_id", t.level_id, MyDBTypes.Int);
            if (t.topic_id == "-1")
            {
                db.Insert("topics");
                t.topic_id = db.GetLastAutoID().ToString();
            }
            else
            {
                db.Update("topics", "topic_id=" + t.topic_id);
            }
            return t;
        }
        public List<TopicBean> FillTopics(string level_id)
        {
            List<TopicBean> list = new List<TopicBean>();
            TopicBean t = new TopicBean();
            t.topic_id = "-1";
            t.topic = "--Select--";
            list.Add(t);

            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from topics where level_id=" + level_id + "");
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    t = new TopicBean();
                    t.topic_id = dr["topic_id"].ToString();
                    t.topic = dr["topic"].ToString();
                    list.Add(t);
                }
            }
            db.Dispose();

            return list;
        }
    }
}
