using System;
using System.Threading;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;


namespace HitachiAssessment
{
    class Program
    {

        static void Main(string[] args)
        {
            string prefLang = string.Empty;
            string pathToCSV = string.Empty;
            string senderEmail = string.Empty;
            string senderPassword = string.Empty;
            string receiverEmail = string.Empty;

            while (string.IsNullOrEmpty(prefLang))
            {
                Console.WriteLine("For English enter EN. Fuer Deutsch geben Sie DE ein :");
                prefLang = Console.ReadLine();
            }

            CultureInfo newCulture = CultureInfo.CreateSpecificCulture("en-US");

            if (prefLang.Equals("DE"))
            {
                newCulture = CultureInfo.CreateSpecificCulture("de-DE");               
            }
          
            Thread.CurrentThread.CurrentUICulture = newCulture;
            Thread.CurrentThread.CurrentCulture = newCulture;

            while (string.IsNullOrEmpty(pathToCSV))
            {
                Console.WriteLine(Resources.language.csvPathMsg);             
                pathToCSV = Console.ReadLine();

                if (IsFileValid(pathToCSV) == false)
                {
                    Console.WriteLine(Resources.language.csvPathErrorMsg);
                    pathToCSV = "";
                }
            }
            while (string.IsNullOrEmpty(senderEmail))
            {
                Console.WriteLine(Resources.language.senderEmailMsg);
                senderEmail = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(senderPassword))
            {
                Console.WriteLine(Resources.language.senderPassMsg);
                senderPassword = Console.ReadLine();
            }
            while (string.IsNullOrEmpty(receiverEmail))
            {
                Console.WriteLine(Resources.language.recEmailMsg);
                receiverEmail = Console.ReadLine();
            }
            

            List<Competitor> listOfCompetitors = MakeListOfCompetitors(pathToCSV);

            List<ByCountry> byCountries = MakeListOfCountries(listOfCompetitors);

            byCountries.Sort((x, y) => y.GetAvgScore().CompareTo(x.GetAvgScore()));

            string pathToNewCSV = createNewCSV(pathToCSV, byCountries);

            sendEmailWithNewCSV(senderEmail, senderPassword, receiverEmail, pathToNewCSV);

        }


        private static List<Competitor> MakeListOfCompetitors(string pathToCSV)
        {
            List<Competitor> listOfCompetitors = new List<Competitor>();

            using(var reader = new StreamReader(@pathToCSV))
            {
                reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var row = reader.ReadLine();
                    var attributes = row.Split(';');

                    Competitor competitor = new Competitor(attributes[0], attributes[1], attributes[2], int.Parse(attributes[4]));
                    listOfCompetitors.Add(competitor);
                }

                reader.Close();
            }

            return listOfCompetitors;
        }


        private static List<ByCountry> MakeListOfCountries(List<Competitor> competitors)
        {
            
            List<ByCountry> byCountries = new List<ByCountry>();
            
            foreach (Competitor competitor in competitors){
                bool ctrExist = false;

                foreach (ByCountry country in byCountries)
                {
                    if (competitor.GetCountry().Equals(country.GetCountryName()))
                    {
                        country.AddCompetitor(competitor);
                        ctrExist = true;
                        break;
                    }                   
                }

                if (ctrExist)
                {
                    continue;
                }
                else
                {
                    ByCountry ctr = new ByCountry(competitor.GetCountry());
                    ctr.AddCompetitor(competitor);
                    byCountries.Add(ctr);
                }             
            }

            foreach (ByCountry country in byCountries)
            {
                country.CalcDataForCountry();
            }

            return byCountries;
        }

        private static string createNewCSV(string pathToCSV, List<ByCountry> byCountries)
        {
            string pathToNewCSV = Path.GetDirectoryName(pathToCSV) + @"\ReportByCountry.csv";

            var newCSV = File.CreateText(pathToNewCSV);

            //header
            newCSV.WriteLine("Country;Average score;Median score;Max score;Max score person;Min score;Min score person; Record counts");
            
            //fill
            foreach(ByCountry country in byCountries)
            {
                newCSV.Write(country.GetCountryName());
                newCSV.Write(";");
                newCSV.Write(country.GetAvgScore());
                newCSV.Write(";");
                newCSV.Write(country.GetMedScore());
                newCSV.Write(";");
                newCSV.Write(country.GetMaxScore());
                newCSV.Write(";");
                newCSV.Write(country.GetMaxScorePerson());
                newCSV.Write(";");
                newCSV.Write(country.GetMinScore());
                newCSV.Write(";");
                newCSV.Write(country.GetMinScorePerson());
                newCSV.Write(";");
                newCSV.Write(country.GetRecordCount());
                newCSV.WriteLine();
            }

            newCSV.Close();

            return pathToNewCSV;
        }

        private static void sendEmailWithNewCSV(string senderEmail, string senderPass, string recEmail, string pathToNewCSV)
        {
            MailMessage newCSVmail = new MailMessage();

            newCSVmail.From = new MailAddress(senderEmail);
            newCSVmail.To.Add(recEmail);
            newCSVmail.Subject = "Report By Country";

            Attachment newCSVfile = new Attachment(pathToNewCSV);
            newCSVmail.Attachments.Add(newCSVfile);

            SmtpClient emailProvider = new SmtpClient("smtp-mail.outlook.com");
            emailProvider.Port = 587;
            emailProvider.Credentials = new NetworkCredential(senderEmail, senderPass);
            emailProvider.EnableSsl = true;

            emailProvider.Send(newCSVmail);
        }

        private static bool IsFileValid(string pathToFile)
        {
            bool valid = true;

            if (!File.Exists(pathToFile))
            {
                valid = false;
            }
            else if (Path.GetExtension(pathToFile).ToLower() != ".csv")
            {
                valid = false;
            }

            return valid;
        }
    }
}
