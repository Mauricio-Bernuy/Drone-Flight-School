using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Oculus.Interaction;


using UnityEngine;
using System.Collections;
using System.Threading.Tasks;





public class MoveObjectV3 : MonoBehaviour
{
    public int droneId = 1;
    public float rotationSpeed = 5f;
    public ConstantForce cForce;
    public Vector3 forcedir,forcedir_cp;
    private Rigidbody rb;
    public float elevationSpeed = 20f;
    public float movementAcceleration = 1.0f;

    public float uprightStrength = 10.0f;
    public float turbulence = 0.05f;
    public float angularTurbulence = 0f;
    
    public float prevAngularDrag;
    public float prevDragX = 0;
    public float prevDragY = 0;
    public float prevDragZ = 0;
    private float gravConst = 9.81f;
    private bool droneOn = true;
    
    public Animator animFL;
    public Animator animFR;
    public Animator animBL;
    public Animator animBR;

    public float speedFL;
    public float speedFR;
    public float speedBL;
    public float speedBR;
    
    public GameObject controller;
    
    public GrabInteractor GrabInteractorL;
    public GrabInteractor GrabInteractorR;
    public GrabInteractable target;
    private GrabInteractable droneTarget;

    public bool grabbedL = false;
    public bool grabbedR = false;

    public AudioSource droneAudio;
    public AudioSource droneBeep;
    public AudioSource droneHit;
    public AudioSource droneHardHit;

    public float rotAudio = 0;
    public float heightAudio = 0;
    public float FBAudio = 0;
    public float LRAudio = 0;

    private bool checkedFinalCrash = false;

    void Start(){
        //* Setup drone config
        GameObject droneObj = transform.Find("drone").gameObject;
        var droneRenderer = droneObj.GetComponent<Renderer>();
        var mats = droneRenderer.materials;
        droneObj = transform.Find("drone/Fans").gameObject;
        droneRenderer = droneObj.GetComponent<Renderer>();
        var mats2 = droneRenderer.materials;

        try{
            droneId = GameManager.manager.GetDroneId();
        } 
        catch{
            Debug.Log("defaulting to drone 1");
        }
        switch(droneId){
        case 0:
            rotationSpeed = 5f;
            elevationSpeed = 10f;
            movementAcceleration = 2f;
            mats[0].SetColor("_Color", Color.red);
            mats[1].SetColor("_Color", Color.white);
            mats2[0].SetColor("_Color", Color.red);
            mats2[1].SetColor("_Color", Color.white);
            break;
        case 1:
            rotationSpeed = 10f;
            elevationSpeed = 20f;
            movementAcceleration = 6f;
            mats[0].SetColor("_Color", Color.green);
            mats[1].SetColor("_Color", Color.white);
            mats2[0].SetColor("_Color", Color.green);
            mats2[1].SetColor("_Color", Color.white);
            break;
        case 2:
            rotationSpeed = 10f;
            elevationSpeed = 25f;
            movementAcceleration = 6f;
            gravConst = 0;
            mats[0].SetColor("_Color", Color.black);
            mats[1].SetColor("_Color", Color.green);
            mats2[0].SetColor("_Color", Color.black);
            mats2[1].SetColor("_Color", Color.green);
            break;
        default:
            break;
        }

        cForce = GetComponent<ConstantForce>();
        forcedir = new Vector3(0, gravConst, 0);
        cForce.force = forcedir;
        rb = GetComponent<Rigidbody>();
        prevAngularDrag = rb.angularDrag;      
        droneTarget = transform.GetComponent<GrabInteractable>();

        animFL = transform.Find("drone/Fans/fan.FL").GetComponent<Animator>();
        animFR = transform.Find("drone/Fans/fan.FR").GetComponent<Animator>();
        animBL = transform.Find("drone/Fans/fan.BL").GetComponent<Animator>();
        animBR = transform.Find("drone/Fans/fan.BR").GetComponent<Animator>();

        AudioSource[] audioSources = transform.GetComponents<AudioSource>();
        droneAudio = audioSources[0];
        droneBeep = audioSources[1];
        droneHardHit = audioSources[2];
        droneHit = audioSources[3];
    }

