using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Resul.Helper.World
{
    public class SimpleMoveLoop1D : MonoBehaviour
    {
        [SerializeField] private Vector3 _targetPosition;
        [SerializeField] private float _duration = 0.7f;
        [SerializeField] private float _delayDuration = 0f;
        [SerializeField] private bool _isLocalPos;

        public UnityEvent OnStepCompleted;

        private Tween _loopTween;
        private Vector3 _firstPosition;
        private void Start()
        {
            if(_isLocalPos)
            {
                _firstPosition = transform.localPosition;
            }
            else
            {
                _firstPosition = transform.position;
            }
            
            StartDynamicLoop();
        }
        

        private IEnumerator SimpleLoopProcess()
        {
            yield return new WaitForSeconds(_delayDuration);
            while(true)
            {
                if(_isLocalPos)
                {
                    yield return transform.DOLocalMove(_targetPosition, _duration).WaitForCompletion();
                    transform.localPosition = _firstPosition;
                }
                else
                {
                    yield return transform.DOMove(_targetPosition, _duration).WaitForCompletion();
                    transform.position = _firstPosition;
                }
                OnStepCompleted?.Invoke();
            }
        }
        
        private void StartDynamicLoop()
        {
            if(_isLocalPos)
            {
                _loopTween = transform.DOLocalMove(_targetPosition, _duration).SetLoops(-1, LoopType.Yoyo);
            }
            else
            {
                _loopTween = transform.DOMove(_targetPosition, _duration).SetLoops(-1, LoopType.Yoyo);
            }
        }
    }
}