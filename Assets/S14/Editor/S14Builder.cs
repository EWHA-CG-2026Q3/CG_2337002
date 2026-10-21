using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class S14Builder
{
    const string Dir = "Assets/S14/";
    const string TexDir = "Assets/ThirdParty/Textures/";

    [MenuItem("S14/실습 3 화로에 불 피우기")]
    static void BuildFireBowl()
    {
        if (GameObject.Find("FireBowl") != null)
        {
            Debug.LogWarning("FireBowl이 이미 있습니다.");
            return;
        }

        float tableTop = FindTableTop();
        var bowlPos = new Vector3(0.1f, tableTop + 0.05f, 1.65f);

        var bowl = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        bowl.name = "FireBowl";
        bowl.transform.position = bowlPos;
        bowl.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
        bowl.GetComponent<Renderer>().sharedMaterial = FireBowlMaterial();
        Undo.RegisterCreatedObjectUndo(bowl, "S14 실습 3");

        var coals = new GameObject("Coals");
        coals.transform.position = bowlPos + Vector3.up * 0.05f;
        Undo.RegisterCreatedObjectUndo(coals, "S14 실습 3");

        var coalMat = CoalMaterial();
        Vector3[] offsets = { new(0, 0.03f, 0), new(0.06f, 0.02f, 0.03f), new(-0.05f, 0.02f, 0.04f), new(0.01f, 0.02f, -0.06f), new(-0.04f, 0.04f, -0.02f) };
        Vector3[] scales = { new(0.15f, 0.1f, 0.12f), new(0.1f, 0.07f, 0.09f), new(0.11f, 0.08f, 0.1f), new(0.09f, 0.06f, 0.11f), new(0.08f, 0.07f, 0.07f) };
        for (int i = 0; i < offsets.Length; i++)
        {
            var coal = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            coal.name = "Coal" + (i + 1);
            coal.transform.SetParent(coals.transform, false);
            coal.transform.localPosition = offsets[i];
            coal.transform.localScale = scales[i];
            coal.transform.localRotation = Quaternion.Euler(i * 37f, i * 71f, i * 23f);
            coal.GetComponent<Renderer>().sharedMaterial = coalMat;
            Object.DestroyImmediate(coal.GetComponent<Collider>());
        }

        var lightGo = new GameObject("FireLight");
        lightGo.transform.position = bowlPos + Vector3.up * 0.25f;
        var light = lightGo.AddComponent<Light>();
        light.type = LightType.Point;
        light.useColorTemperature = true;
        light.colorTemperature = 1800f;
        light.color = Color.white;
        light.range = 1.5f;
        light.intensity = 2f;
        light.shadows = LightShadows.Soft;
        Undo.RegisterCreatedObjectUndo(lightGo, "S14 실습 3");

        AssetDatabase.SaveAssets();
        EditorSceneManager.MarkSceneDirty(bowl.scene);
        Selection.activeGameObject = bowl;
    }

    static float FindTableTop()
    {
        var table = GameObject.Find("MyTable");
        foreach (var r in table.GetComponentsInChildren<Renderer>())
            if (r.name == "Top") return r.bounds.max.y;
        return 1.61f;
    }

    static Material FireBowlMaterial()
    {
        var mat = LoadOrCreate("Mat_S14_FireBowl");
        var tex = AssetDatabase.LoadAssetAtPath<Texture2D>(TexDir + "T_Metal_Rust.jpg");
        mat.SetTexture("_BaseMap", tex);
        mat.SetTexture("_MainTex", tex);
        mat.SetFloat("_Metallic", 0.5f);
        mat.SetFloat("_Smoothness", 0.25f);
        EditorUtility.SetDirty(mat);
        return mat;
    }

    static Material CoalMaterial()
    {
        var mat = LoadOrCreate("Mat_S14_Coal");
        var black = new Color(0.05f, 0.05f, 0.05f);
        mat.SetColor("_BaseColor", black);
        mat.SetColor("_Color", black);
        mat.SetFloat("_Smoothness", 0.1f);
        mat.EnableKeyword("_EMISSION");
        mat.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        mat.SetTexture("_EmissionMap", AssetDatabase.LoadAssetAtPath<Texture2D>(TexDir + "T_Lava.jpg"));
        mat.SetColor("_EmissionColor", new Color(191 / 255f, 117 / 255f, 3 / 255f) * Mathf.Pow(2f, 5f));
        EditorUtility.SetDirty(mat);
        return mat;
    }

    static Material LoadOrCreate(string name)
    {
        string path = Dir + name + ".mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat != null) return mat;
        mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }
}
