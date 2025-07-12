using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{

    public class KappaMono : Mono, Kappa
    {
        public BrainBase brain;

        virtual public Kappa assoc(Brain owner)
        {
            brain = owner as BrainBase;
            return this;
        }

        virtual public Kappa prime()
        {
            return this;
        }

        public void update(float dt)
        {
            updateKappa(dt);
        }

        virtual protected void updateKappa(float dt)
        { }
    }
}