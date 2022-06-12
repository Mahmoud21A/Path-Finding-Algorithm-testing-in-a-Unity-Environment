using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Pathfinding : MonoBehaviour {

	public Transform seeker, target;
	Grid grid;

	void Awake() {
		grid = GetComponent<Grid> ();
	}

	void Update() {
		FindPath (seeker.position, target.position);
	}

	void FindPath(Vector3 startPos, Vector3 targetPos) {
		var watch = Stopwatch.StartNew();
		Node startNode = grid.NodeFromWorldPoint(startPos);
		Node targetNode = grid.NodeFromWorldPoint(targetPos);

		SortedList<double, Node> openSet = new SortedList<double, Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(0, startNode);
		double i = 0;

		while (openSet.Count > 0) {
			i = i + 0.00000001;
			Node currentNode = openSet.Values[0];
			openSet.RemoveAt(0);
			closedSet.Add(currentNode);

			if (currentNode == targetNode) {
				grid.openSet = openSet;
				grid.closedSet = closedSet;
				RetracePath(startNode,targetNode);
				UnityEngine.Debug.Log("Time elapsed: " + watch.ElapsedMilliseconds + " Milliseconds");
				return;
			}

			foreach (Node neighbour in grid.GetNeighbours(currentNode)) {
				if (!neighbour.walkable || closedSet.Contains(neighbour)) {
					continue;
				}

					if (!openSet.ContainsValue(neighbour) && !openSet.ContainsKey(GetDistance(startNode, neighbour)+i)){
						neighbour.parent = currentNode;
						openSet.Add((GetDistance(startNode, neighbour)+i), neighbour);
					}

			}
		}
	}

	void RetracePath(Node startNode, Node endNode) {
		List<Node> path = new List<Node>();
		Node currentNode = endNode;

		while (currentNode != startNode) {
			path.Add(currentNode);
			currentNode = currentNode.parent;
		}
		path.Reverse();

		grid.path = path;

	}

	int GetDistance(Node nodeA, Node nodeB) {
		int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
		int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

		if (dstX > dstY)
			return 14*dstY + 10* (dstX-dstY);
		return 14*dstX + 10 * (dstY-dstX);
	}
}
