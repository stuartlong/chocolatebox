using UnityEngine;
using System.Collections;

public class BackgroundMusicController : MonoBehaviour {
    /// <summary>
    /// Where should the music start playing?
    /// </summary>
    public UnityEngine.Time startTime;

    /// <summary>
    /// Where should we begin each successive loop?
    /// </summary>
    public UnityEngine.Time beginLoopTime;

    /// <summary>
    /// Where should we stop and rebegin the loop?
    /// </summary>
    public Time endLoopTime;

    private Time currentTime;

    public AudioClip backgroundMusic;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
