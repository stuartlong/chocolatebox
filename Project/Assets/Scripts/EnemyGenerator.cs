/*
 * File: Enemy Generator
 * Author: James Fitzpatrick
 * Date: 2/26/2013
 *  
 */



using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {

    private LevelGenerator _level_generator;
    private int _emptysquare = 0;
    private bool generated = false;

    public GameObject EnemyBlock;

	void Start () {
        _level_generator = (LevelGenerator) this.GetComponent("LevelGenerator");
		// Launch the generator. Runs at load time in Unity
		CalculateQuantities ();
		GenerateEnemies ();
	}

	void CalculateQuantities(){
		// Calculate how many enemies we want to include in each section. 
		// Use arrays to store each enemy prefab and how often these prefabs occur
		// The user will provide a desired (average) amount of units per level for this level. 
		// Use a random seed (As a percentage of the value of the 
	}

	void GenerateEnemies() {
		// Function to eventually generate all of the enemies

		// Starting from lower left to upper right, we will place enemies on the map
		// Ground units will be placed first as they have real estate issues
		// Aerial Units generally have more flexibility

		// Units that are calculated to have more units placed in a section will
		// be placed more in the following sections and will be placed before others

	}
	
	void Update () {
        if (!generated)
        {
            Debug.Log("Now I will generate shit!!");
            generated = true;

            foreach (int[,] section in _level_generator.master)
            {
                int i = 5;
                int j = 5;
               

                float centerX = _level_generator.groundBlock.sprite.bounds.extents.x * 2 * i + (
                                _level_generator.groundBlock.sprite.bounds.extents.y * 2 * section.GetLength(0) * _level_generator.master.GetLength(1));
                float centerY = _level_generator.groundBlock.sprite.bounds.extents.y * 2 * j + (
                    _level_generator.groundBlock.sprite.bounds.extents.y * 2 * section.GetLength(1) * _level_generator.master.GetLength(0));
                if (section[i, j] == _emptysquare)
                {
                    Instantiate(EnemyBlock, new Vector3(centerX, centerY, 0), new Quaternion());
                }
            }
        }
	}
}
