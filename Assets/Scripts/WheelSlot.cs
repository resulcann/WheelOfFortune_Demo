using Resul.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSlot : MonoBehaviour
{
    [Space] [Header("REFERENCES")]
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _rewardMultiplierText; 

    private WheelItem _currentItem;
    private int _rewardMultiplier = 0;

    public void SetItem(WheelItem item)
    {
        _currentItem = item;
        _itemImage.sprite = item.ItemSprite;
    }

    public void SetRewardMultiplication(int amount, WheelItem wheelItem)
    {
        _rewardMultiplier = amount;
        _rewardMultiplierText.text = wheelItem.IsBomb ? $"FAIL!" : $"x{GameUtility.FormatFloatToReadableString(amount, true , false)}";
    }
    
    public WheelItem GetItem() => _currentItem;
    public int GetRewardMultiplier() => _rewardMultiplier;
}