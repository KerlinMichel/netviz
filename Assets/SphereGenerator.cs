using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class SphereGenerator : MonoBehaviour {

    //Main server sphere position
    Vector3 pVec = new Vector3(0,0,0);

    string[][] data;
    DateTime realtime_start;
    DateTime sim_start;
    int sim_iter;
    ArrayList users;
    ArrayList sizes;
    ArrayList spheres;
	void Start () {
        //read csv file to get data
        string text = System.IO.File.ReadAllText("U:\\Desktop\\netflow\\net-data.csv");
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
        //mainServerSphere represents the FIU network being used by users
        GameObject mainServerSphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        mainServerSphere.transform.position = pVec;
        mainServerSphere.transform.localScale = new Vector3(10, 10, 10);
    }
	
	// Update is called once per frame
	void Update () {
        while(true)
        {
            //reached end of data
            if (sim_iter+1 > data.Length-1)
                break;
            //simulate network data by checking the passage of time
            if ((DateTime.Now - realtime_start) > (DateTime.Parse(data[sim_iter][8]) - sim_start))
            {
                if (!users.Contains(data[sim_iter][0]))
                {
                    users.Add(data[sim_iter][0]);
                    sizes.Add(int.Parse(data[sim_iter][6]));
                    GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    spheres.Add(sphere);
                    //the position is determined randomly by spherical coordinates http://mathworld.wolfram.com/SphericalCoordinates.html
                    //the x,y,z equations do match up as the link above since the conventional mathematical coordinate system is different from the unity coordinate system
                    float r = 10;
                    float theta = Random.Range(0, (float)Math.PI);
                    float phi = Random.Range(0, (float)(2*Math.PI));
                    float x = (float)(r * Math.Sin(theta) * Math.Cos(phi));
                    float y = (float)(r * Math.Cos(theta));
                    float z = (float)(r * Math.Sin(theta) * Math.Sin(phi));
                    sphere.transform.position = new Vector3(x, y, z);
                    int size = int.Parse(data[sim_iter][6]);
                    //size of sphere is determine by the size of the packets the user has sent
                    float scale = Math.Min(size / 4000, 1) * 1.5f + 0.5f;
                    sphere.transform.localScale = new Vector3(scale, scale, scale);
                } else
                {
                    //update the size of the sphere by the size of the packets
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
        
    }
}
