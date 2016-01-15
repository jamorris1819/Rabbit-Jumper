using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneration : MonoBehaviour {
    public GameObject platform;
    public GameObject player;

    public GameObject coin;
    public GameObject powerup;
    public GameObject spring;
    public GameObject spike;
    public GameObject enemy;
    public GameObject carrot;

    public List<GameObject> coins;
    public List<GameObject> powerups;
    public List<GameObject> springs;
    public List<GameObject> spikes;
    public List<GameObject> carrots;

    public const float percentageBorderOne = 0.15f;                                 // These are 'borders' between 2 values
    public const float percentageBorderTwo = 0.5f;

    public const int clipAbove = 20;                                                // These are the modifiers so we can locate where to place
    public const int clipBelow = 20;                                                // and remove platforms

    Pool platformPool;                                                              // This will help us deal with reusing platforms once we don't need them
    public int platformPoolSize = Mathf.CeilToInt((clipAbove + clipBelow) * 0.75f); // Which will optimise our game and lower usage of the Garbage Collector
    // The maths here to calculate size is simple. Get the distance between these points by adding them together (imagine distance above + distanve below
    // It can only have 3 platforms max every 4 rows, so times by 3 and divide by 4 - or times by 0.75f. We can get the maximum possible number of platforms
    // available at any one time, which will stop errors in level generation.
    Dictionary<int, GameObject[]> platforms;

    int[] validPos = new int[] { -4, -2, 0, 2, 4 };
    int[] validPosOffset = new int[] { -3, -1, 1, 3 };

    bool start = false;

    void Start ()
    {
        platforms = new Dictionary<int, GameObject[]>();
        // We create the pool managers for each pickup
        Platform.coinPool = new Pool(coin, platformPoolSize);
        Platform.powerupPool = new Pool(powerup, platformPoolSize);
        Platform.springPool = new Pool(spring, platformPoolSize);
        Platform.spikesPool = new Pool(spike, platformPoolSize);
        Platform.carrotPool = new Pool(carrot, platformPoolSize);
        // These lists keep track of what is 'present' in game
        coins = new List<GameObject>(platformPoolSize);
        powerups = new List<GameObject>(platformPoolSize);
        springs = new List<GameObject>(platformPoolSize);
        spikes = new List<GameObject>(platformPoolSize);
        carrots = new List<GameObject>(platformPoolSize);
        platformPool = new Pool(platform, platformPoolSize);                        // We will limit ourselves to this pool
        for (int i = -1; i < 3; i++)                                               // Generate the start of the level
            Generate(i * 4);
        start = true;
    }
	
	void FixedUpdate () {
        if (!start)
            return;
        int playerPosition = Mathf.FloorToInt(player.transform.position.y);         // Position of the player to the int below
        Cleanse(playerPosition - clipBelow);                                        // Attempt to remove the platforms below it (out of view)
        Generate(playerPosition + clipAbove);                                       // Attempt to generate the platform 5 places above it (coming into view)
    }

    void Generate(int position)
    {
        if (position % 4 == 0 && !platforms.ContainsKey(position))                  // We only want platforms every 4th row, so we check if location is a multiple of 4
        {                                                                           // and that we don't already contain the platforms in our dictionary
            List<int> tempLocations = new List<int>();                              // A list of places we can place blocks this generation
            if (position % 8 == 0)                                                  // We want every alternate row to be offset to make it 'interesting'
                tempLocations.AddRange(validPosOffset);                             // We do this by checking that it divides by 8, hence getting every second generation
            else
                tempLocations.AddRange(validPos);
            float percent = Random.value;                                           // Here we're deciding how likely it is for each platform to be generated
            int number = 0;
            if (percent >= 0f && percent < percentageBorderOne)
                number = 1;
            else if (percent >= percentageBorderOne && percent < percentageBorderTwo)
                number = 2;
            else if (percent >= percentageBorderTwo)
                number = 3;
            GameObject[] platformArray = new GameObject[number];                    // We want to hold multiple platforms in our dictionary, so we put an array of
            for (int j = 0; j < number; j++)                                        // platforms into the dictionary
            {
                int numToUse = Random.Range(0, tempLocations.Count);                // We'll choose a random location from the location list
                platformArray[j] = platformPool.Place(tempLocations[numToUse],      // We grab an available platform from the pool, and have it placed for us
                    position);
                platformArray[j].GetComponent<Platform>().Begin();                  // Initiate the population of this platform, so it has new 'furnishings' and items
                tempLocations.RemoveAt(numToUse);                                   // We add it into the array and then remove it from the list so it isn't repeated
            }

            platforms.Add(position, platformArray);                                 // We add this new array into our dictionary
        }
    }

    void Cleanse(int position)
    {
        if (position % 4 == 0 && platforms.ContainsKey(position))                   // We can only cleanse if it's on the right y axis and has a log in the dictionary
        {
            foreach (GameObject go in platforms[position])                          
                platformPool.Remove(go);                                            // Remove each one amd return into the pool
            platforms.Remove(position);                                             // Delete the entry in the dictionary
        }
        // The first item will always be the oldest, and the oldest is closest to the bottom, so we deal with that
        if(coins.Count > 0)
            if (coins[0].transform.position.y <= position)
                coins.RemoveAt(0);
        if (powerups.Count > 0)
            if (powerups[0].transform.position.y <= position)
            powerups.RemoveAt(0);
        if (springs.Count > 0)
            if (springs[0].transform.position.y <= position)
            springs.RemoveAt(0);
        if (spikes.Count > 0)
            if (spikes[0].transform.position.y <= position)
            spikes.RemoveAt(0);
        if (carrots.Count > 0)
        {
            if (carrots[0].transform.position.y <= position)
            {
                Platform.carrotPool.Remove(carrots[0]);
                carrots.RemoveAt(0);
            }
        }
    }
}
