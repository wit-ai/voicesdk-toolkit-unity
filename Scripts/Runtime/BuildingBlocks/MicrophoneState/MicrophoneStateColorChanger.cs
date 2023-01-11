/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Meta.WitAi.ServiceReferences;
using UnityEngine;
using UnityEngine.UI;

namespace Oculus.Voice.Toolkit.MicrophoneState
{
    public class MicrophoneStateColorChanger : MonoBehaviour
    {
        [Header("Components")]
        [Tooltip("Optional reference to an audio input service reference. If set this indicator will use the specified reference for audio state. Otherwise it will attempt to find a reference or service on its own.")]
        [SerializeField] private AudioInputServiceReference _audioInputService;

        [SerializeField] private Graphic _graphic;
        
        [Header("Colors")]
        [SerializeField] private Color _activeColor = Color.red;
        [SerializeField] private Color _inactiveColor = Color.white;

        private void OnEnable()
        {
            _audioInputService.AudioEvents.OnMicStartedListening.AddListener(OnStartedListening);
            _audioInputService.AudioEvents.OnMicStoppedListening.AddListener(OnStoppedListening);
        }

        private void OnDisable()
        {
            _audioInputService.AudioEvents.OnMicStartedListening.RemoveListener(OnStartedListening);
            _audioInputService.AudioEvents.OnMicStoppedListening.RemoveListener(OnStoppedListening);
        }

        private void OnStartedListening()
        {
            _graphic.color = _activeColor;
        }

        private void OnStoppedListening()
        {
            _graphic.color = _inactiveColor;
        }
    }
}

