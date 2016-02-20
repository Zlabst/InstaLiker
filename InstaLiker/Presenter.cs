using System.Windows.Forms;

namespace InstaLiker
{
    public class Presenter
    {
        private readonly FrmMain _frmMain;
        private readonly Model _model;

        public Presenter(FrmMain frmmain)
        {
            _frmMain = frmmain;
            _model = new Model(_frmMain);
        }

        // download file names (tags) from the folder
        public void LoadTagsToGrid()
        {
            _model.LoadTagsInfo();
            var arrTagsInfo = _model.ArrTagsInfo;
            _frmMain.dgrTagsInfo.Rows.Clear();

            for (var i = 0; i < arrTagsInfo.GetUpperBound(0) + 1; i++)
            {
                _frmMain.dgrTagsInfo.Rows.Add(arrTagsInfo[i, 0],
                    arrTagsInfo[i, 1],
                    arrTagsInfo[i, 2]);
            }
        }

        // each tag is going to 60 and added to the links xml files
        public void UpdateLinksUrl()
        {
            _model.StopProcedure = false;
            _frmMain.pbMain.Value = 0;
            _model.UpdateLinksUrl();
            MessageBox.Show("Ok", Application.ProductName,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // main procedure for likes
        public void StartLiker()
        {
            _model.StopProcedure = false;
            _frmMain.pbMain.Value = 0;
            _model.MainProcedureLiker();
            _frmMain.pbMain.Value = 0;
        }

        // creating xml file with tag
        public void CreateXmlTag()
        {
            if (_frmMain.tbTagName.Text == string.Empty ||
                _frmMain.tbCountLike.Text == string.Empty ||
                _frmMain.tbInterval.Text == string.Empty)
            {
                MessageBox.Show("Not all data is entered", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int check;
            if (!int.TryParse(_frmMain.tbCountLike.Text, out check) ||
                !int.TryParse(_frmMain.tbInterval.Text, out check))
            {
                MessageBox.Show("Data not is numeric", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_model.ListNameTagFiles.Contains(_frmMain.tbTagName.Text + ".XML"))
            {
                MessageBox.Show("This tag is already", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _model.CreateXmlFileByTag();
            LoadTagsToGrid();

            _frmMain.tbTagName.Text = string.Empty;
            _frmMain.tbCountLike.Text = string.Empty;
            _frmMain.tbInterval.Text = string.Empty;

            MessageBox.Show("Ok", Application.ProductName,
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // change info in Xml file
        public void ChangeInfo(string columnName, string newValue, string name)
        {
            _model.ChangeElement(columnName, newValue, name);
        }

        // stop main procedure
        public void StopProcedure()
        {
            _model.StopProcedure = true;
            _model.CounterPb = 0;
            _frmMain.pbMain.Value = 0;
        }

        public void ClearCache()
        {
            _model.ClearCache();
        }
    }
}