using System;
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
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // each tag is going to 60 and added to the links xml files
        public void UpdateLinksUrl()
        {
            try
            {
                _model.StopProcedure = false;
                _model.UpdateLinksUrl();
                MessageBox.Show("Ok", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // main procedure for likes
        public void StartLiker()
        {
            try
            {
                _model.StopProcedure = false;
                _model.MainProcedureLiker();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // creating xml file with tag
        public void CreateXmlTag()
        {
            try
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // change info in Xml file
        public void ChangeInfo(string columnName, string newValue, string name)
        {
            try
            {
                _model.ChangeElement(columnName, newValue, name);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace, Application.ProductName, MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        // stop main procedure
        public void StopProcedure()
        {
            _model.StopProcedure = true;
            _model.CounterPb = 0;
            _frmMain.pbMain.Value = 0;
        }
    }
}