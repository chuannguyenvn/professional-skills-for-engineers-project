using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;

public class PlayerGPS : MonoBehaviour
{
    [SerializeField] private float longitude, latitude;

    //[SerializeField] private TextMeshProUGUI debug;
    
    // Start is called before the first frame update
    void Start()
    {
       if(Input.location.isEnabledByUser)
            Input.location.Start();
       StartCoroutine(InitLocation());

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            longitude = Input.location.lastData.longitude;
            latitude = Input.location.lastData.latitude;

            transform.position = Map.MapUtilities.GeoToWorldPosition(new Vector2(longitude, latitude));
            Debug.Log("Player position "+ transform.position);
        }
        else
        {
            //debug.text = "Not Running";
        }
        
    }

    private IEnumerator InitLocation()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        // First, check if user has location service enabled
        if (!Input.location.isEnabledByUser)
            yield return new WaitForSeconds(10);

        // Start service before querying location
        Input.location.Start(1f,1f);

        // Wait until service initializes
        int maxWait = 10;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // Service didn't initialize in 20 seconds
        if (maxWait < 1)
        {
            //debug.text = "Timed out";
            print("Timed out");
            yield break;
        }

        // Connection has failed
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            //debug.text = "Unable to determine device location";
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            //debug.text = "Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude+100f + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp;
            // Access granted and location value could be retrieved
            print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
        }

        
        
    }
    
}
