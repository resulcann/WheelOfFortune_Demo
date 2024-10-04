using System.Collections.Generic;
using DG.Tweening;
using Resul.Helper;
using UnityEngine;

public class GameManager : LocalSingleton<GameManager>
{
    [SerializeField] private int _reviveCost;
    [SerializeField] private float _reviveCostMultiplierPerSpin;
    
    [SerializeField] private List<WheelItem> _earnedItems = new List<WheelItem>();  
    [SerializeField] private int _earnedCash = 0;
    private void Start()
    {
        Application.targetFrameRate = 60;
        
        UIManager.Instance.Init();
        WheelController.Instance.Init();
        CurrencyManager.Instance.Init();
    }
    private void OnApplicationQuit()
    {
        if (WheelController.Instance.GetCurrentSpin().SpinType == SpinType.Bronze)
        {
            DiscardAllEarnings();
            DOTween.KillAll();
        }
    }
    public void Fail()
    {
        UIManager.Instance.Open_FailPanel();
    }

    public void GetPrize(WheelSlotSettings wheelSlotSettings)
    {
        var slot = wheelSlotSettings.WheelSlot;
        var item = slot.GetItem();
        var rewardAmount = wheelSlotSettings.GetTotalReward();
        var prizePanel = UIManager.Instance.GetPrizePanel();
        
        prizePanel.SetPrizeCard(item, rewardAmount, item.ItemName.Equals("Cash"));
        UIManager.Instance.Open_PrizePanel();
    }

    public void Revive(bool withAd)
    {
        if (withAd)
        {
            UIManager.Instance.Open_SpinPanel();
            Debug.Log("Ads shown and revived With Ads");
        }
        else
        {
            if (CurrencyManager.Instance.GetCurrency() >= GetReviveCost())
            {
                CurrencyManager.Instance.DealCurrency(-GetReviveCost());
                UIManager.Instance.Open_SpinPanel();
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
        if (_earnedItems.Count > 0)
        {
            _earnedItems.Clear();
            Debug.Log("All earned items and cash have been discarded.");
        }
        _earnedCash = 0;
        WheelController.Instance.ResetSpinCount();
    }

    public void ClaimAllEarnings()
    {
        CurrencyManager.Instance.DealCurrency(_earnedCash);
        _earnedCash = 0;
        WheelController.Instance.ResetSpinCount();
        WheelController.Instance.SetSpinType();
        UIManager.Instance.Open_MenuPanel();
    }
    
    public int GetTotalEarnedCash() => _earnedCash;
    public int GetReviveCost() => (int)(_reviveCost * _reviveCostMultiplierPerSpin * (WheelController.Instance.GetSpinCount()+1));
    public List<WheelItem> GetEarnedItemList() => _earnedItems;
}
