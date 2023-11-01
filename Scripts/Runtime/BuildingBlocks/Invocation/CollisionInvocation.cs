/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
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
