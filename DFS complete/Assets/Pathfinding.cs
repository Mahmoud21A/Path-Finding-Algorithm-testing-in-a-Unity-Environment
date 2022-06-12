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

		Stack<Node> openSet = new Stack<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Push(startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet.Pop();
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

					if (!openSet.Contains(neighbour)){
						neighbour.parent = currentNode;
						openSet.Push(neighbour);
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
}
