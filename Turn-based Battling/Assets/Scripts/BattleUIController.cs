using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIController : MonoBehaviour
{
    [SerializeField]
    private GameObject spellPanel;
    [SerializeField]
    private Button[] actionButtons;
    [SerializeField]
    private Button button;
    [SerializeField]
    private Text[] characterInfo;



    // Start is called before the first frame update
    void Start()
    {
        //make sure spell panel is not active when the game starts
        spellPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //check to see if there is a character under the mouse when we click using a ray cast
        //converting where the mouse is in screen space to where the camera is in world space


        //if we are clicking down
        if(Input.GetMouseButtonDown(0))
        {
            //convert mouse position in screenspace to convert to a ray
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if a raycast is hit from the mouse's origin position
            RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction);

            //if the mouse is colliding with a character in the game with tag "Character"
            if(hitInfo.collider !=null && hitInfo.collider.CompareTag("Character"))
            {
                //get the character that is selected
                BattleController.Instance.SelectCharacter(hitInfo.collider.GetComponent<Character>());
            }


        }

    }

    //show the spell list based on whos turn it is
    //turn off the panel depending on the state of battle
    public void ToggleSpellPanel(bool state)
    {
        spellPanel.SetActive(state);
        if(state == true)
        {
            //grab the list of spells of a specific character if its their turn
            BuildSpellList(BattleController.Instance.GetCurrentCharacter().spells);
        }
    }

    //Toggle action state depending on battle
    public void ToggleActionState(bool state)
    {
        ToggleSpellPanel(state);
        foreach(Button button in actionButtons)
        {
            //toggles button interactibility depedning on whether player is attacking or not
            button.interactable = state;
        }

    }



    //build out the player spell list depending on the character
    public void BuildSpellList(List<Spell> spells)
    {
        //make sure there are no buttons added already; we start fresh
        //childCount gives the number of children in the panel
        if(spellPanel.transform.childCount > 0)
        {
            //grabs the reference the the button that are children of spell panel and delete it
            foreach(Button button in spellPanel.transform.GetComponentsInChildren<Button>())
            {
                Destroy(button.gameObject);
            }
        }

        // for each spell in the spell collection create a button as a child and populate the spell name
        foreach(Spell spell in spells)
        {
            Button spellButton = Instantiate<Button>(button, spellPanel.transform);
            spellButton.GetComponentInChildren<Text>().text = spell.spellName;
            //add a listener to  be called whenever a spell is selected to assign to a button
            spellButton.onClick.AddListener(() => SelectSpell(spell));


        }

    }




    void SelectSpell(Spell spell)
    {
        //if we have selected a spell use that spell and make sure BatllerController knows we are not melee attacking 
        BattleController.Instance.selectedSpell = spell;
        BattleController.Instance.isAttacking = false;
    }

    public void SelectAttack()
    {
        //logging attack
        Debug.Log("Attack selected...");
        //if we have selected a melee attack use that attack and make sure BatllerController knows we are not using a spell
        BattleController.Instance.selectedSpell = null;
        BattleController.Instance.isAttacking = true;
    }

    //update chracter information based on the battle
    public void UpdateCharacterUI()
    {
        for(int i=0; i < BattleController.Instance.characters[0].Count; i++)
        {
            //get each character 
            Character character = BattleController.Instance.characters[0][i];
            //set the text value of the character to the current battle info
            characterInfo[i].text = string.Format("{0} hp: {1}/{2}, mp: {3}", character.characterName, character.health, character.maxHealth, character.manaPoints);
        }
    }

    //have character defend
    public void Defend()
    {
        BattleController.Instance.GetCurrentCharacter().Defend();
    }




}
