using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SharkAnimation : MonoBehaviour
{
    private SpriteRenderer image;

    void Start()
    {
        image = GetComponent<SpriteRenderer>();
        StartCoroutine(waitAndDestroy());
    }
    void Update()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.007f);
        image.transform.position = new Vector3(image.transform.position.x, image.transform.position.y + 0.005f, image.transform.position.z);
    }

    IEnumerator waitAndDestroy() {
        yield return new WaitForSecondsRealtime(2f);
    }
}
