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

    [Space] 
    [SerializeField] private List<WheelItem> _earnedItems = new();
    
    private Spin _currentSpin;
    private int _spinCount = 0;
    private bool _isSpinning = false;

    public static event Action OnSpinStarted;
    public static event Action OnSpinCompleted;

    public void Init()
    {
        _spinBtn.onClick.AddListener(Spin);
        
        LoadSpinCount();
        CounterManager.Instance.Init();
        SetSpinType();
        _currentSpin.Init();
    }

    private void OnDestroy()
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
                SpinResults(CalculateSelectedSlot(totalAngle));
                SetSpinType();
                OnSpinCompleted?.Invoke();
                Debug.Log("OnSpinCompleted");
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

    public void SetSpinType()
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
        Debug.Log("Changing spin type to: " + newSpin.WheelName);

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
    
    public void ResetSpinCount()
    {
        _spinCount = 0;
        CounterManager.Instance.ResetAllCountersValues();
        SaveSpinCount();
    }

    private void SpinResults(int selectedSlotIndex)
    {
        var selectedSlotSettings = _currentSpin.GetWheelSlotSettingsByIndex(selectedSlotIndex);
        var selectedSlot = selectedSlotSettings.WheelSlot;
        var selectedItem = selectedSlot.GetItem();

        if (selectedItem.IsBomb)
        {
            CounterManager.Instance.UpdateCountersWithoutPunch();
            GameManager.Instance.Fail();
            Debug.Log("BOMB EXPLODED! Item Name: " + selectedItem.ItemName);
            return;
        }

        if (selectedItem.ItemName.Equals("Cash"))
        {
            var earnedMoney = selectedSlotSettings.GetTotalReward();
            GameManager.Instance.AddEarnedCash(earnedMoney);
            Debug.Log("YOU EARNED " + earnedMoney + " Cash");
        }
        else
        {
            GameManager.Instance.AddEarnedItem(selectedItem);
            Debug.Log("YOU EARNED AN ITEM! Item Name: " + selectedItem.ItemName);
        }
        IncreaseSpinCount();
        GameManager.Instance.GetPrize(selectedSlotSettings);
    }
    
    public int GetSpinCount() => _spinCount;
    public bool IsSpinning() => _isSpinning;
    public Spin GetCurrentSpin() => _currentSpin;
}