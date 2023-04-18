using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Scripts.Map;
using UnityEngine;

public class PlayerNavigation : MonoBehaviour
{
    
    [Header("Navigation")] 
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] public RoadIntersectionNode playerRoadNode;
    [SerializeField] private bool isNavigating, isOnNavigationCycle = false;
    [SerializeField] private float navigationUpdateCycleTime = 5f;
    
    [Header("Find Road Node")]
    [SerializeField] float radiusFindingRoadNode = 10f;
    [SerializeField] private LayerMask roadNodeLayer;
    private List<RoadIntersectionNode> _roadJourney;
    private Building _destinationBuilding;

    [Header("Icon")] [SerializeField] private GameObject _destinationIcon;
    
 

    // Update is called once per frame
    void Update()
    {
        if (isNavigating)
        {
            if(lineRenderer.positionCount > 1) lineRenderer.SetPosition(lineRenderer.positionCount-1, transform.position);
            StartCoroutine(CheckNavigation());
        }
    }
    
    public void EnableNavigation(Building destinationBuilding)
    {
        isNavigating = true; 
        isOnNavigationCycle = false;
        _destinationBuilding = destinationBuilding;
    }

    public void DisableNavigation()
    {
        isNavigating = false;
        StopCoroutine(CheckNavigation());
        VisualizeTrack(false);
    }


    private void VisualizeTrack(bool isActive)
    {
        if (!isActive)
        {
            lineRenderer.positionCount = 0;
            _destinationIcon.SetActive(false);
            return;
        }
        
        int count = _roadJourney.Count;
        lineRenderer.positionCount = count;
        var roadPositions = new Vector3[count];
        for (int i = 0 ; i< count ;i++)
        {
            roadPositions[i] = _roadJourney[i].transform.position;
        }
        
        lineRenderer.SetPositions(roadPositions);
        _destinationIcon.SetActive(true);
        _destinationIcon.transform.position = lineRenderer.GetPosition(0);
    }
    

    private void ReCheckingShortestPath()
    {
       _roadJourney =  MapManager.Instance.ShortestPathToDestinations(playerRoadNode, _destinationBuilding.entrances);
    }
    private void RemoveOldNearByNode()
    {
        foreach (var oldRoadNode in playerRoadNode.adjacentRoadNodes)
        {
            if(oldRoadNode == playerRoadNode) continue;

            Debug.Log("Delete "+ oldRoadNode.name);
            MapManager.Instance.RemoveAdjacentRoad(oldRoadNode, playerRoadNode);
            MapManager.Instance.RemoveAdjacentRoad(playerRoadNode, oldRoadNode);
            oldRoadNode.adjacentRoadNodes.Remove(playerRoadNode);
        }

        playerRoadNode.adjacentRoadNodes = new List<RoadIntersectionNode>();   
    }
    private void FindNearByRoadNode()
    {

        var hit2d= Physics2D.OverlapCircleAll(transform.position, radiusFindingRoadNode, roadNodeLayer);

        foreach (var hit in hit2d)
        {
           // Debug.Log("Hit " + hit.gameObject.name);
            var freshRoadNode = hit.gameObject.GetComponent<RoadIntersectionNode>();
            if(freshRoadNode == playerRoadNode) continue;
            
            freshRoadNode.adjacentRoadNodes.Add(playerRoadNode);
            playerRoadNode.adjacentRoadNodes.Add(freshRoadNode);
            
            MapManager.Instance.AddAdjacentRoad(freshRoadNode, playerRoadNode);
            MapManager.Instance.AddAdjacentRoad(playerRoadNode, freshRoadNode);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radiusFindingRoadNode);
    }

    private IEnumerator CheckNavigation()
    {
        if (!isNavigating) yield break;
        if (isOnNavigationCycle) yield break;

        isOnNavigationCycle = true;
        
        RemoveOldNearByNode();
        yield return null;
        FindNearByRoadNode();
        yield return null;
        ReCheckingShortestPath();
        VisualizeTrack(true);
        
        yield return new WaitForSeconds(navigationUpdateCycleTime);
        
        isOnNavigationCycle = false;
    }
}
