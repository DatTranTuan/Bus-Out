using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScaler : MonoBehaviour {

    private float startScale;

    private void Awake() {
        startScale = transform.localScale.x;
    }
    private void Start() {
        SetScalerWithScreen();
    }

    private void SetScalerWithScreen() {
        Camera cam = Camera.main;
        float screenWidth = cam.orthographicSize * cam.aspect * (20f / cam.orthographicSize);
        //float screenHeight = cam.orthographicSize * 2;
        if (screenWidth < 10.8f)
            transform.localScale = Vector3.one * (screenWidth * startScale / 10.8f);
    }
}
