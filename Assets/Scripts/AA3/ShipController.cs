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
        ChangeState(ShipStates.SEARCHINGDRONE);
    }

    public void ChangeState(ShipStates newState)
    {
        switch (currentState)
        {
            case ShipStates.SEARCHINGDRONE:
                break;
            case ShipStates.GRABDRONE:
                takeTarget.SetStartAnimation(false);
                break;
            case ShipStates.SEARCHINGHUMAN:
                Fabrik.SetFindTarget(false);
                takeTarget.SetStartAnimation(true);
                break;
            case ShipStates.DROPDRONE:
                takeTarget.SetStartAnimation(false);
                break;
        }

        switch (newState) 
        {
            case ShipStates.SEARCHINGDRONE:
                Fabrik.enabled = true;
                Fabrik.SetSatet(ShipStates.GRABDRONE);
                break;
            case ShipStates.GRABDRONE:
                takeTarget.SetStartAnimation(true);
                takeTarget.SetSatet(ShipStates.SEARCHINGHUMAN);
                break;
            case ShipStates.SEARCHINGHUMAN:
                takeTarget.SetTarget(targets[0]);
                Fabrik.SetTarget(targets[0]);
                Fabrik.SetSatet(ShipStates.DROPDRONE);
                break;
            case ShipStates.DROPDRONE:
                break;
        }

        currentState = newState;
    }


}