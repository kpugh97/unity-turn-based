using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Character assets
    public string characterName;
    public int health;
    public int maxHealth;
    public int attackPower;
    public int defensePower;
    public int manaPoints;
    public List<Spell> spells;


    //if character is hurt subtract health based on attack power of enemy and defense of character that is attacked
    public void Hurt (int amount)
    {
        //calculate damage done based on defense
        int damageAmount = amount-defensePower;

        //set health to the largest value; does not go below 0
        health = Mathf.Max(health - damageAmount, 0);

        //if target has no more health ( is 0 )
        if(health == 0)
        {
            Die();
        }


    }
   

    //calculate health restored to a character
    public void Heal (int amount)
    {
        //calculate healing based on max health
        int healAmount = amount;

        //set health to the smallest value; does not go above the max health
        health = Mathf.Min(health + healAmount, maxHealth);
    }

    //calculate defnse boost to a character
    public void Defend()
    {
        defensePower += (int)(defensePower * .33f);
        Debug.Log("Defense increase!");
    }

    //calculate if a character can cast a spell
    public bool CastSpell(Spell spell, Character target)
    {
        //decide whether there is enough mana to cast spell
        bool success = manaPoints >= spell.manaCost;

        //if success, cast spell
        if (success)
        {
            //creates a spell object in game
            Spell spellToCast = Instantiate<Spell>(spell, transform.position, Quaternion.identity);
            manaPoints -= spell.manaCost;
            spellToCast.Cast(target);
        }

        return success;
    }


    //log character death
    public virtual void Die()
    {
        //destroy character if dead
        Destroy(this.gameObject);
        Debug.LogFormat("{0} has died!", characterName);
    }

}
