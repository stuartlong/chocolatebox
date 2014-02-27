/*
 * File: Enemy Generator
 * Author: James Fitzpatrick
 * Date: 2/26/2013
 *  
 */



using UnityEngine;
using System.Collections;

public class EnemyGenerator : MonoBehaviour {

	void Start () {
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
	
	}
}
