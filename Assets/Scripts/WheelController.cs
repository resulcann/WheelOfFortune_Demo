using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Resul.Helper;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class WheelController : LocalSingleton<WheelController>
{
    [Header("REFERENCES")]
    [SerializeField] private Button _spinBtn;
    [SerializeField] private Transform _wheelTransform;
    [SerializeField] private TextMeshProUGUI _spinCountText;
    [SerializeField] private Image _wheelImage;
    [SerializeField] private Image _wheelIndicatorImage;
    [SerializeField] private TextMeshProUGUI _wheelNameText;
    [SerializeField] private TextMeshProUGUI _bottomLabelText;
    [SerializeField] private WheelTypeSettings[] _wheelTypeSettings;
    [SerializeField] private WheelItem _bombItem;

    [Space] [Header("SETTINGS")] 
    [SerializeField] private int _fullTurnCount = 5;
    [SerializeField] private float _spinDuration = 5f;
    
    [Header("Slots and Items")]
    [SerializeField] private List<WheelSlot> _wheelSlots;
    [SerializeField] private List<WheelItem> _allWheelItems;

    private WheelTypeSettings _currentWheelType;
    private int _spinCount = 0;
    private bool _isSpinning = false;
    private int _lastSelectedSlotIndex = 0;

    public static event Action<int> OnSpinComplete;

    private void Start()
    {
        LoadSpinCount();
        CalculateWheelLevel();
    }

    private void OnEnable()
    {
        _spinBtn.onClick.AddListener(SpinWheel);
        OnSpinComplete += SpinResults;
    }

    private void OnDisable()
    {
        _spinBtn.onClick.RemoveListener(SpinWheel);
        OnSpinComplete -= SpinResults;
    }

    private void SpinWheel()
    {
        if (_isSpinning) return;
        _isSpinning = true;
        _spinBtn.interactable = false;

        // 45 in katı olan rastgele bir açı belirleniyor ( 45in katı olma sebebi slotların üzerine tam oturması için )
        var randomAngle = Random.Range(0, 8) * 45;

        // Çarkın direk hedefe gitmesi yerine görsellik için önce bir kaç tam tur attırıp daha sonra rastgele açıya döndürüyorum.
        var totalAngle = (360 * _fullTurnCount) + randomAngle;

        // DoTween kullanarak gerçekçi bir çark dönüş görüntüsü sağlamak için Ease ayarını kullandım ve yavaştan hızlıya daha sonra tekrar yavaşlayacak şekilde çark dönüşü sağlanıyor.
        _wheelTransform.DORotate(new Vector3(0, 0, -totalAngle), _spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                _isSpinning = false;
                _spinBtn.interactable = true;
                IncreaseSpinCount(); // önce arttır sonra calculate yap
                CalculateWheelLevel();
                OnSpinComplete?.Invoke(CalculateSelectedSlot(totalAngle));
            });
    }
    
    private void SetItemsToSlots()
    {
        _wheelSlots.ForEach(slot =>  slot.SetNotBomb());
        
        if (_currentWheelType.WheelType is WheelType.Bronze)
        {
            var bombIndex = Random.Range(0, _wheelSlots.Count);  // Rastgele bir slot bomba yapılıyor.
            for (var i = 0; i < _wheelSlots.Count; i++)
            {
                if (i == bombIndex)
                {
                    _wheelSlots[i].SetBomb();  // Slot'a bomba yerleştiriliyor.
                }
                else
                {
                    _wheelSlots[i].SetRandomItem(_allWheelItems, _currentWheelType.WheelType);
                }
            }
        }
        else
        {
            // Silver ve Gold seviyelerinde Safe Zone (bomba yok)
            foreach (var slot in _wheelSlots)
            {
                slot.SetRandomItem(_allWheelItems, _currentWheelType.WheelType);
            }
        }
    }

    private static int CalculateSelectedSlot(int angle)
    {
        var normalizedAngle = angle % 360;
        
        if (normalizedAngle < 0)
        {
            normalizedAngle += 360;
        }
        
        var selectedSlotIndex = normalizedAngle / 45;
        return selectedSlotIndex;
    }

    private void CalculateWheelLevel()
    {
        if (_spinCount % 30 == 0 && _spinCount != 0)
        {
            ChangeWheelType(_wheelTypeSettings[2]); // Gold Wheel
        }
        else if (_spinCount % 5 == 0 && _spinCount != 0)
        {
            ChangeWheelType(_wheelTypeSettings[1]); // Silver Wheel
        }
        else
        {
            ChangeWheelType(_wheelTypeSettings[0]); // Bronze Wheel
        }

        SetItemsToSlots();
    }


    private void ChangeWheelType(WheelTypeSettings newType)
    {
        _currentWheelType = newType;
        
        _wheelNameText.text = _currentWheelType.WheelName;
        _bottomLabelText.text = _currentWheelType.BottomLabelText;
        _wheelImage.sprite = _currentWheelType.WheelSprite;
        _wheelIndicatorImage.sprite = _currentWheelType.IndicatorSprite;
    }
    
    private void IncreaseSpinCount()
    {
        _spinCount++;
        _spinCountText.text = $"SPIN COUNT: {_spinCount}";
        SaveSpinCount();
    }
    
    private void LoadSpinCount()
    {
        _spinCount = PlayerPrefs.GetInt("SpinCount", 0);
        _spinCountText.text = $"SPIN COUNT: {_spinCount}";
    }
    
    private void SaveSpinCount()
    {
        PlayerPrefs.SetInt("SpinCount", _spinCount);
        PlayerPrefs.Save();
    }
    
    private void ResetSpinCount()
    {
        _spinCount = 0;
        _spinCountText.text = $"SPIN COUNT: {_spinCount}";
        SaveSpinCount();
    }

    private void SpinResults(int selectedSlotIndex)
    {
        // Eğer seçilen slot bir bombaysa
        if (_wheelSlots[selectedSlotIndex].IsBomb())
        {
            //ShowFailPanel();  // Fail panelini göster
            ResetSpinCount();  // Spin sayısını sıfırla
            Debug.Log("BOMB EXPLODED!");
        }
        else
        {
            _spinCount++;  // Spin sayısını artır
            SaveSpinCount();  // Yeni spin sayısını kaydet
            Debug.Log("YOU ARE SAFE!");
        }
    }

    public WheelItem GetBombItem() => _bombItem;
}
public enum WheelType { Bronze, Silver, Gold }

[System.Serializable]
public class WheelTypeSettings
{
    [SerializeField] private WheelType _wheelType;
    [SerializeField] private Sprite _wheelSprite;
    [SerializeField] private Sprite _indicatorSprite;
    [SerializeField] private string _wheelName;
    [SerializeField] private string _bottomLabelText;

    #region Fields

    public WheelType WheelType { get => _wheelType; set => _wheelType = value; }
    public Sprite WheelSprite { get => _wheelSprite; set => _wheelSprite = value;}
    public Sprite IndicatorSprite { get => _indicatorSprite; set => _indicatorSprite = value; }
    public string WheelName { get => _wheelName; set => _wheelName = value; }
    public string BottomLabelText { get => _bottomLabelText; set => _bottomLabelText = value; }

    #endregion
}