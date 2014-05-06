using UnityEngine;
using System.Collections;

public class BusterExplosion : MonoBehaviour
{
    [HideInInspector]
    /// <summary>
    /// Time between shots in seconds
    /// </summary>

    System.TimeSpan ttl;
    public float duration = 0.05f;
    private int nanotosecondsConstant = 100000000;
    private System.DateTime startTime;
    
    void Start()
    {
        ttl = new System.TimeSpan((long) duration * nanotosecondsConstant);
        startTime = System.DateTime.Now;
    }

    // Update is called once per frame
    void Update()
    {
        System.DateTime curTime = System.DateTime.Now;

        if (curTime - startTime > ttl)
        {
            Destroy(this.gameObject);
            Destroy(this);
        }
    }
}
