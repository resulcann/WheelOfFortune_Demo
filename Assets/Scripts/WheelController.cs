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
    [SerializeField] private Spin[] _spins;
    [SerializeField] private Button _spinBtn;
    [SerializeField] private TextMeshProUGUI _spinCostText;
    [SerializeField] private TextMeshProUGUI _wheelNameText;
    [SerializeField] private TextMeshProUGUI _bottomLabelText;

    [Space] [Header("SETTINGS")] 
    [SerializeField] private int _fullTurnCount = 5;
    [SerializeField] private float _spinDuration = 5f;
    
    private Spin _currentSpin;
    private int _spinCount = 0;
    private bool _isSpinning = false;
    private int _lastSelectedSlotIndex = 0;

    public static event Action OnSpinStarted;
    public static event Action OnSpinCompleted;

    public void Init()
    {
        LoadSpinCount();
        CounterManager.Instance.Init();
        SetWheel();
    }

    private void OnEnable()
    {
        _spinBtn.onClick.AddListener(Spin);
    }

    private void OnDisable()
    {
        _spinBtn.onClick.RemoveListener(Spin);
    }

    private void Spin()
    {
        if (_isSpinning) return;
        _isSpinning = true;
        _spinBtn.interactable = false;
        
        OnSpinStarted?.Invoke();

        // 45 in katı olan rastgele bir açı belirleniyor ( 45in katı olma sebebi slotların üzerine tam oturması için )
        var randomAngle = Random.Range(0, 8) * 45;

        // Çarkın direk hedefe gitmesi yerine görsellik için önce bir kaç tam tur attırıp daha sonra rastgele açıya döndürüyorum.
        var totalAngle = (360 * _fullTurnCount) + randomAngle;

        // DoTween kullanarak gerçekçi bir çark dönüş görüntüsü sağlamak için Ease ayarını kullandım ve yavaştan hızlıya daha sonra tekrar yavaşlayacak şekilde çark dönüşü sağlanıyor.
        _currentSpin.GetSpinImageTransform().DORotate(new Vector3(0, 0, -totalAngle), _spinDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.InOutCubic)
            .OnComplete(() =>
            {
                _isSpinning = false;
                _spinBtn.interactable = true;
                IncreaseSpinCount(); // önce arttır sonra spin türünü ayarla
                SetWheel();
                SpinResults(CalculateSelectedSlot(totalAngle));
                OnSpinCompleted?.Invoke();
            });
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

    private void SetWheel()
    {
        if (_spinCount % 30 == 0 && _spinCount != 0)
        {
            ChangeSpinType(_spins[2]); // Golden Spin
        }
        else if (_spinCount % 5 == 0 && _spinCount != 0)
        {
            ChangeSpinType(_spins[1]); // Silver Spin
        }
        else
        {
            ChangeSpinType(_spins[0]); // Bronze Spin
        }
    }


    private void ChangeSpinType(Spin newSpin)
    {
        foreach (var spin in _spins)
        {
            spin.gameObject.SetActive(false);
        }
        
        _currentSpin = newSpin;
        newSpin.gameObject.SetActive(true);

        _spinCostText.text = $"Spin Cost: ${newSpin.SpinCost}";
        _wheelNameText.text = newSpin.WheelName;
        _bottomLabelText.text = newSpin.BottomLabelText;
    }
    
    private void IncreaseSpinCount()
    {
        _spinCount++;
        SaveSpinCount();
    }
    
    private void LoadSpinCount()
    {
        _spinCount = PlayerPrefs.GetInt("SpinCount", 0);
    }
    
    private void SaveSpinCount()
    {
        PlayerPrefs.SetInt("SpinCount", _spinCount);
        PlayerPrefs.Save();
    }
    
    private void ResetSpinCount()
    {
        _spinCount = 0;
        SaveSpinCount();
    }

    private void SpinResults(int selectedSlotIndex)
    {
        var selectedSlotSettings = _currentSpin.GetWheelSlotSettingsByIndex(selectedSlotIndex); // seçilen slot settings
        var selectedSlot = selectedSlotSettings.WheelSlot; // seçilen slot
        var selectedItem = selectedSlot.GetItem(); // seçilen item
        
        // Eğer seçilen slot bir bombaysa
        if (selectedItem.IsBomb)
        {
            //ShowFailPanel();  // Fail panelini göster
            ResetSpinCount();  // Spin sayısını sıfırla
            Debug.Log("BOMB EXPLODED! " + "Item Name: " + selectedItem.ItemName);
        }
        else if (selectedItem.ItemName.Equals("Cash"))
        {
            CurrencyManager.Instance.DealCurrency(selectedSlotSettings.GetTotalReward());
            Debug.Log("YOU EARNED " + selectedItem.ItemName + "Amount: " + selectedSlotSettings.GetTotalReward());
        }
        else
        {
            IncreaseSpinCount();
            Debug.Log("YOU ARE SAFE! " + "Item Name: " + selectedItem.ItemName);
        }
    }

    public int GetSpinCount() => _spinCount;
    public bool IsSpinning() => _isSpinning;
    public Spin GetCurrentSpin() => _currentSpin;
}