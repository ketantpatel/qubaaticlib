using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class BatchAdapter
    {
        public UserBean user;
        public BatchAdapter(UserBean user)
        {
            this.user = user;
        }
        public BatchBean SaveBatch(BatchBean b)
        {
            DLS db = new DLS(user);
            db.AddParameters("name", b.name, MyDBTypes.Varchar);
            db.AddParameters("start_date", b.start_date, MyDBTypes.Varchar);
            db.AddParameters("end_date", b.end_date, MyDBTypes.DateTime);
            db.AddParameters("status", b.status, MyDBTypes.DateTime);
            db.AddParameters("frn_id", b.frn_id, MyDBTypes.DateTime);

            db.AddParameters("start_from_time", b.start_from_time, MyDBTypes.DateTime);
            db.AddParameters("start_to_time", b.start_to_time, MyDBTypes.DateTime);
            db.AddParameters("end_from_time", b.end_from_time, MyDBTypes.DateTime);
            db.AddParameters("end_to_time", b.end_to_time, MyDBTypes.DateTime);
            if(!string.IsNullOrEmpty(b.teach_id))
            {
                db.AddParameters("teach_id", b.teach_id, MyDBTypes.Int);

            }
            if (b.batch_id == "-1")
            {
                b.isSuccess = db.Insert("batches");
                b.batch_id = db.GetLastAutoID().ToString();
            }
            else
            {
                b.isSuccess = db.Update("batches", "batch_id=" + b.batch_id);
            }
            db.Dispose();
            return b;
        }
        public List<BatchBean> BatchList(bool isAll,string status,string frn_id)
        {
            List<BatchBean> list = new List<BatchBean>();
            DLS db = new DLS(user);
            string query = "select * from batches where frn_id="+frn_id;
            
            if (!isAll)
            {
                query = "select * from batches where status="+status+" and frn_id="+frn_id;
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BatchBean b = new BatchBean();
                    b.name = dr["name"].ToString();
                    b.batch_id = dr["batch_id"].ToString(); ;
                    b.start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("dd/MM/yyyy");
                    b.end_date=DateTime.Parse(dr["end_date"].ToString()).ToString("dd/MM/yyyy");
                    b.status = dr["status"].ToString();
                    list.Add(b);
                }
            }
            db.Dispose();
            return list;
        }
        public List<BatchBean> TeacherBatchList(string teach_id)
        {
            List<BatchBean> list = new List<BatchBean>();
            DLS db = new DLS(user);
            string query = "SELECT *,(select count(*) from admission_forms a where a.batch_id=b.batch_id) total_students,f.name frn_name from batches  b inner join franchises f on f.frn_id=b.frn_id where teach_id=" + teach_id;

            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BatchBean b = new BatchBean();
                    b.name = dr["name"].ToString();
                    b.batch_id = dr["batch_id"].ToString(); ;
                    b.start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("dd/MM/yyyy");
                    b.end_date = DateTime.Parse(dr["end_date"].ToString()).ToString("dd/MM/yyyy");
                    //b.status = dr["status"].ToString();
                    b.total_students = dr["total_students"].ToString();
                    b.frn_name = dr["frn_name"].ToString();
                    list.Add(b);
                }
            }
            db.Dispose();
            return list;
        }
        public List<BatchBean> ListBatchForApplication(bool isAll, string status, string frn_id)
        {
            List<BatchBean> list = new List<BatchBean>();
            DLS db = new DLS(user);
            string query = "select *,(select count(*) from admission_forms f where f.batch_id=b.batch_id and f.is_active=1) total_students from batches b where b.frn_id=" + frn_id;

            if (!isAll)
            {
                query = "select *,(select count(*) from admission_forms f where f.batch_id=b.batch_id  and f.is_active=1) total_students from batches b where b.status=" + status + " and frn_id=" + frn_id;
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BatchBean b = new BatchBean();
                    b.name = dr["name"].ToString();
                    b.total_students = dr["total_students"].ToString();
                    b.batch_id = dr["batch_id"].ToString(); ;
                    b.start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("dd/MM/yyyy");
                    b.end_date = DateTime.Parse(dr["end_date"].ToString()).ToString("dd/MM/yyyy");
                    b.status = dr["status"].ToString();
                    list.Add(b);
                }
            }
            db.Dispose();
            return list;
        }
        public List<BatchBean> ListStudents(bool isAll, string status, string frn_id)
        {
            List<BatchBean> list = new List<BatchBean>();
            DLS db = new DLS(user);
            string query = "select *,(select count(*) from admission_forms f where f.batch_id=b.batch_id and f.is_active=1 and f.is_verify=1) total_students from batches b where b.frn_id=" + frn_id;

            if (!isAll)
            {
                query = "select *,(select count(*) from admission_forms f where f.batch_id=b.batch_id  and f.is_active=1  and f.is_verify=1) total_students from batches b where b.status=" + status + " and frn_id=" + frn_id;
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BatchBean b = new BatchBean();
                    b.name = dr["name"].ToString();
                    b.total_students = dr["total_students"].ToString();
                    b.batch_id = dr["batch_id"].ToString(); ;
                    b.start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("dd/MM/yyyy");
                    b.end_date = DateTime.Parse(dr["end_date"].ToString()).ToString("dd/MM/yyyy");
                    b.status = dr["status"].ToString();
                    list.Add(b);
                }
            }
            db.Dispose();
            return list;
        }
        public BatchBean BatchInfo(string batch_id)
        {
            BatchBean b = new BatchBean();
            DLS db = new DLS(user);
            string query = "select * from batches where batch_id="+batch_id;

            
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    
                    b.name = dr["name"].ToString();                    
                    b.teach_id = dr["teach_id"].ToString();                    
                    b.start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("dd/MM/yyyy");
                    b.end_date = DateTime.Parse(dr["end_date"].ToString()).ToString("dd/MM/yyyy");
                    b.db_start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("yyyy-MM-dd");
                    b.db_end_date = DateTime.Parse(dr["end_date"].ToString()).ToString("yyyy-MM-dd");
                    b.start_from_time = dr["start_from_time"].ToString();
                    b.start_to_time = dr["start_to_time"].ToString();
                    b.batch_id = batch_id;
                    b.status = dr["status"].ToString();
                    b.frn_id = dr["frn_id"].ToString();
                }
            }
            db.Dispose();
            return b;
        }
        public BatchBean BatchInfoByFormId(string form_id)
        {
            BatchBean b = new BatchBean();
            DLS db = new DLS(user);
            string query = "select * from batches where batch_id in (select f.batch_id from admission_forms f where form_id="+form_id+")";


            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {

                    b.name = dr["name"].ToString();
                    b.start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("dd/MM/yyyy");
                    b.end_date = DateTime.Parse(dr["end_date"].ToString()).ToString("dd/MM/yyyy");
                    b.db_start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("yyyy-MM-dd");
                    b.db_end_date = DateTime.Parse(dr["end_date"].ToString()).ToString("yyyy-MM-dd");
                    b.start_from_time = dr["start_from_time"].ToString();
                    b.start_to_time = dr["start_to_time"].ToString();
                    b.batch_id = dr["batch_id"].ToString();
                    b.status = dr["status"].ToString();
                    b.frn_id = dr["frn_id"].ToString();
                }
            }
            db.Dispose();
            return b;
        } 
        public List<BatchBean> FillBatch(bool isSelect,string frn_id)
        {
            List<BatchBean> list = new List<BatchBean>();
            DLS db = new DLS(user);
            string query = "select * from batches where frn_id="+frn_id;

            if (isSelect)
            {
                BatchBean b = new BatchBean();
                b.batch_id = "";
                b.name = "--Select--";
                list.Add(b);
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BatchBean b = new BatchBean();
                    b.batch_id = dr["batch_id"].ToString();
                    b.name = dr["name"].ToString();
                    b.start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("dd/MM/yyyy");
                    b.end_date = DateTime.Parse(dr["end_date"].ToString()).ToString("dd/MM/yyyy");
                    b.status = dr["status"].ToString();
                    list.Add(b);
                }
            }
            db.Dispose();
            return list;
        }
        public List<BatchBean> FillTeacherBatch(bool isSelect, string frn_id,string teach_id)
        {
            List<BatchBean> list = new List<BatchBean>();
            DLS db = new DLS(user);
            //string query = "select * from batches where frn_id=" + frn_id;
            string query = "SELECT b.* from batches  b inner join franchises f on f.frn_id=b.frn_id where f.frn_id="+frn_id+" and teach_id=" + teach_id;

            if (isSelect)
            {
                BatchBean b = new BatchBean();
                b.batch_id = "";
                b.name = "--Select--";
                list.Add(b);
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BatchBean b = new BatchBean();
                    b.batch_id = dr["batch_id"].ToString();
                    b.name = dr["name"].ToString();
                    b.start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("dd/MM/yyyy");
                    b.end_date = DateTime.Parse(dr["end_date"].ToString()).ToString("dd/MM/yyyy");
                    b.status = dr["status"].ToString();
                    list.Add(b);
                }
            }
            db.Dispose();
            return list;
        }
        public List<BatchBean> FillSingleBatch(bool isSelect, string frn_id,string batch_id)
        {
            List<BatchBean> list = new List<BatchBean>();
            DLS db = new DLS(user);
            string query = "select * from batches where batch_id="+batch_id+" and frn_id=" + frn_id;

            if (isSelect)
            {
                BatchBean b = new BatchBean();
                b.batch_id = "";
                b.name = "--Select--";
                list.Add(b);
            }
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    BatchBean b = new BatchBean();
                    b.batch_id = dr["batch_id"].ToString();
                    b.name = dr["name"].ToString();
                    b.start_date = DateTime.Parse(dr["start_date"].ToString()).ToString("dd/MM/yyyy");
                    b.end_date = DateTime.Parse(dr["end_date"].ToString()).ToString("dd/MM/yyyy");
                    b.status = dr["status"].ToString();
                    list.Add(b);
                }
            }
            db.Dispose();
            return list;
        }

       

    }
}
