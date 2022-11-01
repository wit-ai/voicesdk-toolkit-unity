/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Oculus.Voice.Toolkit
{
    public class HeadGazeInvocation : InvocationBase
    {
        [SerializeField] private Camera _camera;
        [Tooltip("The ideal target object for head gaze is on the center of an object you want to look at")]
        [SerializeField] private GameObject _targetObject;
        [SerializeField] private int _activationTimingBuffer = 30;
        [SerializeField] private int _cooldownTimingBuffer = 30;
        private int _counter = 0;
        private IEnumerator _coroutine;
        private bool _cooldownDuration = false;
        protected void OnValidate()
        {
            _syncActivation = true;
        }
        protected void Awake()
        {
            if (!_camera) _camera = Camera.main;
        }

        protected void OnEnable()
        {
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
                bool _isFoucs = HasFocus();
                if (_isFoucs && !IsActivated && !_cooldownDuration)
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
                else if(!_isFoucs || _cooldownDuration)
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
