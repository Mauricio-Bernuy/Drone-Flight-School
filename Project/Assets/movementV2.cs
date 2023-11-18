using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveObjectV2 : MonoBehaviour
{
    public float speed = 5f;
    public ConstantForce cForce;
    public Vector3 forcedir,forcedir_cp;
    public float factorDeFrenado = 0.95f; // Ajusta el factor de frenado según tus necesidades
    private Rigidbody rb;
    public int restauration_speed = 1000;
    public float fuerzaDeFrenado = 10.0f; // Ajusta la fuerza de frenado según tus necesidades
    public float fuerzadetorque = 20f;
    public float precision = 0.01f;
    public float aceleracion = 1.0f;
    public float frenado = 10.0f;
    public float uprightStrength = 10.0f;
    public float prevAngularDrag;
    private float gravConst = 9.81f;
    private bool droneOn = true;


    void Start(){
        cForce = GetComponent<ConstantForce>();
        forcedir = new Vector3(0, gravConst, 0);
        cForce.force = forcedir;
        rb = GetComponent<Rigidbody>();
        prevAngularDrag = rb.angularDrag;            
    }

    public IEnumerator AngularDecelerate()
    {
        rb.angularDrag = 10f;
        //Wait here
        yield return new WaitForSeconds(0.5f);
        rb.angularDrag = prevAngularDrag;       
    }

    // para sacar inputs de controles en OVR: https://developer.oculus.com/documentation/unity/unity-ovrinput/?locale=es_ES 
    // when controller model ready: detect if left or right hand grabbing controller, and then enable those mappings.
    // using UnityEngine;
    // using UnityEngine.SceneManagement;
    
    // public class SceneLoader : MonoBehaviour {
    //     void Update() {
    //         OVRInput.Update()
    //         if (OVRInput.Get(OVRInput.Button.Two))
    //             SceneManager.LoadScene("name");
    //     }
    // }

    private void Update()
    {
        // PRENDER APAGAR DRON
        if (Input.GetKeyDown(KeyCode.F)){
            droneOn = !droneOn;
            if (droneOn)
                forcedir = new Vector3(0, gravConst, 0);
            else
                forcedir = new Vector3(0, 0, 0);        
            cForce.force = forcedir;
        }
        if (!droneOn){
            return;
        }
        
        // MANTAIN UPRIGHT
        
        var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
        rb.AddTorque(new Vector3(rot.x, rot.y, rot.z)*uprightStrength);

        // transform.localRotation *= Quaternion.AngleAxis(30f * Time.deltaTime, Vector3.up);

        //ROTACION
        if (Input.GetKey(KeyCode.E)){
            // transform.Rotate(Vector3.up * speed*10  * Time.deltaTime);
            rb.angularDrag = prevAngularDrag;       
            rb.AddTorque(Vector3.up * speed * 100  * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.Q)){
            rb.angularDrag = prevAngularDrag;       
            rb.AddTorque(-Vector3.up * speed * 100  * Time.deltaTime);
        }

        if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Q)){ //|| Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D)
            StartCoroutine(AngularDecelerate());    
        }

        //ELEVACION +
        if (Input.GetKeyDown("space")){
            forcedir_cp = forcedir;
            forcedir_cp.y = forcedir.y + fuerzadetorque ;
            cForce.force = forcedir_cp;
        }
        if (Input.GetKeyUp("space")){
            forcedir_cp = forcedir;
            forcedir_cp.y = forcedir.y - fuerzadetorque ;
            cForce.force = forcedir;
        }

        //ELEVACION -
        if (Input.GetKeyDown("left ctrl")){
            forcedir_cp = forcedir;
            forcedir_cp.y = forcedir.y - fuerzadetorque ;
            cForce.force = forcedir_cp;
        }
        if (Input.GetKeyUp("left ctrl")){
            forcedir_cp = forcedir;
            forcedir_cp.y = forcedir.y + fuerzadetorque ;
            cForce.force = forcedir;
        }
        
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

            rb.AddRelativeTorque(new Vector3(0, 0, 2f)*1f);
        }

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

            rb.AddRelativeTorque(new Vector3(0, 0, -2f)*1f);
        }

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
            rb.AddRelativeTorque(new Vector3(2f, 0, 0)*1f);
        }
        
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
            rb.AddRelativeTorque(new Vector3(-2f, 0, 0)*1f);
        }
        

    }
}


