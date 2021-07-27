using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class QuestionBankAdapter : MasterAdapter
    {
        public QuestionBankAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }
        public int TotalQuestions(string topic_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select count(*) from question_bank where topic_id=" + topic_id + "";
            string value = db.GetSingleValue(query);
            db.Dispose();
            int found = 0;
            int.TryParse(value, out found);
            return found;
        }
        public DataTable QuestionList(string level_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from question_bank where level_id="+level_id+"";
            DataTable dt = db.GetDataTable(query);
            db.Dispose();
            return dt;
        }

        public DataTable ExamQuestionList(string exam_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from question_bank where question_id in (select question_id from exam_questions where exam_id="+exam_id+")";
            DataTable dt = db.GetDataTable(query);
            db.Dispose();
            return dt;
        }
        public PaperSessionBean GetPaperSession(string exam_id,string form_id,string exam_time)
        {
            PaperSessionBean bean = new PaperSessionBean();
            bean.exam_id = exam_id;
            bean.form_id = form_id;
            bean.exam_time = exam_time;
            bean.start_date = DateTime.Now.ToString();
            bean.modify_date = DateTime.Now.ToString();
            bean.expire_time= DateTime.Now.AddMinutes(double.Parse(exam_time)).ToString();
            DLS db = new DLS(this.AppUserBean);
            DataRow value = db.GetSingleDataRow("select * from paper_sessions where exam_id="+exam_id+" and form_id="+form_id+"");
          
            int found = 0;
            if (value != null)
            {
                string session_id = value["session_id"].ToString();
                int.TryParse(session_id, out found);
            }
            if (found == 0)
            {
                db.AddParameters("exam_id", exam_id, MyDBTypes.Int);
                db.AddParameters("form_id", form_id, MyDBTypes.Int);
                db.AddParameters("start_date", DateTime.Parse(bean.start_date), MyDBTypes.DateTime);
                db.AddParameters("modify_date", DateTime.Parse(bean.modify_date), MyDBTypes.DateTime);
                db.AddParameters("exam_time", exam_time, MyDBTypes.Int);
                db.AddParameters("expire_time",DateTime.Parse(bean.expire_time), MyDBTypes.Int);
                db.Insert("paper_sessions");
            }
            else
            {
                db.AddParameters("modify_date", DateTime.Now, MyDBTypes.DateTime);
                db.Update("paper_sessions","exam_id="+exam_id+" and form_id="+form_id+"");

                bean.start_date = value["start_date"].ToString();
                bean.expire_time = DateTime.Parse(value["expire_time"].ToString()).ToString("HH:mm:ss");
            }
            bean.expire_minute = exam_time;
            db.Dispose();
            return bean;
        }
        public List<QuestionBankBean> GetQuestionPaper(string exam_id)
        {
            List<QuestionBankBean> list = new List<QuestionBankBean>();
            DLS db = new DLS(this.AppUserBean);
            string query = "select * from question_bank where question_id in (select question_id from exam_questions where exam_id=" + exam_id + ")";
            DataTable dt = db.GetDataTable(query);
            db.Dispose();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    QuestionBankBean q = new QuestionBankBean();
                    q.question_text = dr["question_text"].ToString();
                    q.answer=dr["answer"].ToString();
                    q.question_id = dr["question_id"].ToString();
                    list.Add(q);
                }
            }
            return list;
        }
        public bool SaveQuestion(QuestionBankBean q)
        {
            DLS db = new DLS(this.AppUserBean);
            int found = 0;
            string value = db.GetSingleValue("select count(*) from question_bank where question_text_series='"+q.question_text_series+"'");
            int.TryParse(value, out found);
            if (found == 0)
            {
                db.AddParameters("question_text", q.question_text, MyDBTypes.Varchar);
                db.AddParameters("operator_series", q.operator_series, MyDBTypes.Varchar);
                db.AddParameters("question_text_series", q.question_text_series, MyDBTypes.Varchar);
                db.AddParameters("answer", q.answer, MyDBTypes.Varchar);
                db.AddParameters("level_id", q.level_id, MyDBTypes.Int);
                if(!string.IsNullOrEmpty(q.topic_id))
                    db.AddParameters("topic_id", q.topic_id, MyDBTypes.Int);

                db.Insert("question_bank");
            }
            db.Dispose();
            return true;
        }
        
    }
}
