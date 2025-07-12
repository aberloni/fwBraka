using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace fwp.braka
{
    /// <summary>
    /// Mono.validate
    /// includes various methods to react to validate
    /// </summary>
    public partial class Mono : MonoBehaviour
    {

#if UNITY_EDITOR
        /// <summary>
        /// helper to force a validate
        /// </summary>
        [SerializeField]
        bool forceValidate;

        bool _dirty;

        protected bool isBuildingPlayer() => UnityEditor.BuildPipeline.isBuildingPlayer;

        [ContextMenu("validate")]
        void cmValidate() => validate();

        private void OnValidate()
        {
            //if (UnityEditor.BuildPipeline.isBuildingPlayer) return;
            if (Application.isPlaying || !Application.isEditor)
            {
                forceValidate = false;
                //Debug.LogWarning("validate.disabled");
                return;
            }

            validate(forceValidate);

            if (isBuildingPlayer()) validateBuilding(forceValidate);
            else
            {
                if (isSourcePrefab()) validateSource(forceValidate);
                else validateInstance(forceValidate);
            }

            if (_dirty || forceValidate)
            {
                _dirty = false;
                forceValidate = false;
                UnityEditor.EditorUtility.SetDirty(gameObject);
            }
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            var monos = FindObjectsByType<Mono>(FindObjectsSortMode.None);
            foreach (var m in monos)
            {
                m.validateCompile();
            }
        }

        virtual public void validateCompile()
        { }

        /// <summary>
        /// duyring validate
        /// </summary>
        protected void setValidateDirty()
        {
            _dirty = true;
        }

        protected void setDirty(Object tar = null)
        {
            if (tar == null) tar = this;

            if (!isSourcePrefab())
            {
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);
            }

            UnityEditor.EditorUtility.SetDirty(tar);


        }

        /// <summary>
        /// only called during build process
        /// </summary>
        virtual protected void validateBuilding(bool force = false)
        {
            logc("validate:build");
        }

        /// <summary>
        /// only !runtime
        /// all instance AND prefabs in source
        /// </summary>
        [ContextMenu("validate")]
        virtual protected void validate(bool force = false)
        {
            logc("validate:" + force);

            //if (verbose) logInte("is verbose");
        }

        /// <summary>
        /// valide on prefab in folder (outside of a scene)
        /// </summary>
        virtual protected void validateSource(bool force = false)
        {
            logc("validate:source:" + force);
        }

        /// <summary>
        /// only instance in scenes
        /// force is when user clicked "force validate" in inspector
        /// </summary>
        /// <returns>true : set dirty</returns>
        virtual protected void validateInstance(bool force = false)
        {
            logc("validate:instance:" + force);
        }

        /// <summary>
        /// 
        /// true : is a prefab in the resources Assets/
        /// false : is an instance place in a scene
        /// </summary>
        protected bool isSourcePrefab()
        {
            // https://stackoverflow.com/questions/56155148/how-to-avoid-the-onvalidate-method-from-being-called-in-prefab-mode
            // https://stackoverflow.com/questions/54737893/how-can-i-tell-whether-an-object-is-a-prefab-asset-or-an-instance-of-that-prefab
            if (string.IsNullOrEmpty(gameObject.scene.name)) return true;

            // attached to scene and not a gameobject ?
            // false positive : if base game object has same name as scene
            if (gameObject.scene.name == gameObject.name) return true;

            return false;
        }

        public void appendName(string append)
        {
            overrideName(name + append);
        }

        public void overrideName(string newName)
        {
            if (name != newName)
            {
                name = newName;
                UnityEditor.EditorUtility.SetDirty(this);
            }
        }

#endif
    }

}
