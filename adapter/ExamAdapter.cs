using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class ExamAdapter : MasterAdapter
    {
        public List<ExamLevelBean> ListLevel()
        {
            List<ExamLevelBean> list = new List<ExamLevelBean>();
            string query = @"select * from exam_levels";
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            db.Dispose();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ExamLevelBean l = new ExamLevelBean();
                    l.level_id = dr["level_id"].ToString();
                    l.level_title = dr["level_title"].ToString();

                    list.Add(l);
                }
            }
            return list;
        }

        public DataRow LevelInfo(string level_id)
        {
            DLS dls = new DLS(this.AppUserBean);
            DataRow singleDataRow = dls.GetSingleDataRow("select * from exam_levels where level_id=" + level_id);
            dls.Dispose();
            return singleDataRow;
        }
        public ExamAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }
        public void DeleteExamTopic(string ex_topic_id)
        {
            DLS db = new DLS(this.AppUserBean);
            db.Delete("exam_topics", "ex_topic_id=" + ex_topic_id + "");
            db.Dispose();
        }
        public ExamTopicBean SaveExamTopic(ExamTopicBean ex)
        {
            DLS db = new DLS(this.AppUserBean);
            string value = db.GetSingleValue("select count(*) from exam_topics where exam_id=" + ex.exam_id + " and topic_id=" + ex.topic_id + "");
            int found = 0;
            int.TryParse(value, out found);
            if (found == 0)
            {
                db.AddParameters("topic_id", ex.topic_id, MyDBTypes.Int);
                db.AddParameters("exam_id", ex.exam_id, MyDBTypes.Int);
                db.AddParameters("question_nos", ex.question_nos, MyDBTypes.Int);
                db.Insert("exam_topics");
            }
            else
            {
                db.AddParameters("question_nos", ex.question_nos, MyDBTypes.Int);
                db.Update("exam_topics", "exam_id=" + ex.exam_id + " and topic_id=" + ex.topic_id + "");
            }
            db.Dispose();
            return ex;
        }
        public DataTable ListExamTopics(string exam_id)
        {
            string query = "select et.ex_topic_id,et.topic_id,et.question_nos,et.exam_id,t.topic from exam_topics et inner join topics t on t.topic_id=et.topic_id where et.exam_id=" + exam_id + "";
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            db.Dispose();
            return dt;
        }
        public ExamBean SubmitExam(ExamBean exam)
        {
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("is_terminate", 1, MyDBTypes.Int);
            db.AddParameters("modify_date", DateTime.Now, MyDBTypes.DateTime);
            db.Update("paper_sessions", "form_id=" + exam.form_id + " and exam_id=" + exam.exam_id + "");
            db.Dispose();
            return exam;
        }
        public void GenerateResult(string exam_id)
        {
            string query = @"select distinct frn_id from admission_forms where form_id in (select form_id from paper_sessions where exam_id=" + exam_id + ")";
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            db.Dispose();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string frn_id = dr["frn_id"].ToString();
                    FranchiseResultGenerate(exam_id, frn_id);
                }
            }
        }
        public List<ResultBean> FranchiseResultGenerate(string exam_id, string frn_id)
        {
            string query = @"select f.form_id,concat(f.first_name,' ',f.last_name) name,f.enroll_no,s.start_date,s.expire_time,s.modify_date from paper_sessions s
left join admission_forms f on f.form_id=s.form_id
where exam_id=" + exam_id + "";
            DLS db = new DLS(this.AppUserBean);
            DataRow dtRows = db.GetSingleDataRow("select passing_marks,exam_type,level_id,total_questions,each_question_marks from exams where exam_id=" + exam_id + "");
            float passingmarks = 0;
            float.TryParse(dtRows["passing_marks"].ToString(), out passingmarks);

            float each_question_marks = 0;
            float.TryParse(dtRows["each_question_marks"].ToString(), out each_question_marks);

            DataTable dt = db.GetDataTable(query);
            List<ResultBean> list = new List<ResultBean>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ResultBean r = new ResultBean();
                    r.form_id = dr["form_id"].ToString();
                    r.full_name = dr["name"].ToString();
                    r.enroll_no = dr["enroll_no"].ToString();
                    r.start_time = dr["start_date"].ToString();
                    r.expire_time = dr["expire_time"].ToString();
                    r.terminate_time = dr["expire_time"].ToString();
                    DataRow dtRow = db.GetSingleDataRow(@"select count(*) total,sum(is_correct) correct,sum(attempt_count) attempted from (select e.question_id,a.answer,a.correct_answer,a.is_checked,a.marks,is_correct,
case when a.correct_answer is null then 0 else 1 end attempt_count from exam_questions e left join paper_answersheet a on a.exam_id=e.exam_id
and a.form_id=" + r.form_id + " and e.question_id=a.question_id where e.exam_id=" + exam_id + ") as main");
                    if (dtRow != null)
                    {
                        r.total_questions = dtRow["total"].ToString();
                        r.total_attempted = dtRow["attempted"].ToString();
                        r.total_correct = dtRow["correct"].ToString();
                    }
                    float correctmarks = 0;
                    float.TryParse(r.total_correct, out correctmarks);
                    correctmarks = correctmarks * each_question_marks;
                    //  if (correctmarks>=passingmarks >= correctmarks)
                    // {
                    db.AddParameters("score", correctmarks, MyDBTypes.Int);
                    bool isPass = false;
                    if (correctmarks >= passingmarks)
                    {
                        isPass = true;
                        //pass
                        db.AddParameters("result_status", 1, MyDBTypes.Int);
                    }
                    else
                    {
                        //fail
                        db.AddParameters("result_status", 2, MyDBTypes.Int);
                    }
                    db.Update("paper_sessions", "form_id=" + r.form_id + " and exam_id=" + exam_id + "");

                    if (dtRows["exam_type"].ToString() == "2" && isPass)
                    {
                        db.AddParameters("current_level", dtRows["level_id"].ToString(), MyDBTypes.Int);
                        db.Update("admission_forms", "form_id=" + r.form_id + "");
                    }
                    //}
                    list.Add(r);
                }
            }
            db.Dispose();

            return list;
        }
        public List<ResultBean> FranchiseResult(string exam_id, string frn_id)
        {
            string query = @"select s.score,f.form_id,concat(f.first_name,' ',f.last_name) name,f.enroll_no,s.start_date,s.expire_time,s.modify_date from paper_sessions s
left join admission_forms f on f.form_id=s.form_id
where exam_id=" + exam_id + "";
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            List<ResultBean> list = new List<ResultBean>();
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ResultBean r = new ResultBean();
                    r.form_id = dr["form_id"].ToString();
                    r.full_name = dr["name"].ToString();
                    r.enroll_no = dr["enroll_no"].ToString();
                    r.start_time = dr["start_date"].ToString();
                    r.score = dr["score"].ToString();
                    r.expire_time = dr["expire_time"].ToString();
                    r.terminate_time = dr["expire_time"].ToString();
                    DataRow dtRow = db.GetSingleDataRow(@"select count(*) total,sum(is_correct) correct,sum(attempt_count) attempted from (select e.question_id,a.answer,a.correct_answer,a.is_checked,a.marks,is_correct,
case when a.correct_answer is null then 0 else 1 end attempt_count from exam_questions e left join paper_answersheet a on a.exam_id=e.exam_id
and a.form_id=" + r.form_id + " and e.question_id=a.question_id where e.exam_id=" + exam_id + ") as main");
                    if (dtRow != null)
                    {
                        r.total_questions = dtRow["total"].ToString();
                        r.total_attempted = dtRow["attempted"].ToString();
                        r.total_correct = dtRow["correct"].ToString();
                    }
                    list.Add(r);
                }
            }
            db.Dispose();

            return list;
        }
        public List<ExamQuestionBean> FindQuestionForExams(string total_qestions, string question_choice, string exam_id)
        {
            DLS db = new DLS(this.AppUserBean);
            DataTable dtTopics = db.GetDataTable("select * from exam_topics where exam_id=" + exam_id + "");
            DataTable dt = null;
            List<ExamQuestionBean> list = new List<ExamQuestionBean>();
            if (dtTopics != null)
            {
                foreach(DataRow dtTotalRow in dtTopics.Rows)
                {
                    total_qestions = dtTotalRow["question_nos"].ToString();
                    string topic_id = dtTotalRow["topic_id"].ToString();
                    string query = "select top " + total_qestions + " * from question_bank where question_id not in (select question_id from exam_questions)";
                    if (question_choice == "1")
                    {
                        query = "select * from question_bank  where  topic_id=" + topic_id + " limit 0," + total_qestions + " ";
                    }
                    else if (question_choice == "2")
                    {
                        query = "select  * from question_bank where question_id not in (select question_id from exam_questions)  and topic_id=" + topic_id + " limit 0," + total_qestions + "  ";
                    }

                    dt = db.GetDataTable(query);
                
                    if (dt != null)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            ExamQuestionBean q = new ExamQuestionBean();
                            q.question_id = dr["question_id"].ToString();
                            q.level_id = dr["level_id"].ToString();
                            list.Add(q);
                        }
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public ExamBean GetExamInfo(string exam_id)
        {
            DLS db = new DLS(this.AppUserBean);
            DataRow dr = db.GetSingleDataRow("select * from exams where exam_id=" + exam_id + "");
            db.Dispose();
            ExamBean e = new ExamBean();
            if (dr != null)
            {
                e.title = dr["title"].ToString();
                e.total_questions = dr["total_questions"].ToString();
                e.from_time = dr["from_time"].ToString();
                e.each_question_marks = dr["each_question_marks"].ToString();
                e.to_time = dr["to_time"].ToString();
                e.exam_level = dr["level_id"].ToString();
                e.exam_type = dr["exam_type"].ToString();
                e.choice_type = dr["choice_type"].ToString();
                e.hours = dr["hours"].ToString();
                e.weightage = dr["weightage"].ToString();
                e.passing_marks = dr["passing_marks"].ToString();
            }
            return e;
        }
        public List<ExamBean> ExamList(string form_id)
        {
            List<ExamBean> list = new List<ExamBean>();

            DLS db = new DLS(this.AppUserBean);
            string level_completed = db.GetSingleValue("select current_level from admission_forms where form_id=" + form_id + "");
            int level_id = 0;
            int.TryParse(level_completed, out level_id);
            DataTable dt = db.GetDataTable("select e.*,f.is_terminate from exams e left join paper_sessions f on f.form_id=" + form_id + " and e.exam_id=f.exam_id order by from_time desc");

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ExamBean e = new ExamBean();
                    e.title = dr["title"].ToString();
                    e.exam_id = dr["exam_id"].ToString();
                    e.total_questions = dr["total_questions"].ToString();
                    e.from_time = dr["from_time"].ToString();
                    e.to_time = dr["to_time"].ToString();
                    e.exam_level = dr["level_id"].ToString();
                    e.exam_type = dr["exam_type"].ToString();
                    e.choice_type = dr["choice_type"].ToString();
                    e.hours = dr["hours"].ToString();
                    e.weightage = dr["weightage"].ToString();
                    e.passing_marks = dr["passing_marks"].ToString();
                    e.is_terminate = dr["is_terminate"].ToString();

                    e.is_expire = false;
                    int exam_level_id = 0;
                    int.TryParse(e.exam_level, out exam_level_id);

                    bool is_terminate = false;
                    bool.TryParse(e.is_terminate, out is_terminate);
                    if (DateTime.Parse(e.to_time) < DateTime.Now)
                    {
                        e.is_expire = true;
                        e.status = "3";//expire
                    }
                    else if (DateTime.Parse(e.from_time) <= DateTime.Now && DateTime.Parse(e.to_time) >= DateTime.Now)
                    {
                        e.status = "1"; // start                       
                    }
                    else if (DateTime.Parse(e.to_time) > DateTime.Now)
                    {
                        e.status = "2"; // later comming                        
                    }
                    if (is_terminate)
                    {
                        e.status = "3";
                    }
                    if (exam_level_id <= level_id)
                    {
                        list.Add(e);
                    }
                    else if ((level_id + 1) == exam_level_id)
                    {
                        list.Add(e);
                    }
                }
            }
            db.Dispose();
            return list;
        }
        public List<ExamBean> ExamList()
        {
            List<ExamBean> list = new List<ExamBean>();

            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from exams");

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ExamBean e = new ExamBean();
                    e.title = dr["title"].ToString();
                    e.exam_id = dr["exam_id"].ToString();
                    e.total_questions = dr["total_questions"].ToString();
                    e.from_time = dr["from_time"].ToString();
                    e.to_time = dr["to_time"].ToString();
                    e.exam_level = dr["level_id"].ToString();
                    e.exam_type = dr["exam_type"].ToString();
                    e.choice_type = dr["choice_type"].ToString();
                    e.hours = dr["hours"].ToString();
                    e.weightage = dr["weightage"].ToString();
                    e.passing_marks = dr["passing_marks"].ToString();
                    e.is_expire = false;
                    if (DateTime.Parse(e.to_time) < DateTime.Now)
                    {
                        e.is_expire = true;
                        e.status = "3";//expire
                    }
                    else if (DateTime.Parse(e.from_time) <= DateTime.Now && DateTime.Parse(e.to_time) >= DateTime.Now)
                    {
                        e.status = "1"; // start
                    }
                    else if (DateTime.Parse(e.to_time) > DateTime.Now)
                    {
                        e.status = "2";// later comming
                    }
                    list.Add(e);
                }
            }
            db.Dispose();
            return list;
        }
        public List<ExamBean> LevelExamList(string level_id)
        {
            List<ExamBean> list = new List<ExamBean>();

            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable("select * from exams where level_id=" + level_id + "");

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    ExamBean e = new ExamBean();
                    e.title = dr["title"].ToString();
                    e.exam_id = dr["exam_id"].ToString();
                    e.total_questions = dr["total_questions"].ToString();
                    e.from_time = dr["from_time"].ToString();
                    e.to_time = dr["to_time"].ToString();
                    e.exam_level = dr["level_id"].ToString();
                    e.exam_type = dr["exam_type"].ToString();
                    e.choice_type = dr["choice_type"].ToString();
                    e.hours = dr["hours"].ToString();
                    e.weightage = dr["weightage"].ToString();
                    e.passing_marks = dr["passing_marks"].ToString();
                    e.is_expire = false;
                    if (DateTime.Parse(e.to_time) < DateTime.Now)
                    {
                        e.is_expire = true;
                        e.status = "3";//expire
                    }
                    else if (DateTime.Parse(e.from_time) <= DateTime.Now && DateTime.Parse(e.to_time) >= DateTime.Now)
                    {
                        e.status = "1"; // start
                    }
                    else if (DateTime.Parse(e.to_time) > DateTime.Now)
                    {
                        e.status = "2";// later comming
                    }
                    list.Add(e);
                }
            }
            db.Dispose();
            return list;
        }
        public bool SaveExamQuestions(string exam_id, List<ExamQuestionBean> questions)
        {
            DLS db = new DLS(this.AppUserBean);
            if (questions != null)
            {
                if (questions.Count > 0)
                {
                    db.Delete("exam_questions", "exam_id=" + exam_id);
                    foreach (ExamQuestionBean exam in questions)
                    {

                        db.AddParameters("exam_id", exam_id, MyDBTypes.Int);
                        db.AddParameters("question_id", exam.question_id, MyDBTypes.Int);
                        db.AddParameters("level_id", exam.level_id, MyDBTypes.Int);
                        if (db.Insert("exam_questions"))
                        {
                            exam.exam_question_id = db.GetLastAutoID().ToString();
                        }
                    }
                }
            }

            db.Dispose();
            return true;
        }

        public ExamBean SaveExam(ExamBean exam)
        {
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("from_time", DateTime.Parse(exam.from_time), MyDBTypes.DateTime);
            db.AddParameters("to_time", DateTime.Parse(exam.to_time), MyDBTypes.DateTime);
            db.AddParameters("exam_type", exam.exam_type, MyDBTypes.Int);
            db.AddParameters("title", exam.title, MyDBTypes.Int);
            db.AddParameters("choice_type", exam.choice_type, MyDBTypes.Int);
            db.AddParameters("total_questions", exam.total_questions, MyDBTypes.Int);
            db.AddParameters("create_date", DateTime.Now, MyDBTypes.DateTime);
            db.AddParameters("modify_date", DateTime.Now, MyDBTypes.DateTime);
            db.AddParameters("level_id", exam.exam_level, MyDBTypes.Int);
            db.AddParameters("hours", exam.hours, MyDBTypes.Int);
            db.AddParameters("weightage", exam.weightage, MyDBTypes.Int);
            db.AddParameters("passing_marks", exam.passing_marks, MyDBTypes.Int);
            db.AddParameters("each_question_marks", exam.each_question_marks, MyDBTypes.Int);

            if (exam.exam_id == "-1")
            {
                if (db.Insert("exams"))
                {
                    exam.exam_id = db.GetLastAutoID().ToString();
                }
            }
            else
            {
                db.Update("exams", "exam_id=" + exam.exam_id + "");
            }
            db.Dispose();
            return exam;
        }
        public void SaveMarks(ExamMarkBean mbean)
        {
            DLS db = new DLS(AppUserBean);
            int recordFoundCount = 0;
            string existCount = db.GetSingleValue("select count(*) from exam_marks where course_id=" + mbean.course_id + " and subject_id=" + mbean.subject_id + " and stud_id=" + mbean.stud_id + "");
            if (!string.IsNullOrEmpty(existCount))
            {
                recordFoundCount = int.Parse(existCount);
            }

            if (!string.IsNullOrEmpty(mbean.course_id))
                db.AddParameters("course_id", mbean.course_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(mbean.subject_id))
                db.AddParameters("subject_id", mbean.subject_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(mbean.stud_id))
                db.AddParameters("stud_id", mbean.stud_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(mbean.marks))
                db.AddParameters("marks", mbean.marks, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(mbean.frn_id))
                db.AddParameters("frn_id", mbean.frn_id, MyDBTypes.Int);

            if (recordFoundCount == 0)
            {
                if (!string.IsNullOrEmpty(mbean.create_date))
                    db.AddParameters("create_date", mbean.create_date, MyDBTypes.DateTime);
            }
            if (!string.IsNullOrEmpty(mbean.modify_date))
                db.AddParameters("modify_date", mbean.modify_date, MyDBTypes.DateTime);

            if (recordFoundCount == 0)
            {
                db.Insert("exam_marks");
            }
            else
            {
                db.Update("exam_marks", "course_id=" + mbean.course_id + " and subject_id=" + mbean.subject_id + " and stud_id=" + mbean.stud_id + "");
            }
            db.Dispose();
        }
        public void SaveMarkByEnrollNo(ExamMarkBean mbean)
        {
            mbean.stud_id = "-1";
            DLS db = new DLS(AppUserBean);
            DataTable dt = db.GetDataTable("SELECT form_id FROM admission_forms where enroll_no='" + mbean.enroll_no + "'");
            if (dt != null)
            {
                if (dt.Rows.Count > 1)
                {
                    db.Dispose();
                    return;
                }
                else
                {
                    mbean.stud_id = dt.Rows[0]["form_id"].ToString();
                }
            }

            if (mbean.stud_id == "-1")
            {
                db.Dispose();
                return;
            }
            int recordFoundCount = 0;
            string existCount = db.GetSingleValue("select count(*) from exam_marks where course_id=" + mbean.course_id + " and parent_course_id=" + mbean.parent_course_id + " and subject_id=" + mbean.subject_id + " and stud_id=" + mbean.stud_id + " and exam_type=" + mbean.exam_type + "");
            if (!string.IsNullOrEmpty(existCount))
            {
                recordFoundCount = int.Parse(existCount);
            }

            if (!string.IsNullOrEmpty(mbean.course_id))
                db.AddParameters("course_id", mbean.course_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(mbean.subject_id))
                db.AddParameters("subject_id", mbean.subject_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(mbean.stud_id))
                db.AddParameters("stud_id", mbean.stud_id, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(mbean.marks))
                db.AddParameters("marks", mbean.marks, MyDBTypes.Int);
            if (!string.IsNullOrEmpty(mbean.frn_id))
                db.AddParameters("frn_id", mbean.frn_id, MyDBTypes.Int);

            if (recordFoundCount == 0)
            {
                if (!string.IsNullOrEmpty(mbean.create_date))
                    db.AddParameters("create_date", mbean.create_date, MyDBTypes.DateTime);
            }
            if (!string.IsNullOrEmpty(mbean.modify_date))
                db.AddParameters("modify_date", mbean.modify_date, MyDBTypes.DateTime);

            if (!string.IsNullOrEmpty(mbean.parent_course_id))
                db.AddParameters("parent_course_id", mbean.parent_course_id, MyDBTypes.Int);

            if (recordFoundCount == 0)
            {
                if (!string.IsNullOrEmpty(mbean.frn_id))
                    db.AddParameters("exam_type", mbean.exam_type, MyDBTypes.Int);
                db.Insert("exam_marks");
            }
            else
            {
                if (db.Update("exam_marks", "course_id=" + mbean.course_id + " and subject_id=" + mbean.subject_id + " and parent_course_id=" + mbean.parent_course_id + " and stud_id=" + mbean.stud_id + " and exam_type=" + mbean.exam_type + ""))
                {
                    mbean.is_success = true;
                }
            }
            db.Dispose();
        }
    }
}
