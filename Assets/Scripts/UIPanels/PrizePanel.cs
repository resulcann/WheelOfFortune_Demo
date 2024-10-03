using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrizePanel : MonoBehaviour, IPanel
{
    private bool _isOpen = false;

    private void Start()
    {
        CheckIsOpen();
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
