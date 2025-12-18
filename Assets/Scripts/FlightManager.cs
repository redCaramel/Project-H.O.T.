using UnityEngine;

public class FlightManager : MonoBehaviour
{
    public static FlightManager instance;
    public bool isflighting = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void stopFlight()
    {
        isflighting = false;
    }
}
