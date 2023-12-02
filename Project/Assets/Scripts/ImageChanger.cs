using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ImageChanger : MonoBehaviour
{
    public Image imageComponent;
    public Sprite[] images;
    private int currentImageIndex = 0;

    void Start()
    {
        imageComponent.sprite = images[currentImageIndex];
    }
    public int GetCurrentIndex(){
        return currentImageIndex;
    }

    public void NextImage()
    {
        currentImageIndex = (currentImageIndex + 1) % images.Length;

        imageComponent.sprite = images[currentImageIndex];
    }
    public void PreviousImage()
    {
        currentImageIndex = (currentImageIndex - 1 + images.Length) % images.Length;

        imageComponent.sprite = images[currentImageIndex];
    }
    public void ChangeDroneId(){
        MenuController menu = FindObjectOfType<MenuController>();
        menu.MenuSetDroneId(currentImageIndex);
    }
    public void ChangeMapId(){
        MenuController menu = FindObjectOfType<MenuController>();
        menu.MenuSetMapId(currentImageIndex+1);
    }
}
