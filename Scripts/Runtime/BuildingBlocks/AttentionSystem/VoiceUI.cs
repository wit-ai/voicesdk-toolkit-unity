/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using System;
using Meta.WitAi.Events;
using UnityEngine.Serialization;

namespace Oculus.Voice.Toolkit
{
    [System.Serializable]
    public class VoiceDataEvent : UnityEvent<VoiceDataBase> {}

    [System.Serializable]
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
