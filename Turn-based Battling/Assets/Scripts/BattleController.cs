using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//set up turn based battle system; player turn all players go; enemy turn all enemies go.
//using a collection of collcetions to determine player turns

public class BattleController : MonoBehaviour
{
    //creating an instance of a battle
    public static BattleController Instance { get; set; }


    //collection of characters with int key 0 or 1 to determine if players or enemies get to battle
    public Dictionary<int, List<Character>> characters = new Dictionary<int, List<Character>>();
    //track the whos turn it is
    public int turnIndex;
    //which player on either side goes 
    private int actTurn;
    //reference to selected attack
    public Spell selectedSpell;
    //see whether player is using a regular attack
    public bool isAttacking;

    //track player spawn points; kept in an array;used to spawn players into one of the 6 spawn points
    [SerializeField] private BattleSpawnPoint[] spawnPoints;
    [SerializeField] private BattleUIController uiController;

    private void Start()
    {
        //checking if there is already a battle instance reference
        if (Instance != null && Instance != this)
        {
            //destroy new instance
            Destroy(this.gameObject);

        }
        //if Instance is not assigned assign new battle control reference
        else
        {
            Instance = this;

        }


        //add player characters
        characters.Add(0, new List<Character>());
        //add enemy characters 
        characters.Add(1, new List<Character>());

        //will find the battle launcher which we'll only have one of and launch the battle after everything is set up
        FindObjectOfType<BattleLauncher>().Launch();

    }

    //select a random player for enemy attack
    public Character GetRandomPlayer()
    {
        return characters[0][Random.Range(0, characters[0].Count-1)];
    }

    //select a random player for enemy attack
    public Character GetWeakestEnemy()
    {
        //set this enemy as lowest health enemy
        Character weakestEnemy = characters[1][0];
        //compare all enemies in the battle to the lowest health enemy
        foreach(Character character in characters[1])
        {
            //if anotehr enemy has lower health, they become the weakest enemy
            if(character.health < weakestEnemy.health)
            {
                weakestEnemy = character;
            }    
        }
        return weakestEnemy;
    }

    //flip back and forth between players and enemies
    private void NextTurn()
    {
        // if key is equal to 1 swap to 0 and vice versa
        actTurn = actTurn == 0 ? 1 : 0;

    }


    private void NextAct()
    {
        //check if there are still players on both sides
        if(characters[0].Count > 0 && characters[1].Count > 0)
        {
            //compare turn index to the actTurn; checks if there are more players to act
            if(turnIndex < characters[actTurn].Count-1)
            {
                //increase turn index
                turnIndex++;
            }
            else
            {
                //if all players on one side have gone, go to next turn;
                NextTurn();
                //reset index
                turnIndex = 0;

                //log turns
                Debug.Log("turn: " + actTurn);
            }

            //if the player turn let them fight
           switch(actTurn)
            {
                case 0:
                    //UI interface
                    //turn on UI for player to interact
                    uiController.ToggleActionState(true);
                    uiController.BuildSpellList(GetCurrentCharacter().spells);
                    break;

                case 1:
                    //calling delay before player act starts
                    StartCoroutine(PerformAct());
                    uiController.ToggleActionState(false);
                    //UI interface and act
                    break;

            }

        }
        else
        {
            //end battle
            Debug.Log("Battle over!");

        }

    }

        //add a delay while everything else works in the background and come back to this method
    IEnumerator PerformAct()
    {
            //wait for 3/4 of a second
            yield return new WaitForSeconds(.75f);

            //check if the character acting is not dead
            if(GetCurrentCharacter().health > 0)
            {
            //do an action
            GetCurrentCharacter().GetComponent<Enemy>().Act();
            }
        
            //update character UI based on progress of battle
            uiController.UpdateCharacterUI();

            //wait a second after the turn is complete
            yield return new WaitForSeconds(1f);
            //go to the next character that can act
            NextAct();
        }



        public void SelectCharacter(Character target)
        {
        //if whoever is attacking
            if(isAttacking)
            {
                //whoever is attacking gets to attack
              DoAttack(GetCurrentCharacter(), target);
            }
            //if using a spell
            else if(selectedSpell !=null)
            {
                //return whether or not the selected spell hits
                 if(GetCurrentCharacter().CastSpell(selectedSpell, target))
                {
                //if hits, move to the next person turn
                    uiController.UpdateCharacterUI();
                    NextAct();
                }
                 else
                 {
                Debug.LogWarning("Not enough mana to cast that spell!");
                 }
            }

        }


        //handle a player attack to a target
        public void DoAttack(Character attacker, Character target)
        {
            Debug.Log("Do attack!");
            target.Hurt(attacker.attackPower);
            NextAct();
        }






    //load in character spawn points
    public void StartBattle(List<Character> players, List<Character> enemies)
    {
        Debug.Log("Setup battle!");
        for (int i = 0; i < players.Count; i++)
        {

            //with key 0 (for  players), add a new player to the dictionary list at a spawn point (player spawn points are at 3-5 indexes so 3 is added to i)
            characters[0].Add(spawnPoints[i + 3].Spawn(players[i]));
        }
        for (int i = 0; i < enemies.Count; i++)
        {

            //with key 1 (for  enemies), add a new enemie to the dictionary list at a spawn point (enemy spawn points are at 0-2)
            characters[1].Add(spawnPoints[i].Spawn(enemies[i]));
        }



    }

    public Character GetCurrentCharacter()
    {
        return characters[actTurn][turnIndex];
    }




}
