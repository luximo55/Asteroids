using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

//Ova skripta kontrolira tipke u sceni MainMenu
public class MenuManager : MonoBehaviour
{
    //U funkciji Play mijenja se scena na scenu Game kada pritisnemo tipku Play
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    //U funkciji Quit igra se iskljucuje kada pritisnemo tipku Quit
    public void Quit()
    {
        Application.Quit();
    }
}
