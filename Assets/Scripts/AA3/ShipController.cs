using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public enum ShipStates { SEARCHINGDRONE, GRABDRONE, SEARCHINGHUMAN, DROPDRONE}

    public ShipStates currentState;
    public Transform[] targets;

    public Fabrik Fabrik;
    public TakeTarget takeTarget;

    private void Start()
    {
        currentState = ShipStates.SEARCHINGDRONE;
    }

    public void ChangeState(ShipStates newState)
    {
        switch (currentState)
        {
            case ShipStates.SEARCHINGDRONE:
                Fabrik.enabled = false;
                break;
            case ShipStates.GRABDRONE:
                takeTarget.SetStartAnimation(false);
                break;
            case ShipStates.SEARCHINGHUMAN:
                break;
            case ShipStates.DROPDRONE:
                break;
        }

        switch (newState) 
        {
            case ShipStates.SEARCHINGDRONE:
                Fabrik.enabled = true;
                break;
            case ShipStates.GRABDRONE:
                takeTarget.SetStartAnimation(true);
                break;
            case ShipStates.SEARCHINGHUMAN:
                Fabrik.enabled = true;
                Fabrik.SetTarget(targets[0]);
                break;
            case ShipStates.DROPDRONE:
                break;
        }

        currentState = newState;
    }


}