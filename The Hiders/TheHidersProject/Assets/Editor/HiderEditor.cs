using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Hiders))]
public class HiderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Hiders myHidersScript = (Hiders)target;

        myHidersScript.speed = EditorGUILayout.FloatField("Speed", myHidersScript.speed);
        //EditorGUILayout.ObjectField("Caught UI", myHidersScript.caughtUI, typeof(GameObject), true);
        EditorGUILayout.LabelField("Player Number", myHidersScript.playerNumber.ToString());
        EditorGUILayout.LabelField("Is Seeker", myHidersScript.isSeeker.ToString());
        EditorGUILayout.LabelField("Current Ability", myHidersScript.currentAbility.ToString());
    }
}
