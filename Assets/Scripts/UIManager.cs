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
        _menuPanel.gameObject.SetActive(true);
        _spinPanel.gameObject.SetActive(false);
        _failPanel.gameObject.SetActive(false);
        _prizePanel.gameObject.SetActive(false);
    }
    
    public MenuPanel GetMenuPanel() => _menuPanel;
    public SpinPanel GetSpinPanel() => _spinPanel;
    public FailPanel GetFailPanel() => _failPanel;
    public PrizePanel GetPrizePanel() => _prizePanel;
}
