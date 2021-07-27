using iTextSharp.text;
using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MDACLib.adapter
{
    public class DocumentAdapter : MasterAdapter
    {
        public DocumentAdapter(UserBean user)
        {
            this.AppUserBean = user;
        }
        public bool IsFileDelete(string form_id, string doc_typ, string frn_id)
        {
            bool flag = false;
            DLS db = new DLS(this.AppUserBean);
           // db.AddParameters("form_id", form_id, MyDBTypes.Int);
           // db.AddParameters("doc_type", doc_typ, MyDBTypes.Int);
             flag = db.Delete("documents","form_id=" + form_id + " and doc_type=" + doc_typ + "");
           
            db.Dispose();
            return flag;
        }
        public bool IsFileExist(string form_id, string doc_typ, string frn_id)
        {
            bool flag = false;
            DLS db = new DLS(this.AppUserBean);
            string  count = db.GetSingleValue("select count(*) from documents where form_id=" + form_id + " and doc_type=" + doc_typ + "");
            if (int.Parse(count) > 0)
                flag = true;
            db.Dispose();
            return flag;
        }
        public byte[] getPhoto(string doc_id)
        {
            DLS db = new DLS(this.AppUserBean);
            DataRow row = db.GetSingleDataRow("select file from documents where doc_id="+doc_id+"");
            byte[] bytes;
            try
            {
                 bytes = (byte[])(row["file"]);
               
            }
            catch (Exception ex)
            {
                bytes = null;
            }
            // byte[] imageByte = System.Text.ASCIIEncoding.Default.GetBytes(imgString);
            // string yourByteString = Convert.ToString(imageByte[20], 2).PadLeft(8, '0');
            db.Dispose();
            return bytes;
        }
        
        public string GetImage(string form_id, string doc_typ, string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            DataRow row = db.GetSingleDataRow("select file from documents where form_id=" + form_id + " and doc_type=" + doc_typ + "");
             string base64String=null;
            try
            {
                byte[] bytes = (byte[])(row["file"]);
                base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                base64String=null;
            }
           // byte[] imageByte = System.Text.ASCIIEncoding.Default.GetBytes(imgString);
           // string yourByteString = Convert.ToString(imageByte[20], 2).PadLeft(8, '0');
            db.Dispose();
            return base64String;
        }
        public string GetPhotoPath(string form_id, string doc_typ, string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            DataRow row = db.GetSingleDataRow("select filePath from documents where form_id=" + form_id + " and doc_type=" + doc_typ + "");
            string base64String = null;
            try
            {
                if(row["filePath"]!=null)
                base64String = row["filePath"].ToString();
            }
            catch (Exception ex)
            {
                base64String = null;
            }
            // byte[] imageByte = System.Text.ASCIIEncoding.Default.GetBytes(imgString);
            // string yourByteString = Convert.ToString(imageByte[20], 2).PadLeft(8, '0');
            db.Dispose();
            return base64String;
        }
        public string GetDocPath(string form_id, string doc_typ, string frn_id)
        {
            DLS db = new DLS(this.AppUserBean);
            DataRow row = db.GetSingleDataRow("select filePath from documents where form_id=" + form_id + " and doc_type=" + doc_typ + "");
            string base64String = null;
            try
            {
                if (row != null)
                {
                    if (row["filePath"] != null)
                        base64String = row["filePath"].ToString();
                }
               
            }
            catch (Exception ex)
            {
                base64String = null;
            }
            // byte[] imageByte = System.Text.ASCIIEncoding.Default.GetBytes(imgString);
            // string yourByteString = Convert.ToString(imageByte[20], 2).PadLeft(8, '0');
            db.Dispose();
            return base64String;
        }
        public List<DocumentBean> GetDocuments(string frn_id,string doc_type)
        {
            List<DocumentBean> list = new List<DocumentBean>();
            DLS db=new DLS(this.AppUserBean);
            try
            {
                string query = "select d.doc_id,d.form_id,a.first_name,a.last_name,a.enroll_no from documents d left join admission_forms a on a.form_id=d.form_id where d.frn_id=" + frn_id + " and doc_type="+doc_type+"";
                DataTable dt = db.GetDataTable(query);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DocumentBean d = new DocumentBean();
                        //byte[] bytes = (byte[])(dr["file"]);
                        //d.file = bytes;
                        d.form_id = dr["form_id"].ToString();
                        d.enroll_no = dr["enroll_no"].ToString();
                        d.doc_id = dr["doc_id"].ToString();
                        d.name = dr["first_name"].ToString() + " " + dr["last_name"].ToString();
                        list.Add(d);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                db.Dispose();
            }
            return list;
        }
        public DocumentBean UploadDocument(DocumentBean dBean)
        {
            DLS db = new DLS(this.AppUserBean);
            string existCount = db.GetSingleValue("select count(*) from documents where form_id="+dBean.form_id+" and doc_type="+dBean.doc_type+"");
                             
            if (dBean.file != null)
                db.AddParameters("file", dBean.file, MyDBTypes.Varchar);
            if (dBean.filePath != null)
                db.AddParameters("filePath", dBean.filePath, MyDBTypes.Varchar);
             dBean.is_opr_success = false;
            if (int.Parse(existCount) == 0)
            {
                if (!string.IsNullOrEmpty(dBean.form_id))
                    db.AddParameters("form_id", dBean.form_id, MyDBTypes.Int);     
                if (!string.IsNullOrEmpty(dBean.frn_id))
                    db.AddParameters("frn_id", dBean.frn_id, MyDBTypes.Int);
                if (!string.IsNullOrEmpty(dBean.doc_type))
                    db.AddParameters("doc_type", dBean.doc_type, MyDBTypes.Int);
               
                if (db.Insert("documents"))
                {
                    dBean.is_opr_success = true;
                    dBean.doc_id = db.GetLastAutoID().ToString();
                }
            }
            else
            {
                if (db.Update("documents", "form_id=" + dBean.form_id + " and doc_type=" + dBean.doc_type + ""))
                {
                    dBean.is_opr_success = true;
                }
            }
            db.Dispose();
            return dBean;
        }

        public bool SavePhoto(string path,string doc_id)
        {
            bool isFlag = false;
            DLS db = new DLS(this.AppUserBean);
            if (!string.IsNullOrEmpty(doc_id))
            {
                db.AddParameters("path", path, MyDBTypes.Varchar);
               isFlag= db.Update("documents", "doc_id=" + doc_id);
            }
            db.Dispose();
            return true;
        }
    }
}
