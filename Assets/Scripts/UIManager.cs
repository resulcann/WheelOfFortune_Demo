using System;
using System.Collections.Generic;
using Resul.Helper;
using UnityEngine;

public class UIManager : LocalSingleton<UIManager>
{
    [Header("PANELS")] 
    [SerializeField] private MenuPanel _menuPanel;
    [SerializeField] private SpinPanel _spinPanel;
    [SerializeField] private FailPanel _failPanel;
    [SerializeField] private PrizePanel _prizePanel;
    
    public void Init()
    {
        _failPanel.Init();
        _prizePanel.Init();
        _menuPanel.Init();
        _spinPanel.Init();
        
        //Open_MenuPanel();
    }
    
    private void OnDestroy()
    {
        _menuPanel.OnDestroyProcess();
        _spinPanel.OnDestroyProcess();
        _prizePanel.OnDestroyProcess();
        _failPanel.OnDestroyProcess();
    }

    public void Open_MenuPanel()
    {
        CloseAllPanels();
        _menuPanel.OpenPanel();
    }
    public void Open_SpinPanel()
    {
        CloseAllPanels();
        _spinPanel.OpenPanel();
        
    }
    public void Open_FailPanel()
    {
        CloseAllPanels();
        _failPanel.OpenPanel();
    }
    public void Open_PrizePanel()
    {
        CloseAllPanels();
        _prizePanel.OpenPanel();
    }

    private void CloseAllPanels()
    {
        _menuPanel.ClosePanel();
        _spinPanel.ClosePanel();
        _failPanel.ClosePanel();
        _prizePanel.ClosePanel();
    }

    
    public MenuPanel GetMenuPanel() => _menuPanel;
    public SpinPanel GetSpinPanel() => _spinPanel;
    public FailPanel GetFailPanel() => _failPanel;
    public PrizePanel GetPrizePanel() => _prizePanel;
}
