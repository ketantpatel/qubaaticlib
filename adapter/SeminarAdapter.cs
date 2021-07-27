using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class SeminarAdapter
    {
        public UserBean user;
        public SeminarAdapter(UserBean user)
        {
            this.user = user;
        }
        public SeminarSetupBean SaveSeminar(SeminarSetupBean s)
        {
            DLS db = new DLS(user);
            db.AddParameters("name", s.name, MyDBTypes.Varchar);
            db.AddParameters("seminarDate", s.seminarDate, MyDBTypes.DateTime);
            db.AddParameters("created_by", s.created_by, MyDBTypes.Varchar);
            db.AddParameters("CreatedDate", DateTime.Now, MyDBTypes.DateTime);
            db.AddParameters("frn_id", s.frn_id, MyDBTypes.Varchar);

            s.isSuccess = db.Insert("seminar_setup");
            s.seminar_setup_id = db.GetLastAutoID().ToString();

            db.Dispose();
            return s;
        }


        public List<SeminarSetupBean> GetSeminars(string frn_id)
        {
            DLS db = new DLS(this.user);

            StringBuilder strQuery = new StringBuilder();
            strQuery.AppendLine(" select * from seminar_setup s");
            strQuery.AppendLine(" where s.frn_id = " + frn_id + "  and seminarDate <" + DateTime.Now);



            string query = strQuery.ToString();
            List<SeminarSetupBean> list = new List<SeminarSetupBean>();
            DataTable dt = db.GetDataTable(query);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow dr in dt.Rows)
                    {
                        SeminarSetupBean seminar = new SeminarSetupBean();
                        seminar.name = dr["name"].ToString();
                        seminar.seminarDate = Convert.ToDateTime(dr["start_date"].ToString()).ToString("yyyy-MM-dd");
                        list.Add(seminar);
                    }
                }
            }
            db.Dispose();
            return list;
        }
    }
}
