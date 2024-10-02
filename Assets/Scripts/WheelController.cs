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
    [SerializeField] private TextMeshProUGUI _spinCountText;
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

    public static event Action<int> OnSpinComplete;

    private void Start()
    {
        LoadSpinCount();
        SetWheel();
    }

    private void OnEnable()
    {
        _spinBtn.onClick.AddListener(Spin);
        OnSpinComplete += SpinResults;
    }

    private void OnDisable()
    {
        _spinBtn.onClick.RemoveListener(Spin);
        OnSpinComplete -= SpinResults;
    }

    private void Spin()
    {
        if (_isSpinning) return;
        _isSpinning = true;
        _spinBtn.interactable = false;

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
                OnSpinComplete?.Invoke(CalculateSelectedSlot(totalAngle));
                IncreaseSpinCount(); // önce arttır sonra spin türünü ayarla
                SetWheel();
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
        // // Eğer seçilen slot bir bombaysa
        // if (_wheelSlots[selectedSlotIndex].IsBomb())
        // {
        //     //ShowFailPanel();  // Fail panelini göster
        //     ResetSpinCount();  // Spin sayısını sıfırla
        //     Debug.Log("BOMB EXPLODED!");
        // }
        // else
        // {
        //     IncreaseSpinCount();
        //     Debug.Log("YOU ARE SAFE!");
        // }
    }
}