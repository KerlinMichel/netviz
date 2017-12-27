using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour {

    // Use this for initialization
    string[][] data;
    DateTime realtime_start;
    DateTime sim_start;
    int sim_iter;
    ArrayList users;
    ArrayList sizes;
    ArrayList spheres;
	void Start () {
        string text = System.IO.File.ReadAllText("Z:\\Desktop\\netflow\\net-data.csv");
        string[] lines = text.Split('\n');
        data = new string[lines.Length][];
        for(int i = 0; i < lines.Length; i++)
        {
            data[i] = lines[i].Split(',');
        }
        print(data[0][8]);
        print(data[1][8]);
        realtime_start = DateTime.Now;
        sim_iter = 1;
        sim_start = DateTime.Parse(data[1][8]);
        users = new ArrayList();
        sizes = new ArrayList();
        spheres = new ArrayList();
    }
	
	// Update is called once per frame
	void Update () {
        while(true)
        {
            if (sim_iter+1 > data.Length-1)
                break;
            if ((DateTime.Now - realtime_start) > (DateTime.Parse(data[sim_iter][8]) - sim_start))
            {
                //print(data[sim_iter][8]);
                if (!users.Contains(data[sim_iter][0]))
                {
                    users.Add(data[sim_iter][0]);
                    sizes.Add(int.Parse(data[sim_iter][6]));
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    spheres.Add(sphere);
                    sphere.transform.position = new Vector3((2f * users.Count) - 40, 1.5F, 0);
                    int size = int.Parse(data[sim_iter][6]);
                    float scale = Math.Min(size / 4000, 1) * 1.5f + 0.5f;
                    sphere.transform.localScale = new Vector3(scale, scale, scale);
                } else
                {
                    int idx = users.IndexOf(data[sim_iter][0]);
                    sizes[idx] = (int)sizes[idx] + int.Parse(data[sim_iter][6]);
                    int size = (int)sizes[idx];
                    float scale = Math.Min(size / 4000, 1) * 1.5f + 0.5f;
                    ((GameObject)spheres[idx]).transform.localScale = new Vector3(scale, scale, scale);
                }
                sim_iter++;
            }
            else
                break;
        }
        /*for(int i = 0; i < users.Count; i++)
        {

        }*/
    }
}
