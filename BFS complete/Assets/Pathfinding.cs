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

		List<Node> openSet = new List<Node>();
		HashSet<Node> closedSet = new HashSet<Node>();
		openSet.Add(startNode);

		while (openSet.Count > 0) {
			Node currentNode = openSet[0];
			openSet.Remove(currentNode);
			closedSet.Add(currentNode);
			for (int i = 1; i < openSet.Count; i ++) {
				if(openSet[i].gCost < currentNode.gCost){
					currentNode = openSet[i];
				}
			}

			openSet.Remove(currentNode);
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
						openSet.Add(neighbour);
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
