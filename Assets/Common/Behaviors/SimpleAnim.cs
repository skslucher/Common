using UnityEngine;
using System.Collections;

[AddComponentMenu("Common/Simple Anim")]
public class SimpleAnim : MonoBehaviour {

    public Sprite[] sprites;
    public float frameRate;
    public bool loop;
    public bool randomizedStart = true;

    private int i;
    private SpriteRenderer rend;

    void Awake() {
        rend = GetComponent<SpriteRenderer>();
    }

    void OnEnable() {
        i = 0;

        if(loop && randomizedStart)
        {
            i = Mathf.FloorToInt(Random.value * sprites.Length);
        }

        ChangeSprite();
	}

    void ChangeSprite()
    {
        rend.sprite = sprites[i];
        i = (i+1)%sprites.Length;

        if(i > 0 || loop)
            Invoke("ChangeSprite", frameRate);
    }

}
