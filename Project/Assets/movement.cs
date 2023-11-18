using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveObject : MonoBehaviour
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

        //ROTACION
        if (Input.GetKey(KeyCode.E)){
            transform.Rotate(Vector3.up * speed*10  * Time.deltaTime);
            
        }
        if (Input.GetKey(KeyCode.Q)){
            transform.Rotate(-Vector3.up * speed *10* Time.deltaTime);
        }
        //ROTACION


        //ELEVACION
        if (Input.GetKeyDown("space")){
            forcedir.y *= -1 ;
            cForce.force = forcedir;
            
            float rotacionActualX = transform.rotation.eulerAngles.x;
            float rotacionActualY = transform.rotation.eulerAngles.y;
            float rotacionActualZ = transform.rotation.eulerAngles.z;

            
            // rb.angularVelocity = new Vector3(0, rb.angularVelocity.y, 0); // Mantener solo la rotación en Y
            // rb.AddTorque(-rb.angularVelocity.x * fuerzaDeFrenado, 0, -rb.angularVelocity.z * fuerzaDeFrenado, ForceMode.Force);

            rb.angularVelocity = new Vector3(0, rb.angularVelocity.y, 0); // Mantener solo la rotación en Y
            rb.AddTorque(-rb.angularVelocity.x * fuerzaDeFrenado, 0, -rb.angularVelocity.z * fuerzaDeFrenado, ForceMode.Force);

            Vector3 rotacion = transform.eulerAngles;
            float x_step = (0-rotacionActualX) / restauration_speed;
            //float y_step = (0-rotacionActualY) / restauration_speed;
            float z_step = (0-rotacionActualZ) / restauration_speed;

            for (int i = 1; i <= restauration_speed; i++)
            {
                rotacionActualX += x_step;
                //rotacionActualY += y_step;
                rotacionActualZ += z_step;
                transform.rotation = Quaternion.Euler(rotacionActualX, rotacionActualY, rotacionActualZ);

            }
        }
        if (Input.GetKeyUp("space")){
            forcedir.y *= -1 ;
            cForce.force = forcedir;
        }
        //ELEVACION

        
        
        //ADELANTE
        if (Input.GetKey(KeyCode.W)){
            float rotacionActualY = transform.rotation.eulerAngles.y;
            float anguloRadianes = rotacionActualY * Mathf.Deg2Rad;
            float velocidadXOriginal = -1;
            float velocidadZ = velocidadXOriginal * Mathf.Sin(anguloRadianes);
            float velocidadX = velocidadXOriginal * Mathf.Cos(anguloRadianes);
            float velocidadXZ = Mathf.Sqrt(velocidadX * velocidadX + velocidadZ * velocidadZ);


            if (Mathf.Abs(velocidadXZ) < 10){   
                Vector3 aux = new Vector3(velocidadX,0,-velocidadZ);
                rb.AddForce( aux* aceleracion, ForceMode.Force);
            }

        }
        //ADELANTE


        //ATRAS
        if (Input.GetKey(KeyCode.S)){
            float rotacionActualY = transform.rotation.eulerAngles.y;
            float anguloRadianes = rotacionActualY * Mathf.Deg2Rad;
            float velocidadXOriginal = 1;
            float velocidadZ = velocidadXOriginal * Mathf.Sin(anguloRadianes);
            float velocidadX = velocidadXOriginal * Mathf.Cos(anguloRadianes);
            float velocidadXZ = Mathf.Sqrt(velocidadX * velocidadX + velocidadZ * velocidadZ);


            if (Mathf.Abs(velocidadXZ) < 10){   
                Vector3 aux = new Vector3(velocidadX,0,-velocidadZ);
                rb.AddForce( aux* aceleracion, ForceMode.Force);
            }

        }
        //ATRAS


        //DERECHA
        if (Input.GetKey(KeyCode.D)){
            float rotacionActualY = transform.rotation.eulerAngles.y;
            float anguloRadianes = rotacionActualY * Mathf.Deg2Rad;
            float velocidadZOriginal = 1;
            float velocidadX = velocidadZOriginal * Mathf.Sin(anguloRadianes);
            float velocidadZ = velocidadZOriginal * Mathf.Cos(anguloRadianes);
            float velocidadXZ = Mathf.Sqrt(velocidadX * velocidadX + velocidadZ * velocidadZ);


            if (Mathf.Abs(velocidadXZ) < 10){   
                Vector3 aux = new Vector3(velocidadX,0,velocidadZ);
                rb.AddForce( aux* aceleracion, ForceMode.Force);
            }

        }
        //DERECHA


        
        //IZQUIERDA
        if (Input.GetKey(KeyCode.A)){
            float rotacionActualY = transform.rotation.eulerAngles.y;
            float anguloRadianes = rotacionActualY * Mathf.Deg2Rad;
            float velocidadZOriginal = -1;
            float velocidadX = velocidadZOriginal * Mathf.Sin(anguloRadianes);
            float velocidadZ = velocidadZOriginal * Mathf.Cos(anguloRadianes);
            float velocidadXZ = Mathf.Sqrt(velocidadX * velocidadX + velocidadZ * velocidadZ);


            if (Mathf.Abs(velocidadXZ) < 10){   
                Vector3 aux = new Vector3(velocidadX,0,velocidadZ);
                rb.AddForce( aux* aceleracion, ForceMode.Force);
            }
        }
        //IZQUIERDA

        

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

