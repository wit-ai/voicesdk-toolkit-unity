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
using UnityEngine.Events;

namespace Oculus.Voice.Toolkit
{
    [CreateAssetMenu(fileName = "InputBridge", menuName = "Voice SDK/Toolkit/InputBridge", order = 1)]

    public class InputBridge : ScriptableObject
    {
        public UnityAction<bool, Ray> Click;
    }

}
