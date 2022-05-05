using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroup
    {
        public Difficulty difficulty;

        GameObject target;
        public List<GameObject> enemies = new List<GameObject>();

        int numberOfEnemys;
        int PointAvailable;

        public int groupSightDist = 100;

        Vector3 position;
        public Bounds groupBounds;

        public void GenerateGroup(GameObject player)
        {
            difficulty = Difficulty.instance;
            PointAvailable = CalculateAvailableDifficultyPoints();
            position = FindAcceptablePositionForGroup(player);

            groupBounds = new Bounds(position, new Vector3(80, 10, 80));

            for (int i = 0; PointAvailable >= 100; i++)
            {
                int pointsConsumed = Enemy.SpawnRandomEnemy(this, position);
                difficulty.DifficultyPoints -= pointsConsumed;
                PointAvailable -= pointsConsumed;
            }
        }


        int CalculateAvailableDifficultyPoints()
        {
            bool enoughForGroup = (difficulty.DifficultyPoints >= difficulty.GroupMinDifficulty);
            if (!enoughForGroup)
            {
                return 0;
            }

            bool useMax = (difficulty.DifficultyPoints >= difficulty.GroupMaxDifficulty);
            int PointAvailable = 0;

            if (useMax)
            {
                PointAvailable = difficulty.GroupMaxDifficulty;
            }
            else
            {
                PointAvailable = difficulty.DifficultyPoints;
            }

            return MyMathFuctions.RoundNum(Random.Range(difficulty.GroupMinDifficulty, PointAvailable), 100);
        }


        static Vector3 FindAcceptablePositionForGroup(GameObject player)
        {
            float positionX = Random.Range(player.transform.position.x - (160 / 1), player.transform.position.x + (160 / 1));
            float positionZ = Random.Range(player.transform.position.z - (160 / 1), player.transform.position.z + (160 / 1));

            return new Vector3(positionX, 2, positionZ);
        }

    }