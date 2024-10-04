using System;
using System.Collections.Generic;
using System.Linq;
using Resul.Helper;
using UnityEngine;

public class GameManager : LocalSingleton<GameManager>
{
    [SerializeField] private int _reviveCost;
    [SerializeField] private float _reviveCostMultiplierPerSpin;
    
    // Kazanılan item'lar ve cash'ler için listeler
    [SerializeField] private List<WheelItem> _earnedItems = new List<WheelItem>();  
    [SerializeField] private int _earnedCash = 0;  // Kazanılan para miktarı
    private void Start()
    {
        Application.targetFrameRate = 60;
        
        UIManager.Instance.Init();
        WheelController.Instance.Init();
        CurrencyManager.Instance.Init();
    }

    private void OnApplicationQuit()
    {
        if(WheelController.Instance.GetCurrentSpin().SpinType is SpinType.Bronze)
            DiscardAllEarnings();
    }

    public void Fail()
    {
        UIManager.Instance.GetFailPanel().Toggle();
        UIManager.Instance.GetSpinPanel().Toggle(); 
    }

    public void Revive(bool withAd)
    {
        if (withAd)
        {
            UIManager.Instance.GetSpinPanel().Toggle();
            UIManager.Instance.GetFailPanel().Toggle();
            Debug.Log("Ads shown and revived With Ads");
        }
        else
        {
            if (CurrencyManager.Instance.GetCurrency() >= GetReviveCost())
            {
                CurrencyManager.Instance.DealCurrency(GetReviveCost());
                UIManager.Instance.GetSpinPanel().Toggle();
                UIManager.Instance.GetFailPanel().Toggle();
                Debug.Log("Revived with money.");
            }
            else
            {
                Debug.Log("Not enough currency to revive.");
            }
        }
    }

    // Spin sonucunda kazanılan item'ı listeye ekle.
    public void AddEarnedItem(WheelItem item)
    {
        _earnedItems.Add(item);
        Debug.Log("Added item: " + item.ItemName);
    }

    // Spin sonucunda kazanılan cash'i ekle.
    public void AddEarnedCash(int amount)
    {
        _earnedCash += amount;
        Debug.Log("Earned cash: " + amount);
    }

    // Kazanılan item'ları ve cash'leri sıfırlar.
    public void DiscardAllEarnings()
    {
        _earnedItems.Clear();
        _earnedCash = 0;
        WheelController.Instance.ResetSpinCount();
        Debug.Log("All earned items and cash have been discarded.");
    }

    // Kazanılan toplam cash'i al.
    public int GetTotalEarnedCash()
    {
        return _earnedCash;
    }

    public int GetReviveCost() => (int)(_reviveCost * _reviveCostMultiplierPerSpin);
    public List<WheelItem> GetEarnedItemList() => _earnedItems;
}
