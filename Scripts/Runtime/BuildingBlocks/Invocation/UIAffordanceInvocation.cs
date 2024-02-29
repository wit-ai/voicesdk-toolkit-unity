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
