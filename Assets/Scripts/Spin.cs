using System;
using System.Collections.Generic;
using UnityEngine;

public enum SpinType { Bronze, Silver, Gold }
public class Spin : MonoBehaviour
{
    [SerializeField] private SpinType spinType;
    [SerializeField] private float _spinCost;
    [SerializeField] private Transform _spinImageTransform;
    [SerializeField] private string _wheelName;
    [SerializeField] private string _bottomLabelText;
    
    [Space]
    [SerializeField] private WheelSlotSettings[] _wheelSlotsSettings;

    private void Start()
    {
        SetItems();
    }

    private void OnValidate()
    {
        SetItems();
    }

    private void SetItems()
    {
        foreach (var slotSetting in _wheelSlotsSettings)
        {
            slotSetting.WheelSlot?.SetItem(slotSetting.WheelItem);
            slotSetting.WheelSlot?.SetRewardMultiplication(slotSetting.GetTotalReward(), slotSetting.WheelItem);   
        }
    }
    public Transform GetSpinImageTransform() => _spinImageTransform;
    public WheelSlotSettings GetWheelSlotSettingsByIndex(int index) => _wheelSlotsSettings[index];

    #region Fields

    public SpinType SpinType { get => spinType; set => spinType = value; }
    public float SpinCost { get => _spinCost; set => _spinCost = value; }
    public string WheelName { get => _wheelName; set => _wheelName = value; }
    public string BottomLabelText { get => _bottomLabelText; set => _bottomLabelText = value; }

    #endregion
}

[System.Serializable]
public class WheelSlotSettings
{
    [SerializeField] private WheelSlot _wheelSlot;
    [SerializeField] private WheelItem _wheelItem;
    [SerializeField] private int _rewardAmount;
    [SerializeField] private int _rewardMultiplication;
    
    #region Fields

    public WheelSlot WheelSlot { get => _wheelSlot; set => _wheelSlot = value; }
    public WheelItem WheelItem { get => _wheelItem; set => _wheelItem = value; }
    public int RewardAmount { get => _rewardAmount; set => _rewardAmount = value; }
    public int RewardMultiplication { get => _rewardMultiplication; set => _rewardMultiplication = value; }
    public int GetTotalReward() => _rewardAmount * _rewardMultiplication;

    #endregion
}
