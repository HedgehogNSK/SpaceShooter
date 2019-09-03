using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceShooter
{
    public class HitBuilder 
    {
        private HitArgs args;

        public HitBuilder()
        {
            args = new HitArgs();
        }

        public HitBuilder SetAttacker(GameObject attacker)
        {
            args.Attacker = attacker;
            return this;
        }

        public HitBuilder SetDamage(int damage)
        {
            args.Damage = damage >0? damage:0;
            return this;
        }

        public HitBuilder SetVictim(GameObject victim)
        {
            args.Victim = victim;
            return this;
        }

        public static implicit operator HitArgs(HitBuilder builder)
        {
            return builder.args;
        }
    }
}