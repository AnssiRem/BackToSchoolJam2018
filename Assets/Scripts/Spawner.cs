using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public int LootAmount;
    public int ObstacleAmount;
    public GameObject Loot;
    public GameObject Obstacle;

    private Vector2[] usedSpawns;

    private void Start()
    {
        usedSpawns = new Vector2[(LootAmount + ObstacleAmount + 9)];
        usedSpawns[0] = new Vector2(0, 0);
        usedSpawns[1] = new Vector2(2, 0);
        usedSpawns[2] = new Vector2(-2, 0);
        usedSpawns[3] = new Vector2(0, 2);
        usedSpawns[4] = new Vector2(0, -2);
        usedSpawns[5] = new Vector2(2, 2);
        usedSpawns[6] = new Vector2(-2, 2);
        usedSpawns[7] = new Vector2(2, -2);
        usedSpawns[8] = new Vector2(-2, -2);

        SpawnLoot(LootAmount);
        SpawnObstacles(ObstacleAmount);
    }

    private void SpawnLoot(int amount)
    {
        bool canSpawn = false;

        int x;
        int y;

        int xIsNegative;
        int yIsNegative;

        for (int i = 0; i < amount;)
        {
            xIsNegative = Random.Range(0, 2) - 1;
            yIsNegative = Random.Range(0, 2) - 1;
            x = 2 * (int)Mathf.Pow(-1, xIsNegative) * (Random.Range(0, 11) - 1);
            y = 2 * (int)Mathf.Pow(-1, yIsNegative) * (Random.Range(0, 11) - 1);

            for (int j = 0; j < usedSpawns.Length; ++j)
            {
                if (usedSpawns[j] == new Vector2(x, y))
                {
                    canSpawn = false;
                    break;
                }
                else
                {
                    canSpawn = true;
                }
            }

            if (canSpawn)
            {
                usedSpawns[9 + i] = new Vector2(x, y);
                Instantiate(Loot, new Vector3(x, 0, y), Quaternion.Euler(0, 0, 90));
                ++i;
            }
        }
    }

    private void SpawnObstacles(int amount)
    {
        bool canSpawn = false;

        int x;
        int y;

        int xIsNegative;
        int yIsNegative;

        for (int i = 0; i < amount;)
        {
            xIsNegative = Random.Range(0, 2) - 1;
            yIsNegative = Random.Range(0, 2) - 1;
            x = 2 * (int)Mathf.Pow(-1, xIsNegative) * (Random.Range(0, 11) - 1);
            y = 2 * (int)Mathf.Pow(-1, yIsNegative) * (Random.Range(0, 11) - 1);

            for (int j = 0; j < usedSpawns.Length; ++j)
            {
                if (usedSpawns[j] == new Vector2(x, y))
                {
                    canSpawn = false;
                    break;
                }
                else
                {
                    canSpawn = true;
                }
            }

            if (canSpawn)
            {
                usedSpawns[9 + LootAmount + i] = new Vector2(x, y);
                Instantiate(Obstacle, new Vector3(x, 0, y), transform.rotation);
                ++i;
            }
        }
    }
}
