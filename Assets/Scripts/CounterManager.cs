using System;
using System.Collections;
using System.Collections.Generic;
using Resul.Helper;
using UnityEngine;

public class CounterManager : LocalSingleton<CounterManager>
{
    [SerializeField] private List<Counter> _counters;
    [Space] 
    [SerializeField] private Color _bronzeColor;
    [SerializeField] private Color _silverColor;
    [SerializeField] private Color _goldColor;
    
    private Coroutine _punchAnimationCor;
    public void Init()
    {
        _counters.ForEach(c => c.Init());
        UpdateCountersWithoutPunch();
    }

    private void Start()
    {
        WheelController.OnSpinCompleted += UpdateCounters;
    }

    private void OnDestroy()
    {
        WheelController.OnSpinCompleted -= UpdateCounters;
    }

    public void UpdateCounters()
    {
        if (_punchAnimationCor != null) StopCoroutine(_punchAnimationCor);
        _punchAnimationCor = StartCoroutine(UpdateCountersWithPunch(0.35f, 0.1f));
    }

    public void UpdateCountersWithoutPunch()
    {
        var spinCount = WheelController.Instance.GetSpinCount();
        for (var i = 0; i < _counters.Count; i++)
        {
            var value = spinCount - 2 + i;
            if (value >= 0)
            {
                _counters[i].SetValue(value);
            }
            else
            {
                _counters[i].SetValue(-1);
            }
            _counters[i].UpdateDisplay(); 
        }
    }

    private IEnumerator UpdateCountersWithPunch(float punchValue = 0.25f, float dur = 0.25f)
    {
        var spinCount = WheelController.Instance.GetSpinCount(); 
        for (var i = 0; i < _counters.Count; i++)
        {
            var value = spinCount - 2 + i;
            if (value >= 0)
            {
                _counters[i].SetValue(value);
            }
            else
            {
                _counters[i].SetValue(-1); 
            }

            _counters[i].UpdateDisplay();
            _counters[i].Punch(punchValue, dur);

            yield return new WaitForSeconds(dur / 2);
        }
    }

    public void ResetAllCountersValues() => _counters.ForEach(c => c.ResetValue());
    public List<Counter> GetCounters() => _counters;
    public Color GetBronzeColor() => _bronzeColor;
    public Color GetSilverColor() => _silverColor;
    public Color GetGoldColor() => _goldColor;
}