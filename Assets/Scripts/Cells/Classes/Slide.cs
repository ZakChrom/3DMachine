using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : Cell
{
    public override (bool, bool) Push(Direction_e dir, int bias)
    {
        if(dir == this.getDirection() || ((int)dir + 3) % 6 == (int)this.getDirection())
            return base.Push(dir, bias);
        return (false, false);
    }
}
