using MDACLib.adapter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.beans
{
    public class TeacherAdapter : MasterAdapter
    {
        public TeacherAdapter(UserBean u)
        {
            this.AppUserBean = u;
        }
        public void SaveTeacherCode(TeacherBean iBean, string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            string NewCode = "";
            string code = db.GetSingleValue("select Max(code) from teachers where frn_id =" + frn_id);
            if (string.IsNullOrEmpty(code))
            {
                NewCode = "1";
            }
            else
            {
                NewCode = Convert.ToString(Int32.Parse(code) + 1);
            }

            if (NewCode.Length < 5)
            {
                int requiredLength = 5 - NewCode.Length;
                for (int i = 0; i < requiredLength; i++)
                {
                    NewCode = "0" + NewCode;
                }
            }

            iBean.code = NewCode;
            db.AddParameters("code", iBean.code, MyDBTypes.Varchar);
            db.Update("teachers", "teach_id=" + iBean.teach_id);

            db.Dispose();
        }
        public TeacherBean CreateTeacher(TeacherBean t)
        {
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("first_name", t.first_name, MyDBTypes.Varchar);
            db.AddParameters("last_name", t.last_name, MyDBTypes.Varchar);
            db.AddParameters("mobile", t.mobile, MyDBTypes.Varchar);
            db.AddParameters("email", t.email, MyDBTypes.Varchar);
            db.AddParameters("birthdate", t.birthdate, MyDBTypes.DateTime);
            db.AddParameters("education_qualification", t.education_qualification, MyDBTypes.Varchar);
            db.AddParameters("teaching_experience", t.teaching_experience, MyDBTypes.Varchar);
            db.AddParameters("residential_address", t.residential_address, MyDBTypes.Varchar);
            db.AddParameters("location", t.location, MyDBTypes.Varchar);
            db.AddParameters("state_id", t.state_id, MyDBTypes.Int);
            db.AddParameters("level_id", t.level_id, MyDBTypes.Int);
            db.AddParameters("class_taken", t.class_taken, MyDBTypes.Varchar);
            db.AddParameters("city", t.city, MyDBTypes.Varchar);
            db.AddParameters("refresh_training_level", t.refresh_training_level, MyDBTypes.Int);
            db.AddParameters("last_level_trained", t.last_level_trained, MyDBTypes.Int);
            db.AddParameters("last_level_trained_date", t.last_level_trained_date, MyDBTypes.DateTime);
            db.AddParameters("partipation_level", t.partipation_level, MyDBTypes.Int);
            db.AddParameters("partipation_level_Date", t.partipation_level_Date, MyDBTypes.Int);
            db.AddParameters("country_id", t.country_id, MyDBTypes.Int);
            db.AddParameters("frn_id", t.frn_id, MyDBTypes.Int);
            db.AddParameters("create_date", t.create_date, MyDBTypes.DateTime);
            db.AddParameters("modify_date", t.modify_date, MyDBTypes.DateTime);
            if (db.Insert("teachers"))
            {
                t.is_success = true;
                t.teach_id = db.GetLastAutoID().ToString();


                UserBean ub = new UserBean();
                ub.username = t.email;
                ub.password = clsFunctions.CreateRandomPassword(4);
                ub.password = "1";
                ub.user_type = "7";
                ub.role = "7";
                ub.is_active = "1";
                ub.state_id = this.AppUserBean.state_id;
                ub.country_id = this.AppUserBean.country_id;
                ub.first_name = t.first_name;
                ub.last_name = t.last_name;
                ub.create_date = CurrentDateSQL();
                ub.modify_date = CurrentDateSQL();
                ub.user_id = "-1";
                UserAdapter ua = new UserAdapter(this.AppUserBean);
                ub = ua.SaveUser(ub);
                if (!ub.is_opr_success)
                {
                    db.Delete("teachers", t.teach_id);
                    db.Dispose();
                }
                else
                {
                    db.AddParameters("user_id", ub.user_id, MyDBTypes.Int);
                    db.Update("teachers", "teach_id=" + t.teach_id);
                    db.Dispose();
                }
            }
            else
            {
                db.Dispose();
            }
            return t;
        }

        public TeacherBean UpdateTeacher(TeacherBean t)
        {
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("first_name", t.first_name, MyDBTypes.Varchar);
            db.AddParameters("last_name", t.last_name, MyDBTypes.Varchar);
            db.AddParameters("mobile", t.mobile, MyDBTypes.Varchar);
            db.AddParameters("email", t.email, MyDBTypes.Varchar);
            db.AddParameters("birthdate", t.birthdate, MyDBTypes.DateTime);
            db.AddParameters("education_qualification", t.education_qualification, MyDBTypes.Varchar);
            db.AddParameters("teaching_experience", t.teaching_experience, MyDBTypes.Varchar);
            db.AddParameters("residential_address", t.residential_address, MyDBTypes.Varchar);
            db.AddParameters("location", t.location, MyDBTypes.Varchar);
            db.AddParameters("city", t.city, MyDBTypes.Varchar);
            db.AddParameters("state_id", t.state_id, MyDBTypes.Int);
            db.AddParameters("level_id", t.level_id, MyDBTypes.Int);
            db.AddParameters("class_taken", t.class_taken, MyDBTypes.Varchar);
            db.AddParameters("refresh_training_level", t.refresh_training_level, MyDBTypes.Int);
            db.AddParameters("last_level_trained", t.last_level_trained, MyDBTypes.Int);
            db.AddParameters("last_level_trained_date", t.last_level_trained_date, MyDBTypes.DateTime);
            db.AddParameters("partipation_level", t.partipation_level, MyDBTypes.Int);
            db.AddParameters("partipation_level_Date", t.partipation_level_Date, MyDBTypes.Int);
            db.AddParameters("country_id", t.country_id, MyDBTypes.Int);
            db.AddParameters("modify_date", t.modify_date, MyDBTypes.DateTime);
            db.AddParameters("frn_id", t.frn_id, MyDBTypes.Int);

            if (db.Update("teachers", "teach_id=" + t.teach_id))
            {
                t.is_success = true;
            }
            else
            {
                db.Dispose();
            }
            return t;
        }
        public List<TeacherBean> ListTeachers(string country_id, string state_id)
        {
            List<TeacherBean> list = new List<TeacherBean>();
            string query = "select * from teachers where country_id=" + country_id + " and state_id=" + state_id + "";
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TeacherBean t = new TeacherBean();
                    t.first_name = dr["first_name"].ToString();
                    t.last_name = dr["last_name"].ToString();
                    t.mobile = dr["mobile"].ToString();
                    t.email = dr["email"].ToString();
                    t.teach_id = dr["teach_id"].ToString();
                    list.Add(t);
                }
            }
            return list;
        }
        public List<TeacherBean> FillFranchTeachers(string frn_id)
        {
            List<TeacherBean> list = new List<TeacherBean>();
            TeacherBean t = new TeacherBean();
            t.teach_id = "";
            t.full_name = "--Select--";
            list.Add(t);
            string query = "SELECT * FROM teachers where teach_id in (select teach_id from teacher_franchise_allocation where frn_id=" + frn_id + ")";
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    t = new TeacherBean();
                    t.first_name = dr["first_name"].ToString();
                    t.last_name = dr["last_name"].ToString();
                    t.mobile = dr["mobile"].ToString();
                    t.email = dr["email"].ToString();
                    t.teach_id = dr["teach_id"].ToString();
                    t.full_name = t.first_name + " " + t.last_name;
                    list.Add(t);
                }
            }
            return list;
        }
        //public List<TeacherBean> ListTeachers(string teach_id, string country_id, string state_id)
        //{
        //    List<TeacherBean> list = new List<TeacherBean>();
        //    string query = "select * from teachers where country_id=" + country_id + " and state_id=" + state_id + "";
        //    DLS db = new DLS(this.AppUserBean);
        //    DataTable dt = db.GetDataTable(query);
        //    if (dt != null)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            TeacherBean t = new TeacherBean();
        //            t.first_name = dr["first_name"].ToString();
        //            t.last_name = dr["last_name"].ToString();
        //            t.mobile = dr["mobile"].ToString();
        //            t.email = dr["email"].ToString();
        //            t.teach_id = dr["teach_id"].ToString();
        //            list.Add(t);
        //        }
        //    }
        //    return list;
        //}
        public TeacherBean TeacherInfoFromUserID(string user_id)
        {
            TeacherBean t = new TeacherBean();

            string query = "select * from teachers where user_id=" + user_id;
            DLS db = new DLS(this.AppUserBean);
            DataRow dr = db.GetSingleDataRow(query);
            if (dr != null)
            {
                t.first_name = dr["first_name"].ToString();
                t.last_name = dr["last_name"].ToString();
                t.mobile = dr["mobile"].ToString();
                t.email = dr["email"].ToString();
                t.teach_id = dr["teach_id"].ToString();
            }
            return t;
        }
        public TeacherBean TeacherInfo(string teach_id)
        {
            string query = "select * from teachers where teach_id=" + teach_id;
            DLS db = new DLS(this.AppUserBean);
            DataRow dr = db.GetSingleDataRow(query);
            TeacherBean t = new TeacherBean();
            if (dr != null)
            {
                t.first_name = dr["first_name"].ToString();
                t.last_name = dr["last_name"].ToString();
                t.mobile = dr["mobile"].ToString();
                t.email = dr["email"].ToString();
                t.teach_id = dr["teach_id"].ToString();
                t.state_id = dr["state_id"].ToString();
                t.country_id = dr["country_id"].ToString();
                if (dr["last_level_trained"].ToString() != "")
                {
                    t.last_level_trained = Convert.ToInt32(dr["last_level_trained"].ToString());
                }

                t.last_level_trained_date = dr["last_level_trained_date"].ToString();

                if (dr["partipation_level"].ToString() != "")
                {
                    t.partipation_level = Convert.ToInt32(dr["partipation_level"].ToString());
                }

                t.partipation_level_Date = dr["partipation_level_Date"].ToString();
                t.code = dr["code"].ToString();
                t.birthdate = dr["birthdate"].ToString();

                t.education_qualification = dr["education_qualification"].ToString();
                t.teaching_experience = dr["teaching_experience"].ToString();
                t.residential_address = dr["residential_address"].ToString();
                t.location = dr["location"].ToString();
                t.city = dr["city"].ToString();
                if ((dr["level_id"].ToString() != ""))
                {
                    t.level_id = Convert.ToInt32(dr["level_id"].ToString());
                }
                t.class_taken = dr["class_taken"].ToString();

                if ((dr["refresh_training_level"].ToString() != ""))
                {
                    t.refresh_training_level = Convert.ToInt32(dr["refresh_training_level"].ToString());
                }

                if ((dr["frn_id"].ToString() != ""))
                {
                    t.frn_id = Convert.ToInt32(dr["frn_id"].ToString());
                }

            }

            return t;
        }

        public TeacherFacultyBean Allocate(TeacherFacultyBean tb)
        {
            DLS db = new DLS(this.AppUserBean);
            string allocate_id = db.GetSingleValue("select allocate_id from teacher_franchise_allocation where teach_id=" + tb.teach_id + " and frn_id=" + tb.frn_id + "");

            if (tb.is_checked)
            {
                tb.status = "1";

                db.AddParameters("teach_id", tb.teach_id, MyDBTypes.Int);
                db.AddParameters("frn_id", tb.frn_id, MyDBTypes.Int);

                db.AddParameters("status", tb.status, MyDBTypes.Int);
                if (string.IsNullOrEmpty(allocate_id))
                {
                    db.AddParameters("create_date", tb.create_date, MyDBTypes.DateTime);
                    db.AddParameters("modify_date", tb.modify_date, MyDBTypes.DateTime);
                    if (db.Insert("teacher_franchise_allocation"))
                    {
                        tb.allocate_id = db.GetLastAutoID().ToString();
                        tb.is_success = true;
                    }
                }
                else
                {
                    if (db.Update("teacher_franchise_allocation", "allocate_id=" + allocate_id))
                    {
                        tb.is_success = true;
                    }
                }
            }
            else
            {
                tb.status = "0";
                db.AddParameters("status", tb.status, MyDBTypes.Int);
                db.AddParameters("modify_date", tb.modify_date, MyDBTypes.DateTime);
                if (db.Update("teacher_franchise_allocation", "allocate_id=" + allocate_id))
                {
                    tb.is_success = true;
                }
            }
            db.Dispose();
            return tb;
        }


        public bool IsTrainingTitleExist(string title, int id)
        {
            string query = "";
            if (id > 0)
            {
                query = "select count(*) from instructor_training where title='" + title.Trim() + "' and  it_id !=" + id;
            }
            else
            {
                query = "select count(*) from instructor_training where title='" + title.Trim() + "'";
            }

            MDACLib.DLS db = new MDACLib.DLS(this.AppUserBean);
            string value = db.GetSingleValue(query);
            db.Dispose();
            bool isExist = false;
            if (!string.IsNullOrEmpty(value))
            {
                if (int.Parse(value) > 0)
                {
                    isExist = true;
                }
            }
            return isExist;
        }

        public Instructor_Training SaveTeacherTraining(Instructor_Training t,bool AlllowGenerateNewCourseId = true)
        {
            DLS db = new DLS(this.AppUserBean);
            db.AddParameters("title", t.title, MyDBTypes.Varchar);
            db.AddParameters("from_date", t.from_date, MyDBTypes.DateTime);
            db.AddParameters("to_date", t.to_date, MyDBTypes.DateTime);
            db.AddParameters("level_id", t.level_id, MyDBTypes.Int);
            db.AddParameters("days", t.days, MyDBTypes.Int);
            db.AddParameters("week_days", t.week_days, MyDBTypes.Varchar);
            db.AddParameters("location", t.location, MyDBTypes.Varchar);
            db.AddParameters("country_id", t.country_id, MyDBTypes.Int);
            if (AlllowGenerateNewCourseId) { 
                db.AddParameters("user_id", this.AppUserBean.user_id, MyDBTypes.Int);
            }
            else
            {
                db.AddParameters("user_id", t.user_id, MyDBTypes.Int);
            }
            db.AddParameters("created_date", t.created_date, MyDBTypes.DateTime);

            if (t.it_id > 0)
            {
                db.AddParameters("course_id", t.course_id, MyDBTypes.Int);
                db.AddParameters("it_id", t.it_id, MyDBTypes.Int);
                db.AddParameters("modified_date", t.modified_date, MyDBTypes.DateTime);

                if (db.Update("instructor_training", "it_id=" + t.it_id))
                {
                    t.is_success = true;
                }
                else
                {
                    db.Dispose();
                }
            }
            else
            {
                if (AlllowGenerateNewCourseId)
                {
                    db.AddParameters("course_id", Convert.ToInt32(db.GetSingleValue(" select max(course_id) from  instructor_training")) + 1, MyDBTypes.Int);
                }
                else
                {
                     db.AddParameters("course_id", Convert.ToInt32(t.course_id), MyDBTypes.Int);
                    db.AddParameters("teach_id", Convert.ToInt32(t.teach_id), MyDBTypes.Int);

                }

                if (db.Insert("instructor_training"))
                {
                    t.is_success = true;
                }
                else
                {
                    db.Dispose();
                }
            }

            return t;
        }

        public List<Instructor_Training> ListTrainingCourses(string country_id)
        {
            List<Instructor_Training> list = new List<Instructor_Training>();
            string query = "select * from instructor_training where user_id=" + country_id + " group by course_id";
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Instructor_Training t = new Instructor_Training();
                    t.it_id = Convert.ToInt32(dr["it_id"]);
                    t.title = dr["title"].ToString();
                    t.course_id = dr["course_id"].ToString();
                    t.from_date = DateTime.Parse(dr["from_date"].ToString()).ToShortDateString();
                    t.to_date = DateTime.Parse(dr["to_date"].ToString()).ToShortDateString();
                    t.level_id = dr["level_id"].ToString();
                    LevelBean levelBean = new LevelAdapter(this.AppUserBean).FillLevel(t.level_id);
                    if (levelBean != null)
                    {
                        if (!string.IsNullOrEmpty(levelBean.level_title))
                        {
                            t.level_name = levelBean.level_title;
                        }
                    }
                    list.Add(t);
                }
            }
            return list;
        }

        public bool DeleteCourseById(string country_id, string course_id)
        {
            DLS db = new DLS(this.AppUserBean);
            bool IsDelete = db.Delete("instructor_training", "user_id=" + country_id + " and course_id = " + course_id);

            return IsDelete;
        }

        public bool DeleteById(string it_id)
        {
            DLS db = new DLS(this.AppUserBean);
            bool IsDelete = db.Delete("instructor_training", "it_id=" + it_id);
            return IsDelete;
        }

        public Instructor_Training GetTrainingCourseById(string country_id, string it_id = "",string level_id = "",string teach_id ="",string course_id = "")
        {
            Instructor_Training t = new Instructor_Training();
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine("select * from instructor_training where user_id=" + country_id );
            if (!string.IsNullOrEmpty(it_id))
            {
                strQuery.AppendLine(" and it_id = " + it_id);
            }
            if (!string.IsNullOrEmpty(level_id))
            {
                strQuery.AppendLine(" and level_id = " + level_id);
            }
            

            if (!string.IsNullOrEmpty(teach_id))
            {
                strQuery.AppendLine(" and teach_id = " + teach_id);
            }
            if (!string.IsNullOrEmpty(course_id))
            {
                strQuery.AppendLine(" and course_id = " + course_id);
            }
            string query = strQuery.ToString();
            DLS db = new DLS(this.AppUserBean);
            DataRow dr = db.GetSingleDataRow(query);
            if (dr != null)
            {
                t.it_id = Convert.ToInt32(dr["it_id"]);
                t.title = dr["title"].ToString();
                t.course_id = dr["course_id"].ToString();
                t.from_date = DateTime.Parse(dr["from_date"].ToString()).ToShortDateString();
                t.to_date = DateTime.Parse(dr["to_date"].ToString()).ToShortDateString();
                t.level_id = dr["level_id"].ToString();
                t.days = dr["days"].ToString();
                t.week_days = dr["week_days"].ToString();
                t.location = dr["location"].ToString();
            }
            return t;
        }

        public Instructor_Training GetTrainingByCourseId(string course_id)
        {
            Instructor_Training t = new Instructor_Training();

            string query = "select * from instructor_training where course_id=" + course_id;
            DLS db = new DLS(this.AppUserBean);
            DataRow dr = db.GetSingleDataRow(query);
            if (dr != null)
            {
                t.it_id = Convert.ToInt32(dr["it_id"]);
                t.title = dr["title"].ToString();
                t.course_id = dr["course_id"].ToString();
                t.from_date = DateTime.Parse(dr["from_date"].ToString()).ToShortDateString();
                t.to_date = DateTime.Parse(dr["to_date"].ToString()).ToShortDateString();
                t.level_id = dr["level_id"].ToString();
                t.user_id = dr["user_id"].ToString();
            }
            return t;
        }

        public List<TeacherBean> ListTeachersSignUpReport(string userid, string month = null, string year = null)
        {
            List<TeacherBean> list = new List<TeacherBean>();
            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select f.frn_code,f.company_share,f.name,te.teach_id,te.first_name,te.last_name,te.education_qualification,te.partipation_level_Date,i.from_date,i.to_date from franchises f ");
            strQuery.AppendLine(" inner join teacher_franchise_allocation t on f.frn_id = t.frn_id ");
            strQuery.AppendLine(" inner join teachers te on te.teach_id = t.teach_id ");
            strQuery.AppendLine(" inner join instructor_training i on te.teach_id = i.teach_id ");
            strQuery.AppendLine(" inner join users u on u.user_id =f.user_id ");
            strQuery.AppendLine(" where  u.user_id in (" + userid + ")");

            string query = strQuery.ToString();


            //if (!string.IsNullOrEmpty(year))
            //{
            //    if (year.ToLower() != "all")
            //    {
            //        query += " and YEAR(partipation_level_Date)=" + year;
            //    }
            //}
            //if (!string.IsNullOrEmpty(month))
            //{
            //    if (month.ToLower() != "all")
            //    {
            //        query += " and MONTH(partipation_level_Date) =" + month;
            //    }
            //}

            query += " order by i.from_date";


            string frn_code = "";
            string frn_name = "";

            DLS db = new DLS(this.AppUserBean);
            DataRow dataRow = db.GetSingleDataRow(" select frn_code,name from franchises f inner join users u on u.user_id =f.user_id  where u.user_id =" + this.AppUserBean.user_id);

            if (dataRow != null)
            {
                frn_code = dataRow["frn_code"].ToString();
                frn_name = dataRow["name"].ToString();
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TeacherBean t = new TeacherBean();
                    t.frn_code = dr["frn_code"].ToString();
                    t.frn_name = dr["name"].ToString();
                    t.frn_share = dr["company_share"].ToString();
                    t.teach_id = dr["teach_id"].ToString();
                    t.full_name = dr["first_name"].ToString() +" "+ dr["last_name"].ToString();
                    t.education_qualification = dr["education_qualification"].ToString();
                    t.partipation_level_Date = Convert.ToDateTime(dr["from_date"].ToString()).ToShortDateString() + " To " + Convert.ToDateTime(dr["to_date"].ToString()).ToShortDateString();
                    t.frn_master_code = frn_code;
                    t.frn_master_name = frn_name;
                    list.Add(t);
                }
            }
            return list;
        }


        public List<int> ListTrainedTeachersByCourseId(string course_id)
        {
            List<int> list = new List<int>();
            string query = "select teach_id from instructor_training where course_id=" + course_id ;
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["teach_id"] != null)
                    {
                        if (!string.IsNullOrEmpty(dr["teach_id"].ToString()))
                        {
                            list.Add(Convert.ToInt32(dr["teach_id"].ToString()));
                        }
                    }
                }
            }
            return list;
        }

        public List<Instructor_Training> ListTrainingInstructionTrainingSchedule(string country_id, string month = null, string year = null)
        {
            List<Instructor_Training> list = new List<Instructor_Training>();
            string query = "select * from instructor_training  where country_id=" + country_id ;


            if (!string.IsNullOrEmpty(year))
            {
                if (year.ToLower() != "all")
                {
                    query += " and YEAR(from_date)>=  "+ year + " and YEAR(to_date)<=" + year;
                }
            }
            if (!string.IsNullOrEmpty(month))
            {
                if (month.ToLower() != "all")
                {
                    query += " and MONTH(from_date) <=" + month + " and MONTH(to_date)>=" + month;
                }
            }
            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    Instructor_Training t = new Instructor_Training();
                    t.from_date = DateTime.Parse(dr["from_date"].ToString()).ToShortDateString() + "       TO       " +DateTime.Parse(dr["to_date"].ToString()).ToShortDateString();
                    t.days = dr["days"].ToString();
                    t.week_days = dr["week_days"].ToString();
                    t.location = dr["location"].ToString();
                    LevelBean levelBean = new LevelAdapter(this.AppUserBean).FillLevel(dr["level_id"].ToString());
                    if (levelBean != null)
                    {
                        if (!string.IsNullOrEmpty(levelBean.level_title))
                        {
                            t.level_name = levelBean.level_title;
                        }
                    }
                    list.Add(t);
                }
            }
            return list;
        }

        public List<TeacherBean> ListTrainingInstructionTrainingReport(string state_id, string month = null, string year = null,string teacher_id= null)
        {
            List<TeacherBean> list = new List<TeacherBean>();
            string query = "select * from teachers t inner join franchises f on f.frn_id = t.frn_id where t.state_id=" + state_id;

            if (!string.IsNullOrEmpty(year))
            {
                if (year.ToLower() != "all")
                {
                    query += " and ((YEAR(last_level_trained_date)>=  " + year + " and YEAR(last_level_trained_date)<=" + year + ")" + " or (YEAR(partipation_level_date) >= " + year + " and YEAR(partipation_level_date)<= " + year + "))";
                }
            }
            if (!string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year))
            {
                if (month.ToLower() != "all")
                {
                    query += " and(( YEAR(last_level_trained_date) >= " + year + " and MONTH(last_level_trained_date) >=" + month + " and YEAR(last_level_trained_date)<=" + year + " and MONTH(last_level_trained_date)<=" + month + ")" + " or ( YEAR(partipation_level_date) >= " + year + " and MONTH(partipation_level_date) >=" + month + " and YEAR(partipation_level_date)<=" + year + " and MONTH(partipation_level_date)<=" + month + "))";
                }
            }
            if (!string.IsNullOrEmpty(teacher_id))
            {
                if (teacher_id.ToLower() != "all")
                {
                    query += " and t.teach_id =" + teacher_id;
                }
            }



            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TeacherBean t = new TeacherBean();
                    t.frn_code = dr["frn_code"].ToString();
                    t.frn_name = dr["name"].ToString();
                    t.teach_id = dr["teach_id"].ToString();
                    t.full_name = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                    LevelBean levelBean = new LevelAdapter(this.AppUserBean).FillLevel(dr["level_id"].ToString());
                    if (levelBean != null)
                    {
                        if (!string.IsNullOrEmpty(levelBean.level_title))
                        {
                            t.last_level_name = levelBean.level_title;
                        }
                    }
                    LevelBean previouslevelBean = new LevelAdapter(this.AppUserBean).FillLevel(dr["level_id"].ToString());
                    if (previouslevelBean != null)
                    {
                        if (!string.IsNullOrEmpty(previouslevelBean.level_title))
                        {
                            t.previous_level_name = previouslevelBean.level_title;
                        }
                    }
                    t.partipation_level_Date = dr["partipation_level_Date"] != null ? Convert.ToDateTime(dr["partipation_level_Date"].ToString()).ToString("dd/MM/yyyy") : "";
                    t.last_level_trained_date = dr["last_level_trained_date"] != null ? Convert.ToDateTime(dr["last_level_trained_date"].ToString()).ToString("dd/MM/yyyy") : "";
                   
                    list.Add(t);
                }
            }
            return list;
        }

        public List<TeacherBean> ListTrainingInstructionTrainingUnitFranchiseReport(string frn_id, string month = null, string year = null, string teacher_id = null)
        {
            List<TeacherBean> list = new List<TeacherBean>();
            string query = "select * from teachers t inner join franchises f on f.frn_id = t.frn_id where t.frn_id=" + frn_id;

            if (!string.IsNullOrEmpty(year))
            {
                if (year.ToLower() != "all")
                {
                    query += " and ((YEAR(last_level_trained_date)>=  " + year + " and YEAR(last_level_trained_date)<=" + year + ")" + " or (YEAR(partipation_level_date) >= " + year + " and YEAR(partipation_level_date)<= " + year + "))";
                }
            }
            if (!string.IsNullOrEmpty(month) && !string.IsNullOrEmpty(year))
            {
                if (month.ToLower() != "all")
                {
                    query += " and(( YEAR(last_level_trained_date) >= " + year + " and MONTH(last_level_trained_date) >=" + month + " and YEAR(last_level_trained_date)<=" + year + " and MONTH(last_level_trained_date)<=" + month + ")" + " or ( YEAR(partipation_level_date) >= " + year + " and MONTH(partipation_level_date) >=" + month + " and YEAR(partipation_level_date)<=" + year + " and MONTH(partipation_level_date)<=" + month + "))";
                }
            }
            if (!string.IsNullOrEmpty(teacher_id))
            {
                if (teacher_id.ToLower() != "all")
                {
                    query += " and t.teach_id =" + teacher_id;
                }
            }

            DLS db = new DLS(this.AppUserBean);
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    TeacherBean t = new TeacherBean();
                    t.frn_code = dr["frn_code"].ToString();
                    t.frn_name = dr["name"].ToString();
                    t.teach_id = dr["teach_id"].ToString();
                    t.full_name = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                    LevelBean levelBean = new LevelAdapter(this.AppUserBean).FillLevel(dr["level_id"].ToString());
                    if (levelBean != null)
                    {
                        if (!string.IsNullOrEmpty(levelBean.level_title))
                        {
                            t.last_level_name = levelBean.level_title;
                        }
                    }
                    LevelBean previouslevelBean = new LevelAdapter(this.AppUserBean).FillLevel(dr["level_id"].ToString());
                    if (previouslevelBean != null)
                    {
                        if (!string.IsNullOrEmpty(previouslevelBean.level_title))
                        {
                            t.previous_level_name = previouslevelBean.level_title;
                        }
                    }
                    t.partipation_level_Date = dr["partipation_level_Date"] != null ? Convert.ToDateTime(dr["partipation_level_Date"].ToString()).ToString("dd/MM/yyyy") : "";
                    t.last_level_trained_date = dr["last_level_trained_date"] != null ? Convert.ToDateTime(dr["last_level_trained_date"].ToString()).ToString("dd/MM/yyyy") : "";

                    list.Add(t);
                }
            }
            return list;
        }
    }
}
