using System;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace InstaLiker
{
    public partial class FrmMain : MetroForm
    {
        private readonly Presenter _presenter;

        public FrmMain()
        {
            InitializeComponent();
            wbMain.Navigate("http://websta.me/login");
            _presenter = new Presenter(this);
            _presenter.LoadTagsToGrid();
        }

        private void btnUpdateLinks_Click_1(object sender, EventArgs e)
        {
            _presenter.UpdateLinksUrl();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            _presenter.StartLiker();
        }

        private void btnAddTag_Click(object sender, EventArgs e)
        {
            _presenter.CreateXmlTag();
        }

        private void dgrTagsInfo_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var row = dgrTagsInfo.Rows[e.RowIndex];
            var col = dgrTagsInfo.Columns[e.ColumnIndex];
            if (col.Index == 1 || col.Index == 2)
                _presenter.ChangeInfo(col.Name,
                    dgrTagsInfo.Rows[row.Index].Cells[col.Index].Value.ToString(),
                    dgrTagsInfo.Rows[row.Index].Cells[0].Value.ToString());
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (
                MessageBox.Show("Close", Application.ProductName, MessageBoxButtons.OKCancel, MessageBoxIcon.Question) !=
                DialogResult.OK)
                e.Cancel = true;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            _presenter.StopProcedure();
        }
    }
}