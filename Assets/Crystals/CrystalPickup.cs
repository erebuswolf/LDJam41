using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalPickup : MonoBehaviour
{

    [SerializeField]
    private int Value;

    [SerializeField]
    private float respawnTimeInSeconds = 30.0f;

    private bool childIsInactive = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // If other is the player have them pick up the crystal.
        PlayerData data = other.GetComponent<PlayerData>();
        if ((data != null) && !childIsInactive)
        {
            data.AddResources(Value);
            StartCoroutine(RespawnCrystal());
        }
    }

    IEnumerator RespawnCrystal()
    {
        Transform trans = gameObject.transform;
        if (trans.childCount > 0)
        {
            Transform child = trans.GetChild(0);
            child.gameObject.SetActive(false);
            childIsInactive = true;

            yield return new WaitForSeconds(respawnTimeInSeconds);

            child.gameObject.SetActive(true);
            childIsInactive = false;
        }
    }

}
