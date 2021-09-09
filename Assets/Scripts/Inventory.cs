using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public class Stuff
    {
        public int bullets;
        public int grenades;

        public Stuff()
        {
            bullets = 1;
            grenades = 1;
        }
        public Stuff(int bul, int gre)
        {
            bullets = bul;
            grenades = gre;
        }
    }

    public Stuff stuff = new Stuff(999,999);



}
