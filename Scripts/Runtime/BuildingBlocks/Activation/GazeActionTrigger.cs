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
using UnityEngine;
using UnityEngine.Events;

namespace Oculus.Voice.Toolkit.Activation
{
    public class GazeActionTrigger : MonoBehaviour
    {
        [Header("Activation")]
        [SerializeField] private float _activationTime = 2;

        [Header("Events")]
        [SerializeField] private UnityEvent _onGazeStart = new UnityEvent();
        [SerializeField] private UnityEvent _onGazeEnd = new UnityEvent();

        [SerializeField] private UnityEvent _onGazeActionTriggeredEvent = new UnityEvent();

        [SerializeField]
        private GazeTriggerProgressEvent _onGazeTriggerProgress = new
            GazeTriggerProgressEvent();

        [Header("Camera")]
        [SerializeField] private Camera _gazeSourceCamera;

        private bool _gazing = false;

        private bool _triggered;
        private float _gazeStart;
        private float _deltaTime;

        private void OnValidate()
        {
            if(!_gazeSourceCamera) _gazeSourceCamera = Camera.main;
        }

        private void Awake()
        {
            if(!_gazeSourceCamera) _gazeSourceCamera = Camera.main;
        }

        private void Update()
        {
            if (Physics.Raycast(_gazeSourceCamera.transform.position, _gazeSourceCamera.transform.forward,
                out var hit) && hit.collider.gameObject == gameObject)
            {
                if (!_gazing)
                {
                    _gazeStart = Time.time;
                    _onGazeTriggerProgress.Invoke(0);
                    OnGazeStarted();
                }

                _gazing = true;
            }
            else if (_gazing)
            {
                _triggered = false;
                _gazing = false;
                OnGazeStopped();
            }

            if (_gazing && !_triggered)
            {
                _deltaTime = Time.time - _gazeStart;
                _onGazeTriggerProgress.Invoke(_deltaTime / _activationTime);
                if (_deltaTime > _activationTime && !_triggered)
                {
                    _triggered = true;
                    _onGazeTriggerProgress.Invoke(0);
                    OnGazeActionTriggered();
                }
            }

            if (!_gazing && _deltaTime > 0)
            {
                _deltaTime = Mathf.Clamp01(_deltaTime - Time.deltaTime);
            }
        }

        protected virtual void OnGazeStarted()
        {
            _onGazeStart.Invoke();
        }

        protected virtual void OnGazeStopped()
        {
            _onGazeEnd.Invoke();
        }

        protected virtual void OnGazeActionTriggered()
        {
            _onGazeActionTriggeredEvent.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            if (_gazeSourceCamera)
            {
                Gizmos.DrawRay(_gazeSourceCamera.transform.position, _gazeSourceCamera.transform.forward * 1000);
            }
        }
    }

    [Serializable]
    public class GazeTriggerProgressEvent : UnityEvent<float> {}
}
