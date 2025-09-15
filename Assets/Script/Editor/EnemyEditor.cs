using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy))]
public class EnemyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Enemy enemy = (Enemy)target;

        enemy.Type = (Enemy.eEnemyCategory)EditorGUILayout.EnumPopup("Enemy Type", enemy.Type);
        
        if (enemy.Type == Enemy.eEnemyCategory.Boss)
        {
            enemy.MissilePort1 = (Transform)EditorGUILayout.ObjectField("Boss Projectile 1", enemy.MissilePort1, typeof(Transform), true);
            enemy.MissilePort2 = (Transform)EditorGUILayout.ObjectField("Boss Projectile 2", enemy.MissilePort2, typeof(Transform), true);
        }

        if (GUI.changed)
            EditorUtility.SetDirty(enemy);
    }
}


