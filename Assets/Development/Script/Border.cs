using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Enemy":
                GameManager.instance.DecreaseHP();
                Destroy(other.gameObject, 2f);
                break;
            case "Item":
                Destroy(other.gameObject, 1f);
                break;
        }
    }

}
