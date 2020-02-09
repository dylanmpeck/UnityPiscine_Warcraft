using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [SerializeField] List<GameObject> tiles;

    [SerializeField] int width;
    [SerializeField] int height;

    static bool[,] tilesMap;

    static PathFind.Grid grid;

    static bool foundPath;

    // Start is called before the first frame update
    void Start()
    {
        tilesMap = new bool[width, height];

        foreach (GameObject tile in tiles)
        {
            if (tile.name.Contains("Water") || tile.name.Contains("Rock"))
            {
                tilesMap[(int)tile.transform.localPosition.x, (int)-tile.transform.localPosition.y] = false;
            }
            else
            {
                tilesMap[(int)tile.transform.localPosition.x, (int)-tile.transform.localPosition.y] = true;
            }
        }

        grid = new PathFind.Grid(width, height, tilesMap);
    }

    public static Stack<PathFind.Point> GetPath(int startX, int startY, int endX, int endY)
    {
        PathFind.Point _from = new PathFind.Point(startX, startY);
        PathFind.Point _to = new PathFind.Point(endX, endY);

        foundPath = false;
        Queue<PathFind.Point> q = new Queue<PathFind.Point>();
        _from.parent = null;
        q.Enqueue(_from);
        BFS(q, new bool[19, 11], _to);

        PathFind.Point traverse = _to;
        Stack<PathFind.Point> path = new Stack<PathFind.Point>();
        while (traverse != null)
        {
            path.Push(new PathFind.Point(traverse.x, -traverse.y));
            traverse = traverse.parent;
        }
        if (path.Count > 0)
            path.Pop();
        return path;
    }

    static void BFS(Queue<PathFind.Point> queue, bool[,] visited, PathFind.Point dest)
    {
        if (queue.Count == 0)
            return;

        PathFind.Point p = queue.Dequeue();

        if (foundPath)
            return;

        if (p == dest)
        {
            //Debug.Log("Found Path");
            dest.parent = p.parent;
            foundPath = true;
            return;
        }

        if (p.x - 1 >= 0 && !visited[p.x - 1, p.y] && tilesMap[p.x - 1, p.y])
        {
            visited[p.x - 1, p.y] = true;
            queue.Enqueue(new PathFind.Point(p.x - 1, p.y, p));
        }
        if (p.x + 1 < 19 && !visited[p.x + 1, p.y] && tilesMap[p.x + 1, p.y])
        {
            visited[p.x + 1, p.y] = true;
            queue.Enqueue(new PathFind.Point(p.x + 1, p.y, p));
        }
        if (p.y - 1 >= 0 && !visited[p.x, p.y - 1] && tilesMap[p.x, p.y - 1])
        {
            visited[p.x, p.y - 1] = true;
            queue.Enqueue(new PathFind.Point(p.x, p.y - 1, p));
        }
        if (p.y + 1 < 11 && !visited[p.x, p.y + 1] && tilesMap[p.x, p.y + 1])
        {
            visited[p.x, p.y + 1] = true;
            queue.Enqueue(new PathFind.Point(p.x, p.y + 1, p));
        }
        if (p.y + 1 < 11 && p.x + 1 < 19 && !visited[p.x + 1, p.y + 1] && tilesMap[p.x + 1, p.y + 1])
        {
            visited[p.x + 1, p.y + 1] = true;
            queue.Enqueue(new PathFind.Point(p.x + 1, p.y + 1, p));
        }
        if (p.y + 1 < 11 && p.x - 1 >= 0 && !visited[p.x - 1, p.y + 1] && tilesMap[p.x - 1, p.y + 1])
        {
            visited[p.x - 1, p.y + 1] = true;
            queue.Enqueue(new PathFind.Point(p.x - 1, p.y + 1, p));
        }
        if (p.y - 1 >= 0 && p.x + 1 < 19 && !visited[p.x + 1, p.y - 1] && tilesMap[p.x + 1, p.y - 1])
        {
            visited[p.x + 1, p.y - 1] = true;
            queue.Enqueue(new PathFind.Point(p.x + 1, p.y - 1, p));
        }
        if (p.y - 1 >= 0 && p.x - 1 >= 0 && !visited[p.x - 1, p.y - 1] && tilesMap[p.x - 1, p.y - 1])
        {
            visited[p.x - 1, p.y - 1] = true;
            queue.Enqueue(new PathFind.Point(p.x - 1, p.y - 1, p));
        }

        BFS(queue, visited, dest);
    }
}
