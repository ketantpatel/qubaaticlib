using iTextSharp.text;
using iTextSharp.text.pdf;
using MDACLib.adapter;
using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MDACLib
{
    public class CertificateGenerator
    {
        public List<CertificateBean> students;

        public CertificateGenerator(List<CertificateBean> students)
        {
            this.students = students;
        }
        public void ChangePhoto(string fname, string enrollment_no)
        {
            enrollment_no = enrollment_no.Replace("/", "-");
            PdfReader sample = new PdfReader(System.IO.File.ReadAllBytes(fname));
            using (FileStream fs = new FileStream(fname, FileMode.Create))
            {
                Image img;
                try
                {
                    //img = Image.GetInstance("http://localhost:11324//files/documents/photos/" + enrollment_no + ".jpg");
                     img = Image.GetInstance("http://sis.mdac.co.in//files/documents/photos/" + enrollment_no + ".jpg");
                }
                catch (Exception ex)
                {
                    //img = Image.GetInstance("http://localhost:11324//user1.jpg");
                    img = Image.GetInstance("http://sis.mdac.co.in//user1.jpg"); 
                }
                //  Image img = Image.GetInstance("user.jpg");
                PdfStamper stamper = new PdfStamper(sample, fs);
                PdfWriter writer = stamper.Writer;
                PdfDictionary pg = sample.GetPageN(1);
                PdfDictionary res =
                  (PdfDictionary)PdfReader.GetPdfObject(pg.Get(PdfName.RESOURCES));
                PdfDictionary xobj =
                  (PdfDictionary)PdfReader.GetPdfObject(res.Get(PdfName.XOBJECT));
                if (xobj != null)
                {
                    int imagecount = 0;
                    foreach (PdfName name in xobj.Keys)
                    {
                        PdfObject obj = xobj.Get(name);
                        if (obj.IsIndirect())
                        {
                            PdfDictionary tg = (PdfDictionary)PdfReader.GetPdfObject(obj);
                            PdfName type =
                              (PdfName)PdfReader.GetPdfObject(tg.Get(PdfName.SUBTYPE));
                            if (PdfName.IMAGE.Equals(type))
                            {
                                if (imagecount == 0)
                                {
                                    PdfReader.KillIndirect(obj);
                                    Image maskImage = img.ImageMask;
                                    if (maskImage != null)
                                        writer.AddDirectImageSimple(maskImage);
                                    writer.AddDirectImageSimple(img, (PRIndirectReference)obj);

                                    break;
                                }

                                imagecount++;
                            }
                        }
                    }
                }
                stamper.Writer.CloseStream = false;
                stamper.FormFlattening = true;
                stamper.Close();
                stamper.Dispose();
                sample.Close();
                sample.Dispose();
                //writer.Close();
                //writer.Dispose();
            }
        }
        public void GeneratMarksheet(string bastPath)
        {
            string filePath = bastPath + "\\files\\certificate\\certificate.pdf";

            foreach (CertificateBean s in students)
            {

                PdfReader rdr = new PdfReader(filePath);
                PdfReader reader = new PdfReader(rdr);
                string original_enroll = s.reg_no;
                s.reg_no = s.reg_no.Replace("/", "");
                string newFileName = bastPath + "\\files\\certificate\\download\\" + s.reg_no + ".pdf";
                using (FileStream fs = new FileStream(newFileName, FileMode.Create))
                {
                    PdfStamper stamper = new PdfStamper(reader, fs);
                    var a = stamper.AcroFields;
                    a.SetField("STUDENT_NAME", s.student_name);


                    a.SetField("GUARDIAN_NAME", s.gaurdian);
                    a.SetField("ENROLL_NO", original_enroll);
                    a.SetField("COURSE_NAME", s.course_name);
                    a.SetField("PERIOD_FROM", s.period_from);
                    a.SetField("PERIOD_TO", s.period_to);
                    a.SetField("EXAM_DATE", s.exam_date);
                    a.SetField("FRAN_NAME", s.fran_city);
                    a.SetField("GRADE", s.grade);

                    if (s.serial_no.Length == 1)
                        s.serial_no = "000" + s.serial_no;

                    if (s.serial_no.Length == 2)
                        s.serial_no = "00" + s.serial_no;

                    if (s.serial_no.Length == 3)
                        s.serial_no = "0" + s.serial_no;

                    
                    a.SetField("SERIAL_NO", s.serial_no);
                    a.SetField("ENROLL_NO_TOP", original_enroll);
                    stamper.Close();
                    stamper.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
                ChangePhoto(newFileName, original_enroll);
                rdr.Dispose();
                rdr.Close();
                reader.Dispose();
                rdr.Close();
            }
        }

        public void GeneraLevelCertificate(string bastPath)
        {
            string filePath = bastPath + "\\files\\levelcertificate\\level_certificate_master.pdf";

            foreach (CertificateBean s in students)
            {

                PdfReader rdr = new PdfReader(filePath);
                PdfReader reader = new PdfReader(rdr);
                string original_enroll = s.reg_no;
                s.reg_no = s.reg_no.Replace("/", "");
                string newFileName = bastPath + "\\files\\levelcertificate\\download\\" + s.reg_no + ".pdf";
                using (FileStream fs = new FileStream(newFileName, FileMode.Create))
                {
                    PdfStamper stamper = new PdfStamper(reader, fs);
                    var a = stamper.AcroFields;
                   // a.SetField("lblName", s.reg_no);
                    a.SetField("txtRegNo", s.reg_no);
                    PdfContentByte content = stamper.GetOverContent(1);
                    //  ColumnText ct = new ColumnText(content);
                    // this are the coordinates where you want to add text
                    // if the text does not fit inside it will be cropped
                    //ct.SetSimpleColumn(20, 80, 1000, 500);
                    //ct.SetText(new Phrase("ketan"));

                    //ct.Go();
                    int x = 118;
                    int y = 338;
                    int m=302;

                    string s1 = "Awarded To";
                    string s2 = s.student_name;
                    string s3 = "Code :" + s.reg_no;
                    string s4 = "of Gauharbaug Bilimora";
                    string s5 = "Venue : Panchal Samajvalid, Bilimora";
                    string s6 = "Category : U1";
                    string s7 = "Acheivement : Merit Award";


                    Font font = new Font();
                    font.Size = 13;
                    font.SetStyle(Font.BOLDITALIC);

                    Font fontName = new Font();
                    fontName.SetColor(29, 152, 246);
                    fontName.SetStyle(Font.BOLDITALIC);
                    fontName.Size = 22;
                    //  ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase("a", font), x, y, 0);

                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase(s1, font), x+m+(s1.Length/2), y, 0);
                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase(s2, fontName), x+m+(s2.Length/2), y-30, 0);
                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase(s3, font), x + m+(s3.Length/2), y-60, 0);
                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase(s4, font), x + m+(s4.Length/2), y-90, 0);
                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase(s5, font), x + m+(s5.Length/2), y- 120, 0);
                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase(s6, font), x + m+(s6.Length/2), y- 150, 0);
                    ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase(s7, font), x + m+(s7.Length/2), y- 178, 0);

                    //ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase("b", font), x+604, y, 0);
                    //ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase("c", font), x, y-184, 0);
                    //ColumnText.ShowTextAligned(content, Element.ALIGN_CENTER, new Phrase("d", font), x+604, y-184, 0);

                    //a.SetField("GUARDIAN_NAME", s.gaurdian);
                    //a.SetField("ENROLL_NO", original_enroll);
                    //a.SetField("COURSE_NAME", s.course_name);
                    //a.SetField("PERIOD_FROM", s.period_from);
                    //a.SetField("PERIOD_TO", s.period_to);
                    //a.SetField("EXAM_DATE", s.exam_date);
                    //a.SetField("FRAN_NAME", s.fran_city);
                    //a.SetField("GRADE", s.grade);

                    //if (s.serial_no.Length == 1)
                    //    s.serial_no = "000" + s.serial_no;

                    //if (s.serial_no.Length == 2)
                    //    s.serial_no = "00" + s.serial_no;

                    //if (s.serial_no.Length == 3)
                    //    s.serial_no = "0" + s.serial_no;


                    //a.SetField("SERIAL_NO", s.serial_no);
                    //a.SetField("ENROLL_NO_TOP", original_enroll);
                    stamper.Close();
                    stamper.Dispose();
                    fs.Close();
                    fs.Dispose();
                }
               // ChangePhoto(newFileName, original_enroll);
                rdr.Dispose();
                rdr.Close();
                reader.Dispose();
                rdr.Close();
            }
        }
    }
}
