using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CellFunctions : MonoBehaviour
{	
	public GameObject gridManager;
	public GameObject player;
	public static Vector3[] rotations;
	public static int rotIndex;
	
	private static GameObject[] inventory;
	
	void Start() {
		inventory = player.GetComponent<Placing>().inventory;
		rotations = Placing.rotations;
		gridHeight = 300;
		gridWidth = 300;
		gridLength = 300;
		gridManager.GetComponent<GridManager>().InitGridSize();
	}
	void Update() {
		if (Input.GetKeyDown("r")) {
			GridManager.playSimulation = !GridManager.playSimulation;
		}
		if (Input.GetKeyDown("f")) {
			Reset();
		}
	}
	public void Reset() {
        GridManager.instance.Reset();

        LinkedListNode<Cell> selectedNode = CellFunctions.generatedCellList.First;
		{
            Cell cell;
            while (selectedNode != null)
            {
                cell = selectedNode.Value;
                selectedNode = selectedNode.Next;
                cell.Delete(true);
            }
        }

        CellFunctions.generatedCellList.Clear();

        foreach (Cell cell in CellFunctions.cellList)
        {
            cell.Delete(false);
        }

        foreach (Cell cell in CellFunctions.cellList)
        {
            cell.gameObject.SetActive(true);
            cell.Setup(cell.spawnPosition, (Direction_e)cell.spawnRotation, false);
            cell.suppresed = false;
        }
        GridManager.enemyCount = GridManager.initialEnemyCount;
	}
	public void Save() {
		GUIUtility.systemCopyBuffer = Worlds.Save();
	}
	public void Load() {
		Worlds.Load();
	}
	
	public static Direction_e[] directionUpdateOrder = { Direction_e.RIGHT, Direction_e.LEFT, Direction_e.UP, Direction_e.DOWN , Direction_e.FRONT , Direction_e.BACK };
    public static CellType_e[] cellUpdateOrder = {CellType_e.GENERATOR, CellType_e.FIXEDROTATOR, CellType_e.CWROTATER, CellType_e.CCWROTATER, CellType_e.MOVER};
    public static Dictionary<CellType_e, CellUpdateType_e> cellUpdateTypeDictionary = new Dictionary<CellType_e, CellUpdateType_e>
    {
		[CellType_e.GENERATOR] = CellUpdateType_e.TRACKED,
        [CellType_e.MOVER] = CellUpdateType_e.TRACKED,
        [CellType_e.WALL] = CellUpdateType_e.BASE,
        [CellType_e.TRASH] = CellUpdateType_e.BASE,
		[CellType_e.FIXEDROTATOR] = CellUpdateType_e.TICKED,
		[CellType_e.ENEMY] = CellUpdateType_e.BASE,
		[CellType_e.SLIDE] = CellUpdateType_e.BASE,
        [CellType_e.PUSH] = CellUpdateType_e.BASE,
		[CellType_e.CWROTATER] = CellUpdateType_e.TICKED,
        [CellType_e.CCWROTATER] = CellUpdateType_e.TICKED
    };

    //An disctionary defining the specialized ID's used in sorting, for tracked and 
    public static Dictionary<CellType_e, int> steppedCellIdDictionary = new Dictionary<CellType_e, int>();

    public static int gridWidth = 1;
    public static int gridHeight = 1;
	public static int gridLength = 1;
    //Used to check which cell might be at location x, y, z.
    public static Cell[,,] cellGrid;
    //Used to check if x, y, z is considered a placeable tile.
    public static bool[,,] placeableTiles;

    public static LinkedList<Cell> cellList;
    //Cells made during the simulation
    public static LinkedList<Cell> generatedCellList;

    //Cells that need to be updated but not in a specific order.
    //tickedCellList[CellType];
    public static LinkedList<Cell>[] tickedCellList;

    //Cells that need to be updated but in a specific order (Depending on direction).
    //trackedCells[TrackedCell ID][Direction, Distince];
    //public static LinkedList<Cell>[][,] trackedCells;
    // changed to [,][]
    // the jagged array is 2 dimensional
    public static LinkedList<Cell>[,][] trackedCells;


    //trackedCellRotationUpdateQueue[CellType] Cell type must be sorted into a new direction queue if it has been rotated since it was last sorted
    public static LinkedList<Cell>[] trackedCellRotationUpdateQueue;
    //trackedCellPositionUpdateQueue[CellType, Cell Direction] Cell type "X" facing direction "Y" must be sorted before Cells of type X facing direction Y are updated
    public static LinkedList<Cell>[,] trackedCellPositionUpdateQueue;

    public static int GetSteppedCellId(CellType_e type) {
		return steppedCellIdDictionary[type];
    }

    public static CellUpdateType_e GetUpdateType(CellType_e type) {
        return cellUpdateTypeDictionary[type];
    }
	
	public static CellType_e PrefabToCellType_e(GameObject prefab) {
		for (int i = 0; i < inventory.Length; i++) {
			if (prefab == inventory[i]) {
				return (CellType_e)i;
			}
		}
		throw new InvalidCastException("Invalid prefab: " + prefab.name);
	}
	public static CellType_e StringToCellType_e(string name) {
		return (CellType_e)Enum.Parse(typeof(CellType_e), name);
	}
	public static CellType_e IntToCellType_e(int num) {
		int numValues = Enum.GetValues(typeof(CellType_e)).Length;
		if (num >= 0 && num < numValues) {
			return (CellType_e)num;
		} else {
			throw new InvalidCastException("Invalid cell type: " + num);
		}
	}
	public static Direction_e Vector3ToDirection_e(Vector3 dir) {
		var map = new Dictionary<Vector3, Direction_e> {
			{ rotations[0], Direction_e.FRONT },
			{ rotations[1], Direction_e.RIGHT },
			{ rotations[2], Direction_e.DOWN },
			{ rotations[3], Direction_e.BACK },
			{ rotations[4], Direction_e.LEFT },
			{ rotations[5], Direction_e.UP }
		};

		Direction_e result;
		if (map.TryGetValue(dir, out result)) {
			return result;
		} else {
			throw new InvalidCastException("Invalid direction: " + dir);
		}
	}
	public static Vector3 Direction_eToVector3(Direction_e dir) {
		switch (dir) {
			case Direction_e.FRONT:
				return rotations[0];
			case Direction_e.RIGHT:
				return rotations[1];
			case Direction_e.DOWN:
				return rotations[2];
			case Direction_e.BACK:
				return rotations[3];
			case Direction_e.LEFT:
				return rotations[4];
			case Direction_e.UP:
				return rotations[5];
			default:
				throw new InvalidCastException("Invalid direction: " + dir);
		}
	}
}