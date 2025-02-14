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
using Meta.WitAi.Events;
using Meta.WitAi.Events.UnityEventListeners;
using Meta.WitAi.Interfaces;
using Meta.WitAi.ServiceReferences;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
#endif

namespace Oculus.Voice.Toolkit.MicrophoneState
{
    public class VolumeMicIndicator : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("Optional reference to an audio input service reference. If set this indicator will use the specified reference for audio state. Otherwise it will attempt to find a reference or service on its own.")]
        [SerializeField] private AudioInputServiceReference _audioInputService;

        [Tooltip("The lerp speed for transitioning between mic volume levels")]
        [SerializeField] private float lerpSpeed = 20;

        [Tooltip("The peak volume level you expect to see based on the input you are using. Leaving at 0 will detect peaks automatically and adjust.")]
        [SerializeField] private float minimumPeakVolume = 0;

        [Header("Events")] [SerializeField]
        private WitMicLevelChangedEvent onMicFill = new WitMicLevelChangedEvent();
        public UnityEvent<float> OnMicFill => onMicFill;

        private float _minVol = float.MaxValue;
        private float _maxVol = float.MinValue;
        private float _lastLevel;
        private float _displayedLevel;
        private float _lerpPercent;
        private bool _isListening;

        private IAudioInputEvents _audioEvents;

        public float MinVol => _minVol;
        public float MaxVol => _maxVol;

        public float DisplayedLevel => _displayedLevel;
        public float MicLevel
        {
            get => _lastLevel;
            set
            {
                var level = _isListening ? value : 0;
                if (Math.Abs(_lastLevel - level) > .0001)
                {
                    _lerpPercent = 0;
                }

                _lastLevel = level;
                onMicFill.Invoke(_lastLevel);
            }
        }


        protected virtual void OnValidate()
        {
            if (!_audioInputService) _audioInputService = FindAnyObjectByType<AudioInputServiceReference>();
        }

        protected virtual void Awake()
        {
            // Find an audio input service using a search priority if none is set in the inspector.
            if (!_audioInputService) _audioInputService = GetComponent<AudioInputServiceReference>();
            if (!_audioInputService) _audioInputService = GetComponentInParent<AudioInputServiceReference>();
            if (!_audioInputService) _audioInputService = FindAnyObjectByType<AudioInputServiceReference>();

            _maxVol = minimumPeakVolume;
        }

        protected virtual void OnEnable()
        {
            if (!_audioInputService) _audioEvents = FindAnyObjectByType<AudioEventListener>();
            else _audioEvents = _audioInputService.AudioEvents;

            _audioEvents.OnMicAudioLevelChanged.AddListener(OnMicLevelChanged);
            _audioEvents.OnMicStartedListening.AddListener(OnListening);
            _audioEvents.OnMicStoppedListening.AddListener(OnStoppedListening);
        }

        protected virtual void OnDisable()
        {
            _audioEvents.OnMicAudioLevelChanged.RemoveListener(OnMicLevelChanged);
            _audioEvents.OnMicStartedListening.RemoveListener(OnListening);
            _audioEvents.OnMicStoppedListening.RemoveListener(OnStoppedListening);
        }

        protected virtual void OnListening()
        {
            _isListening = true;
        }

        protected virtual void OnStoppedListening()
        {
            _isListening = false;
            MicLevel = 0;
        }

        protected virtual void OnMicLevelChanged(float level)
        {
            // Normalize the mic levels
            _minVol = Mathf.Min(level, _minVol);
            _maxVol = Mathf.Max(level, _maxVol);
            if (_maxVol == _minVol)
            {
                MicLevel = 0;
            }
            else
            {
                MicLevel = Mathf.Clamp01((level - _minVol) / (_maxVol - _minVol));
            }
        }

        protected virtual void Update()
        {
            _lerpPercent = Mathf.Clamp01(_lerpPercent + lerpSpeed * Time.deltaTime);
            _displayedLevel = Mathf.Lerp(_displayedLevel, MicLevel, _lerpPercent);
        }
    }

#if UNITY_EDITOR
#endif
}
