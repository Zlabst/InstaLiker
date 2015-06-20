﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InstaLiker
{
    public partial class Model
    {
        public int CounterPb;
        private List<string> _downloadReadUrlList;
        public bool StopProcedure = false;
        // check or put like
        private bool IsLiked
        {
            get
            {
                try
                {
                    var resultHtml = _mainWebBrowser.DocumentText;
                    const string regexPatt = @"<button type=""button"" class=""btn btn-default btn-xs done likeButton""";
                    return Regex.IsMatch(resultHtml, regexPatt, RegexOptions.IgnoreCase);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                return false;
            }
        }

        // waiting a full page load
        private void WaitLoadPage(WebBrowser webBrowser)
        {
            try
            {
                _frmMain.Text = "Loading...";
                do
                {
                    Application.DoEvents();
                } while (webBrowser.ReadyState != WebBrowserReadyState.Complete || webBrowser.IsBusy);
                _frmMain.Text = HeaderFrmText;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /* procedure for updating links
         * each tag is going to 60 and added to the links xml files 
        */

        public void UpdateLinksUrl()
        {
            EnableCtrls(false);

            for (var i = 0; i < ArrTagsInfo.GetUpperBound(0) + 1; i++)
            {
                if (StopProcedure) { EnableCtrls(true); return; }
                ChangeProgress(ArrTagsInfo.GetUpperBound(0) + 1);

                _downloadReadUrlList = new List<string>();
                var uriTag = new Uri("http://websta.me/tag/" + ArrTagsInfo[i, 0]);
                _mainWebBrowser.Navigate(uriTag);

                ParsePage();

                for (var j = 0; j < 2; j++)
                {
                    if (StopProcedure) { EnableCtrls(true); return; }
                    LoadNextPage();
                    ParsePage();
                }
                AddNewReadUrlInXml(ListFullNameTagFiles[i]);
            }
            CounterPb = 0;

            EnableCtrls(true);
        }

        // enabled controls
        private void EnableCtrls(bool status)
        {
            _frmMain.btnStart.Enabled = status;
            _frmMain.btnUpdateLinks.Enabled = status;
            _frmMain.btnAddTag.Enabled = status;
        }

        // parsing page
        private void ParsePage()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // load next page
        private void LoadNextPage()
        {
            try
            {
                WaitLoadPage(_mainWebBrowser);
                var resultHtml = _mainWebBrowser.DocumentText;
                const string regexPatt =
                    @"<li><a href=""(?'url'.+)"" rel=""next""><i class=""fa fa-chevron-down""></i> Earlier</a></li>";
                var mcRes = Regex.Matches(resultHtml, regexPatt, RegexOptions.IgnoreCase);

                if (mcRes.Count == 0) return;

                _mainWebBrowser.Navigate("http://websta.me" + mcRes[0].Groups[1].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // main procedure for likes
        public async void MainProcedureLiker()
        {
            EnableCtrls(false);

            var indexTag = 0;
            // cycle all tags
            foreach (var tag in ListFullNameTagFiles)
            {
                if (StopProcedure) { EnableCtrls(true);  return; }
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
                    if (StopProcedure) { EnableCtrls(true); return; }
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

                    if (IsLiked) continue;

                    ClickLike();

                    await Task.Delay((int) Math.Round(Interval*60000));
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

        // Like
        private void ClickLike()
        {
            try
            {
                if (_mainWebBrowser.Document == null) return;
                _mainWebBrowser.Document.GetElementsByTagName("ul")
                    .Cast<HtmlElement>()
                    .Where(el => el.GetAttribute("className") == "list-inline pull-left").ToList()[0]
                    .GetElementsByTagName("button")[0].InvokeMember("Click");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // changing the progress bar when loading
        private void ChangeProgress(int countChanges)
        {
            Application.DoEvents();
            CounterPb++;
            _mainProgress.Value = (CounterPb*(100/countChanges));
        }
    }
}