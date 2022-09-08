using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackedCell : Cell
{
    public LinkedListNode<Cell> trackedCellNode;
    //public LinkedListNode<Cell> trackedCellSortNode;
    public bool queuedForPositionSorting = false;
    public bool queuedForRotationSorting = false;

    void queueForRotationSorting()
    {
        if (this.queuedForRotationSorting)
            return;
     
        queuedForRotationSorting = true;
        queuedForPositionSorting = true;

        trackedCellNode.List.Remove(trackedCellNode);
        CellFunctions.trackedCellRotationUpdateQueue[CellFunctions.GetSteppedCellId(cellType)].AddLast(trackedCellNode);
    }

    public void queueForPositionSorting() {
        if (this.queuedForPositionSorting)
            return;

        queuedForPositionSorting = true;

        trackedCellNode.List.Remove(trackedCellNode);
        CellFunctions.trackedCellPositionUpdateQueue[CellFunctions.GetSteppedCellId(cellType), (int)this.getDirection()].AddLast(trackedCellNode);
    }
    //Base.Step() should always be called first in derived classes to mantain suppressed functionality
    public virtual void Step()
    {
    }

    public uint GetDistanceFromFacingEdge() {
        switch (this.getDirection())
        {
			case (Direction_e.FRONT):
                return (uint)(CellFunctions.gridLength - this.position.z - 1);
            case (Direction_e.RIGHT):
                return (uint)(CellFunctions.gridWidth - this.position.x - 1);
            case (Direction_e.DOWN):
                return (uint)this.position.y;
			case (Direction_e.BACK):
                return (uint)this.position.z;
            case (Direction_e.LEFT):
                return (uint)this.position.x;
			case (Direction_e.UP):
                return (uint)(CellFunctions.gridHeight - this.position.y - 1);
            default:
                break;
        }
        return 0;
    }

    public override (bool, bool) Push(Direction_e dir, int bias)
    {
        (bool, bool) pushResult = base.Push(dir, bias);
        if (pushResult.Item1 && !pushResult.Item2) {
            if ((dir == this.getDirection() || dir == (Direction_e)(((int)this.getDirection() + 3) % 6))  && !deleted) {
                this.queueForPositionSorting();
            }
        }
        return pushResult;
    }

    public override void FaceDirection(Direction_e dir)
    {
        if ((int)dir != rotation)
            this.queueForRotationSorting();
        base.FaceDirection(dir);
    }

    public override void Rotate(int amount)
    {
        base.Rotate(amount);
        if (amount % 6 != 0)
        {
            this.queueForRotationSorting();
        }
    }

    public override void Delete(bool destroy)
    {
        if(trackedCellNode != null && trackedCellNode.List != null)
            trackedCellNode.List.Remove(trackedCellNode);
        trackedCellNode = null;
        
        queuedForRotationSorting = false;
        queuedForPositionSorting = false;
        base.Delete(destroy);
    }

    public override void Setup(Vector3 position, Direction_e rotation, bool generated)
    {
        base.Setup(position, rotation, generated);
        if (trackedCellNode == null)
        {
            trackedCellNode = CellFunctions.trackedCells
                [CellFunctions.GetSteppedCellId(this.cellType), (int)this.getDirection()]
                [this.GetDistanceFromFacingEdge()].AddLast(this);
        }
        else
        {
            print("Error in setting up tracked cell; the trackedCellNode was already set when Setup() was called");
        }
    }
}