using UnityEngine;
using System.Collections;

public class EnemyAttachment : MonoBehaviour
{
    //A 0 x 0 requires any amount of space. This is a bad idea. 
    // Will likely default to some reasonable amount of space, such as 3x body size. 
    public Vector2 requiredSpace;

    public float probability;
    public float difficulty;

    [HideInInspector]
    public float summedProbability;

    public bool flying; 
    public bool stationary;
    public bool intelligent;

}
