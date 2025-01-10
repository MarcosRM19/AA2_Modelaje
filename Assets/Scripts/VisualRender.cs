using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualRender : MonoBehaviour
{
    public Transform Joint0;
    public Transform Joint1;
    public Transform Joint2;
    public Transform Joint3;
    public Transform Joint4;
    public Transform Joint5;
    public Transform Joint6;
    public Transform Joint7;
    public Transform Joint8;
    public Transform endFactor;

    public LineRenderer lineRender1;
    public LineRenderer lineRender2;
    public LineRenderer lineRender3;
    public LineRenderer lineRender4;
    public LineRenderer lineRender5;
    public LineRenderer lineRender6;
    public LineRenderer lineRender7;
    public LineRenderer lineRender8;
    public LineRenderer lineRenderEnd;
    void Start()
    {
        InitializeLineRenderer(lineRender1);
        InitializeLineRenderer(lineRender2);
        InitializeLineRenderer(lineRender3);
        InitializeLineRenderer(lineRender4);
        InitializeLineRenderer(lineRender5);
        InitializeLineRenderer(lineRender6);
        InitializeLineRenderer(lineRender7);
        InitializeLineRenderer(lineRender8);
        InitializeLineRenderer(lineRenderEnd);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisualLinks();
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
}
