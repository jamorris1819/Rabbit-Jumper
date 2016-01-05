using UnityEngine;
using System.Collections;

public class Platform : MonoBehaviour {

    public bool plain = false;

    public float CHANCE_COINS = 0.2f;
    public float CHANCE_PICKUP = 0.2f;
    public float CHANCE_SPRING = 0.2f;
    public float CHANCE_ENEMY = 0.2f;
    public float CHANCE_SPIKES = 0.2f;

    public GameObject coin;
    public GameObject pickup;
    public GameObject spring;
    public GameObject spikes;
    public GameObject enemy;
    public GameObject carrot;

    public Sprite[] sprites;
    public Sprite[] damagedSprites;
    GameObject player;
    BoxCollider2D boxCollider;

	void Start () {
        GetComponent<SpriteRenderer>().sprite = sprites[PlayerPrefs.GetInt("platformType", 0)];
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.Find("Player");
        if (plain) return;
        // What is on on each platform?
        if (Random.value < 0.7f)
        {
            // This is the scenery half. We might add something to this platform that is purely visual
            if (Random.value <= 0.5f)
            {
                GameObject carrotObject = (GameObject)Instantiate(carrot, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                carrotObject.transform.parent = transform;
            }
        }
        else
        {
            // This is the item half - we may add items on this platform
            float genVal = Random.value;
            if (genVal >= 0 && genVal < CHANCE_COINS)
            {
                // coins
                GameObject coinObject = (GameObject)Instantiate(coin, transform.position, Quaternion.identity);
                coinObject.transform.parent = transform;
                //coin.SetActive(true);
            }
            else if (genVal >= CHANCE_COINS && genVal < CHANCE_COINS + CHANCE_PICKUP)
            {
                // pickup
                GameObject pickupObject = (GameObject)Instantiate(pickup, transform.position, Quaternion.identity);
                pickupObject.transform.parent = transform;
            }
            else if (genVal >= CHANCE_COINS + CHANCE_PICKUP && genVal < CHANCE_COINS + CHANCE_PICKUP + CHANCE_SPRING)
            {
                // spring
                GameObject springObject = (GameObject)Instantiate(spring, transform.position, Quaternion.identity);
                springObject.transform.parent = transform;
            }
            else if (genVal >= CHANCE_COINS + CHANCE_PICKUP + CHANCE_SPRING && genVal < CHANCE_COINS + CHANCE_PICKUP + CHANCE_SPRING + CHANCE_ENEMY)
            {
                // enemy
                GameObject enemyObject = (GameObject)Instantiate(enemy, transform.position, Quaternion.identity);
                enemyObject.transform.parent = transform;
            }
            else if (genVal >= CHANCE_COINS + CHANCE_PICKUP + CHANCE_SPRING + CHANCE_ENEMY && genVal < CHANCE_COINS + CHANCE_PICKUP + CHANCE_SPRING + CHANCE_ENEMY + CHANCE_SPIKES)
            {
                // spikes
                GameObject spikesObject = (GameObject)Instantiate(spikes, transform.position, Quaternion.identity);
                spikesObject.transform.parent = transform;
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
