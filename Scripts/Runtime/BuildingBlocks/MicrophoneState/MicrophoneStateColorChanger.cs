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

