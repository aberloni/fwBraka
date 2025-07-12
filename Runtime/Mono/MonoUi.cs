using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
    public class MonoUi : MonoPresence
    {
        Canvas canvas;

        /// <summary>
        /// desired visibility (internal)
        /// </summary>
        bool _visibility = true;

        [Tooltip("mask when layer is active")]
        public UiLayer layerInterruption;
        public UiLayer getInterruptionLayer() => layerInterruption;

        [Tooltip("should be alway visible")]
        public bool staticVisibility = false;

        UiWorldPositionTracker _trackerWorldPosition;
        public UiWorldPositionTracker TrackerWorldPosition
        {
            get
            {
                if (_trackerWorldPosition == null)
                    _trackerWorldPosition = new UiWorldPositionTracker(transform as RectTransform, canvas, Camera.main);

                return _trackerWorldPosition;
            }
        }

        /// <summary>
        /// must call show or hide ?
        /// </summary>
        virtual protected bool hideOnStartup() => false;
        
        protected override void build()
        {
            base.build();

            if (canvas == null) canvas = GetComponentInParent<Canvas>();

            if (hideOnStartup()) hide();
        }

        protected override void update(float dt)
        {
            if (_trackerWorldPosition != null) _trackerWorldPosition.update();
        }

        override public void show()
        {
            _visibility = true;
            makeVisible(computeVisibilityState());

            if (layerInterruption != UiLayer.none)
            {
                iLayerInterruptable cand = this as iLayerInterruptable;
                if (cand != null) UiLayerInterruptor.instance.addCandidate(cand);
            }
        }

        override public void hide()
        {
            _visibility = false;
            makeVisible(computeVisibilityState());

            // unlock layer interrupt
            if (layerInterruption != UiLayer.none)
            {
                iLayerInterruptable cand = this as iLayerInterruptable;
                if (cand != null) UiLayerInterruptor.instance.remCandidate(cand);
            }
        }

        /// <summary>
        /// true : is visible
        /// </summary>
        virtual protected bool computeVisibilityState()
        {
            if (staticVisibility) return true;
            return _visibility;
        }

    }

}
