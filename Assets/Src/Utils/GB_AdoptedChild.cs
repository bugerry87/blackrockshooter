using System;
using UnityEngine;

namespace GBAssets.Utils
{
    public sealed class GB_AdoptedChild : GB_AUpdateMode
    {
        [SerializeField] Transform target = null;
        [SerializeField] Vector3 localPosition = Vector3.zero;
        [SerializeField] Vector3 localRotation = Vector3.zero;
        [SerializeField] Vector3 localScale = Vector3.one;

        void Awake ()
        {
            DoUpdate(1);
        }

        protected override void DoUpdate(float deltaTime)
        {
            transform.SetParent(target);
            transform.localPosition = localPosition;
            transform.localEulerAngles = localRotation;
            transform.localScale = localScale;
        }
    }
}
