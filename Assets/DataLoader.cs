using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader : MonoBehaviour
{
   [SerializeField] private Vector3 _pos;
   public Data data=new Data();

   private void FixedUpdate()
   {
      float yAxis = Input.GetAxisRaw("Vertical");
      float xAxis = Input.GetAxisRaw("Horizontal");
      Vector3 movement = new Vector2(xAxis, yAxis);
      transform.position = Vector3.Lerp(transform.position, transform.position + movement, Time.deltaTime*2);
   }

   private void Update()
   {
      
   }

   public void SaveData()
   {
      data = new Data(transform.position, gameObject.name);
      string json = JsonUtility.ToJson(data);
      PlayerPrefs.SetString(gameObject.name,json);
   }

   public void LoadData()
   {
      data = JsonUtility.FromJson<Data>(PlayerPrefs.GetString(gameObject.name));
      transform.position = data.Position;
   }
}

[Serializable]
public class Data
{
   public Vector3 Position;
   public string Name;

   public Data(){}
   public Data(Vector3 position, string name)
   { 
      Position = position;
      Name = name;
   }
   
}
