using UnityEngine;
using System.Collections;

public class Randomize : MonoBehaviour {

    public Sprite[] sprites;

    public float positionVariation = .5f;

    public Vector2 velocity = Vector2.up;
    public float velocityVariation = .5f;

    public float angularVelocity = 720f;
    public float angularVelocityVariation = 360f;
    

    void Start()
    {
        //GetComponent<SpriteRenderer>().sprite = sprites[Mathf.FloorToInt(Random.value * sprites.Length)];
        //GetComponent<SpriteRenderer>().sprite = sprites[RandUtil.Int(sprites.Length)];
        GetComponent<SpriteRenderer>().sprite = sprites.RandomValue();

        transform.position += (Vector3)(positionVariation * Random.insideUnitCircle);
        
        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        if (rigid != null)
        {
            GetComponent<Rigidbody2D>().velocity = velocity + velocityVariation * Random.insideUnitCircle;
            GetComponent<Rigidbody2D>().angularVelocity = RandUtil.Variance(angularVelocity, angularVelocityVariation);
        }

    }



}
