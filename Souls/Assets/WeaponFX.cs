using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class WeaponFX : MonoBehaviour
    {
        [Header("Weapon FX")]
        public ParticleSystem normalWeaponTrail;

        public void PlayWeaponFX()
        {
            normalWeaponTrail.Stop();

            if (normalWeaponTrail.isStopped)
            {
                normalWeaponTrail.Play();
                Debug.Log("PLay");
            }
        }
    }
}
