using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {
    public Sprite[] sprites;
    public enum Type { JETPACK, WINGS, BUBBLE };
    public Type type;

    void Start()
    {
        int num = Mathf.FloorToInt(Random.value * 3);

        switch(num)
        {
            case 0:
                type = Type.JETPACK;
                GetComponent<SpriteRenderer>().sprite = sprites[0];
                break;
            case 1:
                type = Type.WINGS;
                GetComponent<SpriteRenderer>().sprite = sprites[1];
                break;
            case 2:
                type = Type.BUBBLE;
                GetComponent<SpriteRenderer>().sprite = sprites[2];
                break;
        }
    }
}
