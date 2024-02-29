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

using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using Meta.WitAi.Events;

namespace Oculus.Voice.Toolkit
{
    [Serializable]
    public class VoiceDataEvent : UnityEvent<VoiceDataBase> {}

    [Serializable]
    public class VoiceUIEvent
    {
        [HideInInspector] public string name;
        [HideInInspector] public VoiceState stateMask;
        public VoiceDataEvent uiEvent = new VoiceDataEvent();
        public VoiceUIEvent(string name, VoiceState states) {
            this.name = name;
            this.stateMask = states;
        }
    }

    public class VoiceUI : BuildingBlockBase
    {
        public VoiceState registerStates;
        [Header("** Events are automatically populated based on register states" )]
        public List<VoiceUIEvent> events = new List<VoiceUIEvent>();

        public override void EventHandler(VoiceState state, VoiceDataBase dataObject)
        {
            InvokeEvent(state, dataObject);
        }

        bool InvokeEvent(VoiceState a, VoiceDataBase dataObject)
        {
            VoiceUIEvent voiceUIEvent = events.Where(x => x.stateMask == a).FirstOrDefault();
            if (voiceUIEvent == null)
            {
                return false;
            }
            else
            {
                if (voiceUIEvent.uiEvent != null)
                {
                    voiceUIEvent.uiEvent.Invoke(dataObject);
                }

                return true;
            }
        }

        /// <summary>
        /// If you want to listen add more states in registerStates during the runtime, called this function.
        /// </summary>
        public VoiceUIEvent GetOrAddEvent(VoiceState state)
        {
            registerStates = VoiceUXUtility.AddFlag(registerStates, state);
            UpdateEvents();
            return VoiceUXUtility.HasEvent(state, events);
        }

        /// <summary>
        /// This should be called when the registerStates are changed.
        /// </summary>
        public void UpdateEvents()
        {
            foreach (VoiceState state in Enum.GetValues(typeof(VoiceState)))
            {
                UpdateEvent(state);
            }
        }

        void UpdateEvent(VoiceState state)
        {
            VoiceUIEvent voiceUIEvent = VoiceUXUtility.HasEvent(state, events);
            if (!VoiceUXUtility.HasFlag(registerStates, state) && voiceUIEvent != null)
            {
                bool result = events.Remove(voiceUIEvent);
            }
            else if (VoiceUXUtility.HasFlag(registerStates, state) && voiceUIEvent == null)
            {
                voiceUIEvent = new VoiceUIEvent(state.ToString(), state);
                events.Add(voiceUIEvent);

            }
        }
    }

}
