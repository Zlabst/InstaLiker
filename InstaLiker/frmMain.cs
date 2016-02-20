using MetroFramework.Forms;
using System;
using System.Windows.Forms;

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
                _presenter = new Presenter(this);
                _presenter.LoadTagsToGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }

        private void btnUpdateLinks_Click_1(object sender, EventArgs e)
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
                _presenter.CreateXmlTag();
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

        private void btnStop_Click(object sender, EventArgs e)
        {
            try
            {
                _presenter.ClearCache();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
            }
        }
    }
}