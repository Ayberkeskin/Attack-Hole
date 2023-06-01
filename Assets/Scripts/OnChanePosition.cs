using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnChanePosition : MonoBehaviour
{
    public PolygonCollider2D hole2Dcolider;

    public PolygonCollider2D ground2DColider;

    public MeshCollider generatedMeshColider;

    public Collider GroundColider;

    public float initialScale = 0.5f;

    Mesh GeneratedMesh;

    private void Start()
    {
        GameObject[] AllGOs = FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (var go in AllGOs)
        {
            if (go.layer==LayerMask.NameToLayer("Obstacles"))
            {
                Physics.IgnoreCollision(go.GetComponent<Collider>(),generatedMeshColider,true);
            }
        }
    }

    private void FixedUpdate()
    {
        if (transform.hasChanged==true)
        {
            transform.hasChanged = false;
            hole2Dcolider.transform.position = new Vector2(transform.position.x,transform.position.z);
            hole2Dcolider.transform.localScale = transform.localScale * initialScale;
            MakeHole2D();
            Make3DMeshColider();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(other,GroundColider,true);
        Physics.IgnoreCollision(other, generatedMeshColider, false);
    }
    private void OnTriggerExit(Collider other)
    {
        Physics.IgnoreCollision(other, GroundColider, false);
        Physics.IgnoreCollision(other, generatedMeshColider, true);
    }

    private void MakeHole2D()
    {
        Vector2[] PointPosition = hole2Dcolider.GetPath(0);

        for (int i = 0; i < PointPosition.Length; i++)
        {
            PointPosition[i] = hole2Dcolider.transform.TransformPoint(PointPosition[i]);
        }
        ground2DColider.pathCount = 2;
        ground2DColider.SetPath(1, PointPosition);
    }
    private void Make3DMeshColider()
    {
        if (GeneratedMesh != null) Destroy(GeneratedMesh);
        GeneratedMesh = ground2DColider.CreateMesh(true,true);
        generatedMeshColider.sharedMesh = GeneratedMesh;
    }
}
