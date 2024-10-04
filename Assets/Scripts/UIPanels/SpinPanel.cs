using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinPanel : MonoBehaviour, IPanel
{
    [SerializeField] private MenuPanel _menuPanel;
    [SerializeField] private Button _menuBtn;
    [SerializeField] private Button _clamAllBtn;
    
    private bool _isOpen = false;

    private void Start()
    {
        CheckIsOpen();
    }

    private void OnEnable()
    {
        _menuBtn.onClick.AddListener(Toggle);
        _menuBtn.onClick.AddListener(_menuPanel.Toggle);
        WheelController.OnSpinStarted += CheckMenuButtonVisibility;
        WheelController.OnSpinCompleted += CheckMenuButtonVisibility;
    }

    private void OnDisable()
    {
        _menuBtn.onClick.RemoveListener(Toggle);
        _menuBtn.onClick.RemoveListener(_menuPanel.Toggle);
        WheelController.OnSpinStarted -= CheckMenuButtonVisibility;
        WheelController.OnSpinCompleted -= CheckMenuButtonVisibility;
    }

    public void Toggle()
    {
        gameObject.SetActive(!_isOpen);
        _isOpen = !_isOpen;
    }

    private void CheckIsOpen()
    {
        _isOpen = gameObject.activeInHierarchy;
    }

    private void CheckMenuButtonVisibility()
    {
        _menuBtn.gameObject.SetActive(WheelController.Instance.GetSpinCount() == 0 && !WheelController.Instance.IsSpinning());
        _clamAllBtn.gameObject.SetActive((WheelController.Instance.GetCurrentSpin().SpinType != SpinType.Bronze 
                                         && GameManager.Instance.GetEarnedItemList().Count > 0)
                                         && !WheelController.Instance.IsSpinning());
    }
}
