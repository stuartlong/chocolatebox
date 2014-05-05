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
    private Section[,] master;
    private float xunitc;
    private float yunitc;

    void Start()
    {
        _level_generator = (LevelGenerator)this.gameObject.GetComponent("LevelGenerator");
        master = _level_generator.master;

        //Converting from tiles to coordinates
        xunitc = 2 * _level_generator.GetBaseBlock().sprite.bounds.extents.x;
		yunitc = 2 * _level_generator.GetBaseBlock().sprite.bounds.extents.y;



        //0,0 is the bottom left tile of a section/level

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

        int minHorizontalDistance = (int)FindMinimumHorizontalDistance();

        int x = 3*minHorizontalDistance;
        int y = 0;
        int lastValidColumn = 2*minHorizontalDistance;
        
        for (int i = 0; i < _level_generator.sectionsX; i++)
        {
            for (int j = 0; j < _level_generator.sectionsY; j++)
            {
                //Debug.Log("Section: i = " + i + ", j = " + j);
                Section section = master[i, j];

                
                while (x < section.getWidth())
                {
                    if ((int)LevelGenerator.AssetTypeKey.UndergroundBlock == section.get(x, y) ||
                        (int)LevelGenerator.AssetTypeKey.TopGroundBlock == section.get(x, y)
                        
                        
                        )
                    {
                        if (ValidSection(section, lastValidColumn, x, y))
                        {
                            //Got there!
                            MakeEnemy(section, x, y);
                            lastValidColumn += minHorizontalDistance / 2;
                            x = lastValidColumn + minHorizontalDistance;
                        }
                        else //Well, we got close guys... oh well...
                        {
                            lastValidColumn = x;
                            x += minHorizontalDistance;
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
                            while (x < section.getWidth())
                            {
                                if ((int)LevelGenerator.AssetTypeKey.UndergroundBlock == section.get(x, y))
                                {
                                    lastValidColumn = x;
                                    x += minHorizontalDistance;
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
                x = minHorizontalDistance;
                y = 0;
            }
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

    private bool ValidSection(Section section, int startx, int endx, int maxy)
    {
        int checkx = (startx + endx) / 2; ; 
        int y = maxy;
        while (y >= 0 && Mathf.Abs(endx - checkx) > 1)
        {
            if ((int) LevelGenerator.AssetTypeKey.UndergroundBlock == section.get(checkx, y))
            {
                checkx = (endx + checkx) / 2;
            }
            else
            {
                y--;
                if (y < 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    private void MakeEnemy(Section section, int x, int y)
    {
        int newY = NextAvailableY(section, x,y);
        if (newY <= 0)
        {
            return;
        }
        float convertedY = newY * yunitc;

        convertedY += Enemies[0].gameObject.renderer.bounds.extents.y;

        //Should have a better random algorithm. Generation for now is ok. 
        Instantiate(Enemies[0].gameObject, new Vector3(xunitc * x, yunitc * newY, 0), Quaternion.identity);

    }

    private int NextAvailableY(Section section, int x, int y)
    {
        int tempy = y + 1;
        while (tempy < section.getHeight())
        {
            if ((int)LevelGenerator.AssetTypeKey.None == section.get(x, y))
            {
                return y+1;
            }
            else
            {
                y++;
            }
        }
        

        Debug.Log("Unable to find available space");
        return -1;
    }

 
}