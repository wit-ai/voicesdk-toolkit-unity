/*
 * Copyright (c) Meta Platforms, Inc. and affiliates.
 * All rights reserved.
 *
 * This source code is licensed under the license found in the
 * LICENSE file in the root directory of this source tree.
 */

using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Oculus.Voice.Toolkit.Samples
{
   public class TimerDisplay : MonoBehaviour
   {
      [SerializeField]private Image bar;
      [SerializeField]private TextMeshProUGUI label;

      public void TimerEventHandler(int counter, float percentage, float cycleLength)
      {
         float time = cycleLength * (1-percentage);
         label.text = time.ToString("00:00");
         bar.fillAmount = 1-percentage;
      }
   }

}
