using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class TakeSpiderman : MonoBehaviour
{
    public Transform[] claw;
    private bool startAnimation;
    private bool animationDoing;
    public ArmController armController;

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
                    target.SetParent(this.transform);
                    armController.currentState = ArmController.state.GRABBED;
                    target.GetComponent<PathFinding>().canMove = false;
                }
                StartCoroutine(StopAnimation());
                return;
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
                animationDoing = false;
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

    public void SetStartAnimation()
    {
        startAnimation = true;
        animationDoing = true;
    }
}
