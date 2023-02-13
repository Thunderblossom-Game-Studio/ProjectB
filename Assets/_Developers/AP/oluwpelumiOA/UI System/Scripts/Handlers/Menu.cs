using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public abstract class Menu : MonoBehaviour
{
    public static List<Menu> ActiveMenus { get; private set; } = new List<Menu>();

    public static Menu lastMenu { get; private set; }
    public static Menu currentMenu { get; private set; }
    public static Type LastMenuType { get; private set; }

    public static EventHandler OnAnyUIOpened;
    public static EventHandler OnAnyUIClosed;
    public static EventHandler OnAnyUIDestroyed;

    protected Coroutine openCloseRoutine;
        
    [SerializeField] protected bool destroyOnClose = false;
    [SerializeField] protected bool navigatable = true;
    [SerializeField] protected Selectable firstSelectedButton;

    protected virtual void Awake()
    {
        ActiveMenus.Add(this);
    }

    public virtual void OpenMenu(Action OnComplected = null)
    {
        if(currentMenu && currentMenu.navigatable)
        {
            lastMenu = currentMenu;
            LastMenuType = GetType();
        }

        currentMenu = this;

        ResetUI();
        
        gameObject.SetActive(true);

        OnMenuOpened();

        if (openCloseRoutine != null) StopCoroutine(openCloseRoutine);
        openCloseRoutine = StartCoroutine(OpenMenuRoutine());
        
        if (OnAnyUIOpened != null) OnAnyUIOpened.Invoke(this, EventArgs.Empty);
    }

    public virtual void CloseMenu(Action OnComplected = null)
    {
        if (openCloseRoutine != null) StopCoroutine(openCloseRoutine); 
        openCloseRoutine = StartCoroutine(CloseMenuRoutine(OnComplected));
    }

    public virtual IEnumerator OpenMenuRoutine(Action OnComplected = null)
    {
        openCloseRoutine = null;
        yield return null;
    }

    public virtual IEnumerator CloseMenuRoutine(Action OnComplected = null)
    {
        if (OnComplected != null) OnComplected();
        OnMenuClosed();
        yield return null;
    }

    public virtual void OnMenuOpened()
    {
        
    }

    public virtual void OnMenuClosed()
    {
        if (OnAnyUIClosed != null) OnAnyUIClosed.Invoke(this, EventArgs.Empty);
        
        if (destroyOnClose)
        {
            if (OnAnyUIDestroyed != null) OnAnyUIDestroyed.Invoke(this, EventArgs.Empty);
            ActiveMenus.Remove(this);
            Destroy(gameObject);
        }  
        else gameObject.SetActive(false);
    }

    public void Focus()
    {
        firstSelectedButton?.Select();
    }

    protected virtual void ResetUI()
    {

    }
}
