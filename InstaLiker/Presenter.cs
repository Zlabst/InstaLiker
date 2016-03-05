using System;
using System.Data;
using System.Windows.Forms;
using InstaLiker.ModelData;

namespace InstaLiker
{
    public class Presenter : IModelComponents
    {
        private readonly Model _model;

        public Presenter(string documentText)
        {
            _model = new Model(documentText);
            Data = _model.Data;

            _model.OnClickLike += () => { if (OnClickLike != null) OnClickLike.Invoke(); };
            _model.OnWaitLoadingPage += WaitLoadingPage;
            _model.OnRefreshBrowser += () => { if (OnRefreshBrowser != null) OnRefreshBrowser.Invoke(); };
            _model.OnChangeUrlBrowser += uri => { if (OnChangeUrlBrowser != null) OnChangeUrlBrowser.Invoke(uri); };
            _model.OnEnableControls += state => { if (OnEnableControls != null) OnEnableControls.Invoke(state); };
            _model.OnChangeProgressBar += i => { if (OnChangeProgressBar != null) OnChangeProgressBar.Invoke(i); };
            _model.OnSelectRow += row => { if (OnSelectRow != null) OnSelectRow.Invoke(row); };
            _model.OnComplete += () => { if (OnComplete != null) OnComplete.Invoke(); };
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

        private void WaitLoadingPage(out string documentText)
        {
            string docText = null;
            if (OnWaitLoadingPage != null) OnWaitLoadingPage.Invoke(out docText);
            documentText = docText;
        }

        // download file names (tags) from the folder
        public void LoadTagsToGrid()
        {
            _model.LoadTagsInfo();
            var arrTagsInfo = _model.ArrTagsInfo;

            _model.Data.Rows.Clear();

            for (var i = 0; i < arrTagsInfo.GetUpperBound(0) + 1; i++)
            {
                DataRow row = _model.Data.NewRow();
                row["TagName"] = arrTagsInfo[i, 0];
                row["Interval"] = arrTagsInfo[i, 1];
                row["NeedCountLikes"] = arrTagsInfo[i, 2];
                _model.Data.Rows.Add(row);
            }

            _model.Data.AcceptChanges();
        }

        // each tag is going to 60 and added to the links xml files
        public void UpdateLinksUrl()
        {
            if (OnChangeProgressBar != null) OnChangeProgressBar.Invoke(0);

            _model.UpdateLinksUrl();

            if (OnComplete != null) OnComplete.Invoke();
        }

        // main procedure for likes
        public void StartLiker()
        {
            if (OnChangeProgressBar != null) OnChangeProgressBar.Invoke(0);
            _model.MainProcedureLiker();
            if (OnChangeProgressBar != null) OnChangeProgressBar.Invoke(0);
        }

        // creating xml file with tag
        public void CreateXmlTag(string tagName, string countLikes, string interval)
        {
            if (_model.ListNameTagFiles.Contains(tagName + ".XML"))
            {
                MessageBox.Show("This tag is already", Application.ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _model.CreateXmlFileByTag(tagName, countLikes, interval);
            LoadTagsToGrid();
        }

        // change info in Xml file
        public void ChangeInfo(string columnName, string newValue, string name)
        {
            _model.ChangeElement(columnName, newValue, name);
        }
    }
}