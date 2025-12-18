using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms.Impl;

public class MissonHandler : MonoBehaviour
{
    public static MissonHandler instance;
    public int currentMisson = 0;
    public int scoreInt;
    public float scoreFloat;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
