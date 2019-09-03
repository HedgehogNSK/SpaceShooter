using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public struct HitArgs
    { 
        public GameObject Attacker { get; set; }
        public GameObject Victim { get; set; }
        public int Damage { get; set; }
        public static HitBuilder CreateBuilder()
        {
            return new HitBuilder();
        }

        public override string ToString()
        {
            return "Hit by "+ Attacker.name + " bringing "+Damage+" damage" + ", vistim is:"+ Victim.name;
        }
    }

}
