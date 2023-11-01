/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using UnityEngine;
using Meta.WitAi.Events;

namespace Oculus.Voice.Toolkit
{
    public class BuildingBlockBase : MonoBehaviour
    {
        [SerializeField] protected internal BuildingBlockBridge _buildingBlockBridge;
        protected virtual void Awake()
        {
            var registerFromVoiceService = false;
            if (!_buildingBlockBridge)
            {
                _buildingBlockBridge = ScriptableObject.CreateInstance(typeof(BuildingBlockBridge)) as BuildingBlockBridge;
                registerFromVoiceService = true;
            }
            _buildingBlockBridge.SetupVoiceUXAdapter(FindObjectOfType<VoiceUXAdapter>());
            _buildingBlockBridge.ResetValues();
            if (registerFromVoiceService)
            {
                _buildingBlockBridge.ListenFromVoiceService();
            }

        }
        protected virtual void OnEnable()
        {
            _buildingBlockBridge.voiceUIEvent += EventHandler;
        }
        protected virtual void OnDisable()
        {
            _buildingBlockBridge.voiceUIEvent -= EventHandler;
        }
        public virtual void EventHandler(VoiceState state, VoiceDataBase dataObject)
        {
        }
    }

}
