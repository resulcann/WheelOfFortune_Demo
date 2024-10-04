using Resul.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PrizePanel : MonoBehaviour, IPanel
{
    [Header("REFERENCES")] 
    [SerializeField] private GameObject _innerPanel;
    [SerializeField] private Image _rewardItemImage;
    [SerializeField] private TextMeshProUGUI _itemNameText;
    [SerializeField] private TextMeshProUGUI _rewardCountText;
    [SerializeField] private Button _nextBtn;

    public void Init()
    {
        _nextBtn.onClick.AddListener(UIManager.Instance.Open_SpinPanel);
    }
    
    public void OnDestroyProcess()
    {
        _nextBtn.onClick.RemoveListener(UIManager.Instance.Open_SpinPanel);
    }
    
    public void SetPrizeCard(WheelItem item, int rewardAmount, bool isCash)
    {
        _itemNameText.text = item.ItemName;
        _rewardItemImage.sprite = item.ItemSprite;
        _rewardCountText.text = isCash ? GameUtility.FormatFloatToReadableString(rewardAmount)
                                        : $"x{GameUtility.FormatFloatToReadableString(rewardAmount)}";
    }
    public void OpenPanel() => _innerPanel.SetActive(true);
    public void ClosePanel() => _innerPanel.SetActive(false);
}
