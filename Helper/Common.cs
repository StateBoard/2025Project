using Newtonsoft.Json;
using OfficeOpenXml;
using Open_Schooling.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web.Mvc;
using System.Web;
namespace Open_Schooling.Helper
{
    public class Common
    {
        Open_Schooling_2025Entities db = new Open_Schooling_2025Entities();

        public Login_Model Get_Login_Details()
        {
            string login_string = HttpContext.Current.User.Identity.Name;
            Login_Model login_model = JsonConvert.DeserializeObject<Login_Model>(login_string);
            return login_model;
        }

        //public Registration_Model Get_Login_Details()
        //{
        //    //string login_string= System.Web.HttpContext.Current.User.Identity.Name;
        //    string login_string = HttpContext.Current.User.Identity.Name;
        //    Registration_Model login_model = JsonConvert.DeserializeObject<Registration_Model>(login_string);
        //    return login_model;
        //}
        public static string Get_Year_Id()
        {
            return "25";
           


            }
        

        public static string Get_Year()
        {
            return "2024-25";
        }

        public void CreateExcelFile(DataTable dt_list, string filename, string excelfor)
        {
            try
            {
                DataSet ds1 = new DataSet();
                ds1.Tables.Add(dt_list);
                using (DataSet ds = ds1)
                {
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        string rootFolder;
                        if (excelfor == "state")
                        {
                            rootFolder = HttpContext.Current.Server.MapPath("~/State").ToString();
                        }
                        else if (excelfor == "application")
                        {
                            rootFolder = HttpContext.Current.Server.MapPath("~/Application_Excel").ToString();
                        }
                        else
                        {
                            rootFolder = HttpContext.Current.Server.MapPath("~/Reg_Excel").ToString();
                        }


                        string fileName = @"" + filename + ".xlsx";

                        System.IO.FileInfo file = new System.IO.FileInfo(Path.Combine(rootFolder, fileName));
                        using (ExcelPackage xp = new ExcelPackage(file))
                        {
                            foreach (DataTable dt in ds.Tables)
                            {
                                ExcelWorksheet ws = xp.Workbook.Worksheets.Add(dt.TableName);
                                int rowstart = 1;
                                int colstart = 1;
                                int rowend = rowstart;
                                int colend = colstart + dt.Columns.Count;
                                rowend = rowstart + dt.Rows.Count;
                                ws.Cells[rowstart, colstart].LoadFromDataTable(dt, true);
                                int i = 1;
                                foreach (DataColumn dc in dt.Columns)
                                {
                                    i++;
                                    if (dc.DataType == typeof(decimal))
                                        ws.Column(i).Style.Numberformat.Format = "#0.00";
                                }
                                ws.Cells[ws.Dimension.Address].AutoFitColumns();
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Top.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Bottom.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Left.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            }
                            xp.Save();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }


        public Tbl_Subject[] Get_Nsqf_Sub(Tbl_Registration oS_Form_Data)
        {
            Tbl_Subject[] model = new Tbl_Subject[5];

            if (oS_Form_Data.SUBNSQF != null)
            {
                var normal_sub = oS_Form_Data.SUB.ToList();
                var nsqfSub = oS_Form_Data.SUBNSQF.ToList();
                //string[] nsqfsub = oS_Form_Data.Nsqf_Subject.Split(' ');
                if (nsqfSub.Count == 2)
                {
                    string sub1 = "";
                    sub1 = normal_sub[0];
                    string sub2 = "";
                    sub2 = normal_sub[1];
                    string sub3 = "";
                    sub3 = normal_sub[2];
                    model[0] = db.Tbl_Subject.Where(s => s.Subject_Code == sub1).FirstOrDefault();
                    model[1] = db.Tbl_Subject.Where(s => s.Subject_Code == sub2).FirstOrDefault();
                    model[2] = db.Tbl_Subject.Where(s => s.Subject_Code == sub3).FirstOrDefault();
                    string nsqf1 = "";
                    nsqf1 = nsqfSub[0];
                    string nsqf2 = "";
                    nsqf2 = nsqfSub[1];
                    model[3] = db.Tbl_Subject.Where(s => s.Subject_Code == nsqf1).FirstOrDefault();
                    model[4] = db.Tbl_Subject.Where(s => s.Subject_Code == nsqf2).FirstOrDefault();
                }
                else if (nsqfSub.Count == 1)
                {
                    string sub1 = "";
                    sub1 = normal_sub[0];
                    string sub2 = "";
                    sub2 = normal_sub[1];
                    string sub3 = "";
                    sub3 = normal_sub[2];
                    string sub4 = "";
                    sub4 = normal_sub[3];
                    model[0] = db.Tbl_Subject.Where(s => s.Subject_Code == sub1).FirstOrDefault();
                    model[1] = db.Tbl_Subject.Where(s => s.Subject_Code == sub2).FirstOrDefault();
                    model[2] = db.Tbl_Subject.Where(s => s.Subject_Code == sub3).FirstOrDefault();
                    model[3] = db.Tbl_Subject.Where(s => s.Subject_Code == sub4).FirstOrDefault();
                    string nsqf = "";
                    nsqf = nsqfSub[0];
                    model[4] = db.Tbl_Subject.Where(s => s.Subject_Code == nsqf).FirstOrDefault();
                }
                else
                {
                    model[0] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject1).FirstOrDefault();
                    model[1] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject2).FirstOrDefault();
                    model[2] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject3).FirstOrDefault();
                    model[3] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject4).FirstOrDefault();
                    model[4] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject5).FirstOrDefault();
                }
            }
            /*---------------------Ashok------------------------*/
            //if (oS_Form_Data.Nsqf_Subject != null && oS_Form_Data.Nsqf_Subject != "No")
            //{
            //    model[0] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject1).FirstOrDefault();
            //    model[1] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject2).FirstOrDefault();
            //    model[2] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject3).FirstOrDefault();
            //    model[3] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject4).FirstOrDefault();
            //    model[4] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Nsqf_Subject).FirstOrDefault();
            //}
            else
            {
                model[0] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject1).FirstOrDefault();
                model[1] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject2).FirstOrDefault();
                model[2] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject3).FirstOrDefault();
                model[3] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject4).FirstOrDefault();
                model[4] = db.Tbl_Subject.Where(s => s.Subject_Code == oS_Form_Data.Subject5).FirstOrDefault();

            }


            return model;
        }

        public void RegistrationMail(String Email, String Title, String Message)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("support@msbshse.ac.in");
                msg.To.Add(Email);
                msg.Bcc.Add("balbharati_exam@bmmonline.org");
                msg.Bcc.Add("msbospune@gmail.com");
                msg.Bcc.Add("stateboardonline@gmail.com");
                msg.Subject = Title;
                msg.Body = Message;
                msg.IsBodyHtml = true;
                // MailAddress copy = new MailAddress("receipt@msbshse.ac.in");
                //msg.CC.Add(copy);

              /*  msg.IsBodyHtml = true;*/
                SmtpClient smtp = new SmtpClient();

                smtp.Host = "smtp.sendgrid.net";
                smtp.Port = 587;
                //smtp.Port = 25;
                smtp.Credentials = new System.Net.NetworkCredential("apikey", "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");
                smtp.EnableSsl = false;
                smtp.Send(msg);
                return Json("Password Sent to the mail");
            }
            catch (Exception ex)
            {
                throw new Exception("Email sending failed: " + ex.Message);
            }
        }

        public string Send_Mail_Data_Payment(Tbl_payment model)
        {


            return @"<html>
           <body>
      <table style='max-width:600px;margin:auto;border-spacing:0;background:#249a96b7;padding:4px;border-radius:16px;overflow:hidden' align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
         <tbody>
            <tr>
               <td style='border-collapse:collapse'>
                  <table style='margin:auto;border-spacing:0;background:white;border-radius:12px;overflow:hidden' align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                     <tbody>
                        <tr>
                           <td style='border-collapse:collapse'>
                              <table style='border-spacing:0;border-collapse:collapse' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                 <tbody>
                                    <tr>
                                       <td style='border-collapse:collapse;padding:16px 32px' align='left' valign='middle'>
                                          <table style='border-spacing:0;border-collapse:collapse' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td style='padding:0;text-align:left;border-collapse:collapse' width='40' align='left' valign='middle'>                     <a href='https://examinfo.mh-hsc.ac.in/'>                     <img src='https://examinfo.mh-hsc.ac.in/design/images/MSBSHSE-logo.png' title='MSBSHSE'  style='margin:auto;text-align:center;border:0;outline:none;text-decoration:none;min-height:40px' align='middle' border='0' width='60' class='CToWUd'>                     </a>                     </td>
                                                  
                                                   <td  align='left' valign='middle' style='border-collapse:collapse;padding-left: 20px;font-weight: 600'> Maharashtra State Board of Open Schooling  Pune-4 <Br>   महाराष्ट्र राज्य मुक्त विद्यालय मंडळ, पुणे-४११००४ 
</td>                                                   
                        
                                                
                                             </tbody>
                                          </table>
                                          <table style='border-spacing:0;border-collapse:collapse' bgcolor='#ffffff' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td style='padding:0;text-align:left;border-collapse:collapse' width='40' align='left' valign='middle'>                     
                                                   
                                                   <Br>
                                                  <tr> <td  valign='middle' style='border-collapse:collapse;padding-left:0px ;font-weight: 600'>Official Website For Maharashtra State Board of Open Schooling (MSBOS)</td></tr>
                                                   <td align='right' valign='middle'>                           </td>
                                                </tr>
                                             </tbody>
                                         </table>
                                       </td>
                                    </tr>
                                 </tbody>
                              </table>
                           </td>
                        </tr>
                        <tr>
                           <td style='border-collapse:collapse;padding:0 16px'>
                              <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%' style='background:#f7f9fa;padding:16px;border-radius:8px;overflow:hidden'>
                                 <tbody>                                  
                                       <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0;border-bottom:1px solid #eaeaed;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>               <b>                     Order ID           </b>                         </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                    <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>                                     " + model.order_id + @"                                      </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>

                                       <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0;border-bottom:1px solid #eaeaed;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>   <b>                                Reference ID                 </b>                 </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                    <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>                                     " + model.tracking_id + @"                                      </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>

                                    <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0;border-bottom:1px solid #eaeaed;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>           <b>                         Amount                </b>                  </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                    <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>                                     " + model.amount_money + @"                                      </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>


                                    <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0;border-bottom:1px solid #eaeaed;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>                     <b>                 Student Name                </b>                   </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                     <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>                                     " + model.billing_name + @"                                      </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>
                                    
                                     <tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0;border-bottom:1px solid #eaeaed;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>                        <b>            Mobile No.            </b>                       </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                     <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>                                     " + model.billing_tel + @"                                      </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>

<tr>
                                       <td align='left' valign='middle' style='border-collapse:collapse;padding:8px 0;border-bottom:1px solid #eaeaed;font-size:12px'>
                                          <table align='center' border='0' cellpadding='0' cellspacing='0' width='100%'>
                                             <tbody>
                                                <tr>
                                                   <td width='28%' align='left' valign='top' style='border-collapse:collapse;text-transform:capitalize'>                        <b>            Email ID            </b>                       </td>
                                                   <td width='16' align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>:</td>
                                                     <td align='left' valign='top' style='border-collapse:collapse;font-weight:normal'>                                     " + model.billing_email + @"                                      </td>
                                                </tr>
                                             </tbody>
                                          </table>
                                       </td>
                                    </tr>
                                     
                                 </tbody>
                              </table>
                           </td>
                        </tr>
                       
                                  
                                 </tbody>
                              </table>
                           </td>
                        </tr>
                     </tbody>
                  </table>
               </td>
            </tr>
         </tbody>
      </table>
   </body>
        </html>";
        }

        public void MailSend(String Email, String Title, String Message)
        {
            try
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("lalitasawle1906@gmail.com");
                msg.To.Add(Email);

                msg.Subject = Title;
                msg.Body = Message;
                msg.IsBodyHtml = true;
                //MailAddress copy = new MailAddress("receipt@msbshse.ac.in");
                //msg.CC.Add(copy);

                msg.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.sendgrid.net";
                smtp.Port = 587;
                //smtp.Port = 25;
                smtp.Credentials = new System.Net.NetworkCredential("apikey", "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");
                smtp.EnableSsl = false;
                smtp.Send(msg);

            }
            catch (Exception ex)
            {
                //throw ex;
            }
        }




    }
}