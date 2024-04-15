using System;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public Menu mainMenu;

    public Menu[] menuPrefabs;
    public Dialog[] dialogPrefabs;

    private readonly Stack<Menu> menuStack = new Stack<Menu>();
    private readonly List<Dialog> dialogs = new List<Dialog>();

    public static MenuManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;

        //Instantiate and open default menu
        if (mainMenu != null)
        {
            var menu = Instantiate(mainMenu, transform);
            OpenMenu(menu);
        }
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    #region MENU

    public void CreateInstance<T>() where T : Menu
    {
        var prefab = GetPrefab<T>();

        Instantiate(prefab, transform);
    }

    private T GetPrefab<T>() where T : Menu
    {
        for (var i = 0; i < menuPrefabs.Length; i++)
        {
            if (menuPrefabs[i] != null && menuPrefabs[i].GetType() == typeof(T))
            {
                return (T) menuPrefabs[i];
            }
        }

        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }

    public void OpenMenu(Menu instance)
    {
        // De-activate top menu
        var maxSortingOrder = 0;
        var topCanvas = instance.GetComponent<Canvas>();
        if (dialogs.Count > 0)
        {
            foreach (var dialog in dialogs)
            {
                if (dialog != null && dialog.GetComponent<Canvas>() != null)
                {
                    var sortingOrder = dialog.GetComponent<Canvas>().sortingOrder;
                    if (sortingOrder > maxSortingOrder)
                        maxSortingOrder = sortingOrder;
                }
            }
        }

        if (menuStack.Count > 0)
        {
            if (instance.DisableMenusUnderneath)
            {
                foreach (var menu in menuStack)
                {
                    menu.gameObject.SetActive(false);
                    if (menu.DisableMenusUnderneath)
                        break;
                }
            }

            var topMenuCanvas = menuStack.Peek().GetComponent<Canvas>();
            if (topMenuCanvas != null && topMenuCanvas.sortingOrder > maxSortingOrder)
                maxSortingOrder = topMenuCanvas.sortingOrder;
        }

        topCanvas.sortingOrder = maxSortingOrder + 1;
        if (menuStack.Count > 0)
        {
            var topMenu = menuStack.Peek();
            if (topMenu != null)
                topMenu.OnMenuBecameInvisible();
        }

        instance.OnMenuBecameVisible();
        if (!menuStack.Contains(instance))
            menuStack.Push(instance);
        Debug.Log("OpenMenu: " + instance.name);
    }

    public void CloseMenu(Menu menu)
    {
        if (menuStack.Count == 0)
        {
            Debug.LogErrorFormat(menu, "{0} cannot be closed because menu stack is empty", menu.GetType());
            return;
        }

        if (menuStack.Peek() != menu)
        {
            Debug.LogErrorFormat(menu, "{0} cannot be closed because it is not on top of stack", menu.GetType());
            return;
        }

        CloseTopMenu();
    }

    public void CloseTopMenu()
    {
        var instance = menuStack.Pop();

        if (instance.DestroyWhenClosed)
            Destroy(instance.gameObject);
        else
            instance.gameObject.SetActive(false);

        // Re-activate top menu
        // If a re-activated menu is an overlay we need to activate the menu under it

        Menu topMenu = null;
        if (dialogs.Count <= 0)
        {
            if (menuStack.Count > 0)
            {
                topMenu = menuStack.Peek();
                topMenu.OnMenuBecameVisible();
            }

            foreach (var menu in menuStack)
            {
                menu.gameObject.SetActive(true);
                if (menu.DisableMenusUnderneath)
                    break;
            }

            return;
        }

        Dialog topDialog = null;
        if (menuStack.Count > 0)
        {
            topMenu = menuStack.Peek();
        }

        var maxSortingOrder = (topMenu == null && dialogs[0] != null && dialogs[0].GetComponent<Canvas>() != null)
            ? dialogs[0].GetComponent<Canvas>().sortingOrder
            : topMenu.GetComponent<Canvas>().sortingOrder;
        foreach (var dialog in dialogs)
        {
            if (dialog == null || dialog.GetComponent<Canvas>() == null)
                continue;
            if (maxSortingOrder < dialog.GetComponent<Canvas>().sortingOrder)
            {
                topDialog = dialog;
                topMenu = null;
            }
        }

        if (topDialog != null)
        {
            topDialog.gameObject.SetActive(true);
            topDialog.OnDialogBecameVisible();
        }
        else if (topMenu != null)
        {
            foreach (var menu in menuStack)
            {
                menu.gameObject.SetActive(true);
                if (menu.DisableMenusUnderneath)
                    break;
            }

            topMenu.OnMenuBecameVisible();
        }
    }

    private void Update()
    {
        // On Android the back button is sent as Esc
        if (!Input.GetKeyUp(KeyCode.Escape))
        {
            return;
        }

        Dialog topDialog = null;
        Menu topMenu = null;
        if (menuStack.Count > 0)
        {
            topMenu = menuStack.Peek();
        }

        if (dialogs.Count <= 0)
        {
            if (topMenu != null)
                topMenu.OnBackPressed();
            return;
        }

        var maxSortingOrder = (topMenu == null && dialogs[0] != null && dialogs[0].GetComponent<Canvas>() != null)
            ? dialogs[0].GetComponent<Canvas>().sortingOrder
            : topMenu.GetComponent<Canvas>().sortingOrder;
        foreach (var dialog in dialogs)
        {
            if (dialog == null || dialog.GetComponent<Canvas>() == null)
                continue;
            if (maxSortingOrder < dialog.GetComponent<Canvas>().sortingOrder)
            {
                topDialog = dialog;
                topMenu = null;
            }
        }

        if (topMenu != null)
            topMenu.OnBackPressed();
        if (topDialog != null)
            topDialog.OnBackPressed();
    }

    #endregion

    #region DIALOG

    public void CreateDialog<T>() where T : Dialog
    {
        var prefab = GetDialogPrefab<T>();

        Instantiate(prefab, transform);
    }

    private T GetDialogPrefab<T>() where T : Dialog
    {
        for (var i = 0; i < dialogPrefabs.Length; i++)
        {
            if (dialogPrefabs[i] != null && dialogPrefabs[i].GetType() == typeof(T))
            {
                return (T) dialogPrefabs[i];
            }
        }

        throw new MissingReferenceException("Prefab not found for type " + typeof(T));
    }

    public void OpenDialog(Dialog instance)
    {
        // De-activate top menu
        var maxSortingOrder = 0;
        var topCanvas = instance.GetComponent<Canvas>();
//        dialogs.Remove(instance);
        if (dialogs.Count > 0)
        {
            foreach (var dialog in dialogs)
            {
                if (dialog == null || dialog.GetComponent<Canvas>() == null)
                {
                    continue;
                }

                var sortingOrder = dialog.GetComponent<Canvas>().sortingOrder;
                if (sortingOrder > maxSortingOrder)
                    maxSortingOrder = sortingOrder;
            }
        }

        if (menuStack.Count > 0)
        {
            var topMenuCanvas = menuStack.Peek().GetComponent<Canvas>();
            if (topMenuCanvas != null && topMenuCanvas.sortingOrder > maxSortingOrder)
                maxSortingOrder = topMenuCanvas.sortingOrder;
        }

        topCanvas.sortingOrder = maxSortingOrder + 1;
        if (!dialogs.Contains(instance))
            dialogs.Add(instance);
        if (menuStack.Count > 0)
        {
            var topMenu = menuStack.Peek();
            if (topMenu != null)
                topMenu.OnMenuBecameInvisible();
        }

        instance.OnDialogBecameVisible();
    }

    public void CloseDialog(Dialog instance)
    {
        if (dialogs.Count == 0)
        {
//            Debug.LogErrorFormat(instance, "{0} cannot be closed because dialog list is empty", instance.GetType());
            return;
        }

        for (var i = 0; i < dialogs.Count; i++)
        {
            try
            {
                if (instance != dialogs[i])
                {
                    continue;
                }

                dialogs.Remove(dialogs[i]);
                if (instance.DestroyWhenClosed)
                    Destroy(instance.gameObject);
                else
                    instance.gameObject.SetActive(false);

                //Re-active top dialog
                Dialog topDialog = null;
                Menu topMenu = null;
                if (menuStack.Count > 0)
                {
                    topMenu = menuStack.Peek();
                }

                if (topMenu == null && dialogs.Count == 0)
                    return;
                var maxSortingOrder =
                    (topMenu == null && dialogs[0] != null && dialogs[0].GetComponent<Canvas>() != null)
                        ? dialogs[0].GetComponent<Canvas>().sortingOrder
                        : topMenu.GetComponent<Canvas>().sortingOrder;
                foreach (var dialog in dialogs)
                {
                    if (instance == dialog || dialog == null || dialog.GetComponent<Canvas>() == null)
                        continue;
                    if (maxSortingOrder < dialog.GetComponent<Canvas>().sortingOrder)
                    {
                        topDialog = dialog;
                        topMenu = null;
                    }
                }

                if (topDialog != null && topDialog.gameObject.activeSelf)
                {
//                    topDialog.gameObject.SetActive(true);
                    topDialog.OnDialogBecameVisible();
                }
                else if (topMenu != null)
                {
                    foreach (var menu in menuStack)
                    {
                        menu.gameObject.SetActive(true);
                        if (menu.DisableMenusUnderneath)
                            break;
                    }

                    topMenu.OnMenuBecameVisible();
                }

                break;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }

    #endregion
}