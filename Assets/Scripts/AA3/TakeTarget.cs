using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TakeTarget : MonoBehaviour
{
    public DroneMovement drone;

    private bool startAnimation;
    public ShipController shipController;

    public Transform target;
    public float distance;

    public ShipController.ShipStates changeState;

    void Start()
    {
        startAnimation = false;
    }


    void Update()
    {
        if (startAnimation)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < distance)
            {
                target.SetParent(transform);
                shipController.ChangeState(changeState);
                if (drone != null)
                    drone.SetCanMove(false);
            }
        }
    }

    public void SetStartAnimation(bool state)
    {
        startAnimation = state;
    }
}
