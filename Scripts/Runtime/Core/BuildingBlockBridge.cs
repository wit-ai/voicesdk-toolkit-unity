/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System;
using UnityEngine;
using UnityEngine.Events;
using Facebook.WitAi.Events;
using UnityEngine.Serialization;

namespace Oculus.Voice.Toolkit
{
 [CreateAssetMenu(fileName = "BuildingBlockBridge", menuName = "Voice SDK/Toolkit/BuildingBlockBridge", order = 1)]
 public class BuildingBlockBridge : ScriptableObject
 {
     private VoiceUXAdapter _voiceUXAdapter;
     private bool _isRegisteredEvents = false;
     private bool _wasRegisteredEvents = false;
     private bool _subscription = false;
     public UnityAction<VoiceState, VoiceDataBase> voiceUIEvent;
     [SerializeField]private bool _debugEvent = false;
     public void SetupVoiceUXAdapter(VoiceUXAdapter uxAdapter)
     {
         _voiceUXAdapter = uxAdapter;
     }

     public void ResetValues()
     {
         _isRegisteredEvents = false;
         _wasRegisteredEvents = false;
         DeregisterEvents();
     }
     public void ListenFromVoiceService()
     {
         RegisterEvents();
     }
     public void TriggerInvocation(bool activated)
    {
        if (activated)
        {
            _voiceUXAdapter.Activate();
            RegisterEvents();
        }
        else
        {
            _voiceUXAdapter.Deactivate();
        }
    }

     void RegisterEvents()
     {
         if (_isRegisteredEvents == false)
         {
             if (_voiceUXAdapter != null)
             {
                 _voiceUXAdapter.voiceUIEvent += EventHandler;
                 _subscription = true;
             }
         }
         _isRegisteredEvents = true;
     }
     void DeregisterEvents()
     {
         if (_voiceUXAdapter != null)
         {
             _voiceUXAdapter.voiceUIEvent -= EventHandler;
             _subscription = false;
         }
     }
     /// <summary>
     /// The voice service events only allow to pass to VoiceUI when the invocation is on.
     /// </summary>
     /// <param name="activated"></param>
     void EventHandler(VoiceState state, VoiceDataBase dataObject)
     {
         if (_debugEvent)
         {
             Debug.Log($"{this.name} invoke {state.ToString()}");
         }

         voiceUIEvent.Invoke(state,dataObject);
         switch (state)
         {
             case VoiceState.MicOff :
                 if (_isRegisteredEvents)
                 {
                     _isRegisteredEvents = false;
                     _wasRegisteredEvents = true;
                 }
                 break;
             case VoiceState.Response:
                 _wasRegisteredEvents = false;
                 break;
         }

         if (_isRegisteredEvents == false && _wasRegisteredEvents == false)
         {
             DeregisterEvents();
         }
     }
 }
}
