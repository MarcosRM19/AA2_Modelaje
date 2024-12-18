using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Gradient : MonoBehaviour
{
    public Transform Joint0;
    public Transform Joint1;
    public Transform Joint2;
    public Transform Joint3;  
    public Transform Joint4; 
    public Transform Joint5; 
    public Transform endFactor;

    public LineRenderer lineRender1;
    public LineRenderer lineRender2;
    public LineRenderer lineRender3;
    public LineRenderer lineRender4;
    public LineRenderer lineRender5;
    public LineRenderer lineRenderEnd;

    public Transform target;

    private float costFunction;

    private Vector3 D1;
    private Vector3 D2;
    private Vector3 D3;
    private Vector3 D4; 
    private Vector3 D5; 
    private Vector3 D6; 

    public float alpha;
    public float initAlpha;

    private Vector6 theta; 

    public float tolerance = 1f;

    private Vector6 gradient;

    private bool canMove;
    private Vector3[] initPosition;
    private float resetProgress = 0f;
    public float resetDuration = 1f;

    public ArmController armController;
    public bool firtsArm;

    void Start()
    {
        D1 = Joint1.position - Joint0.position;
        D2 = Joint2.position - Joint1.position;
        D3 = Joint3.position - Joint2.position; 
        D4 = Joint4.position - Joint3.position; 
        D5 = Joint5.position - Joint4.position; 
        D6 = endFactor.position - Joint5.position;

        theta = Vector6.zero;

        costFunction = Vector3.Distance(endFactor.position, target.position) * Vector3.Distance(endFactor.position, target.position);

        InitializeLineRenderer(lineRender1);
        InitializeLineRenderer(lineRender2);
        InitializeLineRenderer(lineRender3);
        InitializeLineRenderer(lineRender4);
        InitializeLineRenderer(lineRender5);
        InitializeLineRenderer(lineRenderEnd);

        initPosition = new Vector3[6];
        initPosition[0] = Joint1.localPosition;
        initPosition[1] = Joint2.localPosition;
        initPosition[2] = Joint3.localPosition;
        initPosition[3] = Joint4.localPosition;
        initPosition[4] = Joint5.localPosition;
        initPosition[5] = endFactor.localPosition;

        initAlpha = alpha;
    }

    void Update()
    {
        if (canMove)
        {
            if (costFunction > tolerance)
            {
                gradient = GetGradient(theta);
                theta -= alpha * gradient;
                Vector3[] newPosition = endFactorFunction(theta);

                float distanceToTarget = Vector3.Distance(armController.transform.position, target.position);
                float distanceFactor = Mathf.Clamp01(distanceToTarget / armController.GetDistance());

                Joint1.position = Vector3.Lerp(newPosition[0], Joint0.position, 1 - distanceFactor);
                Joint2.position = Vector3.Lerp(newPosition[1], Joint1.position, 1 - distanceFactor);
                Joint3.position = Vector3.Lerp(newPosition[2], Joint2.position, 1 - distanceFactor);
                Joint4.position = Vector3.Lerp(newPosition[3], Joint3.position, 1 - distanceFactor);
                Joint5.position = Vector3.Lerp(newPosition[4], Joint4.position, 1 - distanceFactor);
                endFactor.position = Vector3.Lerp(newPosition[5], Joint5.position, 1 - distanceFactor);
            }

            costFunction = lossCostFunction(theta);
        }
        else
        {
            ResetToInitialPositions();
        }
        UpdateVisualLinks();
    }

    Vector3[] endFactorFunction(Vector6 theta)
    {
        Quaternion baseRotation = Joint0.rotation;

        Quaternion[] q = new Quaternion[6];
        q[0] = baseRotation * Quaternion.AngleAxis(theta.x, Vector3.up);
        q[1] = Quaternion.AngleAxis(theta.y, Vector3.forward);
        q[2] = Quaternion.AngleAxis(theta.z, Vector3.up);
        q[3] = Quaternion.AngleAxis(theta.w, Vector3.forward);
        q[4] = Quaternion.AngleAxis(theta.v, Vector3.up);
        q[5] = Quaternion.AngleAxis(theta.u, Vector3.forward);

        Vector3 j1 = Joint0.position + q[0] * q[1] * D1;
        Vector3 j2 = j1 + q[0] * q[1] * q[2] * D2;
        Vector3 j3 = j2 + q[0] * q[1] * q[2] * q[3] * D3; 
        Vector3 j4 = j3 + q[0] * q[1] * q[2] * q[3] * q[4] * D4; 
        Vector3 j5 = j4 + q[0] * q[1] * q[2] * q[3] * q[4] * q[5] * D5;
        Vector3 endfactor = j5 + q[0] * q[1] * q[2] * q[3] * q[4] * q[5] * D6;

        Vector3[] result = new Vector3[6];

        result[0] = j1;
        result[1] = j2;
        result[2] = j3; 
        result[3] = j4; 
        result[4] = j5; 
        result[5] = endfactor;

        return result;
    }

    float lossCostFunction(Vector6 theta)
    {
        Vector3 endpostion = endFactorFunction(theta)[5];
        return Vector3.Distance(endpostion, target.position) * Vector3.Distance(endpostion, target.position);
    }

    Vector6 GetGradient(Vector6 theta)
    {
        Vector6 gradientVector;
        float step = 1e-2f;

        // x
        Vector6 thetaPlus = theta;
        thetaPlus.x = theta.x + step;
        gradientVector.x = (lossCostFunction(thetaPlus) - lossCostFunction(theta)) / step;

        // y
        thetaPlus = theta;
        thetaPlus.y = theta.y + step;
        gradientVector.y = (lossCostFunction(thetaPlus) - lossCostFunction(theta)) / step;

        // z
        thetaPlus = theta;
        thetaPlus.z = theta.z + step;
        gradientVector.z = (lossCostFunction(thetaPlus) - lossCostFunction(theta)) / step;

        // w
        thetaPlus = theta;
        thetaPlus.w = theta.w + step;
        gradientVector.w = (lossCostFunction(thetaPlus) - lossCostFunction(theta)) / step;

        // v
        thetaPlus = theta;
        thetaPlus.v = theta.v + step;
        gradientVector.v = (lossCostFunction(thetaPlus) - lossCostFunction(theta)) / step;

        // u
        thetaPlus = theta;
        thetaPlus.u = theta.u + step;
        gradientVector.u = (lossCostFunction(thetaPlus) - lossCostFunction(theta)) / step;

        gradientVector.Normalize();
        return gradientVector;
    }

    void InitializeLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.white;
        lineRenderer.endColor = Color.white;
    }

    void UpdateVisualLinks()
    {
        lineRender1.SetPosition(0, Joint0.position);
        lineRender1.SetPosition(1, Joint1.position);

        lineRender2.SetPosition(0, Joint1.position);
        lineRender2.SetPosition(1, Joint2.position);

        lineRender3.SetPosition(0, Joint2.position);
        lineRender3.SetPosition(1, Joint3.position);

        lineRender4.SetPosition(0, Joint3.position);
        lineRender4.SetPosition(1, Joint4.position);

        lineRender5.SetPosition(0, Joint4.position);
        lineRender5.SetPosition(1, Joint5.position);

        lineRenderEnd.SetPosition(0, Joint5.position);
        lineRenderEnd.SetPosition(1, endFactor.position);
    }

    private void ResetToInitialPositions()
    {
        resetProgress += Time.deltaTime / resetDuration;

        Joint1.localPosition = Vector3.Lerp(Joint1.localPosition, initPosition[0], resetProgress);
        Joint2.localPosition = Vector3.Lerp(Joint2.localPosition, initPosition[1], resetProgress);
        Joint3.localPosition = Vector3.Lerp(Joint3.localPosition, initPosition[2], resetProgress);
        Joint4.localPosition = Vector3.Lerp(Joint4.localPosition, initPosition[3], resetProgress);
        Joint5.localPosition = Vector3.Lerp(Joint5.localPosition, initPosition[4], resetProgress);
        endFactor.localPosition = Vector3.Lerp(endFactor.localPosition, initPosition[5], resetProgress);

        if (resetProgress >= 1f)
        {
            resetProgress = 0f;
        }
    }
    public void SetCanMove(bool state)
    {
        canMove = state;
        if(state == false)
        {
            StartReset();
        }
    }

    public void StartReset()
    {
        theta = Vector6.zero;
        alpha = initAlpha;
        resetProgress = 0f;
    }
}

public struct Vector6
{
    public float x, y, z, w, v, u;

    public static Vector6 zero => new Vector6 { x = 0, y = 0, z = 0, w = 0, v = 0, u = 0 };

    public static Vector6 operator -(Vector6 a, Vector6 b)
    {
        return new Vector6 { x = a.x - b.x, y = a.y - b.y, z = a.z - b.z, w = a.w - b.w, v = a.v - b.v, u = a.u - b.u };
    }

    public static Vector6 operator *(float d, Vector6 a)
    {
        return new Vector6 { x = d * a.x, y = d * a.y, z = d * a.z, w = d * a.w, v = d * a.v, u = d * a.u };
    }

    public void Normalize()
    {
        float magnitude = Mathf.Sqrt(x * x + y * y + z * z + w * w + v * v + u * u);
        if (magnitude > 1e-5f)
        {
            x /= magnitude;
            y /= magnitude;
            z /= magnitude;
            w /= magnitude;
            v /= magnitude;
            u /= magnitude;
        }
    }
}
