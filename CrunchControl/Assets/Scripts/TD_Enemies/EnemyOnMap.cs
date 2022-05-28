using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOnMap
{
    // Start is called before the first frame update
    public int id;
    public int alive;
/*    public float posx;
    public float posy;
    public float posz;*/

    //public Vector3 pos;

    // Update is called once per frame
    public EnemyOnMap(int id, int alive)
    {
        this.id = id;
        this.alive = alive;
        /*        , float posx, float posy, float posz
         *        this.posx = posx;
                this.posy = posy;
                this.posz = posz;*/
    }
}
