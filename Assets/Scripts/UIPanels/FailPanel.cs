using System;
using Resul.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : MonoBehaviour , IPanel
{
    [SerializeField] private GameObject _innerPanel;
    [SerializeField] private Button _giveUpBtn;
    [SerializeField] private Button _reviveWithMoneyBtn;
    [SerializeField] private Button _reviveWithAdsBtn;
    [SerializeField] private TextMeshProUGUI _reviveWithMoneyText;
    [SerializeField] private GameObject _toppomBar;

    public void Init()
    {
        UpdateReviveCostText();
        WheelController.OnSpinCompleted += UpdateReviveCostText;
        
        _giveUpBtn?.onClick.AddListener(GiveUpButton_OnClick);
        _reviveWithMoneyBtn?.onClick.AddListener(ReviveWithMoneyButton_OnClick);
        _reviveWithAdsBtn?.onClick.AddListener(ReviveWithAdsButton_OnClick);
    }
    
    public void OnDestroyProcess()
    {
        WheelController.OnSpinCompleted -= UpdateReviveCostText;
        
        _giveUpBtn?.onClick.RemoveListener(GiveUpButton_OnClick);
        _reviveWithMoneyBtn?.onClick.RemoveListener(ReviveWithMoneyButton_OnClick);
        _reviveWithAdsBtn?.onClick.RemoveListener(ReviveWithAdsButton_OnClick);
    }
    
    private void GiveUpButton_OnClick()
    {
        GameManager.Instance.DiscardAllEarnings();
        UIManager.Instance.Open_MenuPanel();
    }
    private void ReviveWithMoneyButton_OnClick()
    {
        GameManager.Instance.Revive(false);
    }
    private void ReviveWithAdsButton_OnClick()
    {
        GameManager.Instance.Revive(true);
    }
    public void OpenPanel()
    {
        _innerPanel.SetActive(true);
        _toppomBar.SetActive(false);
    }

    public void ClosePanel()
    {
        _innerPanel.SetActive(false);
        _toppomBar.SetActive(true);   
    }

    public void UpdateReviveCostText()
    {
        _reviveWithMoneyText.text = GameUtility.FormatFloatToReadableString(GameManager.Instance.GetReviveCost());
    }
}
