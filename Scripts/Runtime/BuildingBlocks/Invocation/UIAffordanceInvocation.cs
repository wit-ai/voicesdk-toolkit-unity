/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Oculus.Voice.Toolkit
{
    public class UIAffordanceInvocation : InvocationBase
    {
        public enum type
        {
            Click, ClickHold
        }
        [SerializeField] private Collider _uiCollider;
        [SerializeField] private InputBridge _inputBridge;
        [SerializeField] private type _type;

        protected void OnValidate()
        {
            if (_type == type.Click)
            {
                _syncActivation = true;
            }else if (_type == type.ClickHold)
            {
                _syncActivation = false;
            }
        }

        protected void OnEnable()
        {
            _inputBridge.Click += ClickEventHandler;
        }
        protected void OnDisable()
        {
            _inputBridge.Click -= ClickEventHandler;
        }

        void ClickEventHandler(bool isDown, Ray ray)
        {
            switch (_type)
            {
                case type.Click:
                    Click(isDown, ray);
                    break;
                case type.ClickHold:
                    ClickHold(isDown, ray);
                    break;
            }
        }

        void Click(bool isDown, Ray ray)
        {
            if (isDown && HitCollider(_uiCollider,ray))
            {
                base.ToggleActivation();
            }
        }

        void ClickHold(bool isDown, Ray ray)
        {

            if (isDown && HitCollider(_uiCollider,ray))
            {
                base.Activate();
            }else if (!isDown)
            {
                base.Deactivate();
            }
        }

        bool HitCollider(Collider collider, Ray ray)
        {
            RaycastHit hit;
            return collider.Raycast(ray, out hit, 100.0f);
        }

    }
}
