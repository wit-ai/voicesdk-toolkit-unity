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
