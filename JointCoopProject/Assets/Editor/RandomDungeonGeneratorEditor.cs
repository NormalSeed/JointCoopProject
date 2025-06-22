using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGenerator),true)]
public class RandomDungeonGeneratorEditor : Editor
{
    private AbstractDungeonGenerator _generator;

    private void Awake()
    {
        _generator = (AbstractDungeonGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate Dungeon"))
        {
            _generator.GenerateDungeon();
        }
    }
}
