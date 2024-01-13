using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Ova skripta kontrolira igraca, njegove kretnje, pucanje te zabijanje u objekte Asteroid
public class Player : MonoBehaviour
{
    public Bullet bulletPrefab;
    private Rigidbody2D rb;
    private bool thrusting;
    public float thrustSpeed = 1.0f;
    private float turnDirection;
    public float turnSpeed = 1.0f;

    //U funkciji Awake definiramo Rigidbody2D objekta Player
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    //U funkciji Update provjeravamo unos igraca, te informacije o unosu koristimo kako bismo odredili kada i kamo igrac treba ici i kada treba pucati
    private void Update()
    {
        thrusting = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);

        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            turnDirection = 1.0f;
        }
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            turnDirection = -1.0f;
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    //U funkciji FixedUpdate brinemo o fizici igre, specificno o kretanju igraca
    private void FixedUpdate()
    {  
        //Ovaj if kaze kada ce se igrac kretati prema naprijed
        if(thrusting)
        {
            rb.AddForce(this.transform.up * thrustSpeed);
        }

        //Ovaj if kaze kada i u kojem smjeru ce se igrac kretati
        if(turnDirection != 0.0f)
        {
            rb.AddTorque(turnDirection * turnSpeed);
        }
    }

    //U funkciji Shoot instanciramo Bullet, govorimo gdje se instancira i prema kuda
    private void Shoot()
    {
        Bullet bullet = Instantiate(this.bulletPrefab, this.transform.position, this.transform.rotation);
        bullet.Project(this.transform.up);
    }

    /*U funkciji OnCollisionEnter2D kazemo da, kada se Player sudari sa objektima sa tagom Asteroid, on:
        - izgubi brzinu
        - izgubi brzinu okretanja
        - iskljuci se objekt Player
        - te se iz skripte GameManager pozove funkcija PlayerDied*/
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Asteroid")
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = 0.0f;

            this.gameObject.SetActive(false);

            FindAnyObjectByType<GameManager>().PlayerDied();
        }
    }
}
