/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */
using UnityEngine;
using System;
using System.Collections.Generic;

namespace Oculus.Voice.Toolkit
{
    public class InvocationBase : MonoBehaviour
    {
        private bool _isActivated = false;
        internal bool _syncActivation = false;
        public bool IsActivated => _isActivated;
        public Action On;
        public Action Off;

        public void Activate()
        {
            if (!_isActivated)
            {
                _isActivated = true;
                On?.Invoke();
            }
        }

        public void Deactivate()
        {
            if (_isActivated)
            {
                _isActivated = false;
                Off?.Invoke();
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
