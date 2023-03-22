using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    
    [Header("Navigation")] 
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] public RoadIntersectionNode playerRoadNode;
    [SerializeField] private bool isNavigating, isOnNavigationCycle = false;

    [SerializeField] private float navigationUpdateCycleTime = 5f;

    private List<RoadIntersectionNode> _roadJourney;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isNavigating)
        {
            StartCoroutine(CheckNavigation());
        }
    }
    
    public void EnableNavigation(List<RoadIntersectionNode> journey)
    {
        isNavigating = true; 
        isOnNavigationCycle = false;
        _roadJourney = journey;
        StartCoroutine(CheckNavigation());
    }

    public void DisableNavigation()
    {
        isNavigating = false;
        StopCoroutine(CheckNavigation());
    }


    private void Navigating()
    {
        int count = _roadJourney.Count;
        lineRenderer.positionCount = count;
        var roadPositions = new Vector3[count];
        for (int i = 0 ; i< count ;i++)
        {
            roadPositions[i] = _roadJourney[i].transform.position;
        }
        
        lineRenderer.SetPositions(roadPositions);
    }

    private IEnumerator CheckNavigation()
    {
        if (!isNavigating) yield break;
        if (isOnNavigationCycle) yield break;

        isOnNavigationCycle = true;
        Navigating();
        
        yield return new WaitForSeconds(navigationUpdateCycleTime);
        isOnNavigationCycle = false;
    }
}
