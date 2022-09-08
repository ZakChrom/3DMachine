using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : TrackedCell
{
    public bool isActive() {
        int offsetX = 0;
        int offsetY = 0;
		int offsetZ = 0;

        switch (this.getDirection()) {
            case (Direction_e.RIGHT):
                offsetX += 1;
                break;
            case (Direction_e.DOWN):
                offsetY += -1;
                break;
            case (Direction_e.LEFT):
                offsetX += -1;
                break;
            case (Direction_e.UP):
                offsetY += 1;
                break;
			case (Direction_e.FRONT):
                offsetZ += 1;
                break;
			case (Direction_e.BACK):
                offsetZ -= 1;
                break;
        }
        //Array index error prevention
        if (this.position.x - offsetX < 0 || this.position.y - offsetY < 0 || this.position.z - offsetZ < 0)
            return false;
        if (this.position.x - offsetX >= CellFunctions.gridWidth || this.position.y - offsetY >= CellFunctions.gridHeight || this.position.z - offsetZ >= CellFunctions.gridLength)
            return false;
        if (this.position.x + offsetX < 0 || this.position.y + offsetY < 0 || this.position.z + offsetZ < 0)
            return false;
        if (this.position.x + offsetX >= CellFunctions.gridWidth || this.position.y + offsetY >= CellFunctions.gridHeight || this.position.z + offsetZ >= CellFunctions.gridLength)
            return false;
        //If we don't have a refrence cell return
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY, (int)this.position.z - offsetZ] == null)
            return false;
        return true;
    }

    public override void Step()
    {
        //Subract to find refrence, add to find target
        int offsetX = 0;
        int offsetY = 0;
		int offsetZ = 0;

        switch (this.getDirection()) {
            case (Direction_e.RIGHT):
                offsetX += 1;
                break;
            case (Direction_e.DOWN):
                offsetY += -1;
                break;
            case (Direction_e.LEFT):
                offsetX += -1;
                break;
            case (Direction_e.UP):
                offsetY += 1;
                break;
			case (Direction_e.FRONT):
                offsetZ += 1;
                break;
			case (Direction_e.BACK):
                offsetZ -= 1;
                break;
        }
        //Array index error prevention
        if (this.position.x - offsetX < 0 || this.position.y - offsetY < 0 || this.position.z - offsetZ < 0)
            return;
        if (this.position.x - offsetX >= CellFunctions.gridWidth || this.position.y - offsetY >= CellFunctions.gridHeight || this.position.z - offsetZ >= CellFunctions.gridLength)
            return;
        if (this.position.x + offsetX < 0 || this.position.y + offsetY < 0 || this.position.z + offsetZ < 0)
            return;
        if (this.position.x + offsetX >= CellFunctions.gridWidth || this.position.y + offsetY >= CellFunctions.gridHeight || this.position.z + offsetZ >= CellFunctions.gridLength)
            return;
        //If we don't have a refrence cell return
        if (CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY, (int)this.position.z - offsetZ] == null)
            return;
        //If there is a cell in our way push it :3
        if (CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY, (int)this.position.z + offsetZ] != null)
        {
            //if (CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY].cellType == CellType_e.TRASH)
            //    return;

            (bool, bool) pushResult = CellFunctions.cellGrid[(int)this.position.x + offsetX, (int)this.position.y + offsetY, (int)this.position.z + offsetZ].Push(this.getDirection(), 1);
            if (pushResult.Item2 || !pushResult.Item1)
                return;
        }

        Cell refrenceCell = CellFunctions.cellGrid[(int)this.position.x - offsetX, (int)this.position.y - offsetY, (int)this.position.z - offsetZ];
        Cell newCell = GridManager.instance.SpawnCell(
            refrenceCell.transform.gameObject,
            new Vector3((int)this.position.x + offsetX, (int)this.position.y + offsetY, (int)this.position.z + offsetZ),
			refrenceCell.transform.eulerAngles,
            refrenceCell.getDirection(),
            true
            );
        newCell.oldPosition = this.position;
        newCell.generated = true;
    }
}
