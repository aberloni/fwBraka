using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiWorldPositionTracker
{
    Camera _camera;
    Canvas _canvas;
    RectTransform _target;

    Transform track;
    Vector3 trackWorldPosition;

    public UiWorldPositionTracker(RectTransform target, Canvas context, Camera camera)
    {
        _target = target;
        
        if (camera != null) _camera = camera;
        else _camera = Camera.main;

        _canvas = context;
    }

    public void setPosition(Vector3 world)
    {
        trackWorldPosition = world;
        update();
    }

    public void trackTransform(Transform tr)
    {
        track = tr;
        setPosition(tr.position);
    }

    public void update()
    {
        if (track != null) trackWorldPosition = track.position;

        // align to position
        _target.position = WorldToUISpace(_canvas, trackWorldPosition, _camera);
    }

    public static Vector3 WorldToUISpace(Canvas parentCanvas, Vector3 worldPos, Camera camera)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = camera.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
