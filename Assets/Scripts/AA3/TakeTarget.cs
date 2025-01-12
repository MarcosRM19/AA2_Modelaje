using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TakeTarget : MonoBehaviour
{
    public Transform[] claw;
    private bool startAnimation;
    public ShipController shipController;

    public Transform target;

    private ShipController.ShipStates changeState = ShipController.ShipStates.SEARCHINGDRONE;
    private bool secondTarget = false;

    void Start()
    {
        startAnimation = false;
    }


    void Update()
    {
        if (startAnimation)
        {
            if (Vector3.Distance(transform.position, target.transform.position) < 1.5f)
            {
                if(!secondTarget) 
                    target.SetParent(transform);
                else
                    transform.GetChild(0).SetParent(null);
                shipController.ChangeState(changeState);
            }
        }
    }

    public void SetStartAnimation(bool state)
    {
        startAnimation = state;
    }

    public void SetSatet(ShipController.ShipStates state)
    {
        changeState = state;
    }

    public void SetTarget(Transform _target)
    {
        secondTarget = true;
        target = _target;
    }
}
