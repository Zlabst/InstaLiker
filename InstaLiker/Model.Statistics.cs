using MetroFramework.Controls;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace InstaLiker
{
    public partial class Model
    {
        private readonly FrmMain _frmMain;
        private readonly MetroGrid _mainMetroGrid;
        private readonly ProgressBar _mainProgress;
        private readonly WebBrowser _mainWebBrowser;
        private XDocument _newXmlDocument;
        public string[,] ArrTagsInfo;

        public Model(FrmMain frmMain)
        {
            _frmMain = frmMain;
            _mainWebBrowser = _frmMain.wbMain;
            _mainMetroGrid = _frmMain.dgrTagsInfo;
            _mainProgress = _frmMain.PbMain;
            HeaderFrmText = frmMain.Text;
        }

        private string NewFileXmlTag { get; set; }
        private string PathTags { get; set; }
        public List<string> ListNameTagFiles { get; set; }
        private List<string> ListFullNameTagFiles { get; set; }
        private List<string> ExistCompUrlList { get; set; }
        private List<string> ExistReadUrlList { get; set; }
        private int CountNeedLikes { get; set; }
        private double Interval { get; set; }
        private string HeaderFrmText { get; set; }
        // download file names (tags) from the folder
        public void LoadTagsInfo()
        {
            ListFullNameTagFiles = new List<string>();
            ListNameTagFiles = new List<string>();
            var c = 0;
            PathTags = Path.Combine(Application.StartupPath, "Tags");
            var dir = new DirectoryInfo(PathTags);
            var countTagFiles = dir.GetFiles("*.xml").Count(item => item.Name != "Templete.XML");
            ArrTagsInfo = new string[countTagFiles, 4];

            foreach (var file in dir.GetFiles("*.xml").Where(item => item.Name != "Templete.XML"))
            {
                var xmlDocTag = XDocument.Load(file.FullName);
                ListFullNameTagFiles.Add(file.FullName);
                ListNameTagFiles.Add(file.Name);

                ArrTagsInfo[c, 0] = Path.GetFileNameWithoutExtension(file.Name);
                ArrTagsInfo[c, 1] = xmlDocTag.Elements("DATA").Elements("Interval").FirstOrDefault().Value;
                ArrTagsInfo[c, 2] = xmlDocTag.Elements("DATA").Elements("NeedCountLikes").FirstOrDefault().Value;
                ++c;
            }
        }

        // add new links for likes
        private void AddNewReadUrlInXml(string fileName)
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

            CountNeedLikes = int.Parse(xmlDocTag.Elements("DATA").Elements("NeedCountLikes").FirstOrDefault().Value);
            Interval = double.Parse(xmlDocTag.Elements("DATA").Elements("Interval").FirstOrDefault().Value);

            ExistCompUrlList = xmlDocCompUrls.Elements("DATA").Elements("COMPLETEURL").
                ToList().Select(item => item.Value).ToList();
            ExistReadUrlList = xmlDocTag.Elements("DATA").Elements("READURL").
                ToList().Select(item => item.Value).ToList();
        }

        // creating xml file with tag
        public void CreateXmlFileByTag()
        {
            NewFileXmlTag = string.Format(@"{0}\{1}.XML", PathTags, _frmMain.TbTagName.Text);

            File.Copy(PathTags + @"\Templete.XML", NewFileXmlTag);
            _newXmlDocument = XDocument.Load(NewFileXmlTag);

            ChangeElement("NeedCountLikes", _frmMain.TbCountLike.Text);
            ChangeElement("Interval", _frmMain.TbInterval.Text);

            _newXmlDocument.Save(NewFileXmlTag);
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
                var fileXmlTag = string.Format(@"{0}\{1}.XML", PathTags, fileName);
                var xmlDocument = XDocument.Load(fileXmlTag);
                var element = xmlDocument.Elements("DATA").Elements(columnName).ElementAt(0);

                element.SetValue(newValue);
                xmlDocument.Save(fileXmlTag);
            }
        }
    }
}