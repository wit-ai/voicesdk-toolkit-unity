/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * Licensed under the Oculus SDK License Agreement (the "License");
 * you may not use the Oculus SDK except in compliance with the License,
 * which is provided at the time of installation or download, or which
 * otherwise accompanies this software in either electronic or hard copy form.
 *
 * You may obtain a copy of the License at
 *
 * https://developer.oculus.com/licenses/oculussdk/
 *
 * Unless required by applicable law or agreed to in writing, the Oculus SDK
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
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
