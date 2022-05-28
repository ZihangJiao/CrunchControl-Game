using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapDisplay : MonoBehaviour
{
    public Trap trap;
    public Text TrapName;
    public Text KnockBackText;
    public Text DamageText;
    public int EnemyId = 0;

    // Start is called before the first frame update
    void Start()
    {
        ShowTrap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowTrap()
    {
        TrapName.text = trap.trap_name.ToString();
        KnockBackText.text = trap.knockback.ToString();
        DamageText.text = trap.damage.ToString();
    }
}
