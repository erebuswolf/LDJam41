using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBeam : MonoBehaviour {

    Material mat;

	void Start ()
    {
        mat = GetComponent<Renderer>().material;		
	}
	
	void Update ()
    {
        transform.localScale += 10.0f * Time.deltaTime * Vector3.one;
        mat.SetTextureOffset("_MainTex", new Vector2(0.3f * Time.deltaTime, -0.5f * Time.time));
	}
}
