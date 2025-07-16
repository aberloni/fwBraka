using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// manager qui gère les interruptions de visuel UI
/// </summary>
public class UiLayerInterruptor
{
    static UiLayerInterruptor _instance;
    static public UiLayerInterruptor instance
    {
        get
        {
            if (_instance == null) _instance = new UiLayerInterruptor();
            return _instance;
        }
    }

    List<iLayerInterruptable> candidates = new List<iLayerInterruptable>();
    Dictionary<UiLayer, List<object>> interruptors = new Dictionary<UiLayer, List<object>>();

    public UiLayerInterruptor()
    {
        foreach (UiLayer v in System.Enum.GetValues(typeof(UiLayer)))
        {
            if (v == UiLayer.none) continue;

            interruptors.Add(v, new List<object>());
            Debug.Log("+" + v);
        }
    }

    public void addCandidate(iLayerInterruptable target)
    {
        if (candidates.IndexOf(target) > -1)
        {
            Debug.LogError("already sub ?");
            Debug.LogError(target.stringify(), target as Component);
            return;
        }

        candidates.Add(target);
        eval(target);

    }
    public void remCandidate(iLayerInterruptable target)
    {
        candidates.Remove(target);
    }

    public void remLock(object locker)
    {
        bool changed = false;
        foreach (var kp in interruptors)
        {
            if (kp.Value.Contains(locker))
            {
                kp.Value.Remove(locker);
                Debug.Log("  layer      UNLOCK " + kp.Key + " x" + kp.Value.Count);
                changed = true;
            }
        }

        if (changed) eval(UiLayer.none);
    }

    public void addLock(object locker, UiLayer whatLayer)
    {
        if (locker == null)
            return;

        bool changed = false;
        foreach (UiLayer v in System.Enum.GetValues(typeof(UiLayer)))
        {
            if (v == UiLayer.none) continue;

            if (whatLayer.HasFlag(v))
            {
                if (interruptors[v].IndexOf(locker) < 0)
                {
                    //Debug.Log(whatLayer + " has " + v);
                    interruptors[v].Add(locker);
                    changed = true;
                    Debug.Log("  layer      LOCK " + v + " x" + interruptors[v].Count);
                }

            }
        }

        if (changed) eval(whatLayer);
    }

    /// <summary>
    /// any of target layer is locked ?
    /// </summary>
    void eval(iLayerInterruptable target)
    {
        UiLayer lyrs = target.getInterruptionLayer();
        foreach (var kp in interruptors)
        {
            if (lyrs.HasFlag(kp.Key) && kp.Value.Count > 0)
            {
                target.setInterruption(kp.Key);
            }
        }
    }

    void eval(UiLayer onlyTriggerLayer)
    {
        //Debug.Log("eval x" + candidates.Count + " candidates");

        foreach (var c in candidates)
        {
            var lyr = c.getInterruptionLayer();
            if (lyr == UiLayer.none)
            {
                Debug.LogWarning("none layer ?");
                continue;
            }

            // skip ?
            if (onlyTriggerLayer != UiLayer.none)
            {
                if (!lyr.HasFlag(onlyTriggerLayer)) continue;
            }

            bool noneLocked = true;

            foreach (var kp in interruptors)
            {
                if (lyr.HasFlag(kp.Key) && kp.Value.Count > 0)
                {
                    //Debug.Log(c.stringify() + " ==> " + lyr + " has " + kp.Key + " & cnt x" + kp.Value.Count);
                    c.setInterruption(kp.Key);
                    noneLocked = false;
                }
            }

            if (noneLocked) c.setInterruption(UiLayer.none);
        }
    }

#if UNITY_EDITOR
    [UnityEditor.MenuItem("Modules/show interruptors")]
    static void miShowInterruptors()
    {
        Debug.Log("x" + instance.candidates.Count + " candidates");

        foreach (var kp in instance.interruptors)
        {
            Debug.Log(kp.Key + " ? " + kp.Value.Count);
        }
    }
#endif
}

public interface iLayerInterruptable
{
    public string stringify();
    public UiLayer getInterruptionLayer();
    public void setInterruption(UiLayer interruptions);
}

[System.Flags]
public enum UiLayer
{
    none = 0,
    nav = 1 << 1,   // link to navigating within game
    talk = 1 << 2,  // talking to people
    ui = 1 << 3,    // major ui elements
}
