using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        for (int j = 0; j < 10; j++)
        {
            for (int i = 0; i < 10000; i++)
            {
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                GameObject capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);

                cube.transform.position = new Vector3(0f, j * 2, i * 2);
                sphere.transform.position = new Vector3(-2f, j * 2, i * 2);
                capsule.transform.position = new Vector3(2f, j * 2, i * 2);
            }
        }

        Debug.Log("Load Time: " + Time.realtimeSinceStartup);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
