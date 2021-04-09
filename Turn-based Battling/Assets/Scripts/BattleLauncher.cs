using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLauncher : MonoBehaviour
{
    public List<Character> Players { get; set; }
    public List<Character> Enemies { get; set; }

    //right before the Start 
    void Awake()
    {
        //allows the players to persist in the battle scene
        DontDestroyOnLoad(this);
        
    }

    //set up characters in the battle scene before the battle scene is loaded
    public void PrepareBattle(List<Character> enemies, List<Character> players)
    {

        Players = players;
        Enemies = enemies;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Battle");

    }

    //launch the battle after the scene has loaded
    public void Launch()
    {
        //pass in players to the Battle Controller instance
        BattleController.Instance.StartBattle(Players, Enemies); 
    }
}
