using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
    abstract public class MonoPresence : MonoUpdate
    {
        /// <summary>
        /// HOW to show/hide
        /// </summary>
        virtual protected void materialize()
        {
            gameObject.SetActive(true);
        }

        virtual protected void dematerialize()
        {
            gameObject.SetActive(false);
        }

        virtual public bool isMaterialize()
        {
            return gameObject.activeSelf;
        }

        /// <summary>
        /// action
        /// how to make visible or not
        /// </summary>
        protected void makeVisible(bool flag)
        {
            if (flag) materialize();
            else dematerialize();

            log("visible:" + flag);
            //gameObject.SetActive(flag);
        }

        virtual public void show()
        {
            makeVisible(true);
        }
        virtual public void hide()
        {
            makeVisible(false);
        }

    }

}
