using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGeneration : MonoBehaviour {
    public GameObject platform;
    Dictionary<int, GameObject[]> platforms;
    int[] validPos = new int[] { -4, -2, 0, 2, 4 };
    int[] validPosOffset = new int[] { -3, -1, 1, 3 };

    void Start () {
        platforms = new Dictionary<int, GameObject[]>();
        platforms.Add(-8, null);
        for(int i = -4; i < 12; i++) {
            if (i % 4 == 0 && !platforms.ContainsKey(i))
            {
                List<int> tempLocations = new List<int>(); // A list of places we can place blocks this round
                if (i % 8 == 0)      // We want every alternate row to be offset
                    tempLocations.AddRange(validPosOffset);
                else
                    tempLocations.AddRange(validPos);
                float percent = Random.value;
                int number = 0;
                if (percent >= 0f && percent < 0.15f)
                    number = 1;
                else if (percent >= 0.15f && percent < 0.75f)
                    number = 2;
                else if (percent >= 0.75f)
                    number = 3;

                GameObject[] p = new GameObject[number];
                for (int j = 0; j < number; j++)
                {
                    int numToUse = Random.Range(0, tempLocations.Count);
                    p[j] = (GameObject)Instantiate(platform, new Vector3(tempLocations[numToUse], i), Quaternion.identity);
                    tempLocations.RemoveAt(numToUse);
                }

                platforms.Add(i, p);
            }
        }
    }
	
	void FixedUpdate () {
        int playerPosition = Mathf.FloorToInt(GameObject.Find("Player").transform.position.y) + 16;
        if (playerPosition % 4 == 0 && !platforms.ContainsKey(playerPosition))
        {
            List<int> tempLocations = new List<int>(); // A list of places we can place blocks this round
            if (playerPosition % 8 == 0)      // We want every alternate row to be offset
                tempLocations.AddRange(validPosOffset);
            else
                tempLocations.AddRange(validPos);
            float percent = Random.value;
            int number = 0;
            if (percent >= 0f && percent < 0.15f)
                number = 1;
            else if (percent >= 0.15f && percent < 0.75f)
                number = 2;
            else if (percent >= 0.75f)
                number = 3;

            GameObject[] p = new GameObject[number];
            for (int j = 0; j < number; j++)
            {
                int numToUse = Random.Range(0, tempLocations.Count);
                p[j] = (GameObject)Instantiate(platform, new Vector3(tempLocations[numToUse], playerPosition), Quaternion.identity);
                tempLocations.RemoveAt(numToUse);
            }

            platforms.Add(playerPosition, p);
        }

        playerPosition = Mathf.FloorToInt(GameObject.Find("Player").transform.position.y) - 16;

        if (playerPosition >= transform.position.y + 16)
            Destroy(gameObject);
    }

    IEnumerator Generate()
    {
        
        yield return null;
    }
}
