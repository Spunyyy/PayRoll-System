using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace PayRoll_System
{
    /// <summary>
    /// Interakční logika pro ViewPayoutsPage.xaml
    /// </summary>
    public partial class ViewPayoutsPage : Page
    {
        public ViewPayoutsPage(List<Payout> payouts)
        {
            InitializeComponent();
            payoutsListView.ItemsSource = payouts;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Payout dataContext = (Payout)button.DataContext;

            CreatePDFPayCheck(dataContext);
            SendEmail(dataContext);
        }

        private void CreatePDFPayCheck(Payout payout)
        {
            string path = AppContext.BaseDirectory + "\\temp";
            string filePath = path + "\\"+payout.Month+payout.Employee.Name+payout.Employee.Surname+".pdf";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }


            Document document = new Document(PageSize.A4, 25, 25, 30, 30);
            PdfWriter.GetInstance(document, new FileStream(filePath, FileMode.Create));

            document.Open();

            Font bold = FontFactory.GetFont("Arial", 16, Font.BOLD);
            Font normalFont = FontFactory.GetFont("Arial", 10, Font.NORMAL);
            Font finalFont = FontFactory.GetFont("Arial", 14, Font.NORMAL);

            PdfPTable infoTable = new PdfPTable([3, 3]);
            infoTable.AddCell(new PdfPCell(new Phrase("Company s. r. o.", bold)) { Colspan = 2, Border = Rectangle.NO_BORDER });
            infoTable.AddCell(new PdfPCell(new Phrase("Name: " + payout.Employee.Name + " " + payout.Employee.Surname)) { Border = Rectangle.NO_BORDER });
            infoTable.AddCell(new PdfPCell(new Phrase(payout.Month)) { HorizontalAlignment = 2, Border = Rectangle.NO_BORDER });
            infoTable.SpacingAfter = 10f;  

            document.Add(infoTable);

            PdfPTable wageCompTable = new PdfPTable([3, 3, 3]);
            wageCompTable.AddCell(new PdfPCell(new Phrase("Wage components", bold)) { Colspan = 2, Border = Rectangle.NO_BORDER }); 
            wageCompTable.AddCell(new PdfPCell(new Phrase("Shifts: " + payout.NoOfShifts, normalFont)) { Border = Rectangle.NO_BORDER });
            wageCompTable.AddCell(new PdfPCell(new Phrase("Time wage: ", normalFont)) { Border = Rectangle.NO_BORDER });
            wageCompTable.AddCell(new PdfPCell(new Phrase(payout.Employee.HourlyRate + "Kc/hour, worked: " + payout.Time + "H", normalFont)) { Colspan = 2, Border = Rectangle.NO_BORDER });

            wageCompTable.SpacingBefore = 10f; 

            document.Add(wageCompTable);

            PdfPTable taxesTable = new PdfPTable([3, 3, 3]);
            taxesTable.AddCell(new PdfPCell(new Phrase("Taxable income: " + (int)PayoutWithTaxes(payout), normalFont)) { Border = Rectangle.NO_BORDER });
            taxesTable.AddCell(new PdfPCell(new Phrase("Social employee: " + (int)payout.SocialEmployee, normalFont)) { Border = Rectangle.NO_BORDER });
            taxesTable.AddCell(new PdfPCell(new Phrase("Medical employee: " + (int)payout.MedicalEmployee, normalFont)) { Border = Rectangle.NO_BORDER });
            taxesTable.AddCell(new PdfPCell(new Phrase("Tax: " + (int)payout.Tax, normalFont)) { Border = Rectangle.NO_BORDER });
            taxesTable.AddCell(new PdfPCell(new Phrase("Social employer: " + (int)payout.SocialEmployer, normalFont)) { Border = Rectangle.NO_BORDER });
            taxesTable.AddCell(new PdfPCell(new Phrase("Medical employer: " + (int)payout.MedicalEmployer, normalFont)) { Border = Rectangle.NO_BORDER });

            document.Add(taxesTable);

            PdfPTable netSalaryTable = new PdfPTable(1);
            netSalaryTable.AddCell(new PdfPCell(new Phrase("Net salary: " + (int)payout.Sum, finalFont)) { Border = Rectangle.NO_BORDER });

            document.Add(netSalaryTable);


            document.Close();
        }

        private void SendEmail(Payout payout)
        {
            string path = AppContext.BaseDirectory + "\\temp";
            string filePath = path + "\\" + payout.Month + payout.Employee.Name + payout.Employee.Surname + ".pdf";
            try
            {
                MailMessage mail = new MailMessage();

                mail.From = new MailAddress(Settings.ReturnSetting("Email"));
                mail.To.Add(payout.Employee.Email);

                mail.Subject = "Paycheck";

                Attachment attachment = new Attachment(filePath);
                mail.Attachments.Add(attachment);

                SmtpClient smtpServer = new SmtpClient(Settings.ReturnSetting("smtpServer"));
                smtpServer.Port = 587;
                smtpServer.Credentials = new NetworkCredential(Settings.ReturnSetting("Email"), Settings.ReturnSetting("EmailPassword"));
                smtpServer.EnableSsl = true;

                smtpServer.Send(mail);

                attachment.Dispose();
                mail.Dispose(); 

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    MessageBox.Show("E-mail was sent.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private decimal PayoutWithTaxes(Payout payout)
        {
            return payout.Sum + payout.Tax + payout.SocialEmployee + payout.MedicalEmployee;
        }
    }
}
