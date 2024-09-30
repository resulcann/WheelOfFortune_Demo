using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSlot : MonoBehaviour
{
    [Header("REFERENCES")]
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _rewardMultiplierText; 

    private WheelItem _currentItem;
    private bool _isBomb = false;
    
    public void SetBomb()
    {
        _isBomb = true;
        _itemImage.sprite = WheelController.Instance.GetBombItem().ItemSprite;  // Bomba için özel bir görsel kullan
        _rewardMultiplierText.text = "FAIL!";
    }

    public void SetNotBomb() => _isBomb = false;
    public bool IsBomb() => _isBomb;
    
    public void SetRandomItem(List<WheelItem> items, WheelType type)
    {
        var filteredItems = items.FindAll(item => item.ItemLevel == type);
        
        if (filteredItems.Count > 0)
        {
            _currentItem = filteredItems[Random.Range(0, filteredItems.Count)];
            
            _itemImage.sprite = _currentItem.ItemSprite;
            _rewardMultiplierText.text = "x" + _currentItem.RewardMultiplier.ToString();
        }
        else
        {
            Debug.LogWarning("Can't found any item on this level."); // Bu levelda bir eşya yoksa mesaj döndür.
        }
    }
}