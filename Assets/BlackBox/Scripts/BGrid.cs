using System;
using UnityEngine;

namespace BlackBox
{
    public class BGrid : MonoBehaviour
    {
        public GameObject cellPrefab;

        private int width;
        private int height;
        private float cellSize;
        private Dir direction;
        private Vector3 origin;
        private Cell[,] gridArray;
        private Ray ray;

        public void CreateGrid(int width, int height, float cellSize, Vector3 origin, CellType cellType, Dir direction = Dir.None)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.origin = origin;
            this.direction = direction;
            gridArray = new Cell[width, height];

            SetupListeners();

            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    GameObject cellobject = Instantiate(cellPrefab, GetWorldPosition(x, y) + new Vector3(cellSize / 2, cellSize / 2), gameObject.transform.rotation, gameObject.transform);
                    Cell cell = cellobject.GetComponent<Cell>().CreateCell(x, y, cellSize, origin, cellType, direction);
                    gridArray[x, y] = cell;

                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 3f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 3f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 3f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 3f);
        }

        private void SetupListeners()
        {
            if (direction == Dir.None)
                GameEvents.FireRay.AddListener((rayOrigin, rayDirection) => FireRay(rayOrigin, rayDirection));
            else
                GameEvents.MarkUnit.AddListener((text, gridDirection, destPosition) => MarkUnits(text, gridDirection, destPosition));
        }

        private void MarkUnits(string text, Dir gridDirection, Vector3Int destPosition)
        {
            if (direction != gridDirection)
                return;

            gridArray[destPosition.x, destPosition.y].SetText(text);
        }

        private Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + origin; 
        }

        private Vector3 GetWorldPosition(Vector3 gridPosition)
        {
            return new Vector3(gridPosition.x, gridPosition.y) * cellSize + origin;
        }

        public Vector3Int GetCellPosition(Vector3 worldPosition)
        {
            Vector3Int result = new Vector3Int
            {
                x = Mathf.FloorToInt((worldPosition - origin).x / cellSize),
                y = Mathf.FloorToInt((worldPosition - origin).y / cellSize)
            };

            return result;
        }

        public void Interact(Vector3 worldPosition)
        {
            Vector3Int cellPosition = GetCellPosition(worldPosition);

            if (cellPosition.x < 0 || cellPosition.x >= width || cellPosition.y < 0 || cellPosition.y >= height)
                return;

            if (direction == Dir.None) //avoid nodes on edge cells on main grid
                if (cellPosition.x == 0 || cellPosition.x == width - 1 || cellPosition.y == 0 || cellPosition.y == height - 1)
                    return;

            gridArray[cellPosition.x, cellPosition.y].Interact();
        }

        private void FireRay(Vector3Int rayOrigin, Dir rayDirection)
        {
            ray = new Ray(rayOrigin, rayDirection, width, height);

            while (RayInPlay())
                UpdateRayPosition();

            ray.AddMarkers();
        }

        private void UpdateRayPosition()
        {
            Vector3Int oldPosition = ray.position;
            GetFrontAndDiagonalCellPositions(out Vector3Int frontMid, out Vector3Int frontRight, out Vector3Int frontLeft);

            //Check if nodes exist and update position accordingly
            if (HasNode(frontMid)) //Hit
                ray.Flip(true);
            else if (HasNode(frontRight) && HasNode(frontLeft)) //Reflect
                ray.Flip(false);
            else if (HasNode(frontRight)) //Detour Left
                ray.TurnLeft();
            else if (HasNode(frontLeft)) //Detour Right
                ray.TurnRight();

            //update position
            ray.Forward();

            //draw debug line
            Debug.DrawLine(
                GetWorldPosition(oldPosition) + new Vector3(cellSize / 2, cellSize / 2), 
                GetWorldPosition(ray.position) + new Vector3(cellSize / 2, cellSize / 2), 
                Color.white, 
                3f);
        }

        private void GetFrontAndDiagonalCellPositions(out Vector3Int frontMid, out Vector3Int frontRight, out Vector3Int frontLeft)
        {
            frontMid = ray.position;
            frontRight = ray.position;
            frontLeft = ray.position;

            switch (ray.direction)
            {
                case Dir.Left:
                    frontMid.x++;
                    frontRight.x++; frontRight.y--;
                    frontLeft.x++; frontLeft.y++;
                    break;
                case Dir.Bot:
                    frontMid.y++;
                    frontRight.x++; frontRight.y++;
                    frontLeft.x--; frontLeft.y++;
                    break;
                case Dir.Right:
                    frontMid.x--;
                    frontRight.x--; frontRight.y++;
                    frontLeft.x--; frontLeft.y--;
                    break;
                case Dir.Top:
                    frontMid.y--;
                    frontRight.x--; frontRight.y--;
                    frontLeft.x++; frontLeft.y--;
                    break;
            }
        }

        private bool HasNode(Vector3Int gridPosition)
        {
            if (gridPosition.x < 0 || gridPosition.x >= width || gridPosition.y < 0 || gridPosition.y >= height)
                return false;

            Cell cell = gridArray[gridPosition.x, gridPosition.y];
            if (cell != null)
                if (cell.hasNode)
                    return true;

            return false;
        }

        private bool RayInPlay()
        {
            bool result = true;
            if (ray.position.x < 0 || ray.position.x >= width || ray.position.y < 0 || ray.position.y >= height)
                result = false;

            return result;
        }
    }
}
