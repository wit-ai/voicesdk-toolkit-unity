/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using Meta.WitAi.Utilities;
using UnityEngine;

namespace Oculus.Voice.Toolkit.Activation
{
    public class GazeVoiceActivator : GazeActionTrigger
    {
        [Header("Voice Service")]
        [SerializeField] private VoiceServiceReference voiceServiceReference;

        protected override void OnGazeActionTriggered()
        {
            base.OnGazeActionTriggered();

            if (!voiceServiceReference.VoiceService) return;

            if (voiceServiceReference.VoiceService.Active)
            {
                voiceServiceReference.VoiceService.Deactivate();
            }
            else
            {
                voiceServiceReference.VoiceService.Activate();
            }
        }
    }
}
