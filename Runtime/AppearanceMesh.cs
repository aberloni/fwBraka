using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{

    [System.Serializable]
    class AppearanceMesh
    {
        public MeshRenderer msh;
        public Material mat;

        public void fetch()
        {
            //var msh = GetComponentInChildren<MeshRenderer>();
            mat = new Material(msh.material);
            msh.material = mat;
        }

        public void set(Texture tex)
        {
            mat.SetTexture("_MainTex", tex);
        }
    }
}