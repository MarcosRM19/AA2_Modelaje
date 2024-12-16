using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmController : MonoBehaviour
{
    public enum state { OUTDISTANCE, MOVEFIRSTARM, MOVESECONDARM}
    public state currentState;

    public Transform target;
    public float distance;

    public Gradient[] arms;
    private void Update()
    {
        switch(currentState)
        {
            case state.OUTDISTANCE:
                SetGradientMovement(false);
                break;
            case state.MOVEFIRSTARM:
                arms[1].SetCanMove(false);
                arms[0].SetCanMove(true);
                break;
            case state.MOVESECONDARM:
                arms[0].SetCanMove(false);
                arms[1].SetCanMove(true);
                break;
        }

        DetectDistances();
    }

    private void DetectDistances()
    {
        if (Vector3.Distance(transform.position, target.position) < distance)
        {
            currentState = state.MOVEFIRSTARM;
        }
        else
        {
            currentState = state.OUTDISTANCE;
        }
    }

    private void SetGradientMovement(bool state)
    {
        for (int i = 0; i < arms.Length; i++)
        {
            arms[i].SetCanMove(state);
        }
    }

    public void ChangeArmMovement(bool _state)
    {
        if(_state)
        {
            currentState = state.MOVESECONDARM;
        }
        else
        {
            currentState = state.MOVEFIRSTARM;
        }
    }
}
