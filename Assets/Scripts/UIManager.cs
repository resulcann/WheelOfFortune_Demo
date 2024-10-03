using Resul.Helper;
using UnityEngine;

public class UIManager : LocalSingleton<UIManager>
{
    [Header("PANELS")] 
    [SerializeField] private MenuPanel _menuPanel;
    [SerializeField] private SpinPanel _spinPanel;
    [SerializeField] private FailPanel _failPanel;
    [SerializeField] private PrizePanel _prizePanel;
    
    
    
}
