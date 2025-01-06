using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TestTween : MonoBehaviour
{
    Transform go;
    Tweener tweener;

    private void OnEnable()
    {
        tweener = go.DOMove(Vector3.zero,1);
    }
    private void OnDisable()
    {
        if (tweener != null) {
            tweener.Kill();
            tweener.Complete();
        }
    }
}