    public IEnumerator AngularDecelerate()
    {
        rb.angularDrag = 10f;
        yield return new WaitForSeconds(0.5f);
        rb.angularDrag = prevAngularDrag;       
    }


    // async void droneDecelerate(float x, float y, float z){
    //     prevDragX = x;
    //     prevDragY = y;
    //     prevDragZ = z;
    //     await Task.Delay(1000); 
    //     prevDragX = 0;
    //     prevDragY = 0;
    //     prevDragZ = 0;
    // }

    async void crashDroneOff()
    {
        await Task.Delay(1000); 
        droneBeep.pitch = 0.5F;
        droneBeep.Play(0);
        droneOn = false;
        forcedir = new Vector3(0, 0, 0);   
        cForce.force = forcedir;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 2)
            droneHit.Play();
        
        if (!(droneTarget == GrabInteractorL.SelectedInteractable || droneTarget == GrabInteractorR.SelectedInteractable)){
            if (collision.relativeVelocity.magnitude > 10){
                droneHardHit.Play();
                turbulence += 0.2f;
                angularTurbulence += 0.1f;
            }
        }
    }

    private void Update()
    {
        //* CHECK IF CRASHED
        if (turbulence > 1.5F && !checkedFinalCrash){ 
            checkedFinalCrash = true;
            crashDroneOff();
        }
        
        //* CHECK IF HOLDING CONTROLLER
        if (target == GrabInteractorL.SelectedInteractable)
            grabbedL = true;
        else  
            grabbedL = false;
        
        if (target == GrabInteractorR.SelectedInteractable)
            grabbedR = true;
        else 
            grabbedR = false;

        //* CHECK IF HOLDING DRONE 
        if (droneTarget == GrabInteractorL.SelectedInteractable || droneTarget == GrabInteractorR.SelectedInteractable){
            rb.useGravity = false;
        
        }
        else{
            rb.useGravity = true;
        }

        //* CHECK IF HOLDING DRONE WITH 2 HANDS AND FIX
        if (droneTarget == GrabInteractorL.SelectedInteractable && droneTarget == GrabInteractorR.SelectedInteractable){
            if (turbulence != 0.1f && angularTurbulence != 0f){
                turbulence = 0.1f;
                angularTurbulence = 0f;
                droneBeep.pitch = 2F;
                droneBeep.Play(0);
            }
        }
        
        //* TOGGLE ON
        if (Input.GetKeyDown(KeyCode.F) || (grabbedR && OVRInput.GetDown(OVRInput.Button.One))){
            droneOn = !droneOn;
            if (droneOn){
                forcedir = new Vector3(0, gravConst, 0);
                droneBeep.pitch = 1F;
                droneAudio.pitch = Mathf.Lerp(droneAudio.pitch, 1f, 5f * Time.deltaTime);
                droneAudio.volume = Mathf.Lerp(droneAudio.volume, 1f, 5f * Time.deltaTime);
            }
            else{
                forcedir = new Vector3(0, 0, 0);        
                droneBeep.pitch = 0.5F;
            }

            cForce.force = forcedir;
            droneBeep.Play(0);
            checkedFinalCrash = false;
        }

        //* BASE FAN SPEED
        if(droneOn){
            animFL.speed = 2F; 
            animFR.speed = 2F; 
            animBL.speed = 2F; 
            animBR.speed = 2F; 
        }
        else{
            animFL.speed = 0F; 
            animFR.speed = 0F; 
            animBL.speed = 0F; 
            animBR.speed = 0F; 
        }

        if(droneOn){
            droneAudio.pitch = 1F;
            droneAudio.volume = 1F;
        }
        else{
            droneAudio.pitch = Mathf.Lerp(droneAudio.pitch, 0.5f, 5f * Time.deltaTime);
            droneAudio.volume = Mathf.Lerp(droneAudio.volume, 0f, 5f * Time.deltaTime);
        }

        //* MANTAIN UPRIGHT        
        if (droneOn){
            var rot = Quaternion.FromToRotation(transform.up, Vector3.up);
            float turbulenceX = UnityEngine.Random.Range(-turbulence, turbulence);
            float turbulenceY = UnityEngine.Random.Range(-angularTurbulence, angularTurbulence);
            float turbulenceZ = UnityEngine.Random.Range(-turbulence, turbulence);
            rb.AddTorque(new Vector3(rot.x + turbulenceX, rot.y + turbulenceY, rot.z + turbulenceZ) * uprightStrength);
        }

        //* CONTROLLER CONTROLS
        if (droneOn){
            // AUDIO
            if (grabbedL){
                Vector2 valL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                rotAudio = (float)Math.Round(Mathf.Lerp(rotAudio, Mathf.Abs(valL.x), 5f * Time.deltaTime),8);
                heightAudio = (float)Math.Round(Mathf.Lerp(heightAudio, Mathf.Abs(valL.y), 5f * Time.deltaTime),8);
                droneAudio.pitch += rotAudio + heightAudio;
            }
            if (grabbedR){
                Vector2 valR = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
                LRAudio = (float)Math.Round(Mathf.Lerp(LRAudio, Mathf.Abs(valR.x), 5f * Time.deltaTime),8);
                FBAudio = (float)Math.Round(Mathf.Lerp(FBAudio, Mathf.Abs(valR.y), 5f * Time.deltaTime),8);

                droneAudio.pitch += FBAudio + LRAudio;
            }

            droneAudio.pitch += UnityEngine.Random.Range(-turbulence, turbulence) * 0.1f;
            
            // ASSISTS

            // Vector3 t_velocity = rb.velocity;

            // t_velocity.x *= 1f + prevDragX;
            // t_velocity.y *= 1f + prevDragY;
            // t_velocity.z *= 1f + prevDragZ;

            // rb.velocity = t_velocity;

            // if (grabbedL){
            //     Vector2 valL = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
            //     if (valL.x == 0f){
            //         StartCoroutine(AngularDecelerate());    
            //     }
                // if (valL.y == 0f){
                //     droneDecelerate(10f,0f,0f);  
                // }
            // }
            // if (grabbedR){
            //     Vector2 valR = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
                // if (valR.x == 0f){
                //     droneDecelerate(0f,10f,0f);  
                // }
            //     if (valR.y == 0f){
            //         droneDecelerate(0f,0f,10f);  
            //     }
            // }

            if (grabbedL){
                // ROTATION CONTROLLER
                if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick) != new Vector2(0f,0f)){
                    Vector2 val = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                    rb.angularDrag = prevAngularDrag;       
                    rb.AddTorque(Vector3.up * rotationSpeed * 100 * val.x * Math.Abs(val.x) * Time.deltaTime); // al cuadrado

                    // fans
                    animFL.speed += val.x; 
                    animFR.speed -= val.x; 
                    animBL.speed -= val.x; 
                    animBR.speed += val.x; 
                }


                // ELEVATION CONTROLLER
                if (Math.Abs(OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).y) != 0.5){
                    Vector2 val = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);
                    forcedir_cp = forcedir;
                    forcedir_cp.y = forcedir.y + elevationSpeed * val.y * Math.Abs(val.y) * Math.Abs(val.y) * Math.Abs(val.y);
                    cForce.force = forcedir_cp;

                    // fans
                    animFL.speed += val.y; 
                    animFR.speed += val.y; 
                    animBL.speed += val.y; 
                    animBR.speed += val.y; 

                }
            } 

            if (grabbedR){
                // FORWARD BACK CONTROLLER
                if (true){
                    Vector2 val = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

                    float rotacionActualY = transform.rotation.eulerAngles.y;
                    float anguloRadianes = rotacionActualY * Mathf.Deg2Rad;
                    float velocidadXOriginal = -1;
                    float velocidadZ = velocidadXOriginal * Mathf.Sin(anguloRadianes);
                    float velocidadX = velocidadXOriginal * Mathf.Cos(anguloRadianes);
                    float velocidadXZ = Mathf.Sqrt(velocidadX * velocidadX + velocidadZ * velocidadZ);


                    if (Mathf.Abs(velocidadXZ) < 10){   
                        Vector3 aux = new Vector3(velocidadX,0,-velocidadZ);
                        rb.AddForce( aux * movementAcceleration * val.y * 2f, ForceMode.Force);
                    }

                    rb.AddRelativeTorque(new Vector3(0, 0, 2f) * val.y);

                    // fans
                    animFL.speed -= val.y; 
                    animFR.speed -= val.y; 
                    animBL.speed += val.y; 
                    animBR.speed += val.y; 
                }

                // LEFT RIGHT CONTROLLER
                if (true){
                    Vector2 val = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

                    float rotacionActualY = transform.rotation.eulerAngles.y;
                    float anguloRadianes = rotacionActualY * Mathf.Deg2Rad;
                    float velocidadZOriginal = 1;
                    float velocidadX = velocidadZOriginal * Mathf.Sin(anguloRadianes);
                    float velocidadZ = velocidadZOriginal * Mathf.Cos(anguloRadianes);
                    float velocidadXZ = Mathf.Sqrt(velocidadX * velocidadX + velocidadZ * velocidadZ);


                    if (Mathf.Abs(velocidadXZ) < 10){   
                        Vector3 aux = new Vector3(velocidadX,0,velocidadZ);
                        rb.AddForce( aux* movementAcceleration * val.x * 2f, ForceMode.Force);
                    }
                    rb.AddRelativeTorque(new Vector3(2f, 0, 0)*val.x);

                    // fans
                    animFL.speed += val.x; 
                    animFR.speed -= val.x; 
                    animBL.speed += val.x; 
                    animBR.speed -= val.x; 
                }
            }
        }
        
        //* KEYBOARD CONTROLS
        if (droneOn){
            //ROTACION
            if (Input.GetKey(KeyCode.E)){
                rb.angularDrag = prevAngularDrag;       
                rb.AddTorque(Vector3.up * rotationSpeed * 100  * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Q)){
                rb.angularDrag = prevAngularDrag;       
                rb.AddTorque(-Vector3.up * rotationSpeed * 100  * Time.deltaTime);
            }
            if (Input.GetKeyUp(KeyCode.E) || Input.GetKeyUp(KeyCode.Q) ){ 
                StartCoroutine(AngularDecelerate());    
            }

            //ELEVACION +
            if (Input.GetKeyDown("space")){
                forcedir_cp = forcedir;
                forcedir_cp.y = forcedir.y + elevationSpeed;
                cForce.force = forcedir_cp;
            }
            if (Input.GetKeyUp("space")){
                forcedir_cp = forcedir;
                forcedir_cp.y = forcedir.y - elevationSpeed;
                cForce.force = forcedir;
            }

            //ELEVACION -
            if (Input.GetKeyDown("left ctrl")){
                forcedir_cp = forcedir;
                forcedir_cp.y = forcedir.y - elevationSpeed;
                cForce.force = forcedir_cp;
            }
            if (Input.GetKeyUp("left ctrl")){
                forcedir_cp = forcedir;
                forcedir_cp.y = forcedir.y + elevationSpeed;
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
                    rb.AddForce( aux* movementAcceleration, ForceMode.Force);
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
                    rb.AddForce( aux* movementAcceleration, ForceMode.Force);
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
                    rb.AddForce( aux* movementAcceleration, ForceMode.Force);
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
                    rb.AddForce( aux* movementAcceleration, ForceMode.Force);
                }
                rb.AddRelativeTorque(new Vector3(-2f, 0, 0)*1f);
            }
        }

        speedFL = animFL.speed;
        speedFR = animFR.speed;
        speedBL = animBL.speed;
        speedBR = animBR.speed;

    }


}


