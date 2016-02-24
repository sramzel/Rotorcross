using UnityEngine;
using System.Collections;

public class SaveDataExample : MonoBehaviour {
	
	public string fileName;
	private SaveData data;
	
	void Start ()
	{
		//Create data instance
		data = new SaveData(fileName);
		
		//Add keys with significant names and values
		data["Name"] = "Steve";
		data["Dude"] = "Tom";
		data["Key"] = true;
		data["HealthPotions"] = 10;
		data["Position"] = new Vector3(20, 3, -5);
		data["Rotation"] = new Quaternion(0.1f,0.1f,0.1f,1);
	
		//Save the data
		data.Save();
		
		//Load the data we just saved
		data = SaveData.Load(Application.streamingAssetsPath+"\\"+fileName+".uml");
		
		int potions; //variable for out value
		
		//Use data
		Debug.Log("Name : "+ data.GetValue<string>("Name"));
		Debug.Log("Has health potions : " + data.TryGetValue<int>("HealthPotions", out potions));
		Debug.Log("Health potion count : " + potions);
		Debug.Log("Has buddy : " + data.HasKey("Dude"));
		Debug.Log("Buddy's name : " + data.GetValue<string>("Dude"));
		Debug.Log("Current position : " + data.GetValue<Vector3>("Position"));
		Debug.Log("Has key : " + data.GetValue<bool>("Key"));
		Debug.Log("Rotation : " + data.GetValue<Quaternion>("Rotation"));
	}
}