/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using System.Collections.Generic;
using System.Linq;
using Meta.WitAi.Events;
using UnityEngine;

namespace Oculus.Voice.Toolkit
{
    public static class VoiceUXUtility
    {
        #region VoiceState
        public static VoiceUIEvent HasEvent(this VoiceState a, List<VoiceUIEvent> b)
        {
            return (b == null || b.Count == 0) ? null : b.Where(x => x.stateMask == a).FirstOrDefault();
        }

        public static bool HasFlag(VoiceState a, VoiceState b)
        {
            return (a & b) == b;
        }
        public static VoiceState AddFlag(VoiceState value, VoiceState toAdd)
        {
            return value | toAdd;
        }
        #endregion VoiceState

        //TODO: This could potentially slow down the application if there are many objects checking at the same time.
        public static bool InCameraFOV(Camera c, GameObject target)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(c);
            Vector3 point = target.transform.position;

            for (int i = 0; i< planes.Length; i++)
            {
                if (planes[i].GetDistanceToPoint(point) < 0)
                {
                    return false;
                }
            }
            return true;
        }
    }


}
