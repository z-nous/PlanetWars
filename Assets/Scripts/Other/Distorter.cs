using UnityEngine;
using System.Collections;

/*
 * 
 * 
 * 
 * 
 * 
 * 
 */

public class Distorter : MonoBehaviour {

    public float intensity = 0.5f;
    public float interval = 0f;
    public float multiplier = 1f;

    public bool isDistortionActie = false;
    public float distortTime = 1f;

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] originalNormals;
    private int[] originalTriangles;
    private float timer1 = 0f;
    private float timer2 = 0f;


    // Use this for initialization
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        originalVertices = mesh.vertices;
        originalNormals = mesh.normals;
        originalTriangles = mesh.triangles;
    }

    // Update is called once per frame
    void Update()
    {

        if (isDistortionActie == true)
        {
            timer2 += Time.deltaTime;
            distort();
            if (timer2 > distortTime)
            {
                isDistortionActie = false;
                timer2 = 0f;
                timer1 = 0f;
                mesh.Clear();
                mesh.vertices = originalVertices;
                mesh.triangles = originalTriangles;
                mesh.RecalculateNormals();
            }

        }

    }

    public void distort(float timeToDistort, float distortIntensity)
    {
        distortTime = timeToDistort;
        intensity = distortIntensity;
        isDistortionActie = true;
        //timer2 = 0f;
    }

    public void distort(float timeToDistort, float distortIntensity, float distortInterval)
    {
        distortTime = timeToDistort;
        intensity = distortIntensity;
        interval = distortInterval;
        isDistortionActie = true;
        //timer2 = 0f;
    }

    private void distort()
    {
        Vector3[] vertices = mesh.vertices;

        timer1 += Time.deltaTime;
        if (timer1 > interval)
        {
            timer1 = 0f;
            int i = 0;

            while (i < vertices.Length)
            {
                vertices[i] =multiplier * originalNormals[i] * Mathf.PerlinNoise(Random.Range(0f, intensity), Random.Range(0f, intensity));
                i++;

            }
            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = originalTriangles;
            mesh.RecalculateNormals();
        }
    }
}
