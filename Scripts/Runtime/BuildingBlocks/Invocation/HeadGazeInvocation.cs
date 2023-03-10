/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Oculus.Voice.Toolkit
{
    public class HeadGazeInvocation : InvocationBase
    {
        [SerializeField] private Camera _camera;
        [Tooltip("The ideal target object for head gaze is on the center of an object you want to look at")]
        [SerializeField] private GameObject _targetObject;
        [SerializeField] private int _activationTimingBuffer = 30;
        [SerializeField] private int _cooldownTimingBuffer = 30;
        [SerializeField] private bool _limitActivationDistance = false;
        [Min(0f)]
        [SerializeField] private float _maxActivationDistance = 0;
        public UnityEvent onGaze = new UnityEvent();
        public UnityEvent onStoppedGazing = new UnityEvent();
        private int _counter = 0;
        private IEnumerator _coroutine;
        private bool _cooldownDuration = false;
        private bool _isGazed;

        protected void OnValidate()
        {
            _syncActivation = true;
        }
        
        protected void OnEnable()
        {
            if (!_camera) _camera = Camera.main;
            _coroutine = CheckFocus(0.03f);
            StartCoroutine(_coroutine);
        }

        protected void OnDisable()
        {
            StopCoroutine(_coroutine);
        }

        public override void InvocationAggregationUpdate(bool isOn)
        {
            if (!isOn)
            {
                _counter = _cooldownTimingBuffer;
                _cooldownDuration = true;
            }
            else
            {
                _cooldownDuration = false;
            }
            base.InvocationAggregationUpdate(isOn);
        }

        IEnumerator CheckFocus(float time)
        {
            while (true)
            {
                bool hasFocus = HasFocus();
                if (_isGazed != hasFocus)
                {
                    if(hasFocus) onGaze.Invoke();
                    else onStoppedGazing.Invoke();
                    _isGazed = hasFocus;
                }
                
                if (hasFocus && !IsActivated && !_cooldownDuration)
                {
                    var cameraDistance = Vector3.Distance(_camera.transform.position, transform.position);
                    if (_maxActivationDistance <= 0 || !_limitActivationDistance || cameraDistance < _maxActivationDistance)
                    {
                        if (_counter >= _activationTimingBuffer)
                        {
                            base.Activate();
                        }
                        else
                        {
                            _counter++;
                        }
                    }
                }
                else if(!hasFocus || _cooldownDuration)
                {
                    if (_counter <= 0)
                    {
                        _cooldownDuration = false;
                        base.Deactivate();
                    }
                    else
                    {
                        _counter--;
                    }
                }
                yield return new WaitForSeconds(time);
            }
        }

        bool HasFocus()
        {
            return VoiceUXUtility.InCameraFOV(_camera,_targetObject);
        }
    }
}
