using System;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : MonoBehaviour , IPanel
{
    [SerializeField] private Button _giveUpBtn;
    [SerializeField] private Button _reviveWithMoneyBtn;
    [SerializeField] private Button _reviveWithAdsBtn;
    [SerializeField] private GameObject _toppomBar;
    
    private bool _isOpen = false;
    public static event Action OnFailOpen;

    private void Start()
    {
        CheckIsOpen();
    }

    private void OnEnable()
    {
        _giveUpBtn.onClick.AddListener(GiveUpButton_OnClick);
        _reviveWithMoneyBtn.onClick.AddListener(ReviveWithMoneyButton_OnClick);
        _reviveWithAdsBtn.onClick.AddListener(ReviveWithAdsButton_OnClick);
    }

    private void OnDisable()
    {
        _giveUpBtn.onClick.RemoveListener(GiveUpButton_OnClick);
        _reviveWithMoneyBtn.onClick.RemoveListener(ReviveWithMoneyButton_OnClick);
        _reviveWithAdsBtn.onClick.RemoveListener(ReviveWithAdsButton_OnClick);
    }

    public void Toggle()
    {
        gameObject.SetActive(!_isOpen);
        _toppomBar.gameObject.SetActive(_isOpen);
        _isOpen = !_isOpen;
    }

    private void CheckIsOpen()
    {
        _isOpen = gameObject.activeInHierarchy;
    }

    private void GiveUpButton_OnClick()
    {
        GameManager.Instance.DiscardAllEarnings();
        Toggle();
        UIManager.Instance.GetMenuPanel().Toggle();
    }
    private void ReviveWithMoneyButton_OnClick()
    {
        GameManager.Instance.Revive(false);
    }
    private void ReviveWithAdsButton_OnClick()
    {
        GameManager.Instance.Revive(true);
    }
}
