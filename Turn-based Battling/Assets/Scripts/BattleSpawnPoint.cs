using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//class to move characters from overwold into into battle scene

public class BattleSpawnPoint : MonoBehaviour
{

    //pass in a character to spawn into the game
    //creates an instance of a character to return
  public Character Spawn(Character character)
    {
        //creates a spawn point to be the parent of our character that is passed in
        Character charToSpawn = Instantiate<Character>(character, this.transform);
        return charToSpawn;

    }


}
