using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap
{
    public string trap_name;
    public int damage;
    public int knockback;
    public int pos;
    // Start is called before the first frame update
    public Trap(string trapname, int damage, int knockback,int pos)
    {
        this.trap_name = trapname;
        this.damage = damage;
        this.knockback = knockback;
        this.pos = pos;
    }
}
