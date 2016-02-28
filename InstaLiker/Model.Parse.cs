using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstaLiker
{
    public partial class Model
    {
        private List<string> _downloadReadUrlList;
        public int CounterPb;
        public bool StopProcedure = false;

        // check if need to go to the next link
        private bool NeedGoToNextLink
        {
            get
            {
                var resultHtml = _mainWebBrowser.DocumentText;
                string[] patterns = { "ERROR 404", "404 Not Found",
                                        @"<h1>This user is private.</h1>",
                                        @"<button type=""button"" class=""btn btn-default btn-xs done likeButton"""};

                return patterns.Any(pattern => Regex.IsMatch(resultHtml, pattern, RegexOptions.IgnoreCase));
            }
        }

        // check if error 504
        private async Task CheckErorr504()
        {
            var resultHtml = _mainWebBrowser.DocumentText;
            const string regexPatt = @"ERROR 504";
            var result = Regex.IsMatch(resultHtml, regexPatt, RegexOptions.IgnoreCase);

            if (result)
            {
                await Task.Delay(120000);
                _mainWebBrowser.Refresh();
                WaitLoadPage(_mainWebBrowser);
                await CheckErorr504();
            }
        }

        // checking internet connection
        public async Task CheckInternetConnection()
        {
            if (!IsOnline())
            {
                await Task.Delay(120000);
                _mainWebBrowser.Refresh();
                WaitLoadPage(_mainWebBrowser);
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

        // check if account off and on in account
        private void LoginOn(Uri curUrl)
        {
            const string regexPatt = @"<li><a href=""/login"">Войти</a></li>";

            while (Regex.IsMatch(_mainWebBrowser.DocumentText, regexPatt, RegexOptions.IgnoreCase))
            {
                _mainWebBrowser.Navigate("http://websta.me/login");
                WaitLoadPage(_mainWebBrowser);
                _mainWebBrowser.Navigate(curUrl);
                WaitLoadPage(_mainWebBrowser);
            }
        }

        // waiting a full page load with timer
        private void WaitLoadPage(WebBrowser webBrowser)
        {
            var sw = new Stopwatch();
            sw.Start();

            _frmMain.Text = "Loading...";
            do
            {
                Application.DoEvents();

                if (sw.Elapsed.Minutes >= 3)
                {
                    _mainWebBrowser.Refresh();
                    sw.Restart();
                }
            } while (webBrowser.ReadyState != WebBrowserReadyState.Complete || webBrowser.IsBusy);

            _frmMain.Text = HeaderFrmText;
        }

        /* procedure for updating links
         * each tag is going to 60 and added to the links xml files 
        */

        public void UpdateLinksUrl()
        {
            EnableCtrls(false);

            for (var i = 0; i < ArrTagsInfo.GetUpperBound(0) + 1; i++)
            {
                if (StopProcedure)
                {
                    EnableCtrls(true);
                    return;
                }
                ChangeProgress(ArrTagsInfo.GetUpperBound(0) + 1);

                _downloadReadUrlList = new List<string>();
                var uriTag = new Uri("http://websta.me/tag/" + ArrTagsInfo[i, 0]);
                _mainWebBrowser.Navigate(uriTag);

                ParsePage();

                for (var j = 0; j < 2; j++)
                {
                    if (StopProcedure)
                    {
                        EnableCtrls(true);
                        return;
                    }
                    LoadNextPage();
                    ParsePage();
                }
                AddNewReadUrlInXml(ListFullNameTagFiles[i]);
            }
            CounterPb = 0;

            EnableCtrls(true);
        }

        // enabled controls
        private void EnableCtrls(bool state)
        {
            _frmMain.BtnStart.Enabled = state;
            _frmMain.BtnUpdateLinks.Enabled = state;
            _frmMain.BtnAddTag.Enabled = state;
            _frmMain.BtnStop.Enabled = state;
        }

        // parsing page
        private void ParsePage()
        {
            WaitLoadPage(_mainWebBrowser);
            const string regexPatt = @"<a href=""(?'url'.+)"" class=""mainimg"">";
            var resultHtml = _mainWebBrowser.DocumentText;
            var mcRes = Regex.Matches(resultHtml, regexPatt, RegexOptions.IgnoreCase);

            foreach (Match match in mcRes)
            {
                _downloadReadUrlList.Add(match.Groups["url"].Value);
            }
        }

        // load next page
        private void LoadNextPage()
        {
            WaitLoadPage(_mainWebBrowser);
            var resultHtml = _mainWebBrowser.DocumentText;
            const string regexPatt =
                @"<li><a href=""(?'url'.+)"" rel=""next""><i class=""fa fa-chevron-down""></i> Earlier</a></li>";
            var mcRes = Regex.Matches(resultHtml, regexPatt, RegexOptions.IgnoreCase);

            if (mcRes.Count == 0) return;

            _mainWebBrowser.Navigate("http://websta.me" + mcRes[0].Groups[1].Value);
        }

        // main procedure for likes
        public async void MainProcedureLiker()
        {
            EnableCtrls(false);

            var indexTag = 0;
            // cycle all tags
            foreach (var tag in ListFullNameTagFiles)
            {
                if (StopProcedure)
                {
                    EnableCtrls(true);
                    return;
                }
                // formation lists info
                MakeListsUrlAndEtc(tag);

                var countChangesPb = Math.Min(ExistReadUrlList.Count(readUrl => !ExistCompUrlList.Contains(readUrl)),
                    CountNeedLikes);
                if (countChangesPb == 0)
                {
                    indexTag++;
                    continue;
                }

                var swTimer = new Stopwatch();
                var countLikes = 0;
                swTimer.Start();
                _mainMetroGrid.Rows[indexTag].Selected = true;
                // change status
                _mainMetroGrid.Rows[indexTag].Cells[4].Value = "Work";
                _mainMetroGrid.Rows[indexTag].Cells[2].Value = countChangesPb;
                _mainMetroGrid.Update();

                foreach (var readUrl in ExistReadUrlList.Where(readUrl => !ExistCompUrlList.Contains(readUrl)))
                {
                    if (StopProcedure)
                    {
                        EnableCtrls(true);
                        return;
                    }
                    if (countLikes >= CountNeedLikes)
                    {
                        _mainMetroGrid.Rows[indexTag].Cells[4].Value = string.Format("Done for {0} min",
                            swTimer.Elapsed.Minutes);
                        _mainMetroGrid.Update();
                        break;
                    }

                    _mainWebBrowser.Navigate("http://websta.me" + readUrl);
                    WaitLoadPage(_mainWebBrowser);
                    AddLikedUrlInXml(readUrl);

                    await CheckInternetConnection();

                    if (NeedGoToNextLink) continue;

                    await CheckErorr504();

                    ClickLike();

                    await Task.Delay((int)Math.Round(Interval * 60000));
                    countLikes++;
                    _mainMetroGrid.Rows[indexTag].Cells[3].Value = countLikes;
                    _mainMetroGrid.Update();
                    ChangeProgress(countChangesPb);
                }
                CounterPb = 0;
                indexTag++;
            }

            EnableCtrls(true);
        }

        // Like with bypass block
        private void ClickLike()
        {
            if (_mainWebBrowser.Document == null) return;

            _mainWebBrowser.Document.GetElementsByTagName("ul")
                .Cast<HtmlElement>()
                .Where(el => el.GetAttribute("className") == "list-inline pull-left").ToList()[0]
                .GetElementsByTagName("button")[0].InvokeMember("Click");
        }

        // changing the progress bar when loading
        private void ChangeProgress(int countChanges)
        {
            Application.DoEvents();
            CounterPb++;
            _mainProgress.Value = (CounterPb * (100 / countChanges));
        }

        public void ClearCache()
        {
            var document = _mainWebBrowser.Document;
            if (document != null) document.ExecCommand("ClearAuthenticationCache", false, null);
            _mainWebBrowser.Navigate(
                "javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((new Date()).getTime()-1e11).toGMTString());}}}})())");
        }
    }
}