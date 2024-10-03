using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FailPanel : MonoBehaviour , IPanel
{
    [SerializeField] private Button _giveUpBtn;
    [SerializeField] private Button _reviveWithMoneyBtn;
    [SerializeField] private Button _reviveWithAdsBtn;
    
    private bool _isOpen = false;

    private void Start()
    {
        CheckIsOpen();
    }

    private void OnEnable()
    {
        throw new NotImplementedException();
    }

    private void OnDisable()
    {
        throw new NotImplementedException();
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
}
