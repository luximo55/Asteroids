using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Ova skripta kontrolira kako ce se kretati objekti Bullet
public class Bullet : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 500.0f;
    public float maxLifetime = 10.0f; 

    //U funkciji Awake definiramo Rigidbody2D objekta Bullet
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();   
    } 

    //U funkciji Project govorimo u kojem smjeru i kojom brzinom ce se metak kretati te nakon maxLifetime sekundi da se unisti
    public void Project(Vector2 direction)
    {
        rb.AddForce(direction * this.speed);

        Destroy(this.gameObject, this.maxLifetime);
    }

    //U funkciji OnCollisionEnter2D govorimo da kada se objekti Bullet zabiju, unistit ce se
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);
    }
}
