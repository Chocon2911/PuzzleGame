using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : HuyMonoBehaviour
{
    //===========================================Unity============================================
    private void Update()
    {
        this.TouchingScreen();
    }

    //===========================================Method===========================================
    private void TouchingScreen()
    {
        if (Time.timeScale == 0) return;
        if (!LevelManager.Instance.CanMoveBlock()) return;
        if (Input.touchCount <= 0 || Input.GetTouch(0).phase != TouchPhase.Began) return;
        Touch touch = Input.GetTouch(0);
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit)) return;
        Touchable touchable = hit.collider.GetComponent<Touchable>();
        
        if (touchable == null) return;
        touchable.Touch();
    }
}
