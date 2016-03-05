using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InstaLiker.ModelData
{
    public partial class Model
    {
        private int _countNeedLikes;
        private List<string> _existCompUrlList;
        private List<string> _existReadUrlList;
        private double _interval;
        private string _newFileXmlTag;
        private XDocument _newXmlDocument;
        private string _pathTags;

        public string[,] ArrTagsInfo { get; private set; }
        public List<string> ListNameTagFiles { get; private set; }
        public List<string> ListFullNameTagFiles { get; private set; }

        // download file names (tags) from the folder
        public void LoadTagsInfo()
        {
            ListFullNameTagFiles = new List<string>();
            ListNameTagFiles = new List<string>();
            var c = 0;
            _pathTags = Path.Combine(Application.StartupPath, "Tags");
            var dir = new DirectoryInfo(_pathTags);
            var countTagFiles = dir.GetFiles("*.xml").Count(item => item.Name != "Templete.XML");
            ArrTagsInfo = new string[countTagFiles, 4];

            foreach (var file in dir.GetFiles("*.xml").Where(item => item.Name != "Templete.XML"))
            {
                var xmlDocTag = XDocument.Load(file.FullName);
                ListFullNameTagFiles.Add(file.FullName);
                ListNameTagFiles.Add(file.Name);

                ArrTagsInfo[c, 0] = Path.GetFileNameWithoutExtension(file.Name);

                var interval = xmlDocTag.Elements("DATA").Elements("Interval").FirstOrDefault();
                if (interval != null) ArrTagsInfo[c, 1] = interval.Value;

                var needCountLikes = xmlDocTag.Elements("DATA").Elements("NeedCountLikes").FirstOrDefault();
                if (needCountLikes != null) ArrTagsInfo[c, 2] = needCountLikes.Value;
                ++c;
            }
        }

        // add new links for likes
        public void AddNewReadUrlInXml(string fileName)
        {
            var xmlDocTag = XDocument.Load(fileName);
            var existReadUrlList = xmlDocTag.Elements("DATA").Elements("READURL").
                ToList().Select(item => item.Value).ToList();

            foreach (var readUrl in _downloadReadUrlList)
            {
                if (existReadUrlList.Contains(readUrl)) continue;
                var newElementReadUrl = new XElement("READURL", readUrl);

                XElement selfElementUrl;
                if (existReadUrlList.Count == 0)
                {
                    selfElementUrl = xmlDocTag.Elements("DATA").Elements("Interval").ElementAt(0);
                    selfElementUrl.AddAfterSelf(newElementReadUrl);
                }
                else
                {
                    selfElementUrl = xmlDocTag.Elements("DATA").Elements("READURL").ElementAt(0);
                    selfElementUrl.AddBeforeSelf(newElementReadUrl);
                }
            }
            xmlDocTag.Save(fileName);
        }

        // add a ready links to the xml file
        private static void AddLikedUrlInXml(string completeUrl)
        {
            var fileCompleteUrls = Application.StartupPath + @"\AllCompleteUrls.XML";
            var xmlDocTag = XDocument.Load(fileCompleteUrls);
            var newElementReadUrl = new XElement("COMPLETEURL", completeUrl);
            var existCompUrlList = xmlDocTag.Elements("DATA").Elements("COMPLETEURL").
                ToList().Select(item => item.Value).ToList();

            if (existCompUrlList.Contains(completeUrl)) return;

            XElement selfElementUrl;
            if (existCompUrlList.Count == 0)
            {
                selfElementUrl = xmlDocTag.Elements("DATA").ElementAt(0);
                selfElementUrl.AddAfterSelf(newElementReadUrl);
            }
            else
            {
                selfElementUrl = xmlDocTag.Elements("DATA").Elements("COMPLETEURL").ElementAt(0);
                selfElementUrl.AddBeforeSelf(newElementReadUrl);
            }

            xmlDocTag.Save(fileCompleteUrls);
        }

        // collection COMPLETEURL, READURL, NEEDLIKES and INTERVAL from the selected file with the tag
        private void MakeListsUrlAndEtc(string tagFile)
        {
            var xmlDocTag = XDocument.Load(tagFile);
            var xmlDocCompUrls = XDocument.Load(Application.StartupPath + @"\AllCompleteUrls.XML");

            var needCountLikes = xmlDocTag.Elements("DATA").Elements("NeedCountLikes").FirstOrDefault();
            if (needCountLikes != null) _countNeedLikes = int.Parse(needCountLikes.Value);
            var interval = xmlDocTag.Elements("DATA").Elements("Interval").FirstOrDefault();
            if (interval != null) _interval = double.Parse(interval.Value);

            _existCompUrlList = xmlDocCompUrls.Elements("DATA").Elements("COMPLETEURL").
                ToList().Select(item => item.Value).ToList();
            _existReadUrlList = xmlDocTag.Elements("DATA").Elements("READURL").
                ToList().Select(item => item.Value).ToList();
        }

        // creating xml file with tag
        public void CreateXmlFileByTag(string tagName, string countLikes, string interval)
        {
            _newFileXmlTag = string.Format(@"{0}\{1}.XML", _pathTags, tagName);

            File.Copy(_pathTags + @"\Templete.XML", _newFileXmlTag);
            _newXmlDocument = XDocument.Load(_newFileXmlTag);

            ChangeElement("NeedCountLikes", countLikes);
            ChangeElement("Interval", interval);

            _newXmlDocument.Save(_newFileXmlTag);
        }

        // change value in element
        public void ChangeElement(string columnName, string newValue, string fileName = "")
        {
            if (fileName == string.Empty)
            {
                var element = _newXmlDocument.Elements("DATA").Elements(columnName).ElementAt(0);
                element.SetValue(newValue);
            }
            else
            {
                var fileXmlTag = string.Format(@"{0}\{1}.XML", _pathTags, fileName);
                var xmlDocument = XDocument.Load(fileXmlTag);
                var element = xmlDocument.Elements("DATA").Elements(columnName).ElementAt(0);

                element.SetValue(newValue);
                xmlDocument.Save(fileXmlTag);
            }
        }
    }
}