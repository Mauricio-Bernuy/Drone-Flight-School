using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    private int id_drone = 0;
    void Awake(){
        if (manager == null){
            manager = this;
            DontDestroyOnLoad(this);
        } else if (manager != this){
            Destroy(gameObject);
        }
    }
    public int getId() {
        return id_drone;
    }
    public void setId(int _id) {
        id_drone = _id;
    }
    void Start()
    {
    
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
