/*
 * File: Enemy Generator
 * Author: James Fitzpatrick
 * Date: 2/26/2013
 *  
 */



using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour
{

    private LevelGenerator _level_generator;

    public List<EnemyAttachment> Enemies = new List<EnemyAttachment>();

    public GameObject EnemyBlock;
    private int[,][,] master;
    private float xunitc;
    private float yunitc;
    
    /*
    void Start()
    {
        _level_generator = (LevelGenerator)this.gameObject.GetComponent("LevelGenerator");
        master = _level_generator.master;

        //Converting from tiles to coordinates
        xunitc = 2 * _level_generator.groundBlock.sprite.bounds.extents.x;
        yunitc = 2 * _level_generator.groundBlock.sprite.bounds.extents.y;


        //0,0 is the bottom left tile of a section/level
    */
        /*
         * Algorithm:
         * 
         * Set a current point to be the furthest left ground space. 
         * Check the column that is the shortest possible required space away. 
         *  Until end of section
         *      If Ground exists in the column
         *        Binary search for the required space between the current check and right marker
         *        If all columns contain required horizontal space
         *          generate minion  
         *      Else
         *          Probe right until end of section or ground space occurs
         */
    /*
        int minHDistance = (int)FindMinimumHorizontalDistance();

        int x = minHDistance;
        int y = 0;
        int lastValidColumn = 0;
        

        foreach (int[,] section in master)
        {
            while (x < section.Length) 
            {
                if ((int) LevelGenerator.AssetTypeKey.GroundBlock == section[x,y])
                {
                    if (ValidSection(section, lastValidColumn, x, y))
                    {
                        //Got there!
                        MakeEnemy(section, x, y);
                        lastValidColumn += minHDistance/2;
                        x = lastValidColumn + minHDistance;
                    }
                    else //Well, we got close guys... oh well...
                    {
                        lastValidColumn = x;
                        x += minHDistance;
                    }
                }
                else
                {
                    if (y != 0) //Maybe the ground is just lower in this column.
                    {
                        y--;
                    }
                    else //We are at rock bottom. try a new column. 
                    {
                        //Find next column height
                        while (x < section.Length)
                        {
                            if ((int) LevelGenerator.AssetTypeKey.GroundBlock == section[x, y])
                            {
                                lastValidColumn = x;
                                x += minHDistance;
                                break;
                            }
                            else
                            {
                                x++;
                            }
                        }
                    }
                }
            }
            lastValidColumn = 0;
            x = minHDistance;
            y = 0;
        }

    }

    private float FindMinimumHorizontalDistance()
    {
        List<float> distances = new List<float>();
        foreach (EnemyAttachment enemy in Enemies)
        {
            distances.Add(enemy.requiredSpace.x);
        }
        distances.Sort();
        return distances[0];
    }

    private bool ValidSection(int[,] section, int startx, int endx, int maxy)
    {
        int y = maxy;
        while (y >= 0)
        {
            if ((int) LevelGenerator.AssetTypeKey.None == section[(endx + startx) / 2, maxy])
            {
                return true;
            }
        }

        return false;
    }

    private void MakeEnemy(int[,] section, int x, int y)
    {
        int newY = NextAvailableY(section, x,y);
        float convertedY = newY * yunitc;
        convertedY += Enemies[0].gameObject.renderer.bounds.extents.y;

        //Should have a better random algorithm. Generation for now is ok. 
        Instantiate(Enemies[0].gameObject, new Vector3(xunitc * x, yunitc * newY, 0), Quaternion.identity);

        Debug.Log("GOT THERE!!!");

    }

    private int NextAvailableY(int[,] section, int x, int y)
    {
        if ((int) LevelGenerator.AssetTypeKey.None == section[x, y]){
            return y;
        }

        Debug.Log("Unable to find available space");
        return -1;
    }

    */
}