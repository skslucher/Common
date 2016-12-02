using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpritesheetOverride : MonoBehaviour {

    private static DictionaryAutoConstruct<Texture2D, MaterialPropertyBlock> blockAtlas = new DictionaryAutoConstruct<Texture2D, MaterialPropertyBlock>(CreateBlock);
    
    public static MaterialPropertyBlock CreateBlock(Texture2D texture)
    {
        MaterialPropertyBlock newBlock = new MaterialPropertyBlock();

        newBlock.SetFloat("_SliceAmount", .5f);
        newBlock.SetTexture("_MainTex", texture);

        return newBlock;
    }


    public Texture2D spritesheet;
    public bool continuous = true;

    private MaterialPropertyBlock block;
    private SpriteRenderer spriteRenderer;
    
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spritesheet == null)
            spritesheet = spriteRenderer.sprite.texture;
        
        block = blockAtlas[spritesheet];
    }

    void LateUpdate()
    {
        if (continuous)
            spriteRenderer.SetPropertyBlock(block);
    }
}
