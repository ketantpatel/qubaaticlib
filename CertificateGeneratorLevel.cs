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
    public class CertificateGeneratorLevel
    {
        public List<CertifcateLeavelBean> students;

        public CertificateGeneratorLevel(List<CertifcateLeavelBean> students)
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


        public void GeneraLevelCertificate(string bastPath)
        {
            string filePath = bastPath + "\\files\\levelcertificate\\level_certificate.pdf";
            System.IO.DirectoryInfo di = new DirectoryInfo(bastPath + "\\files\\levelcertificate\\download");

            foreach (FileInfo file in di.GetFiles())
            {
                file.Delete();
            }
            foreach (CertifcateLeavelBean student in students)
            {

                PdfReader rdr = new PdfReader(filePath);
                PdfReader reader = new PdfReader(rdr);
                string code = student.code;
                string original_enroll = student.code;
                student.code = student.code.Replace("/", "");
                string newFileName = bastPath + "\\files\\levelcertificate\\download\\" + student.code + ".pdf";
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

                    //ct.Go();
                    int num1 = 118;
                    int num2 = 360;
                    int num3 = 290;
                    string str2 = "Sr. No. : " + student.sr_no;
                    string str3 = "Std Code : " + code;
                    string str4 = "This certificate is awarded to ";
                    string titleCase = this.ToTitleCase(student.stud_name);
                    string str5 = "of " + this.ToTitleCase(student.frn_name);
                    string str6 = "For successfully completing";
                    string str7 = student.level_name + " with " + student.marks + "%";
                    string str8 = "Examination Date : " + student.certy_exam_date_str;
                    string str9 = "Certificate Issue Date : " + student.certy_issue_date_str;
                    BaseFont font1 = BaseFont.CreateFont(bastPath + "\\files\\CALIBRII_0.ttf", "Cp1252", true);
                    Font font2 = new Font(font1, 18f);
                    Font font3 = new Font(font1, 16f);
                    Font font4 = new Font(BaseFont.CreateFont(bastPath + "\\files\\LCALLIG_0.ttf", "Cp1252", true), 25f);
                    font4.SetColor(3, 130, 196);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str2, font3), (float)(num1 + num3 + str2.Length / 2 + 261), (float)(num2 + 200), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str3, font3), (float)(num1 + num3 + str3.Length / 2 + 271), (float)(num2 + 180), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str4, font2), (float)(num1 + num3 + str4.Length / 2), (float)num2, 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(titleCase, font4), (float)(num1 + num3 + titleCase.Length / 2), (float)(num2 - 33), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str5, font2), (float)(num1 + num3 + str5.Length / 2), (float)(num2 - 66), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str6, font2), (float)(num1 + num3 + str6.Length / 2), (float)(num2 - 93), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str7, font2), (float)(num1 + num3 + str7.Length / 2), (float)(num2 - 123), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str8, font2), (float)(num1 + num3 + str8.Length / 2), (float)(num2 - 150), 0.0f);
                    ColumnText.ShowTextAligned(content, 1, new Phrase(str9, font2), (float)(num1 + (num3 - 3) + str9.Length / 2), (float)(num2 - 178), 0.0f);


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
