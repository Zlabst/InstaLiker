using System;

namespace InstaLiker.ModelData
{
    public delegate void WaitLoadingPage(out string val);

    internal interface IModelComponents
    {
        event WaitLoadingPage OnWaitLoadingPage;
        event Action OnClickLike;
        event Action OnRefreshBrowser;
        event Action<Uri> OnChangeUrlBrowser;
        event Action<bool> OnEnableControls;
        event Action<int> OnChangeProgressBar;
        event Action<int> OnSelectRow;
        event Action OnComplete;
    }
}