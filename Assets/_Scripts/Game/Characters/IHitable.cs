using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public interface IHitable
    {
        void HitObject(HitArgs hit);
        event System.Action<HitArgs> OnDie;
    }
}
