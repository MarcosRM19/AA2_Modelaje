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

    void Start()
    {
        startAnimation = false;
    }


    void Update()
    {
        if (startAnimation)
        {
            if (claw[0].eulerAngles.z <= 315f)
            {
                if(Vector3.Distance(transform.position, target.transform.position) < 1.5f)
                {
                    target.SetParent(transform);
                    shipController.ChangeState(ShipController.ShipStates.SEARCHINGHUMAN);
                    return;
                }
                shipController.ChangeState(ShipController.ShipStates.SEARCHINGDRONE);
                StartCoroutine(StopAnimation());
            }

            for (int i = 0; i < claw.Length; i++)
            {
                claw[i].Rotate(0, 0, -0.5f);
            }
        }

        if(!startAnimation) 
        {
            if (claw[0].eulerAngles.z >= 345f)
            {
                return;
            }

            for (int i = 0; i < claw.Length; i++)
            {
                claw[i].Rotate(0, 0, 0.5f);
            }
        }
    }

    IEnumerator StopAnimation()
    {
        yield return new WaitForSeconds(0.5f);

        startAnimation = false;
    }

    public void SetStartAnimation(bool state)
    {
        startAnimation = state;
    }
}
