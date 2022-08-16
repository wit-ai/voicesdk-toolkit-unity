/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using UnityEditor;
using UnityEngine;
using System;
namespace Oculus.Voice.Toolkit
{
    [CustomEditor(typeof(VoiceUI), true)]
    public class VoiceUIEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            VoiceUI voiceUI = (VoiceUI)target;
            voiceUI.UpdateEvents();
            base.OnInspectorGUI();
        }
    }

}
