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
using System;
using UnityEngine.Events;

namespace Oculus.Voice.Toolkit
{
    public class InvocationBase : MonoBehaviour
    {
        private bool _isActivated = false;
        internal bool _syncActivation = false;
        public bool IsActivated => _isActivated;
        public Action On;
        public Action Off;
        public UnityEvent onActivate = new UnityEvent();
        public UnityEvent onDeactivate = new UnityEvent();

        public void Activate()
        {
            if (!_isActivated)
            {
                _isActivated = true;
                On?.Invoke();
                onActivate?.Invoke();
            }
        }

        public void Deactivate()
        {
            if (_isActivated)
            {
                _isActivated = false;
                Off?.Invoke();
                onDeactivate?.Invoke();
            }
        }
        public void ToggleActivation()
        {
            if (_isActivated)
            {
                Deactivate();
            }
            else
            {
                Activate();
            }
        }

        public virtual void RegisterAgggreation(InvocationAggregation _aggregation)
        {
            _aggregation.InvocationAggregationUpdate += InvocationAggregationUpdate;
        }


        public virtual void DeregisterAgggreation(InvocationAggregation _aggregation)
        {
            _aggregation.InvocationAggregationUpdate -= InvocationAggregationUpdate;
        }
        public virtual void InvocationAggregationUpdate(bool isOn)
        {
            if (_syncActivation)
            {
                _isActivated = isOn;
            }
        }

    }

}
