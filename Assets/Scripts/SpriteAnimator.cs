using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float delay = 0.1f;
    [SerializeField] float jitter = 0.03f;
    [SerializeField] Sprite[] sprites;
    [SerializeField] bool playOnce;

    void Start()
    {
        delay = delay + Random.Range(-jitter, +jitter);
        if (sprites.Length > 0) StartCoroutine(Loop());
        else Debug.LogError("No sprites");
    }

    IEnumerator Loop()
    {
        int index = 0;

        while(!playOnce || index < sprites.Length)
        {
            if (index >= sprites.Length) index = 0;
            spriteRenderer.sprite = sprites[index];
            yield return new WaitForSeconds(delay);
            index++;
        }
        Destroy(gameObject);
    }
}
