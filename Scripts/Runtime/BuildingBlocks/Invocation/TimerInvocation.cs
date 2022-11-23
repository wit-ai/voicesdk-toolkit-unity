/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Oculus.Voice.Toolkit
{
    public class TimerInvocation : InvocationBase
    {
        public List<Timer> _timers = new List<Timer>();
        private int _currentIndex = 0;
        public bool IsStarted { get; private set; } = false;

        private IEnumerator _updateEnumerator;

        // private bool _isOddTurn = false;
        private int _counter = 0;
        private float _timestamp = 0;
        private float _finish = 0;
        private float _percentage = 0;
        private float _cycleLength = 0;
        public UnityEvent<int,float,float> timerUpdate;
        protected void OnValidate()
        {
            _syncActivation = true;
        }

        protected void OnDisable()
        {
            StopTimers();
        }

        public void StartTimers()
        {
            _currentIndex = 0;
            _counter = 0;
            _timestamp = 0;
            _finish = 0;
            _percentage = 0;
            _cycleLength = 0;

            StartCurrentTimer();
            //Update Timer
            _updateEnumerator = TimerUpdate(0.03f);
            StartCoroutine(_updateEnumerator);
            IsStarted = true;
        }

        void StartCurrentTimer()
        {
            _timestamp = Time.time;
            _finish = _timestamp + _timers[_currentIndex].Duration;
            _cycleLength = _timers[_currentIndex].Duration;
            _timers[_currentIndex].SetupTimer();
            _timers[_currentIndex].statusUpdate.AddListener(StatusUpdateHanlder);
            StartCoroutine(_timers[_currentIndex].Enumerator);
        }

        public void StopTimers()
        {
            StopCurrentTimer();

            //Update Timer
            if(_updateEnumerator != null) {StopCoroutine(_updateEnumerator);}
            IsStarted = false;
        }

        void StopCurrentTimer()
        {
            if (_timers[_currentIndex].Enumerator != null)
            {
                StopCoroutine(_timers[_currentIndex].Enumerator);
            }
            _timers[_currentIndex].Stop();
            _timers[_currentIndex].statusUpdate.RemoveListener(StatusUpdateHanlder);
        }

        void MoveNextIndex()
        {
            _currentIndex++;
            _currentIndex = _currentIndex % _timers.Count;
        }

        private void StatusUpdateHanlder(bool isOn)
        {
            if (isOn && _timers[_currentIndex].EnableMicrophone)
            {
                base.Activate();
            }else if (!isOn && _timers[_currentIndex].EnableMicrophone)
            {
                base.Deactivate();
            }
        }
        public void SkipAndMoveNext()
        {
            //force current timer stop
            StopCurrentTimer();

            MoveNextIndex();

            //Start the next one
            StartCurrentTimer();
        }

        /// <summary>
/// TimerUpdate is called every 0.03s. It's for displaying countdown.
/// TODO: VocieSDK automatically deactivates vocieService when user is not speaking, which is not what the timerInvocation would like. To hack it, we need to force it to activate voiceService again when the timer is still running.
/// </summary>
        IEnumerator TimerUpdate(float delay)
        {
            while (true)
            {
                if (_timers[_currentIndex].EnableMicrophone && !IsActivated)
                {
                    base.Activate();
                }

                _percentage = (Time.time - _timestamp) / (_finish - _timestamp);
                timerUpdate?.Invoke(_counter, _percentage, _cycleLength);
                yield return new WaitForSeconds(delay);
            }
        }
    }
}
