using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : MonoBehaviour
{
    //spell aspects
    public string spellName;
    public int power;
    public int manaCost;
    public enum SpellType { Attack, Heal }
    public SpellType spellType;

    //reference position of target
    private Vector3 targetPosition;


    //update frames to move spells to target position
    private void Update()
    {
        //check if characters are at pos(0,0,0) which shouldn't happen
        if(targetPosition != Vector3.zero)
        {
            // move 15% of a unit toward target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, .15f);

            //if spell is within target position, get rid of the spell
            if(Vector3.Distance(transform.position, targetPosition) < .25f)
            {
                //destory game object 
                Destroy(this.gameObject, 1);
            }
        }
        //else if a spell without a given position exists, destroy it
        else
        {
            Destroy(this.gameObject);
        }
    }




    //cast a spell at a target
    //pass in the target info to direct the spell attack to that postition
    public void Cast(Character target)
    {
        targetPosition = target.transform.position;

        //check spell type to determine what it does next
        Debug.Log(spellName + " was cast on " + target.name + "!");
        if(spellType == SpellType.Attack)
        {
            //hurt character
            target.Hurt(power);


        }
        else if(spellType == SpellType.Heal)
        {
            //heal character
            target.Heal(power);

        }
    }
}
