using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReactorHealth : MonoBehaviour {
    [SerializeField] private Image HealthImage;
    private Reactor reactor;

    private float startHeight;

    // Use this for initialization
    void Start () {
        reactor = FindObjectOfType<Reactor>();
        startHeight = HealthImage.rectTransform.sizeDelta.y;
    }
	
	// Update is called once per frame
	void Update () {
        HealthImage.rectTransform.sizeDelta = new Vector2(
            HealthImage.rectTransform.sizeDelta.x,
            reactor.GetHealthRatio() * startHeight
            );
    }
}
