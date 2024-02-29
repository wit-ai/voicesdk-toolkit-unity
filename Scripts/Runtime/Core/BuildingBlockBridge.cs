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
using UnityEngine.Events;
using Meta.WitAi.Events;

namespace Oculus.Voice.Toolkit
{
 [CreateAssetMenu(fileName = "BuildingBlockBridge", menuName = "Voice SDK/Toolkit/BuildingBlockBridge", order = 1)]
 public class BuildingBlockBridge : ScriptableObject
 {
     private VoiceUXAdapter _voiceUXAdapter;
     private bool _isRegisteredEvents = false;
     private bool _wasRegisteredEvents = false;
     public bool Subscription { get; private set; } = false;
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
                 Subscription = true;
             }
         }
         _isRegisteredEvents = true;
     }
     void DeregisterEvents()
     {
         if (_voiceUXAdapter != null)
         {
             _voiceUXAdapter.voiceUIEvent -= EventHandler;
             Subscription = false;
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
