using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Security.Cryptography;
using System.Timers;

public class PopulationManager : MonoBehaviour {
	public GameObject botPrefab;
	public GameObject startingPos;
	public int populationSize = 50;
	List<GameObject> population = new List<GameObject>();
	public static float elapsed = 0;
	public float trialTime = 5;
	int generation = 1;

	GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		guiStyle.fontSize = 25;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup(new Rect(10, 10, 250, 150));
		GUI.Box(new Rect(0, 0, 140, 140), "Stats", guiStyle);
		GUI.Label(new Rect(10, 25, 200, 30), "Gen: " + generation, guiStyle);
		GUI.Label(new Rect(10, 50, 200, 30), string.Format("Time: {0:0.00}", elapsed), guiStyle);
		GUI.Label(new Rect(10, 75, 200, 30), "Population: " + population.Count, guiStyle);
		GUI.EndGroup();
	}

	private void Start()
	{
		for(int i=0;i<populationSize;i++)
		{
			GameObject agent = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
			agent.GetComponent<Brain>().init();
			population.Add(agent);
		}
		Time.timeScale = 5;
	}
	public GameObject breed(GameObject parent1,GameObject parent2)
	{
		GameObject offspring = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
		Brain b = offspring.GetComponent<Brain>();
		if(UnityEngine.Random.Range(1,100)==1)
		{
			b.init();
			b.dna.mutate();
		}
		else
		{
			b.init();
			b.dna.combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
		}
		return offspring;
	}
	public void BreedNewPopulation()
	{
		List<GameObject> sortedlist = population.OrderBy(o => (o.GetComponent<Brain>().distancetravelled - o.GetComponent<Brain>().crash)).ToList();
		population.Clear();
		for(int i=(int)(3*(sortedlist.Count/4))-1;i<sortedlist.Count-1;i++)
		{
			population.Add(breed(sortedlist[i], sortedlist[i + 1]));
			population.Add(breed(sortedlist[i + 1], sortedlist[i]));
			population.Add(breed(sortedlist[i], sortedlist[i + 1]));
			population.Add(breed(sortedlist[i + 1], sortedlist[i]));
		}	
		//destroy parent
		for(int i=0;i<population.Count; i++)
		{
			Destroy(sortedlist[i]);
		}
		generation++;
	}
	private void Update()
	{
		elapsed += Time.deltaTime;
		if(elapsed>trialTime)
		{
			BreedNewPopulation();
			elapsed = 0;
		}
	}
}
/*public GameObject botPrefab;
	public GameObject startingPos;
	public int populationSize = 50;
	List<GameObject> population = new List<GameObject>();
	public static float elapsed = 0;
	public float trialTime = 5;
	int generation = 1;

	GUIStyle guiStyle = new GUIStyle();
	void OnGUI()
	{
		guiStyle.fontSize = 25;
		guiStyle.normal.textColor = Color.white;
		GUI.BeginGroup (new Rect (10, 10, 250, 150));
		GUI.Box (new Rect (0,0,140,140), "Stats", guiStyle);
		GUI.Label(new Rect (10,25,200,30), "Gen: " + generation, guiStyle);
		GUI.Label(new Rect (10,50,200,30), string.Format("Time: {0:0.00}",elapsed), guiStyle);
		GUI.Label(new Rect (10,75,200,30), "Population: " + population.Count, guiStyle);
		GUI.EndGroup ();
	}


	// Use this for initialization
	void Start () {
		for(int i = 0; i < populationSize; i++)
		{
			GameObject b = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
			b.GetComponent<Brain>().Init();
			population.Add(b);
		}
	}

	GameObject Breed(GameObject parent1, GameObject parent2)
	{
		GameObject offspring = Instantiate(botPrefab, startingPos.transform.position, this.transform.rotation);
		Brain b = offspring.GetComponent<Brain>();
		if(Random.Range(0,100) == 1) //mutate 1 in 100
		{
			b.Init();
			b.dna.Mutate();
		}
		else
		{ 
			b.Init();
			b.dna.Combine(parent1.GetComponent<Brain>().dna,parent2.GetComponent<Brain>().dna);
		}
		return offspring;
	}

	void BreedNewPopulation()
	{
		List<GameObject> sortedList = population.OrderBy(o => (o.GetComponent<Brain>().distanceTravelled)).ToList();
		
		population.Clear();
		for (int i = (int) (3*sortedList.Count / 4.0f) - 1; i < sortedList.Count - 1; i++)
		{
    		population.Add(Breed(sortedList[i], sortedList[i + 1]));
    		population.Add(Breed(sortedList[i + 1], sortedList[i]));
    		population.Add(Breed(sortedList[i], sortedList[i + 1]));
    		population.Add(Breed(sortedList[i + 1], sortedList[i]));
		}


		

		//destroy all parents and previous population
		for(int i = 0; i < sortedList.Count; i++)
		{
			Destroy(sortedList[i]);
		}
		generation++;
	}
	
	// Update is called once per frame
	void Update () {
		elapsed += Time.deltaTime;
		if(elapsed >= trialTime)
		{
			BreedNewPopulation();
			elapsed = 0;
		}
	}*/
