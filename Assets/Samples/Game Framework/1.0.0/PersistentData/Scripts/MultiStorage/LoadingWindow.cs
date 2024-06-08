namespace GameFramework.Samples.PersistentData
{
    public class LoadingWindow : UIWindow
    {
        public override void Show(object arg)
        {
            base.Show(arg);
            string storageName = (string) arg;
            StorageAsyncOperation operation = PersistentManager.Instance.LoadAsync(storageName);
            operation.OnCompleted += _ =>
            {
                UIManager.Instance.HideWindow(WindowName.Loading);
                UIManager.Instance.ShowWindow(WindowName.Game, arg);
            };
        }
    }
}