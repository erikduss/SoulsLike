using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SoulsLike
{
    public class DestroyAfterTime : MonoBehaviour
    {
        public float timeUntilDestroyed = 3f;

        private void Awake()
        {
            Destroy(gameObject, timeUntilDestroyed);
        }
    }
}
