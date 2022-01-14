using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class MVCWindow : EditorWindow
{
    private string _objectName = string.Empty;
    private int _selectedIndex = 0;
    private string[] _objectList = { };
    private bool _createView;

	[MenuItem("Window/MVC")]
	public static void ShowWindow()
    {
		GetWindow<MVCWindow>("MVC");
    }

    private void OnGUI()
    {
        string controllerScriptDirectoryPath = GetControllerScriptDirectoryPath();
        string[] controllerScriptFilePaths = Directory.GetFiles(controllerScriptDirectoryPath, "*.cs");
        _objectList = new string[controllerScriptFilePaths.Length];
        for (int i=0; i< controllerScriptFilePaths.Length; i++)
        {
            _objectList[i] = CleanUpName(Path.GetFileNameWithoutExtension(controllerScriptFilePaths[i]));
        }

        EditorGUILayout.Space(10f);

        _objectName = EditorGUILayout.TextField("MVC Object Name:", _objectName);
        _createView = EditorGUILayout.Toggle("Create View", _createView);

        if (GUILayout.Button("CREATE MVC object"))
        {
            GenerateMVC(_objectName, false);

            UpdateEngineClass();
        }
        if (GUILayout.Button("CREATE SINGLETON MVC object"))
        {
            GenerateMVC(_objectName, true);

            UpdateEngineClass();
        }

        EditorGUILayout.Space(20f);

        _selectedIndex = EditorGUILayout.Popup(_selectedIndex, _objectList);

        if (GUILayout.Button("CREATE Controller Prefab"))
        {
            GenerateControllerPrefab($"{_objectName}Controller");
        }
        if (GUILayout.Button("DELETE MVC object"))
        {
            DeleteMVC(_objectName);

            UpdateEngineClass();
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

    private string GetModelScriptDirectoryPath(bool singleton)
    {
        if (singleton)
        {
            return Path.Combine(Application.dataPath, "Engine", "Data Models", "Singleton");
        }
        else
        {
            return Path.Combine(Application.dataPath, "Engine", "Data Models");
        }
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

            string controllerPrefabPath = GetControllerPrefabPath($"{name}Controller");
            if (File.Exists(controllerPrefabPath))
            {
                File.Delete(controllerPrefabPath);
            }

            AssetDatabase.Refresh();
        }
    }

    private void GenerateMVC(string name, bool singleton)
    {
        name = CleanUpName(name);

        GenerateControllerClass(name, singleton);
        GenerateModelClass(name, singleton);
        if (_createView)
        {
            GenerateViewClass(name, singleton);
        }

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
                    outfile.WriteLine("");
                    outfile.WriteLine($"\t\tpublic void SetObjectID(int objectID)");
                    outfile.WriteLine("\t\t{");
                    outfile.WriteLine("\t\t\t_model.SetObjectID(objectID);");
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
                outfile.WriteLine("\t\t}");
                outfile.WriteLine("");
                outfile.WriteLine("\t\tpublic void SetObjectID(int objectID)");
                outfile.WriteLine("\t\t{");
                outfile.WriteLine("\t\t\tObjectID = objectID;");
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
                    outfile.WriteLine($"\t\tEngine.Instance.{name}Controller.SetObjectID(_objectID);");
                    outfile.WriteLine("\t}");
                }
                outfile.WriteLine("}");
            }
        }
    }

    private string GetEngineScriptFilePath()
    {
        return Path.Combine(Application.dataPath, "Engine", "Engine.cs");
    }

    private void UpdateEngineClass()
    {
        string modelScriptDirectoryPath = GetModelScriptDirectoryPath(false);
        string modelScriptDirectoryPathSingleton = GetModelScriptDirectoryPath(true);
        string[] modelScriptFilePaths = Directory.GetFiles(modelScriptDirectoryPath, "*.cs");
        string[] modelScriptFilePathsSingleton = Directory.GetFiles(modelScriptDirectoryPathSingleton, "*.cs");
        string[] _modelList = new string[modelScriptFilePaths.Length];
        for (int i = 0; i < modelScriptFilePaths.Length; i++)
        {
            _modelList[i] = CleanUpName(Path.GetFileNameWithoutExtension(modelScriptFilePaths[i]));
        }
        string[] _modelListSingleton = new string[modelScriptFilePathsSingleton.Length];
        for (int i = 0; i < modelScriptFilePathsSingleton.Length; i++)
        {
            _modelListSingleton[i] = CleanUpName(Path.GetFileNameWithoutExtension(modelScriptFilePathsSingleton[i]));
        }

        string engineScriptFilePath = GetEngineScriptFilePath();
        if (File.Exists(engineScriptFilePath))
        {
            File.Delete(engineScriptFilePath);
        }

        using (StreamWriter outfile = new StreamWriter(engineScriptFilePath))
        {
            outfile.WriteLine("using MVCEngine.Data;");
            outfile.WriteLine("using System.Collections.Generic;");
            outfile.WriteLine("using UnityEngine;");
            outfile.WriteLine("using Object = UnityEngine.Object;");
            outfile.WriteLine("");
            outfile.WriteLine("namespace MVCEngine");
            outfile.WriteLine("{");
            outfile.WriteLine("\tpublic class Engine");
            outfile.WriteLine("\t{");
            outfile.WriteLine("\t\tprivate static Engine _instance;");
            outfile.WriteLine("");
            outfile.WriteLine("\t\tpublic static Engine Instance");
            outfile.WriteLine("\t\t{");
            outfile.WriteLine("\t\t\tget");
            outfile.WriteLine("\t\t\t{");
            outfile.WriteLine("\t\t\t\tif (Engine._instance == null)");
            outfile.WriteLine("\t\t\t\t{");
            outfile.WriteLine("\t\t\t\t\tEngine._instance = new Engine();");
            outfile.WriteLine("\t\t\t\t}");
            outfile.WriteLine("");
            outfile.WriteLine("\t\t\t\treturn Engine._instance;");
            outfile.WriteLine("\t\t\t}");
            outfile.WriteLine("\t\t}");
            outfile.WriteLine("");
            outfile.WriteLine("\t\tprivate List<ISingletonData> _singletonDataModels;");
            outfile.WriteLine("\t\tprivate SaveGame _saveGame;");
            outfile.WriteLine("");
            foreach (string modelNameSingleton in _modelListSingleton)
            {
                outfile.WriteLine($"\t\tpublic {modelNameSingleton}Controller {modelNameSingleton}Controller {{ get; private set; }}");
            }
            foreach (string modelName in _modelList)
            {
                outfile.WriteLine($"\t\tpublic {modelName}Controller {modelName}Controller {{ get; private set; }}");
            }
            outfile.WriteLine("\t\tprivate Engine()");
            outfile.WriteLine("\t\t{");
            outfile.WriteLine("\t\t\t// Initialize singleton data models");
            outfile.WriteLine("\t\t\t_singletonDataModels = new List<ISingletonData>();");
            outfile.WriteLine("");
            foreach (string modelNameSingleton in _modelListSingleton)
            {
                outfile.WriteLine($"\t\t\t{modelNameSingleton} {modelNameSingleton.ToLower()} = new {modelNameSingleton}();");
                outfile.WriteLine($"\t\t\t_singletonDataModels.Add({modelNameSingleton.ToLower()});");
                outfile.WriteLine("");
            }
            outfile.WriteLine("\t\t\t_saveGame = new SaveGame();");
            outfile.WriteLine("\t\t\t_saveGame.LoadFromJSON(\"save_0\");");
            outfile.WriteLine("");
            outfile.WriteLine("\t\t\t// Initialize controllers");
            outfile.WriteLine("\t\t\tvar engineContainer = new GameObject { name = \"[Core Engine]\" };");
            outfile.WriteLine("\t\t\tTransform engineParentTransform = engineContainer.transform;");
            outfile.WriteLine("\t\t\tObject.DontDestroyOnLoad(engineContainer);");
            foreach (string modelNameSingleton in _modelListSingleton)
            {
                outfile.WriteLine($"\t\t\t{modelNameSingleton}Controller = Object.Instantiate(Resources.Load<{modelNameSingleton}Controller>(\"Controller Prefabs/{modelNameSingleton}Controller\"), engineParentTransform);");
            }
            foreach (string modelName in _modelList)
            {
                outfile.WriteLine($"\t\t\t{modelName}Controller = Object.Instantiate(Resources.Load<{modelName}Controller>(\"Controller Prefabs/{modelName}Controller\"), engineParentTransform);");
            }
            outfile.WriteLine("");
            outfile.WriteLine("\t\t\t// Inject runtime data model dependencies");
            foreach (string modelNameSingleton in _modelListSingleton)
            {
                outfile.WriteLine($"\t\t\t{modelNameSingleton}Controller.Initialize({modelNameSingleton.ToLower()});");
            }
            foreach (string modelName in _modelList)
            {
                outfile.WriteLine($"\t\t\t{modelName}Controller.Initialize();");
            }
            outfile.WriteLine("\t\t}");
            outfile.WriteLine("");
            outfile.WriteLine("\t\tpublic void Save()");
            outfile.WriteLine("\t\t{");
            outfile.WriteLine("\t\t\t_saveGame.SaveToJSON(\"save_0\");");
            outfile.WriteLine("\t\t}");
            outfile.WriteLine("\t}");
            outfile.WriteLine("}");
            outfile.WriteLine("");
        }
    }

    private string GetControllerPrefabPath(string controllerName)
    {
        return Path.Combine(Application.dataPath, "Resources", "Controller Prefabs", $"{controllerName}.prefab");
    }

    private void GenerateControllerPrefab(string controllerName)
    {
        GameObject prefabObject = new GameObject($"{controllerName}");
        Type controllerScriptType = Type.GetType($"MVCEngine.{controllerName}, Assembly-CSharp", true);
        prefabObject.AddComponent(controllerScriptType);
        bool success = false;
        string controllerPrefabPath = GetControllerPrefabPath(controllerName);
        PrefabUtility.SaveAsPrefabAsset(prefabObject, controllerPrefabPath, out success);

        GameObject.DestroyImmediate(prefabObject);

        AssetDatabase.Refresh();
    }
}
