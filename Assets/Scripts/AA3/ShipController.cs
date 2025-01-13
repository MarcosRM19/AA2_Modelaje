using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public enum ShipStates { SEARCHINGDRONE, GRABDRONE, SEARCHINGHUMAN, DROPDRONE, HUMANHASDRONE}

    public ShipStates currentState;
    public Transform[] targets;

    public Fabrik Fabrik;
    public TakeTarget[] takeTarget;

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
                break;
            case ShipStates.SEARCHINGHUMAN:
                Fabrik.SetFindTarget(false);
                break;
            case ShipStates.DROPDRONE:
                break;
            case ShipStates.HUMANHASDRONE:
                break;
        }

        switch (newState) 
        {
            case ShipStates.SEARCHINGDRONE:
                Fabrik.enabled = true;
                Fabrik.SetSatet(ShipStates.GRABDRONE);
                break;
            case ShipStates.GRABDRONE:
                takeTarget[0].SetStartAnimation(true);
                break;
            case ShipStates.SEARCHINGHUMAN:
                takeTarget[0].enabled = false;
                Fabrik.SetTarget(targets[0]);
                Fabrik.SetSatet(ShipStates.DROPDRONE);
                break;
            case ShipStates.DROPDRONE:
                takeTarget[1].SetStartAnimation(true);
                break;
            case ShipStates.HUMANHASDRONE:
                takeTarget[1].enabled = false;
                break;
        }

        currentState = newState;
    }


}