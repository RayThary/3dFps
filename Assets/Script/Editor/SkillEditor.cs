using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(UnitSkill))]
public class SkillEditor : Editor
{
    private bool showMissile = true;
    private bool showMissileMaxLevel = true;
    private bool showShockwave = true;
    private bool showShockwaveMaxLevel = true;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();


        UnitSkill skill = (UnitSkill)target;

        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("outLayer"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("skillSpawnTrs"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("useSkill"));

        EditorGUILayout.Space();

        switch (skill.SkillName)
        {
            case UnitSkill.eSkillName.ThrowMissile:
                DrawMissileSection();
                DrawMissileMaxLevelSection();
                break;

            case UnitSkill.eSkillName.Shockwave:
                DrawShockwaveSection();
                DrawShockwaveMaxLevelSection();
                break;
        }

        serializedObject.ApplyModifiedProperties();
    }


    private void DrawMissileSection()
    {
        showMissile = EditorGUILayout.Foldout(showMissile, "Missile Settings");

        if (showMissile)
        {
            var missile = serializedObject.FindProperty("throwMissile");

            EditorGUILayout.PropertyField(missile.FindPropertyRelative("damage"));
            EditorGUILayout.PropertyField(missile.FindPropertyRelative("missileCount"));
            EditorGUILayout.PropertyField(missile.FindPropertyRelative("fireInterval"));
            EditorGUILayout.PropertyField(missile.FindPropertyRelative("coolTime"));
            EditorGUILayout.PropertyField(missile.FindPropertyRelative("missileSpeed"));

            EditorGUILayout.Space(5);

            EditorGUILayout.PropertyField(missile.FindPropertyRelative("damageUp"));
            EditorGUILayout.PropertyField(missile.FindPropertyRelative("coolDownRate"));
            EditorGUILayout.PropertyField(missile.FindPropertyRelative("missileCountUp"));
            EditorGUILayout.PropertyField(missile.FindPropertyRelative("fireIntervalUp"));
            EditorGUILayout.PropertyField(missile.FindPropertyRelative("missileSpeedUp"));
            EditorGUILayout.PropertyField(missile.FindPropertyRelative("criticalEnable"));
        }
    }

    private void DrawMissileMaxLevelSection()
    {
        showMissileMaxLevel = EditorGUILayout.Foldout(showMissileMaxLevel, "Missile MaxLevel Settings");

        if (showMissileMaxLevel)
        {
            var missileLevel = serializedObject.FindProperty("throwMissileUpgradeLevel");

            EditorGUILayout.PropertyField(missileLevel.FindPropertyRelative("damageMaxLevel"));
            EditorGUILayout.PropertyField(missileLevel.FindPropertyRelative("coolDownMaxLevel"));
            EditorGUILayout.PropertyField(missileLevel.FindPropertyRelative("missileCountMaxLevel"));
            EditorGUILayout.PropertyField(missileLevel.FindPropertyRelative("fireIntervalMaxLevel"));
            EditorGUILayout.PropertyField(missileLevel.FindPropertyRelative("missileSpeedMaxLevel"));
            EditorGUILayout.PropertyField(missileLevel.FindPropertyRelative("criticalEnableMaxLevel"));
        }
    }
    private void DrawShockwaveSection()
    {
        showShockwave = EditorGUILayout.Foldout(showShockwave, "Shockwave Settings");

        if (showShockwave)
        {
            var shockwave = serializedObject.FindProperty("shockwave");

            EditorGUILayout.PropertyField(shockwave.FindPropertyRelative("damage"));
            EditorGUILayout.PropertyField(shockwave.FindPropertyRelative("coolTime"));
            EditorGUILayout.PropertyField(shockwave.FindPropertyRelative("shockwaveRadius"));

            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(shockwave.FindPropertyRelative("damageUp"));
            EditorGUILayout.PropertyField(shockwave.FindPropertyRelative("coolTimeUp"));
            EditorGUILayout.PropertyField(shockwave.FindPropertyRelative("radiusUp"));
            EditorGUILayout.PropertyField(shockwave.FindPropertyRelative("slowAmount"));
            EditorGUILayout.PropertyField(shockwave.FindPropertyRelative("residueDuration"));
            EditorGUILayout.PropertyField(shockwave.FindPropertyRelative("doubleShockwave"));

            EditorGUILayout.Space(5);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("debugSlamRadius"));
        }
    }

    private void DrawShockwaveMaxLevelSection()
    {
        showShockwaveMaxLevel = EditorGUILayout.Foldout(showShockwaveMaxLevel, "MaxLevel Settings");

        var shockLevel = serializedObject.FindProperty("shockwaveUpgradeLevel");
        if (showShockwaveMaxLevel)
        {
            EditorGUILayout.PropertyField(shockLevel.FindPropertyRelative("damageMaxLevel"));
            EditorGUILayout.PropertyField(shockLevel.FindPropertyRelative("coolTimeMaxLevel"));
            EditorGUILayout.PropertyField(shockLevel.FindPropertyRelative("radiusMaxLevel"));
            EditorGUILayout.PropertyField(shockLevel.FindPropertyRelative("slowMaxLevel"));
            EditorGUILayout.PropertyField(shockLevel.FindPropertyRelative("residueMaxLevel"));
            EditorGUILayout.PropertyField(shockLevel.FindPropertyRelative("doubleShockwaveMaxLevel"));
        }
    }
}
