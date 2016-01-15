using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    public bool plain = false;

    public float CHANCE_COINS = 0.2f;
    public float CHANCE_PICKUP = 0.2f;
    public float CHANCE_SPRING = 0.2f;
    public float CHANCE_ENEMY = 0.2f;
    public float CHANCE_SPIKES = 0.2f;

    public static Pool coinPool;
    public static Pool powerupPool;
    public static Pool springPool;
    public static Pool spikesPool;
    public static Pool carrotPool;

    public Sprite[] sprites;
    public Sprite[] damagedSprites;
    GameObject player;
    BoxCollider2D boxCollider;
    LevelGeneration gen;

    void Start()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[PlayerPrefs.GetInt("platformType", 0)];
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player");
    }

	public void Begin () {
        gen = Camera.main.GetComponent<LevelGeneration>();
        foreach (Transform child in transform)
            Destroy(child.gameObject);
        if (plain) return;
        // This is the logic for creating each platform and furnishing it with items
        if (Random.value < 0.7f)
        {
            // This is the scenery half. We might add something to this platform that is purely visual
            if (Random.value <= 0.5f)
            {
                GameObject carrotObject = Platform.carrotPool.Place(transform.position + new Vector3(0, 0.5f, 0));
                gen.carrots.Add(carrotObject);
            }
        }
        else
        {
            // This is the item half - we may add items on this platform
            float genVal = Random.value;
            if (genVal >= 0 && genVal < CHANCE_COINS)
            {
                // coins
                GameObject coinObject = Platform.coinPool.Place(transform.position);
                gen.coins.Add(coinObject);
                //coinObject.transform.parent = transform;
                //coin.SetActive(true);
            }
            else if (genVal >= CHANCE_COINS && genVal < CHANCE_COINS + CHANCE_PICKUP)
            {
                // pickup
                GameObject powerupObject = Platform.powerupPool.Place(transform.position);
                gen.powerups.Add(powerupObject);
                //pickupObject.transform.parent = transform;
            }
            else if (genVal >= CHANCE_COINS + CHANCE_PICKUP && genVal < CHANCE_COINS + CHANCE_PICKUP + CHANCE_SPRING)
            {
                // spring
                GameObject springObject = Platform.springPool.Place(transform.position);
                gen.springs.Add(springObject);
                //springObject.transform.parent = transform;
            }
            else if (genVal >= CHANCE_COINS + CHANCE_PICKUP + CHANCE_SPRING && genVal < CHANCE_COINS + CHANCE_PICKUP + CHANCE_SPRING + CHANCE_ENEMY)
            {
                // enemy
            }
            else if (genVal >= CHANCE_COINS + CHANCE_PICKUP + CHANCE_SPRING + CHANCE_ENEMY && genVal < CHANCE_COINS + CHANCE_PICKUP + CHANCE_SPRING + CHANCE_ENEMY + CHANCE_SPIKES)
            {
                // spikes
                GameObject spikesObject = Platform.spikesPool.Place(transform.position);
                gen.spikes.Add(spikesObject);
                //spikesObject.transform.parent = transform;
            }
        }
    }

    IEnumerator Generate()
    {
        
        yield return true;
    }
	
	void FixedUpdate () {
        boxCollider.enabled = player.transform.position.y >= transform.position.y;
    }
}
