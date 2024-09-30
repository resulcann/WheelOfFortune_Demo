using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Wheel/Item", order = 1)]
public class WheelItem : ScriptableObject
{
    [SerializeField] private string _itemName;  // Eşya ismi
    [SerializeField] private Sprite _itemSprite;  // Eşya görseli
    [SerializeField] private WheelType _itemLevel;  // Eşya seviyesi (bronze, silver, gold)
    [SerializeField] private int _rewardMultiplier;  // Ödül çarpanı

    // Getter, Setters
    #region Fields 

    public string ItemName
    {
        get => _itemName;
        set => _itemName = value;
    }
    
    public Sprite ItemSprite
    {
        get => _itemSprite;
        set => _itemSprite = value;
    }
    
    public WheelType ItemLevel
    {
        get => _itemLevel;
        set => _itemLevel = value;
    }
    
    public int RewardMultiplier
    {
        get => _rewardMultiplier;
        set => _rewardMultiplier = value;
    }

    #endregion 
}