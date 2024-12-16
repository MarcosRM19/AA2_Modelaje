using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private List<Transform> nodeList;
    public GameObject pathfindingNodes;
    private Transform currentNode;
    private int i;

    public float maxSpeed;
    public float proximityThreshold;

    private Vector3 velocity;
    private float distanceToTarget;
    private float deltaTime;

    private void Awake()
    {
        nodeList = new List<Transform>();
        int childCount = pathfindingNodes.transform.childCount;
        for (int i = 0; i < childCount; i++) 
        { 
            Transform node = pathfindingNodes.transform.GetChild(i);
            nodeList.Add(node);
        }

        velocity = Vector3.zero;
        transform.position = nodeList[0].position;
        i = 1;
        currentNode = nodeList[i];
    }

    private void Update()
    {
        deltaTime = Time.deltaTime;

        Seek();

        IsNearTarget();
    }

    private void Seek()
    {
        Vector3 desiredVelocity = (currentNode.position - transform.position).normalized * maxSpeed;

        Vector3 steering = desiredVelocity - velocity;

        velocity += steering * deltaTime;
        transform.position += velocity * deltaTime;

        if (velocity.sqrMagnitude > 0.01f)
        {
            transform.forward = velocity.normalized;
        }
    }

    private void IsNearTarget()
    {
        distanceToTarget = Vector3.Distance(transform.position, currentNode.position);
        if (distanceToTarget <= proximityThreshold)
        {
            i++;

            if (i >= nodeList.Count)
                i = 0;

            currentNode = nodeList[i];
            velocity *= .8f;
        }
    }
}
