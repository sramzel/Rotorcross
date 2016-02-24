#region UsingStatements

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

#endregion

/// <summary>
/// 	- Simple, flexible SaveData class for serializing many data types.
/// 	- Only serializes public, non-static fields and classes.
/// 	- Does not support serialization of classes derived from Component object (Transforms, Renderers, Monobehaviours, ect)
/// </summary>
public class SaveData : ISaveData
{
	#region PublicStaticReadonlyFields
	
	/// <summary>
	/// 	- The saved file's extension.
	/// </summary>
	public static readonly string extension = ".uml";
	
	#endregion
	
	#region PublicFields
	
	/// <summary>
	/// 	- The name of the file.
	/// </summary>
	public string fileName = "SavedData";
	
	/// <summary>
	/// 	- Array used for serialization.
	/// 	- XML requires serialized fields to be public. This will always be empty;
	/// </summary>
	public string[] serializedTypes;
	
	/// <summary>
	/// 	- Array used for serialization.
	/// 	- XML requires serialized fields to be public. This will always be empty.
	/// </summary>
	public DataContainer[] serializedData;
	
	#endregion
	
	#region PublicParameters
	
	/// <summary>
	/// Gets or sets the <see cref="SaveData"/> with the specified key.
	/// </summary>
	/// <param name='key'>
	/// 	- Key to set.
	/// </param>
	public System.Object this[string key]
	{
		get { return _data[key]; }
		set 
		{ 
			if(typeof(Component).IsAssignableFrom(value.GetType())) throw new System.InvalidOperationException("Cannot serialize classes derived from Component!");
			_data[key] = value;
		}
	}
	
	#endregion
	
	#region PrivateFields
	
	/// <summary>
	/// 	- Actual data storage.
	/// </summary>
	private Dictionary<string, System.Object> _data = new Dictionary<string, object>();
	
	#endregion
	
	#region Constructors
	
	/// <summary>
	/// 	- Initializes a new instance of the <see cref="SaveData"/> class.
	/// </summary>
	public SaveData(){}
	
	/// <summary>
	/// 	- Initializes a new instance of the <see cref="SaveData"/> class.
	/// </summary>
	/// <param name='fileName'>
	/// 	- The name of the saved file without any extension
	/// </param>
	public SaveData(string fileName)
	{
		this.fileName = fileName;
	}
	
	#endregion
	
	#region PublicStaticFunctions
	
	/// <summary>
	/// 	- Loads specified file from streaming assets folder.
	/// </summary>
	/// <param name='fileName'>
	/// 	- The file to load from Application.streamingAssetsPath.
	/// </param>
	public static SaveData LoadFromStreamingAssets(string fileName)
	{
		return Load(Path.Combine(Application.streamingAssetsPath, fileName));
	}
	
	/// <summary>
	/// 	- Loads a file from the specified path.
	/// </summary>
	/// <param name='path'>
	/// 	- The path to load from.
	/// </param>
	/// <exception cref='System.InvalidOperationException'>
	/// 	- Is thrown when the passed path does not exist or has the wrong extension.
	/// </exception>
	public static SaveData Load(string path)
	{	
		if(File.Exists(path) && Path.GetExtension(path) == extension)
		{
			List<System.Type> additionalTypes = new List<System.Type>();
			XmlDocument document = new XmlDocument();
			document.Load(path);
			XmlNode objectNode = document.ChildNodes[1];
			
			foreach(XmlNode node in objectNode["serializedTypes"].ChildNodes){
				additionalTypes.Add(System.Type.GetType(node.InnerXml));
			}
			
			XmlSerializer serializer = new XmlSerializer(typeof(SaveData), additionalTypes.ToArray());
			TextReader textReader = new StreamReader(path);
			SaveData instance = (SaveData)serializer.Deserialize(textReader);
			textReader.Close();	
			
			foreach(DataContainer container in instance.serializedData){
				instance[container.key] = container.value;	
			}
			
			instance.serializedData = null;
			
			return instance;
		}
		
		else throw new System.InvalidOperationException("File does not exist!");
	}
	
