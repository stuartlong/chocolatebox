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
    private int[,][,] master;
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
        /*if (!generated){
            Debug.Log("Now I will generate shit!!");
            master = _level_generator.master;
            generated = true;

            float xunit = 2*_level_generator.groundBlock.sprite.bounds.extents.x;
            float yunit = 2*_level_generator.groundBlock.sprite.bounds.extents.y;

            Debug.Log("Units: " + xunit + ", " + yunit);


            Vector2 sectionDims = _level_generator.sectionSize;

            Debug.Log("Section Dims: " + sectionDims.x + ", " + sectionDims.y);

            float xPerSection = sectionDims.x * xunit;
            float yPerSection = sectionDims.y * yunit;

            Debug.Log("X Per Section = " + xPerSection);/
            Debug.Log("Y Per Section = " + yPerSection);

            int num_sections = _level_generator.NUMSECTIONS;
            int num_floors = _level_generator.floorCount;

            for (int x = 0; x < num_sections; x++) {
                for (int y = 0; y < num_floors; y++){
                    double nextX = xPerSection * x + 1;
                    double nextY = yPerSection * y + 1;
                    Debug.Log(nextX + ", " + nextY);
                    Instantiate(EnemyBlock, new Vector3((float) nextX, (float) nextY, 0), new Quaternion());
                }
            }
        }*/
	}
}
