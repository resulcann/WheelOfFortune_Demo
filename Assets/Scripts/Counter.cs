using System;
using DG.Tweening;
using Resul.Helper;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Counter : MonoBehaviour
{
    [SerializeField] private Image _bgImage;
    [SerializeField] private TextMeshProUGUI _counterText;
    [Space]
    [SerializeField] private string _prefName = "";
    [SerializeField] private int _startValue;

    private int _value;
    private Tween _punchTween;

    private void Start()
    {
        LoadValue();
    }

    public void UpdateDisplay()
    {
        _counterText.text = _value >= 0 ? GameUtility.FormatFloatToReadableString(_value) : "";

        if (CounterManager.Instance.GetCounters().IndexOf(this) is 2) return;
        
        if (_value % 30 == 0 && _value != 0)
        {
            _bgImage.color = CounterManager.Instance.GetGoldColor();
        }
        else if (_value % 5 == 0 && _value != 0)
        {
            _bgImage.color = CounterManager.Instance.GetSilverColor();
        }
        else
        {
            _bgImage.color = CounterManager.Instance.GetBronzeColor();
        }
    }

    public void Punch(float punchValue = 0.25f, float dur = 0.25f)
    {
        _punchTween?.Kill(true);
        _punchTween = transform.DOPunchScale(transform.localScale * punchValue, dur);
    }
    
    private void LoadValue()
    {
        _value = PlayerPrefs.GetInt(_prefName, _startValue);
        UpdateDisplay();
    }
    
    private void SaveValue()
    {
        PlayerPrefs.SetInt(_prefName, _value);
        PlayerPrefs.Save();
    }
    
    private void ResetValue()
    {
        _value = 0;
        UpdateDisplay();
        SaveValue();
    }
    
    public void SetValue(int value) => _value = value;
    public int GetValue() => _value;
}