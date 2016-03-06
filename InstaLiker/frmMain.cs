using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace InstaLiker
{
    public partial class FrmMain : MetroForm
    {
        private Presenter _presenter;

        public FrmMain()
        {
            InitializeComponent();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            try
            {
                wbMain.Navigate("http://websta.me/login");
                _presenter = new Presenter(wbMain.DocumentText);

                _presenter.LoadTagsToGrid();

                dgrTagsInfo.DataSource = _presenter.Data;
                _presenter.OnClickLike += ClickLike;
                _presenter.OnWaitLoadingPage += WaitLoadPage;
                _presenter.OnRefreshBrowser += () => { wbMain.Refresh(); };
                _presenter.OnEnableControls += EnableCtrls;
                _presenter.OnChangeProgressBar += i => { pbMain.Value = i; };
                _presenter.OnChangeUrlBrowser += uri => { wbMain.Navigate(uri); };
                _presenter.OnSelectRow += row => { dgrTagsInfo.Rows[row].Selected = true; };
                _presenter.OnComplete += () => { MessageBox.Show("Done", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information); };
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnUpdateLinks_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.UpdateLinksUrl();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.StartLiker();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAddTag_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbTagName.Text == string.Empty ||
                    tbCountLike.Text == string.Empty ||
                    tbInterval.Text == string.Empty)
                {
                    MessageBox.Show("Not all data is entered", Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int check;
                if (!int.TryParse(tbCountLike.Text, out check) ||
                    !int.TryParse(tbInterval.Text, out check))
                {
                    MessageBox.Show("Data not is numeric", Application.ProductName,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _presenter.CreateXmlTag(tbTagName.Text, tbCountLike.Text, tbInterval.Text);

                tbTagName.Text = string.Empty;
                tbCountLike.Text = string.Empty;
                tbInterval.Text = string.Empty;

                MessageBox.Show("Ok", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void dgrTagsInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                var row = dgrTagsInfo.Rows[e.RowIndex];
                var col = dgrTagsInfo.Columns[e.ColumnIndex];
                if (col.Index == 1 || col.Index == 2)
                    _presenter.ChangeInfo(col.Name,
                        dgrTagsInfo.Rows[row.Index].Cells[col.Index].Value.ToString(),
                        dgrTagsInfo.Rows[row.Index].Cells[0].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // clear cache in active page
        private void btnClearCache_Click(object sender, EventArgs e)
        {
            try
            {
                var document = wbMain.Document;
                if (document != null) document.ExecCommand("ClearAuthenticationCache", false, null);
                wbMain.Navigate(
                    "javascript:void((function(){var a,b,c,e,f;f=0;a=document.cookie.split('; ');for(e=0;e<a.length&&a[e];e++){f++;for(b='.'+location.host;b;b=b.replace(/^(?:%5C.|[^%5C.]+)/,'')){for(c=location.pathname;c;c=c.replace(/.$/,'')){document.cookie=(a[e]+'; domain='+b+'; path='+c+'; expires='+new Date((new Date()).getTime()-1e11).toGMTString());}}}})())");
               
                MessageBox.Show("Cache cleared", Application.ProductName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // Like with bypass block
        private void ClickLike()
        {
            try
            {
                if (wbMain.Document == null) return;

                wbMain.Document.GetElementsByTagName("ul")
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

        // waiting a full page load with timer
        private void WaitLoadPage(out string documentText)
        {
            Text = "Loading...";
            var sw = new Stopwatch();
            sw.Start();

            do
            {
                Application.DoEvents();

                if (sw.Elapsed.Minutes >= 3)
                {
                    wbMain.Refresh();
                    sw.Restart();
                }
            } while (wbMain.ReadyState != WebBrowserReadyState.Complete || wbMain.IsBusy);

            documentText = wbMain.DocumentText;

            Text = Application.ProductName;
        }

        // enabled controls
        private void EnableCtrls(bool state)
        {
            btnStart.Enabled = state;
            btnUpdateLinks.Enabled = state;
            btnAddTag.Enabled = state;
            btnClearCache.Enabled = state;
            dgrTagsInfo.ReadOnly = !state;
        }
    }
}