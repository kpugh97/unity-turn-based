using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    //randomize enemy actions
    public void Act()
    {
        int action = Random.Range(0, 2);
        Character target = BattleController.Instance.GetRandomPlayer();
        switch(action)
        {
            case 0:
                //use defensive spell
                Defend();
                break;
            case 1:
                //use a spell
                Spell spellToCast = GetRandomSpell();
                //if a friendly spell heal a friendly weak enemy 
                if(spellToCast.spellType == Spell.SpellType.Heal)
                {
                    //get weakest friendly enemy target to heal
                    target = BattleController.Instance.GetWeakestEnemy();
                }

                if(!CastSpell(spellToCast, target))
                {
                    //attack
                    BattleController.Instance.DoAttack(this, target);

                }
                break;
            case 2:
                //use an attack
                BattleController.Instance.DoAttack(this, target);

                break;

        }


    }

    Spell GetRandomSpell()
    {
        return spells[Random.Range(0, spells.Count - 1)];
    }



    public override void Die()
    {
        base.Die();
        //remove this enemy that has died from the collection
        BattleController.Instance.characters[1].Remove(this);
    }


    
}
