using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedRotater : TickedCell
{
    protected int rotationAmount = 1;

    int[][] rotationOffsets = new int[][] { 
        new int[] {1, 0, 0}, 
        new int[] { 0, -1, 0}, 
        new int[] { -1, 0, 0}, 
        new int[] { 0, 1, 0},
        new int[] { 0, 0, 1},
        new int[] { 0, 0, -1}
    };

    void rotateCell(int xOffset, int yOffset, int zOffset)
    {

        if (this.position.x + xOffset >= CellFunctions.gridWidth || this.position.y + yOffset >= CellFunctions.gridHeight || this.position.z + zOffset >= CellFunctions.gridLength)
            return;
        if (this.position.x + xOffset < 0 || this.position.y + yOffset < 0 || this.position.z + zOffset < 0)
            return;
        if (CellFunctions.cellGrid[(int)this.position.x + xOffset, (int)this.position.y + yOffset, (int)this.position.z + zOffset] == null)
            return;
        
        while (CellFunctions.cellGrid[(int)this.position.x + xOffset, (int)this.position.y + yOffset, (int)this.position.z + zOffset].rotation != this.rotation)
        {
            CellFunctions.cellGrid[(int)this.position.x + xOffset, (int)this.position.y + yOffset, (int)this.position.z + zOffset].Rotate(rotationAmount);
        }
    }
    public override void Step()
    {
        
        foreach (int[] offset in rotationOffsets)
        {
            rotateCell(offset[0], offset[1], offset[2]);
        }
    }
}