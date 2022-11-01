/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Meta.WitAi.Events;

namespace Oculus.Voice.Toolkit
{
    public class InvocationAggregation : BuildingBlockBase
    {
        [SerializeField] private List<InvocationBase> _invocations = new List<InvocationBase>();
        private bool _isOn = false;
        public bool IsOn => _isOn;
        public Action<bool> InvocationAggregationUpdate;

        protected override void OnEnable()
        {
            base.OnEnable();
            foreach (InvocationBase sender in _invocations)
            {
                sender.On += OnHandler;
                sender.Off += OffHandler;
                sender.RegisterAgggreation(this);
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            foreach (InvocationBase sender in _invocations)
            {
                sender.On -= OnHandler;
                sender.Off -= OffHandler;
                sender.DeregisterAgggreation(this);
            }
        }
        public override void EventHandler(VoiceState state, VoiceDataBase dataObject)
        {
            //VoiceService Callbacks
            switch (state)
            {
                case VoiceState.MicOff:
                    if (_isOn)
                    {
                        OffHandler();
                    }
                    break;
                case VoiceState.MicOn:
                    break;
            }
        }
        void OnHandler()
        {
            if (!_isOn && _invocations.All(x => x.IsActivated == true))
            {
                _isOn = true;
                _buildingBlockBridge.TriggerInvocation(_isOn);
                InvocationAggregationUpdate?.Invoke(_isOn);
            }
        }

        void OffHandler()
        {
            if (_isOn)
            {
                _isOn = false;
                _buildingBlockBridge.TriggerInvocation(_isOn);
                InvocationAggregationUpdate?.Invoke(_isOn);
            }
        }
    }

}
