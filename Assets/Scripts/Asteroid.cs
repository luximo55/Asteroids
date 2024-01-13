using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//Ova skripta kontrolira kako ce se ponasati objekti Asteroid kada se instanciraju i sudare s drugim objektima
public class Asteroid : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    public Sprite[] sprites;
    public float maxLifetime = 30.0f;
    public float size = 1.0f;
    public float minSize = 0.5f;
    public float maxSize = 1.5f;
    public float speed = 50.0f;

    ////U funkciji Awake definiramo Rigidbody2D i SpriteRenderer objekta Asteroid
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    //U funkciji Start izabere se jedan nasumican sprite iz niza Sprites, definira velicinu i masu objekta Asteroid 
    private void Start()
    {
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length)];

        this.transform.eulerAngles = new Vector3(0.0f, 0.0f, Random.value * 360.0f);
        this.transform.localScale = Vector3.one * this.size;

        rb.mass = this.size * 2.0f;
    }

    //U funkciji SetTrajectory govorimo kojom brzinom i u kojem smjeru ce se kretati objekti Asteroid i da ce se za maxLifetime sekundi unistiti
    public void SetTrajectory(Vector2 direction)
    {
        rb.AddForce(direction * this.speed);
        
        Destroy(this.gameObject, this.maxLifetime);
    }

    /*U funkciji OnCollisionEnter2D kazemo da ako se sudari s objektom s tagom Bullet, objekt Asteroid ce:
        - provjeriti velicinu objekta Asteroid i ako je pola njegove velicine vece ili jednako minimalnoj velicini, funkcija CreateSplit ce se pozvati dva puta
        - pozvati ce funkciju AstroidDestroyed na skripti GameManager
        - unistiti objekt Asteroid
    */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            if(this.size * 0.5f >= minSize)
            {
                CreateSplit();
                CreateSplit();
            }
            FindObjectOfType<GameManager>().AsteroidDestroyed(this);
            Destroy(this.gameObject);
        }
    }

    //U funkciji CreateSplit govorimo da kada se objekt Asteroid sudari s objektom s tagom Bullet, objekt Asteroid ce napraviti manji objekt Asteroid
    private void CreateSplit()
    {   
        //Postaviti ce objekt na poziciji trenutnog objekta Asteroid
        Vector2 position = this.transform.position;
        position += Random.insideUnitCircle * 0.5f;

        //Instancira manji objekt Asteroid
        Asteroid half = Instantiate(this, position, this.transform.rotation);
        half.size = this.size * 0.5f;

        //Daje brzinu i smjer manjem objektu Asteroid
        half.SetTrajectory(Random.insideUnitCircle.normalized * this.speed);
    }

}
