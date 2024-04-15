using UnityEngine;

public abstract class Dialog<T> : Dialog where T : Dialog<T>
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        Instance = (T) this;
    }

    protected virtual void OnDestroy()
    {
        Instance = null;
    }

    protected static void Open()
    {
        if (Instance == null)
            MenuManager.Instance.CreateDialog<T>();
        else
            Instance.gameObject.SetActive(true);

        MenuManager.Instance.OpenDialog(Instance);
    }

    protected static void Close()
    {
        if (Instance == null)
        {
#if UNITY_EDITOR
            Debug.LogErrorFormat("Trying to close dialog {0} but Instance is null", typeof(T));
#endif
            return;
        }

        MenuManager.Instance.CloseDialog(Instance);
    }

    public override void OnBackPressed()
    {
        if (!Cancelable)
            return;
        Close();
    }

    public override void OnDialogBecameVisible()
    {
    }
}

public abstract class Dialog : MonoBehaviour
{
    [Tooltip("Destroy the Game Object when dialog is closed (reduces memory usage)")]
    public bool DestroyWhenClosed = true;

    [Tooltip("Cancelable dialog when press back key")]
    public bool Cancelable = true;

    public abstract void OnDialogBecameVisible();
    public abstract void OnBackPressed();
}