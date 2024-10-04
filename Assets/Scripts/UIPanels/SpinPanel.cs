using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpinPanel : MonoBehaviour, IPanel
{
    [SerializeField] private GameObject _innerPanel;
    [SerializeField] private MenuPanel _menuPanel;
    [SerializeField] private Button _menuBtn;
    [SerializeField] private Button _claimAllBtn;

    public void Init()
    {
        _menuBtn.onClick.AddListener(MenuButton_OnClick);
        _claimAllBtn.onClick.AddListener(ClaimButton_OnClick);
        
        WheelController.OnSpinStarted += CheckMenuButtonVisibility;
        WheelController.OnSpinCompleted += CheckMenuButtonVisibility;
    }
    
    public void OnDestroyProcess()
    {
        _menuBtn.onClick.RemoveListener(MenuButton_OnClick);
        _claimAllBtn?.onClick.RemoveListener(ClaimButton_OnClick);
        
        WheelController.OnSpinStarted -= CheckMenuButtonVisibility;
        WheelController.OnSpinCompleted -= CheckMenuButtonVisibility;
    }

    private void CheckMenuButtonVisibility()
    {
        _menuBtn.gameObject.SetActive(WheelController.Instance.GetSpinCount() == 0 && !WheelController.Instance.IsSpinning());
        _claimAllBtn.gameObject.SetActive((WheelController.Instance.GetCurrentSpin().SpinType != SpinType.Bronze 
                                         && GameManager.Instance.GetEarnedItemList().Count > 0)
                                         && !WheelController.Instance.IsSpinning());
    }

    private void MenuButton_OnClick()
    {
        UIManager.Instance.Open_MenuPanel();
    }

    private void ClaimButton_OnClick()
    {
        GameManager.Instance.ClaimAllEarnings();
    }

    public void OpenPanel()
    {
        _innerPanel.SetActive(true);
        CheckMenuButtonVisibility();
    }

    public void ClosePanel() => _innerPanel.SetActive(false);
}
