using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShake : MonoBehaviour
{
    
    void Start()
    {
        transform.DORotate(new Vector3(0, 0f, -15f), 0.2f)
                         .SetEase(Ease.InOutSine)
                         .SetLoops(-1, LoopType.Yoyo);
    }

}
