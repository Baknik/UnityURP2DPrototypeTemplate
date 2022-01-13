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
            GenerateMVC(_objectName, false);
        }
        if (GUILayout.Button("CREATE SINGLETON MVC object"))
        {
            GenerateMVC(_objectName, true);
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

            string modelScriptFilePath = GetModelScriptFilePath(name, false);
            if (File.Exists(modelScriptFilePath))
            {
                File.Delete(modelScriptFilePath);
            }
            string modelScriptFilePathSingleton = GetModelScriptFilePath(name, true);
            if (File.Exists(modelScriptFilePathSingleton))
            {
                File.Delete(modelScriptFilePathSingleton);
            }

            string viewScriptFilePath = GetViewScriptFilePath(name, false);
            if (File.Exists(viewScriptFilePath))
            {
                File.Delete(viewScriptFilePath);
            }
            string viewScriptFilePathSingleton = GetViewScriptFilePath(name, true);
            if (File.Exists(viewScriptFilePathSingleton))
            {
                File.Delete(viewScriptFilePathSingleton);
            }

            AssetDatabase.Refresh();
        }
    }

    private void GenerateMVC(string name, bool singleton)
    {
        name = CleanUpName(name);

        GenerateControllerClass(name, singleton);
        GenerateModelClass(name, singleton);
        GenerateViewClass(name, singleton);

        AssetDatabase.Refresh();
    }

    private string GetControllerScriptFilePath(string name)
    {
        return Path.Combine(Application.dataPath, "Engine", "Controllers", $"{name}Controller.cs");
    }

    private void GenerateControllerClass(string name, bool singleton)
    {
        string controllerScriptFilePath = GetControllerScriptFilePath(name);

        if (!File.Exists(controllerScriptFilePath))
        {
            using (StreamWriter outfile = new StreamWriter(controllerScriptFilePath))
            {
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("using MVCEngine.Data;");
                outfile.WriteLine("using System.Collections.Generic;");
                outfile.WriteLine("");
                outfile.WriteLine("namespace MVCEngine");
                outfile.WriteLine("{");
                outfile.WriteLine("\tpublic class " + name + "Controller : MonoBehaviour, IController");
                outfile.WriteLine("\t{");
                if (!singleton)
                {
                    outfile.WriteLine($"\t\tprivate Dictionary<int, {name}> _models;");
                    outfile.WriteLine("");
                    outfile.WriteLine("\t\tpublic void Initialize()");
                    outfile.WriteLine("\t\t{");
                    outfile.WriteLine($"\t\t\t_models = new Dictionary<int, {name}>();");
                    outfile.WriteLine("\t\t}");
                    outfile.WriteLine("");
                    outfile.WriteLine($"\t\tpublic {name} Register(int objectID)");
                    outfile.WriteLine("\t\t{");
                    outfile.WriteLine($"\t\t\t{name} model = null;");
                    outfile.WriteLine("");
                    outfile.WriteLine("\t\t\tif (!_models.ContainsKey(objectID))");
                    outfile.WriteLine("\t\t\t{");
                    outfile.WriteLine($"\t\t\t\tmodel = new {name}(objectID);");
                    outfile.WriteLine("\t\t\t\t_models.Add(objectID, model);");
                    outfile.WriteLine("\t\t\t}");
                    outfile.WriteLine("");
                    outfile.WriteLine("\t\t\treturn model;");
                    outfile.WriteLine("\t\t}");
                    outfile.WriteLine("");
                    outfile.WriteLine($"\t\tpublic void Unregister(int objectID)");
                    outfile.WriteLine("\t\t{");
                    outfile.WriteLine("\t\t\t_models.Remove(objectID);");
                    outfile.WriteLine("\t\t}");
                }
                else
                {
                    outfile.WriteLine($"\t\tprivate {name} _model;");
                    outfile.WriteLine("");
                    outfile.WriteLine($"\t\tpublic void Initialize({name} model)");
                    outfile.WriteLine("\t\t{");
                    outfile.WriteLine("\t\t\t_model = model;");
                    outfile.WriteLine("\t\t}");
                    outfile.WriteLine("");
                    outfile.WriteLine($"\t\tpublic {name} GetModel()");
                    outfile.WriteLine("\t\t{");
                    outfile.WriteLine("\t\t\treturn _model;");
                    outfile.WriteLine("\t\t}");
                }
                outfile.WriteLine("\t}");
                outfile.WriteLine("}");
            }
        }
    }

    private string GetModelScriptFilePath(string name, bool singleton)
    {
        if (singleton)
        {
            return Path.Combine(Application.dataPath, "Engine", "Data Models", "Singleton", $"{name}.cs");
        }
        else
        {
            return Path.Combine(Application.dataPath, "Engine", "Data Models", $"{name}.cs");
        }
    }

    private void GenerateModelClass(string name, bool singleton)
    {
        string modelScriptFilePath = GetModelScriptFilePath(name, singleton);

        if (!File.Exists(modelScriptFilePath))
        {
            using (StreamWriter outfile = new StreamWriter(modelScriptFilePath))
            {
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("");
                outfile.WriteLine("namespace MVCEngine.Data");
                outfile.WriteLine("{");
                outfile.WriteLine("\tpublic class " + name + $" : IDataModel{(singleton ? ", ISingletonData" : "")}");
                outfile.WriteLine("\t{");
                outfile.WriteLine("\t\tpublic int ObjectID { get; private set; }");
                outfile.WriteLine("\t\tpublic bool Enabled { get; private set; }");
                outfile.WriteLine("");
                if (!singleton)
                {
                    outfile.WriteLine("\t\tpublic " + name + "(int objectID)");
                    outfile.WriteLine("\t\t{");
                    outfile.WriteLine("\t\t\tObjectID = objectID;");
                }
                else
                {
                    outfile.WriteLine("\t\tpublic " + name + "()");
                    outfile.WriteLine("\t\t{");
                    outfile.WriteLine("\t\t\tObjectID = -1;");
                }
                outfile.WriteLine("\t\t\tEnabled = true;");
                outfile.WriteLine("\t\t}");
                outfile.WriteLine("");
                outfile.WriteLine("\t\tpublic void SetObjectID(int objectID)");
                outfile.WriteLine("\t\t{");
                outfile.WriteLine("\t\t\tObjectID = objectID;");
                outfile.WriteLine("\t\t}");
                outfile.WriteLine("");
                outfile.WriteLine("\t\tpublic void SetEnabled(bool enabled)");
                outfile.WriteLine("\t\t{");
                outfile.WriteLine("\t\t\tEnabled = enabled;");
                outfile.WriteLine("\t\t}");
                outfile.WriteLine("\t}");
                outfile.WriteLine("}");
            }
        }
    }

    private string GetViewScriptFilePath(string name, bool singleton)
    {
        if (singleton)
        {
            return Path.Combine(Application.dataPath, "Scripts", "Views", "Singleton", $"{name}View.cs");
        }
        else
        {
            return Path.Combine(Application.dataPath, "Scripts", "Views", $"{name}View.cs");
        }
    }

    private void GenerateViewClass(string name, bool singleton)
    {
        string viewScriptFilePath = GetViewScriptFilePath(name, singleton);

        if (!File.Exists(viewScriptFilePath))
        {
            using (StreamWriter outfile = new StreamWriter(viewScriptFilePath))
            {
                outfile.WriteLine("using UnityEngine;");
                outfile.WriteLine("using MVCEngine;");
                outfile.WriteLine("using MVCEngine.Data;");
                outfile.WriteLine("");
                outfile.WriteLine("public class " + name + "View : MonoBehaviour");
                outfile.WriteLine("{");
                outfile.WriteLine($"\tprivate int _objectID;");
                outfile.WriteLine($"\tprivate {name} _model;");
                outfile.WriteLine("");
                outfile.WriteLine("\tprivate void Awake()");
                outfile.WriteLine("\t{");
                outfile.WriteLine("\t\t_objectID = this.gameObject.GetInstanceID();");
                if (!singleton)
                {
                    outfile.WriteLine("\t\t");
                    outfile.WriteLine($"\t\t_model = Engine.Instance.{name}Controller.Register(_objectID);");
                    outfile.WriteLine("\t\t");
                    outfile.WriteLine("\t\t_model.SetEnabled(true);");
                    outfile.WriteLine("\t}");
                    outfile.WriteLine("");
                    outfile.WriteLine("\tprivate void OnDestroy()");
                    outfile.WriteLine("\t{");
                    outfile.WriteLine($"\t\tEngine.Instance.{name}Controller.Unregister(_objectID);");
                    outfile.WriteLine("\t}");
                }
                else
                {
                    outfile.WriteLine($"\t\t_model = Engine.Instance.{name}Controller.GetModel();");
                    outfile.WriteLine("\t\t");
                    outfile.WriteLine("\t\t_model.SetObjectID(_objectID);");
                    outfile.WriteLine("\t\t_model.SetEnabled(true);");
                    outfile.WriteLine("\t}");
                }
                outfile.WriteLine("}");
            }
        }
    }
}
