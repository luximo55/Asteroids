using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

//Ova skripta kontrolira objekte i njihova ponasanja te korisnicko sucelje
public class GameManager : MonoBehaviour
{
    public Player player;
    public ParticleSystem explosion;
    public float respawnTime = 3.0f;
    public float respawnProtection = 3.0f;
    public int score = 0;
    public int lives = 3;
    [Header ("UI")]
    public GameObject GameOverPanel;
    public Text scoreText;
    public Text livesText;

    //U funkciji AsteroidDestroyed govorimo sto se desi kada se unisti Asteroid 
    public void AsteroidDestroyed(Asteroid asteroid)
    {
        //Particle efekt Explosion postavlja se na objekt Asteroid i pokrece se 
        this.explosion.transform.position = asteroid.transform.position;
        this.explosion.Play();

        //Ovaj uvjet definira koliko ce igrac dobiti bodova, broj bodova je obrnuto proporcionalan velicini objekta Asteroid (sto je manji objekt to je veci broj bodova)
        if(asteroid.size < 0.75f)
        {
            score += 100;
            this.scoreText.text = score.ToString();
        }
        else if (asteroid.size < 1.0f)
        {
            score += 50;
            this.scoreText.text = score.ToString();
        }
        else
        {
            score += 25;
            this.scoreText.text = score.ToString();
        }
    }

    //U funkciji PlayerDied govorimo sto ce se desiti kada se igrac zabije u Asteroid
    public void PlayerDied()
    {
        //Particle efekt Explosion postavlja se na objekt Player i pokrece se 
        this.explosion.transform.position = this.player.transform.position;
        this.explosion.Play();

        //Oduzimaju se zivoti
        this.lives--;

        /*Ovaj uvjet provjeravaima koliko zivota ima igrac, ukoliko ima 0 to ce se ispisati i pokrenuti funkcija GameOver,
        a u suprotnom za vrijeme respawnTime ce se pokrenuti funkcija Respawn i ispisati broj zivota*/
        if(this.lives <= 0)
        {
            this.livesText.text = lives.ToString();
            GameOver();
        }
        else
        {
           Invoke(nameof(Respawn), this.respawnTime);
           this.livesText.text = lives.ToString();
        }
    }

    //U funkciji Respawn govorimo sto ce se desiti nakon sto se objekt Player sudario s objektom Asteroid
    private void Respawn()
    {
        /*Nakon sto se objekt Player sudario s objektom Asteroid, on: 
            - promijeni layer u IgnoreCollision da respawnProtection sekundi objekti Asteroid ne bi utjecali na objekt Player
            - resetira svoj transform na 0
            - ukljuci se objekt player*/
        this.player.gameObject.layer = LayerMask.NameToLayer("IgnoreCollision");
        this.player.transform.position = Vector3.zero;
        this.player.gameObject.SetActive(true);
        
        //Nakon vremena respawnProtection se poziva funkcija TurnOnCollisions
        Invoke(nameof(TurnOnCollisions), this.respawnProtection);
    }

    //U funkciji TurnOnCollisions kazemo da kada se objekt Player respawna, nakon respawnProtection sekundi on promjeni svoj layer natrag u Player
    private void TurnOnCollisions()
    {
        this.player.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    //U funkciji GameOver ukljucujemo GameOverPanel kada igrac izgubi sve zivote
    private void GameOver()
    {
        GameOverPanel.SetActive(true);
    }

    //U funkciji Restart govorimo sto se desi kada pritisnemo tipku Restart na GameOverPanelu
    public void Restart()
    {
        //Iskljuci se GameOver Panel
        GameOverPanel.SetActive(false);

        //Resetira se broj zivota i ispisuje na ekranu
        this.lives = 3;
        this.livesText.text = lives.ToString();

        //Resetira se broj bodova i ispisuje na ekranu
        this.score = 0;
        this.scoreText.text = score.ToString();

        //Poziva se funkcija respawn
        Respawn();
    }

    //U funkciji MainMenu ucitavamo MainMenu scenu
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
