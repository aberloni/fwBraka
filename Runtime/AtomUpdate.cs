using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
    abstract public class AtomUpdate : Atom, iBrakaUpdate
    {
        abstract public void update(float dt);
    }
}