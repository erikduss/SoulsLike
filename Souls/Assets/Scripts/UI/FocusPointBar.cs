using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SoulsLike
{
    public class FocusPointBar : MonoBehaviour
    {
        public Slider slider;

        public void SetMaxFocusPoints(float maxFP)
        {
            slider.maxValue = maxFP;
            slider.value = maxFP;
        }

        public void SetCurrentFocusPoints(float currentFP)
        {
            slider.value = currentFP;
        }
    }
}
