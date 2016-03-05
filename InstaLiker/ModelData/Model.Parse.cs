using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstaLiker.ModelData
{
    public partial class Model : IModelComponents
    {
        private string _documentText;
        private List<string> _downloadReadUrlList;
        public int CounterPb;

        public Model(string documentText)
        {
            Data = new DataTable("Data");
            Data.Columns.Add("TagName", typeof (string));
            Data.Columns.Add("Interval", typeof (double));
            Data.Columns.Add("NeedCountLikes", typeof (int));
            Data.Columns.Add("CompCountLikes", typeof (int));
            Data.Columns.Add("Status", typeof (string));

            _documentText = documentText;
        }

        public DataTable Data { get; private set; }
        public event Action OnClickLike;
        public event WaitLoadingPage OnWaitLoadingPage;
        public event Action OnRefreshBrowser;
        public event Action<Uri> OnChangeUrlBrowser;
        public event Action<bool> OnEnableControls;
        public event Action<int> OnChangeProgressBar;
        public event Action<int> OnSelectRow;
        public event Action OnComplete;

        // check if need to go to the next link
        private bool CheckGoToNextLink()
        {
            if (_documentText == null)
                return false;

            var resultHtml = _documentText;
            string[] patterns =
            {
                "ERROR 404", "404 Not Found",
                @"<h1>This user is private.</h1>",
                @"<button type=""button"" class=""btn btn-default btn-xs done likeButton"""
            };

            return patterns.Any(pattern => Regex.IsMatch(resultHtml, pattern, RegexOptions.IgnoreCase));
        }

        // check if error 504
        private async Task CheckErorr504()
        {
            if (_documentText == null)
                return;

            var resultHtml = _documentText;
            const string regexPatt = @"ERROR 504";
            var result = Regex.IsMatch(resultHtml, regexPatt, RegexOptions.IgnoreCase);

            if (result)
            {
                await Task.Delay(120000);
                if (OnRefreshBrowser != null) OnRefreshBrowser.Invoke();
                if (OnWaitLoadingPage != null) OnWaitLoadingPage.Invoke(out _documentText);
                await CheckErorr504();
            }
        }

        // checking internet connection
        public async Task CheckInternetConnection()
        {
            if (!IsOnline())
            {
                await Task.Delay(120000);
                if (OnRefreshBrowser != null) OnRefreshBrowser.Invoke();
                if (OnWaitLoadingPage != null) OnWaitLoadingPage.Invoke(out _documentText);
                await CheckInternetConnection();
            }
        }

        // ping web-site
        public static bool IsOnline()
        {
            var pinger = new Ping();

            try
            {
                var pingReply = pinger.Send("www.websta.me");
                return pingReply != null && pingReply.Status == IPStatus.Success;
            }
            catch (SocketException)
            {
                return false;
            }
            catch (PingException)
            {
                return false;
            }
        }

        /* procedure for updating links
         * each tag is going to 60 and added to the links xml files */

        public void UpdateLinksUrl()
        {
            if (OnEnableControls != null) OnEnableControls.Invoke(false);

            for (var i = 0; i < ArrTagsInfo.GetUpperBound(0) + 1; i++)
            {
                ChangeProgress(ArrTagsInfo.GetUpperBound(0) + 1);

                if (OnSelectRow != null) OnSelectRow.Invoke(i);

                _downloadReadUrlList = new List<string>();
                var uriTag = new Uri("http://websta.me/tag/" + ArrTagsInfo[i, 0]);
                if (OnChangeUrlBrowser != null) OnChangeUrlBrowser.Invoke(uriTag);

                ParsePage();

                for (var j = 0; j < 2; j++)
                {
                    LoadNextPage();
                    ParsePage();
                }
                AddNewReadUrlInXml(ListFullNameTagFiles[i]);
            }
            CounterPb = 0;

            if (OnEnableControls != null) OnEnableControls.Invoke(true);
        }

        // parsing page
        private void ParsePage()
        {
            if (OnWaitLoadingPage != null) OnWaitLoadingPage.Invoke(out _documentText);
            const string regexPatt = @"<a href=""(?'url'.+)"" class=""mainimg"">";
            if (_documentText == null)
                return;

            var resultHtml = _documentText;
            var mcRes = Regex.Matches(resultHtml, regexPatt, RegexOptions.IgnoreCase);

            foreach (Match match in mcRes)
            {
                _downloadReadUrlList.Add(match.Groups["url"].Value);
            }
        }

        // load next page
        private void LoadNextPage()
        {
            if (OnWaitLoadingPage != null) OnWaitLoadingPage.Invoke(out _documentText);
            if (_documentText == null)
                return;

            var resultHtml = _documentText;
            const string regexPatt =
                @"<li><a href=""(?'url'.+)"" rel=""next""><i class=""fa fa-chevron-down""></i> Earlier</a></li>";
            var mcRes = Regex.Matches(resultHtml, regexPatt, RegexOptions.IgnoreCase);

            if (mcRes.Count == 0) return;

            var uriTag = new Uri("http://websta.me" + mcRes[0].Groups[1].Value);
            if (OnChangeUrlBrowser != null) OnChangeUrlBrowser.Invoke(uriTag);
        }

        // main procedure for likes
        public async void MainProcedureLiker()
        {
            if (OnEnableControls != null) OnEnableControls.Invoke(false);

            var indexTag = 0;
            // cycle all tags
            foreach (var tag in ListFullNameTagFiles)
            {
                // formation lists info
                MakeListsUrlAndEtc(tag);

                var countChangesPb = Math.Min(_existReadUrlList.Count(readUrl => !_existCompUrlList.Contains(readUrl)),
                    _countNeedLikes);

                if (countChangesPb == 0)
                {
                    indexTag++;
                    continue;
                }

                var swTimer = new Stopwatch();
                var countLikes = 0;
                swTimer.Start();

                if (OnSelectRow != null) OnSelectRow.Invoke(indexTag);
                // change status
                Data.Rows[indexTag]["Status"] = "Work";
                Data.Rows[indexTag]["NeedCountLikes"] = countChangesPb;

                foreach (var readUrl in _existReadUrlList.Where(readUrl => !_existCompUrlList.Contains(readUrl)))
                {
                    if (countLikes >= _countNeedLikes)
                    {
                        Data.Rows[indexTag]["Status"] = string.Format("Done for {0} min",
                            swTimer.Elapsed.Minutes);
                        break;
                    }

                    var uriTag = new Uri("http://websta.me" + readUrl);
                    if (OnChangeUrlBrowser != null) OnChangeUrlBrowser.Invoke(uriTag);

                    if (OnWaitLoadingPage != null) OnWaitLoadingPage.Invoke(out _documentText);
                    AddLikedUrlInXml(readUrl);

                    await CheckInternetConnection();

                    if (CheckGoToNextLink()) continue;

                    await CheckErorr504();

                    if (OnClickLike != null)
                        OnClickLike.Invoke();

                    await Task.Delay((int) Math.Round(_interval*60000));
                    countLikes++;
                    Data.Rows[indexTag]["CompCountLikes"] = countLikes;
                    ChangeProgress(countChangesPb);
                }
                CounterPb = 0;
                indexTag++;
            }

            Data.AcceptChanges();

            if (OnEnableControls != null) OnEnableControls.Invoke(true);
            if (OnComplete != null) OnComplete.Invoke();
        }

        // changing the progress bar when loading
        private void ChangeProgress(int countChanges)
        {
            Application.DoEvents();
            CounterPb++;

            if (OnChangeProgressBar != null) OnChangeProgressBar.Invoke(CounterPb*(100/countChanges));
        }
    }
}