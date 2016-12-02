using UnityEngine;
using System.Collections;

public class SpritesheetAnimator : MonoBehaviour {

    [System.Serializable]
    public enum SpritesheetAnimations{
        Stand,
        Walk,
        Jump,
        Hurt,
        Die
    }



    public Texture2D spriteSheet;
    public int spriteWidth = 16;
    int width;
    int height;

    private SpriteRenderer _renderer;
    public Sprite[] sprites;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        SetupSprites();
    }

    void Start()
    {
        SetSprite(0, 0);
    }

    private void SetupSprites()
    {
        width = spriteSheet.width / spriteWidth;
        height = spriteSheet.height / spriteWidth;

        //Debug.Log(width + ", " + height);

        sprites = new Sprite[width * height];


        for(int i=0; i<width; i ++)
        {
            for(int j=0; j<height; j ++)
            {
                sprites[i + width*j] = Sprite.Create(spriteSheet, new Rect(i*spriteWidth, j*spriteWidth, spriteWidth, spriteWidth), Vector2.one*.5f, 16f);
                //Debug.Log(i + "," + j + ". ?" + spriteWidth/2);
            }
        }
    }
    
    public void SetSpriteAnim(string coords)
    {
        string[] coord = coords.Split(',');
        SetSprite(int.Parse(coord[1]), int.Parse(coord[0]));
    }

    void SetSprite(SpritesheetAnimations animation, float frame)
    {
        SetSprite(Mathf.FloorToInt(frame), (int)animation);
    }

    void SetSprite(int x, int y)
    {
        _renderer.sprite = sprites[x + width * (height-y-1)];
    }
}
