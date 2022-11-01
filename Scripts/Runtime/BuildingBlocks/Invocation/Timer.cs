/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Oculus.Voice.Toolkit
{
    [Serializable]
    public class Timer
    {
        [SerializeField] private string _name;
        [SerializeField] private float _duration = 5;
        [SerializeField] private bool _enableMicrophone = false;

        private IEnumerator _enumerator;
        private bool _isTurn = false;
        public float Duration => _duration;
        public bool EnableMicrophone=>_enableMicrophone;
        public IEnumerator Enumerator=>_enumerator;
        public bool IsTurn => _isTurn;
        public UnityEvent<bool> statusUpdate;
        public UnityEvent timeout;

        public void SetupTimer()
        {
            _enumerator =  TimerHandler();
        }

        public void Stop()
        {
            _isTurn = false;
            statusUpdate?.Invoke(false);
        }

        IEnumerator TimerHandler()
        {
            statusUpdate?.Invoke(true);
            _isTurn = true;
            yield return new WaitForSeconds(_duration);
            Stop();
            timeout?.Invoke();
        }
    }

}
