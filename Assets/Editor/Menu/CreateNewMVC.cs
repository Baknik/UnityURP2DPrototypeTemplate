using UnityEngine;
using UnityEditor;
using System.IO;

public class MVCWindow : EditorWindow
{
    private string _objectName = string.Empty;
    private int _selectedIndex = 0;
    private string[] _objectList = { };

	[MenuItem("Window/MVC")]
	public static void ShowWindow()
    {
		GetWindow<MVCWindow>("MVC");
    }

    private void OnGUI()
    {
        string controllerScriptDirectoryPath = GetControllerScriptDirectoryPath();
        string[] controllerScriptFilePaths = Directory.GetFiles(controllerScriptDirectoryPath);
        _objectList = new string[controllerScriptFilePaths.Length];
        for (int i=0; i< controllerScriptFilePaths.Length; i++)
        {
            if (Path.GetExtension(controllerScriptFilePaths[i]).Equals(".cs"))
            {
                _objectList[i] = CleanUpName(Path.GetFileNameWithoutExtension(controllerScriptFilePaths[i]));
            }
        }

        EditorGUILayout.Space(10f);

        _objectName = EditorGUILayout.TextField("MVC Object Name:", _objectName);

        if (GUILayout.Button("CREATE MVC object"))
        {
            GenerateMVC(_objectName);
        }

        EditorGUILayout.Space(20f);

        _selectedIndex = EditorGUILayout.Popup(_selectedIndex, _objectList);

        if (GUILayout.Button("DELETE MVC object"))
        {
            DeleteMVC(_objectName);
        }
    }

    private string CleanUpName(string name)
    {
        return name.Trim().Replace(" ", "").Replace("_", "").Replace("-", "").Replace(".", "");
    }

    private string GetControllerScriptDirectoryPath()
    {
        return Path.Combine(Application.dataPath, "Engine", "Controllers");
    }

    private void DeleteMVC(string name)
    {
        if (EditorUtility.DisplayDialog("Delete MVC Object?", $"Are you sure you want to DELETE {_objectName}?", $"DELETE {_objectName}?", "Cancel"))
        {
            name = CleanUpName(name);

            string controllerScriptFilePath = GetControllerScriptFilePath(name);
            if (File.Exists(controllerScriptFilePath))
            {
                File.Delete(controllerScriptFilePath);
            }

            AssetDatabase.Refresh();
        }
    }

    private void GenerateMVC(string name)
    {
        name = CleanUpName(name);


        string viewScriptFilePath = Path.Combine(Application.dataPath, "Scripts", "Views", $"{name}View.cs");
        string modelScriptFilePath = Path.Combine(Application.dataPath, "Engine", "Data Models", $"{name}");
        

        GenerateControllerClass(name);

        AssetDatabase.Refresh();
    }

    private string GetControllerScriptFilePath(string name)
    {
        return Path.Combine(Application.dataPath, "Engine", "Controllers", $"{name}Controller.cs");
    }

    private void GenerateControllerClass(string name)
    {
        string controllerScriptFilePath = GetControllerScriptFilePath(name);

        if (!File.Exists(controllerScriptFilePath))
        {
            using (StreamWriter outfile = new StreamWriter(controllerScriptFilePath))
            {
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("");
                outfile.WriteLine("namespace MVCEngine");
                outfile.WriteLine("{");
                outfile.WriteLine("\tpublic class " + name + " : MonoBehaviour, IController");
                outfile.WriteLine("\t{");
                outfile.WriteLine("\t\tpublic void Initialize()");
                outfile.WriteLine("\t\t{");
                outfile.WriteLine("\t\t\t");
                outfile.WriteLine("\t\t}");
                outfile.WriteLine("\t}");
                outfile.WriteLine("}");
            }
        }
    }
}
