using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    // Start is called before the first frame update
    private int id_drone = 0;
    private int id_map = 1;
    void Awake(){
        if (manager == null){
            manager = this;
            DontDestroyOnLoad(this);
        } else if (manager != this){
            Destroy(gameObject);
        }
    }
    public int GetDroneId() {
        return id_drone;
    }
    public int GetMapId() {
        return id_map;
    }
    public void SetMapId(int _id) {
        id_map = _id;
    }
    public void SetDroneId(int _id){
        id_drone = _id;
    }
}
