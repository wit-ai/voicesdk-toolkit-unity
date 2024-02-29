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

using UnityEngine;

namespace Oculus.Voice.Toolkit
{
    [RequireComponent(typeof(Collider), typeof(Rigidbody))]
    public class CollisionInvocation : InvocationBase
    {
        [SerializeField] private Collider _targetCollider;
        //If there is no look at object, it doesn't care the object is in the camera view or not
        [SerializeField] private GameObject _lookAtObject;
        [SerializeField] private Camera _cam;
        private void Awake()
        {
            if (!_cam) _cam = Camera.main;
            gameObject.GetComponent<Collider>().isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other == _targetCollider && !IsActivated && (!_lookAtObject || VoiceUXUtility.InCameraFOV(_cam, _lookAtObject)))
            {
                base.Activate();
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if ((_lookAtObject!= null && !VoiceUXUtility.InCameraFOV(Camera.main, _lookAtObject)) && IsActivated)
            {
                base.Deactivate();
            }else if (other == _targetCollider && !IsActivated && (!_lookAtObject || VoiceUXUtility.InCameraFOV(Camera.main, _lookAtObject)))
            {
                base.Activate();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other == _targetCollider && IsActivated )
            {
                base.Deactivate();
            }
        }
    }
}
