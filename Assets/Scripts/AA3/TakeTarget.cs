using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TakeTarget : MonoBehaviour
{
    public DroneMovement drone;
    public Fabrik fabrik;

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
                if (drone != null)
                {
                    drone.SetCanMove(false);
                    fabrik.Joints.Add(target);
                }
                else
                    fabrik.Joints.Remove(target);

                target.SetParent(transform);
                shipController.ChangeState(changeState);
            }
        }
    }

    public void SetStartAnimation(bool state)
    {
        startAnimation = state;
    }
}
