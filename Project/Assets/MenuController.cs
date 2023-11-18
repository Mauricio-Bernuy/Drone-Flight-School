using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartBtn(){
        SceneManager.LoadScene("Scenes/SampleScene");
    }
    public void EditBtn(){
        GameManager.manager.setId(1);
    }
}
