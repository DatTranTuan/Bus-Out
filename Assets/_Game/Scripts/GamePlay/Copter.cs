using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Copter : MonoBehaviour
{
    Tween tween;

    private void OnEnable()
    {
       tween = transform.DORotate(new Vector3(0, 360, 0), 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental); 
    }

    private void OnDisable()
    {
        tween.Kill();
    }
}