	#endregion	

	#region PublicFunctions
	
	/// <summary>
	/// 	- Determines whether this instance has the specified key.
	/// </summary>
	/// <returns>
	/// 	- <c>true</c> if this instance has the specified key; otherwise, <c>false</c>.
	/// </returns>
	/// <param name='key'>
	/// 	- Key to check for.
	/// </param>
	public bool HasKey(string key)
	{
		return _data.ContainsKey(key);	
	}
	
	/// <summary>
	/// 	- Gets value associated with the key.
	/// </summary>
	/// <returns>
	/// 	- The value.
	/// </returns>
	/// <param name='key'>
	/// 	- Key to get value of.
	/// </param>
	/// <typeparam name='T'>
	/// 	- The type of the returned value.
	/// </typeparam>
	public T GetValue<T>(string key)
	{
		return (T)_data[key];
	}
	
	/// <summary>
	/// 	- Tries to get the value associated with the key.
	/// </summary>
	/// <returns>
	/// 	- <c>true</c> if the instance contains the key.
	/// </returns>
	/// <param name='key'>
	/// 	- Key to get value of.
	/// </param>
	/// <param name='result'>
	/// 	- If set to <c>true</c>, the value associated with the key.
	/// </param>
	public bool TryGetValue(string key, out System.Object result)
	{
		return _data.TryGetValue(key, out result);
	}
	
	/// <summary>
	/// 	- Tries to get the value associated with the key.
	/// </summary>
	/// <returns>
	/// 	- <c>true</c> if the instance contains the key.
	/// </returns>
	/// <param name='key'>
	/// 	- Key to get value of.
	/// </param>
	/// <param name='result'>
	/// 	- If set to <c>true</c>, the value associated with the key.
	/// </param>
	/// <typeparam name='T'>
	/// 	- The type of the returned value.
	/// </typeparam>
	public bool TryGetValue<T>(string key, out T result)
	{
		System.Object resultOut;
		
		if(_data.TryGetValue(key, out resultOut) && resultOut.GetType() == typeof(T))
		{
			result = (T)resultOut;
			return true;
		}
		
		else
		{
			result = default(T);
			return false;
		}
	}
	
	/// <summary>
	/// 	- Saves this instance to the Streaming Assets path.
	/// </summary>
	public void Save() { Save(Path.Combine(Application.streamingAssetsPath, fileName+extension)); }
	
	/// <summary>
	/// 	- Saves this instance to the specified path.
	/// </summary>
	/// <param name='path'>
	/// 	- Path to save to.
	/// </param>
	public void Save(string path)
	{
		List<System.Type> additionalTypes = new List<System.Type>();
		List<string> typeNameList = new List<string>();
		List<DataContainer> dataList = new List<DataContainer>();
		
		System.Object result;
		System.Type resultType;
		
		foreach(string key in _data.Keys){
			result = _data[key];
			resultType = result.GetType();
			
			if(!resultType.IsPrimitive && !additionalTypes.Contains(resultType))
			{
				additionalTypes.Add(resultType);
				typeNameList.Add(resultType.AssemblyQualifiedName);
			}
			
			dataList.Add(new DataContainer(key, result));
		}
		
		serializedData = dataList.ToArray();
		serializedTypes = typeNameList.ToArray();
		
		XmlSerializer serializer = new XmlSerializer(typeof(SaveData), additionalTypes.ToArray());
		TextWriter textWriter = new StreamWriter(path);
		serializer.Serialize(textWriter, this);
		textWriter.Close();	
		
		serializedData = null;
		serializedTypes = null;
	}
	
	#endregion
	
	#region Utility
	
	/// <summary>
	/// 	- Serializable data container, used for saving and loading.
	/// </summary>
	public class DataContainer
	{
		public string key;
		public System.Object value;
		
		public DataContainer(){}
		public DataContainer(string key, System.Object value)
		{
			this.key = key;
			this.value = value;
		}
	}
	
	#endregion
}