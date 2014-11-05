using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(RandomTool))]
public class RandomToolEditor : Editor 
{
	public override void OnInspectorGUI()
	{
		RandomTool mySpawnTool = (RandomTool)target;
		
		EditorGUILayout.LabelField ("Points", mySpawnTool.spawnPoints.Count.ToString ());
		if (GUILayout.Button ("Print Points")) {
			mySpawnTool.getPoints ();
		}
		mySpawnTool.thePoint = EditorGUILayout.Vector3Field ("New Point", mySpawnTool.thePoint);
		if (GUILayout.Button ("Create Point")) 
		{
			mySpawnTool.addPoint ();
		}
		if (GUILayout.Button ("Remove Point")) {
			mySpawnTool.removePoint();
		}
		if (GUILayout.Button ("Remove All")) {
			mySpawnTool.deletePoints ();
		}
		mySpawnTool.chasePoints = EditorGUILayout.Toggle ("Movement", mySpawnTool.chasePoints);
		
		mySpawnTool.speed = EditorGUILayout.FloatField (mySpawnTool.speed);
	}
}