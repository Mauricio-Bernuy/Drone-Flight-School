using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    private int id_drone;
    private int id_map;
    void Start(){
        try
        {
            id_drone = GameManager.manager.GetDroneId();
            id_map = GameManager.manager.GetMapId();
        }
        catch
        {
            id_drone = 0;
            id_map = 0;
        }
    }
    public int GetDroneId() {
        return id_drone;
    }
    public int GetMapId() {
        return id_map;
    }
    public void MenuSetMapId(int _id) {
        id_map = _id;
    }
    public void MenuSetDroneId(int _id){
        id_drone = _id;
    }
    public void StartBtn(){
        GameManager.manager.SetDroneId(id_drone);
        GameManager.manager.SetMapId(id_map);
        SceneManager.LoadScene(id_map);
        GameManager.manager.SetOutOfLobby(true);

    }
    public void RestartBtn(){
        if (!GameManager.manager.GetOutOfLobby())
            return;
        GameManager.manager.SetDroneId(id_drone);
        GameManager.manager.SetMapId(id_map);
        SceneManager.LoadScene(id_map);
    }
    public void ReturnLobbyBtn(){
        if (!GameManager.manager.GetOutOfLobby())
            return;
        GameManager.manager.SetMapId(1);
        SceneManager.LoadScene(0);
        GameManager.manager.SetOutOfLobby(false);
    }
}
