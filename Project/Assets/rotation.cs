using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rotation : MonoBehaviour
{
    public float speed = 5f;

    public ConstantForce cForce;
    public Vector3 forcedir;
    public float factorDeFrenado = 0.95f; // Ajusta el factor de frenado según tus necesidades
    private Rigidbody rb;
    public int restauration_speed = 1000;
    public float fuerzaDeFrenado = 10.0f; // Ajusta la fuerza de frenado según tus necesidades
    public float fuerzadetorque = 10f;
    public float precision = 0.01f;
    public float aceleracion = 1.0f;
    public float frenado = 10.0f;

    void Start(){
        cForce = GetComponent<ConstantForce>();
        forcedir = new Vector3(0, -10,0);
        cForce.force = forcedir;
        rb = GetComponent<Rigidbody>();
            
    }
    private void Update()
    {
     
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(-Vector3.up * speed*4  * Time.deltaTime);
            
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up * speed *4* Time.deltaTime);
        }



    }


    

    
}




// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class MoveObject : MonoBehaviour
// {
//     public float speed = 5f;
//     public ConstantForce cForce;
//     public Vector3 forcedir;
//     public float restauration_speed = 1000f;
//     public float factorDeFrenadoZ = 0.9f; // Factor de frenado en el eje Z
//     public float factorDeFrenadoX = 0.9f; // Factor de frenado en el eje X
//     public int milisegundos = 1;
//     private Rigidbody rb;

//     void Start(){
//         cForce = GetComponent<ConstantForce>();
//         forcedir = new Vector3(0, -10,0);
//         cForce.force = forcedir;
//         rb = GetComponent<Rigidbody>();
            
//     }

//     private void Update()
//     {
        

//         if (Input.GetKeyDown("space"))
//         {
//             forcedir = forcedir * -1;
//             cForce.force = forcedir;
            
//             float rotacionActualX = transform.rotation.eulerAngles.x;
//             float rotacionActualY = transform.rotation.eulerAngles.y;
//             float rotacionActualZ = transform.rotation.eulerAngles.z;

//             Vector3 velocidad = rb.velocity;

            
//             velocidad.x *= factorDeFrenadoX;
//             velocidad.z *= factorDeFrenadoZ;
//             rb.velocity = velocidad;
            
//             transform.rotation = Quaternion.Euler(rotacionActualX, rotacionActualY, rotacionActualZ);

//             float x_step = (0-rotacionActualX) / restauration_speed;
//             //float y_step = (0-rotacionActualY) / restauration_speed;
//             float z_step = (0-rotacionActualZ) / restauration_speed;

//             rb.angularVelocity = Vector3.zero;

            
//             for (int i = 1; i <= restauration_speed; i++)
//             {
//                 rotacionActualX += x_step;
//                 //rotacionActualY += y_step;
//                 rotacionActualZ += z_step;
//                 transform.rotation = Quaternion.Euler(rotacionActualX, rotacionActualY, rotacionActualZ);

//             }
//         }
        
//         if (Input.GetKeyUp("space")){
//             forcedir = forcedir * -1;
//             cForce.force = forcedir;
//         }

//         if (Input.GetKey(KeyCode.RightArrow))
//         {
//             transform.position += new Vector3(0f, 0f, speed * Time.deltaTime);
//         }

//         if (Input.GetKey(KeyCode.LeftArrow))
//         {
//             transform.position -= new Vector3(0f, 0f, speed * Time.deltaTime);
//         }

//         if (Input.GetKey(KeyCode.UpArrow))
//         {
//             transform.position -= new Vector3(speed * Time.deltaTime, 0f, 0f);
//         }

//         if (Input.GetKey(KeyCode.DownArrow))
//         {
//             transform.position += new Vector3(speed * Time.deltaTime, 0f, 0f);
//         }
//     }


    

    
// }

