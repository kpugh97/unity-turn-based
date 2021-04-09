using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartyMember : Character
{
    public override void Die()
    {
        //do what happens in parent class Die method
        base.Die();
        //remove this party member that has died from the collection
        BattleController.Instance.characters[0].Remove(this);

    }


}
