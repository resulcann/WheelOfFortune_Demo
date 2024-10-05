using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Wheel/Item", order = 1)]
public class WheelItem : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private Sprite _itemSprite; 
    [SerializeField] private bool _isBomb = false;

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
    
    public bool IsBomb
    {
        get => _isBomb;
        set => _isBomb = value;
    }
    #endregion 
}