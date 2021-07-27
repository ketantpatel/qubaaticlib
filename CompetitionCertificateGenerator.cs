using iTextSharp.text;
using iTextSharp.text.pdf;
using MDACLib.adapter;
using MDACLib.beans;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace MDACLib
{
    public class CompetitionCertificateGenerator
    {
        public List<CertifcateLeavelBean> students;

        public CompetitionCertificateGenerator(List<CertifcateLeavelBean> students)
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
        public string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }
        public void GeneraCompetitionCertificate(string bastPath)
        {
            string filePath = bastPath + "\\files\\levelcertificate\\competition_certificate.pdf";
            System.IO.DirectoryInfo di = new DirectoryInfo(bastPath + "\\files\\levelcertificate\\download");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (CertifcateLeavelBean s in students)
            {

                PdfReader rdr = new PdfReader(filePath);
                PdfReader reader = new PdfReader(rdr);
                string original_enroll = s.code;
                s.code = s.code.Replace("/", "");
                string newFileName = bastPath + "\\files\\levelcertificate\\download\\" + s.code + ".pdf";
                using (FileStream fs = new FileStream(newFileName, FileMode.Create))
                {
                    PdfStamper stamper = new PdfStamper(reader, fs);
                    var a = stamper.AcroFields;
                   // a.SetField("lblName", s.reg_no);
                   // a.SetField("txtRegNo", s.code);
                    PdfContentByte content = stamper.GetOverContent(1);
                    //  ColumnText ct = new ColumnText(content);
                    // this are the coordinates where you want to add text
                    // if the text does not fit inside it will be cropped
                    //ct.SetSimpleColumn(20, 80, 1000, 500);
                    //ct.SetText(new Phrase("ketan"));

                    int num1 = 118;
                    int num2 = 338;
                    int num3 = 290;
                    string str3 = "This certificate is awarded to ";
                    string titleCase = this.ToTitleCase(s.stud_name);
                    string str4 = "Student Code : " + s.code;
                    string str5 = "for participating in " + s.competition_name.Trim();
                    string str6 = "Held at " + s.venue;
                    string str7 = "on " + s.competition_date;
                    string str8 = "in category " + s.category + " (" + s.level_name + ") and Achieved " + this.ToTitleCase(s.acheivement) ?? "";
                    Font font1 = FontFactory.GetFont("Calibri", 17f, (BaseColor)BaseColor.BLACK);
                    font1.SetStyle(2);
                    Font font2 = FontFactory.GetFont("LucidaCalligraphy", 24f, (BaseColor)BaseColor.RED);
                    font2.SetColor(3, 130, 196);
                    font2.SetStyle(2);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str3, font1), (float)(num1 + num3 + str3.Length / 2), (float)num2, 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(titleCase, font2), (float)(num1 + num3 + titleCase.Length / 2), (float)(num2 - 33), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str4, font1), (float)(num1 + num3 + str4.Length / 2), (float)(num2 - 66), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str5, font1), (float)(num1 + num3 + str5.Length / 2), (float)(num2 - 93), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str6, font1), (float)(num1 + num3 + str6.Length / 2), (float)(num2 - 123), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str7, font1), (float)(num1 + num3 + str7.Length / 2), (float)(num2 - 150), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str8, font1), (float)(num1 + num3 + str8.Length / 2), (float)(num2 - 178), 0.0f);

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
