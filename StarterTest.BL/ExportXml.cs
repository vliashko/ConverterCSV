using StarterTest.BL.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace StarterTest.BL
{
    public class ExportXml : IDisposable
    {
        bool disposed = false;
        readonly Repository rp = new Repository();
        readonly DBContext db = new DBContext();

        public void ExportToXml(User searchCriterion, string fileName)
        {
            List<User> users = rp.GetNecessaryUserList(searchCriterion);

            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                using (XmlTextWriter xmlOut = new XmlTextWriter(fs, Encoding.Unicode))
                {
                    xmlOut.Formatting = Formatting.Indented;

                    xmlOut.WriteStartDocument();
                    xmlOut.WriteStartElement("TestProgram");

                    ForeachExportXml(users, xmlOut);

                    xmlOut.WriteEndElement();
                    xmlOut.WriteEndDocument();
                    users.Clear();
                }
            }
        }
        void ForeachExportXml(List<User> users, XmlTextWriter xmlOut)
        {
            foreach (User xUser in users)
            {
                SaveToFile(xmlOut, xUser);
            }
        }
        void SaveToFile(XmlTextWriter xmlOut, User xUser)
        {
            xmlOut.WriteStartElement("Record");
            xmlOut.WriteAttributeString("id", xUser.Id.ToString());
            xmlOut.WriteElementString("Date", xUser.DateTime.ToString("dd.MM.yyyy"));
            xmlOut.WriteElementString("FirstName", xUser.Name);
            xmlOut.WriteElementString("LastName", xUser.Surname);
            xmlOut.WriteElementString("SurName", xUser.MiddleName);
            xmlOut.WriteElementString("City", xUser.City);
            xmlOut.WriteElementString("Country", xUser.Country);
            xmlOut.WriteEndElement();
        }
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
