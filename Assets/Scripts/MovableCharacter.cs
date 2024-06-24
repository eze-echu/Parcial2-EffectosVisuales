using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MovableCharacter : MonoBehaviour, iBurnable
{
    public CharacterController CC;
    Vector3 move;
    public float speed;
    private const float Gravity = -9.81f;
    public bool isGrounded;
    public Transform groundCheck;
    public float groundDistance;
    public Vector3 velocity;
    public LayerMask groundMask;

    public Volume postProcessing;

    private Vignette m_Vignette;
    // Start is called before the first frame update
    void Start()
    {
        move = new Vector3();
        m_Vignette = postProcessing.profile.Add<Vignette>();
        m_Vignette.intensity.Override(0.3f);
        m_Vignette.color.Override(Color.black);
    }

    private void FixedUpdate()
    {
        //projects an invisible sphere to check if grounded or not
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += ( Gravity * Time.deltaTime);
        CC.Move(velocity * Time.deltaTime) ;
    }

    // Update is called once per frame
    void Update()
    {
        move.x = Input.GetAxis("Horizontal");
        move.z = Input.GetAxis("Vertical");
        if (move.sqrMagnitude > 1)
        {
            move.Normalize();
        }
        move = (transform.right * move.x + transform.forward * move.z);

        CC.Move(move * (speed * Time.deltaTime));
    }

    public bool OnFire { get; set; }

    public iBurnable.Effect Burning()
    {
        StartCoroutine(BurningCounter());
        return null;
    }

    private IEnumerator BurningCounter()
    {
        OnFire = true;
        m_Vignette.color.Override(Color.red);
        m_Vignette.intensity.Override(Mathf.Lerp(0.3f, 0.6f, 2f));
        m_Vignette.intensity.Override(Mathf.Lerp(0.6f, 0.3f, 2f));
        yield return new WaitForSeconds(5f);
        OnFire = false;
        m_Vignette.color.Override(Color.black);
        m_Vignette.intensity.Override(0.3f);
    }
    private void OnTriggerEnter(Collider other)
    {
        print("collided");
        if (other.gameObject.GetComponent<iBurn>() != null)
        {
            if (other.gameObject.GetComponent<iBurn>().CanBurn)
            {
                Burning();
            }
        }
        else if (other.gameObject.GetComponent<iBurnable>() != null)
        {
            print("Burnable");
            if (!OnFire || other.gameObject.GetComponent<iBurnable>().OnFire) return;
            print("Setting fire");
            other.gameObject.GetComponent<iBurnable>().Burning();
        }
        
    }
}
