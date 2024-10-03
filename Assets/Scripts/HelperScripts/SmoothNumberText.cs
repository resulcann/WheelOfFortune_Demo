﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Resul.Helper.UI
{
    public class SmoothNumberText : MonoBehaviour
    {
        [SerializeField] private bool _isSpeedBased = false;
        [SerializeField] private float _speed = 1f;

        private float _targetPoints = 0;
        private float _points = 0;
        private Tweener _smoothTween;
        private TMP_Text _textComponent;

        public void SetPoints(float points)
        {
            if (_textComponent == null)
            {
                _textComponent = GetComponent<TMP_Text>();
            }
            _targetPoints = points;
            _smoothTween?.Kill();
            _smoothTween = DOTween.To(() => _points, x => _points = x, _targetPoints, _speed).SetSpeedBased(_isSpeedBased).OnUpdate(UpdateText);
        }

        public void SetPointsInstantly(float points)
        {
            if(_textComponent == null)
            {
                _textComponent = GetComponent<TMP_Text>();
            }
            _smoothTween?.Kill();
            _targetPoints = points;
            _points = points;
            UpdateText();
        }

        void UpdateText()
        {
            if (_textComponent)
            {
                _textComponent.text = GameUtility.FormatFloatToReadableString(_points);
            }

        }
    }
}