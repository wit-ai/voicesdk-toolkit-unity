/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Oculus.Voice.Toolkit
{
    [CustomEditor(typeof(TimerInvocation), true)]
    public class TimerInvocationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            TimerInvocation timerInvocation = (TimerInvocation)target;

            if (GUILayout.Button("Start Timer"))
            {
                timerInvocation.StartTimers();
            }
            if (GUILayout.Button("Skip and move next"))
            {
                timerInvocation.SkipAndMoveNext();
            }

            if (GUILayout.Button("Stop Timer"))
            {
                timerInvocation.StopTimers();
            }

            base.OnInspectorGUI();
        }
    }
}
