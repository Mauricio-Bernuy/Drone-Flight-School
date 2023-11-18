using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] drones;
    void Start()
    {
        int selectedDroneIndex = GameManager.manager.getId();
    
        Instantiate(drones[selectedDroneIndex], transform.position, Quaternion.identity);   
    }
}
