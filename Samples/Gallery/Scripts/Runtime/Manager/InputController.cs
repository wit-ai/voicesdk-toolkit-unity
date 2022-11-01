/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */
using UnityEngine;

namespace Oculus.Voice.Toolkit.Samples
{
    public class InputController : MonoBehaviour
    {
        [SerializeField] private InputBridge _inputBridge;
        [SerializeField] private Camera _camera;

        protected void OnEnable()
        {
            if (!_camera)
            {
                _camera = Camera.main;
            }
        }

        void Update(){
            if (Input.GetMouseButtonDown(0))
            {
                _inputBridge.Click(true, _camera.ScreenPointToRay(Input.mousePosition));
            }
            if (Input.GetMouseButtonUp(0))
            {
                _inputBridge.Click(false, _camera.ScreenPointToRay(Input.mousePosition));
            }
        }

    }
}
