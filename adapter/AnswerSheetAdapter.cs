using MDACLib.beans;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class AnswerSheetAdapter : MasterAdapter
    {
        public AnswerSheetAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }
        public AnswerSheetBean SaveAnswer(AnswerSheetBean bean)
        {
            if (bean.question_json == null)
                return bean;
             JObject question=JObject.Parse(bean.question_json);
             bean.question_id = question["question_id"].ToString();
            string query = "select count(*) from paper_answersheet where form_id="+bean.form_id+" and exam_id="+bean.exam_id+" and question_id="+bean.question_id+"";
            DLS db = new DLS(this.AppUserBean);
            string value = db.GetSingleValue(query);
           
            int found = 0;
            int.TryParse(value, out found);
            if (!string.IsNullOrEmpty(bean.answer))
            {
                if (question["answer"].ToString().ToLower() == bean.answer.ToString().ToLower())
                {
                    bean.is_checked = "1";
                    bean.is_correct = "1";
                }
                else
                {
                    bean.is_checked = "1";
                    bean.is_correct = "0";
                }
            }
            db.AddParameters("modify_date", DateTime.Now, MyDBTypes.Int);
            if(!string.IsNullOrEmpty(bean.is_checked))
            db.AddParameters("is_checked", bean.is_checked, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(bean.is_correct))
            db.AddParameters("is_correct", bean.is_correct, MyDBTypes.Int);
            
            db.AddParameters("submit_date", DateTime.Now, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(bean.answer))
                db.AddParameters("answer", bean.answer, MyDBTypes.Int);
            if (found==0)
            {
                if (!string.IsNullOrEmpty(bean.exam_id))
                db.AddParameters("exam_id", bean.exam_id, MyDBTypes.Int);
                if (!string.IsNullOrEmpty(bean.question_id))
                db.AddParameters("question_id", bean.question_id, MyDBTypes.Int);
                if (!string.IsNullOrEmpty(bean.marks))
                    db.AddParameters("marks", bean.marks, MyDBTypes.Int);
                db.AddParameters("correct_answer", question["answer"], MyDBTypes.Int);
                if (!string.IsNullOrEmpty(bean.form_id))
                    db.AddParameters("form_id", bean.form_id, MyDBTypes.Int);
                if (!string.IsNullOrEmpty(bean.stud_id))
                db.AddParameters("stud_id", bean.stud_id, MyDBTypes.Int);
                db.AddParameters("create_date", DateTime.Now, MyDBTypes.DateTime);
            bool flag=    db.Insert("paper_answersheet");

            }
            else
            {
                db.Update("paper_answersheet","form_id="+bean.form_id+" and exam_id="+bean.exam_id+" and question_id="+bean.question_id+"");
            }
            db.Dispose();
            return bean;
        }
    }
}
