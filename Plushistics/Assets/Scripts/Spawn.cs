using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using static Unity.Burst.Intrinsics.X86.Avx;


public class Spawn : MonoBehaviour
{
    public Button locationButton;
    public Button pathButton;
    public Button vanButton;
    public Button fuelOk;
    public Button locationOk;
    public Button vanOk;
    public Button runButton;
    public GameObject locationPrefab;
    public GameObject linePrefab;
    public GameObject vanPrefab;
    public GameObject fuelPopup;
    public GameObject locationPopup;
    public GameObject vanPopup;
    public GameObject sharkAnimation;

    private GameObject lastCreated;

    private bool locationSelected = false;
    private bool pathSelected = false;
    private bool vanSelected = false;
    private bool freeze = false;
    private string fuel = "1";
    private string needed = "1";
    private string available = "1";
    private string name = "The Royal Holloway";
    private string maxLoad = "1";

    public struct Location {
        public Vector2 pos;
        public string name;
        public int sharksNeeded;
        public int sharksAvailable;
        public GameObject instance;
        public string id;
    };

    public struct Path {
        public Location one;
        public Location two;
        public int fuel;
        public GameObject instance;
        public string id;
    };

    public struct Van {
        public Location start;
        public int currentLoad;
        public int maxLoad;
        public GameObject instance;
        public string id;
    };

    public struct Action {
        public string action;
        public Van van;
        public Location city1;
        public Location city2;
    };

    Dictionary<string, Location> locations = new Dictionary<string, Location>();
    Dictionary<string, Path> paths = new Dictionary<string, Path>();
    Dictionary<string, Van> vans = new Dictionary<string, Van>();

    Location first = new Location();
    Location nearest = new Location();
    Location created = new Location();
    Location vanNearest = new Location();
    Location destination = new Location();
    Van movingVan = new Van();

    Queue<Action> actions;

    bool moving = false;
    bool vanMovement = false;
    bool playSim = false;

    // Start is called before the first frame update
    void Start()
    {
        locationButton.onClick.AddListener(enableLocation);
        pathButton.onClick.AddListener(enablePath);
        vanButton.onClick.AddListener(addVan);
        fuelOk.onClick.AddListener(createLine);
        locationOk.onClick.AddListener(assignLocation);
        vanOk.onClick.AddListener(createVan);
        runButton.onClick.AddListener(runSimulation);
        first.pos = new Vector2(1000, 1000);
        movingVan.maxLoad = -1;
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            if (freeze) return;
            if (locationSelected) createLocation();
            else if (pathSelected) drawLine();
            else if (vanSelected) selectVanLocation();
        }
        if (playSim && moving && !vanMovement) return;
        if (playSim && moving && vanMovement)
        {
            if (Vector3.Distance(movingVan.instance.transform.position, destination.pos) > 0.001f) movingVan.instance.transform.position = Vector3.MoveTowards(movingVan.instance.transform.position, destination.instance.transform.position, Time.deltaTime);
            else
            {
                moving = false;
                vanMovement = false;
            }
        }
        else if (playSim && !moving) NextAction();
    }

    void createLocation() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (objectPos.y > -0.5 && objectPos.x < -6.4) return;
        lastCreated = Instantiate(locationPrefab, new Vector3(objectPos.x, objectPos.y, 0), Quaternion.identity);
        created = new Location();
        created.pos = new Vector2(objectPos.x, objectPos.y);
        created.instance = lastCreated;
        created.id = $"C{locations.Count + 1}";
       // Debug.Log($"{created} added");
        ShowLocationPopup();
    }

    void assignLocation() {
        locationPopup.SetActive(false);
        freeze = false;
        locationSelected = false;
        int sharks;
        if (int.TryParse(available, out sharks)) created.sharksAvailable = sharks;
        else created.sharksAvailable = 1;
        if (int.TryParse(needed, out sharks)) created.sharksNeeded = sharks;
        else created.sharksNeeded = 1;
        if (name == "") name = "The Royal Holloway";
        created.name = name;
        locations.Add($"C{locations.Count + 1}", created);
        lastCreated.transform.Find("Name").GetComponent<TMP_Text>().text = name;
        lastCreated.transform.Find("Sharks").GetComponent<TMP_Text>().text = $"{created.sharksAvailable}/{created.sharksNeeded}";
    }

    void selectVanLocation() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (objectPos.y > -2.5 && objectPos.x < -6.4) return;
        vanNearest = GetNearest(objectPos);
        ShowVanPopup();
    }

    void ShowVanPopup() {
        vanPopup.SetActive(true);
        freeze = true;
        vanPopup.GetComponentInChildren<TMP_InputField>().text = "1";
    }

    void createVan() {
        vanPopup.SetActive(false);
        freeze = false;
        vanSelected = false;
        Van van = new Van();
        int load;
        if (int.TryParse(maxLoad, out load)) van.maxLoad = load;
        else van.maxLoad = 1;
        van.start = vanNearest;
        GameObject vanCreated = Instantiate(vanPrefab, new Vector3(vanNearest.pos.x, vanNearest.pos.y, -0.01f), Quaternion.identity);
        vanCreated.GetComponentInChildren<TMP_Text>().text = $"0/{van.maxLoad}";
        van.instance = vanCreated;
        van.id = $"V{vans.Count + 1}";
        vans.Add($"V{vans.Count + 1}", van);
       // Debug.Log($"{van} added");
    }

    Location GetNearest(Vector2 mouseLoc) {
        Location max = new Location();
        if (locations.Count == 0) return max;
        max = locations["C1"];
        foreach (Location loc in locations.Values) {
            if (Vector3.Distance(loc.pos, mouseLoc) < Vector3.Distance(max.pos, mouseLoc)) max = loc;
        }
        return max;
    }

    void ShowFuelPopup() {
        fuelPopup.SetActive(true);
        freeze = true;
        fuelPopup.GetComponentInChildren<TMP_InputField>().text = "1";
    }

    void ShowLocationPopup() {
        locationPopup.SetActive(true);
        freeze = true;
        locationPopup.transform.Find("InputField Available").GetComponent<TMP_InputField>().text = "1";
        locationPopup.transform.Find("InputField Needed").GetComponent<TMP_InputField>().text = "1";
        locationPopup.transform.Find("InputField name").GetComponent<TMP_InputField>().text = "The Royal Holloway";
    }

    void createLine() {
        fuelPopup.SetActive(false);
        freeze = false;
        GameObject line = Instantiate(linePrefab, new Vector3((first.pos.x + nearest.pos.x)/2, (first.pos.y + nearest.pos.y)/2, 0), Quaternion.identity);
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(first.pos.x, first.pos.y, 0));
        lineRenderer.SetPosition(1, new Vector3(nearest.pos.x, nearest.pos.y, 0));
        Path path = new Path();
        path.one = nearest;
        path.two = first;
        path.instance = line;
        first = new Location();
        first.pos = new Vector2(1000, 1000);
        int fuelValue;
        if (int.TryParse(fuel, out fuelValue)) path.fuel = fuelValue;
        else path.fuel = 1;
        line.GetComponentInChildren<TMP_Text>().text = fuelValue.ToString();
        path.id = $"C{paths.Count + 1}";
        paths.Add($"C{paths.Count + 1}", path);
       // Debug.Log($"{path} added");
        pathSelected = false;
    }

    void drawLine() {
        if (first.pos.x > 100)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            if (objectPos.y > 1 && objectPos.x < -6.4) return;
            first = GetNearest(objectPos);
        }
        else {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            if (objectPos.y > 1 && objectPos.x < -6.4) return;
            nearest = GetNearest(objectPos);
            if (nearest.pos == first.pos) return;
            ShowFuelPopup();
        }
    }

    void enableLocation() {
        if (freeze) return;
        locationSelected = true;
        pathSelected = false;
        vanSelected = false;
        first = new Location();
        first.pos = new Vector2(1000, 1000);
    }

    void enablePath() {
        if (freeze) return;
        locationSelected = false;
        pathSelected = true;
        vanSelected = false;
        first = new Location();
        first.pos = new Vector2(1000, 1000);
    }

    void addVan() {
        if (freeze) return;
        locationSelected = false;
        pathSelected = false;
        vanSelected = true;
        first = new Location();
        first.pos = new Vector2(1000, 1000);
    }

    public void ReadFuelBox(string boxContent)
    {
        fuel = boxContent;
    }

    public void ReadAvailableBox(string boxContent)
    {
        available = boxContent;
    }

    public void ReadNeededBox(string boxContent)
    {
        needed = boxContent;
    }

    public void ReadName(string boxContent) {
        name = boxContent;
    }

    public void ReadLoad(string boxContent) {
        maxLoad = boxContent;
    }

    public void runSimulation() {
        locationButton.gameObject.SetActive(false);
        pathButton.gameObject.SetActive(false);
        vanButton.gameObject.SetActive(false);
        runButton.gameObject.SetActive(false);

       // Debug.Log($"{locations.Count} {paths.Count} {vans.Count}");

        CreateFile();

        RunCMD();
        actions = new Queue<Action>();
        ReadFile();

        
        playSim = true;
    }

    private void NextAction() {
        if (actions.Count <= 0 || moving) return;
        Action action = actions.First();
        actions.Dequeue();
        if (action.action == "LOAD") StartCoroutine(LoadShark(action.van.id, action.city1.id));
        else if (action.action == "UNLOAD") StartCoroutine(UnloadShark(action.van.id, action.city1.id));
        else StartCoroutine(MoveVan(action.van.id, action.city1.id, action.city2.id));
    }

    IEnumerator LoadShark(string vanID, string locationID) {
        Van van = vans[vanID];
        Location location = locations[locationID];
        UnityEngine.Debug.Log("LOAD!");
        moving = true;
        yield return new WaitForSeconds(0.7f);
        van.currentLoad++;
        location.sharksAvailable--;
        van.instance.GetComponentInChildren<TMP_Text>().text = $"{van.currentLoad}/{van.maxLoad}";
        location.instance.transform.Find("Sharks").GetComponent<TMP_Text>().text = $"{location.sharksAvailable}/{location.sharksNeeded}";
        vans.Remove(van.id);
        vans.Add(van.id, van);
        locations.Remove(location.id);
        locations.Add(location.id, location);
        moving = false;
        Instantiate(sharkAnimation, new Vector3(location.pos.x, location.pos.y, 0), Quaternion.identity);
    }

    IEnumerator UnloadShark(string vanID, string locationID) {
        Van van = vans[vanID];
        Location location = locations[locationID];
        UnityEngine.Debug.Log("UNLOAD!");
        moving = true;
        yield return new WaitForSeconds(0.7f);
        van.currentLoad--;
        location.sharksAvailable++;
        van.instance.GetComponentInChildren<TMP_Text>().text = $"{van.currentLoad}/{van.maxLoad}";
        location.instance.transform.Find("Sharks").GetComponent<TMP_Text>().text = $"{location.sharksAvailable}/{location.sharksNeeded}";
        vans.Remove(van.id);
        vans.Add(van.id, van);
        locations.Remove(location.id);
        locations.Add(location.id, location);
        moving = false;
        Instantiate(sharkAnimation, new Vector3(location.pos.x, location.pos.y, 0), Quaternion.identity);
    }

    IEnumerator MoveVan(string vanID, string city1ID, string city2ID) {
        Van van = vans[vanID];
        Location city2 = locations[city2ID];
        UnityEngine.Debug.Log("MOVE!");
        moving = true;
        yield return new WaitForSeconds(0.7f);
        destination = city2;
        movingVan = van;
        vanMovement = true;
    }

    void ReadFile()
    {
        string text = System.IO.File.ReadAllText(@"./output.txt");
        string[] t = text.Split("step");

        string[] t2 = t[1].Split("time");

        string[] s = t2[0].Split("\n"); //cos

        for (int i = 0; i < s.Length; ++i)
        {
            string[] p = s[i].Split(" "); //per num,action,parametres per separat + espais :(
            Action act = new Action();
            int state = 0;
            int van_num = 0;
            int city_num = 0;
            int city_num2 = 0;
            for (int j = 0; j < p.Length; ++j)
            {
                for (int x = 0; x < p[j].Length; ++x)
                {
                    //per char
                    if (p[j][x] == ' ') continue;
                    else if (state == 0 && Char.IsDigit(p[j][x])) continue;
                    else if (state == 0 && p[j][x] == ':') state = 1;
                    else if (state == 1 && p[j][x] == 'L')
                    {
                        state = 2;
                        act.action = "LOAD";
                        break;
                    }
                    else if (state == 1 && p[j][x] == 'M')
                    {
                        state = 2;
                        act.action = "MOVE";
                        break;
                    }
                    else if (state == 1 && p[j][x] == 'U')
                    {
                        state = 2;
                        act.action = "UNLOAD";
                        break;
                    }
                    else if (state == 2 && p[j][x] == 'V') continue;
                    else if (state == 2)
                    {
                        if (Char.IsDigit(p[j][x])) van_num = van_num * 10 + (p[j][x] - '0');
                        else state = 3;
                    }
                    else if (state == 3)
                    {
                        if (Char.IsDigit(p[j][x])) city_num = city_num * 10 + (p[j][x] - '0');
                        else state = 4;
                    }

                    else if (state == 4)
                    {
                        if (Char.IsDigit(p[j][x])) city_num2 = city_num2 * 10 + (p[j][x] - '0');
                    }

                    else Console.WriteLine(p[j][x]);
                }
                //Console.WriteLine("hola");
            }
            if (van_num <= 0) break;
            act.van = vans["V" + (van_num).ToString()];
            act.city1 = locations["C" + (city_num).ToString()];
            if (city_num2 > 0) act.city2 = locations["C" + (city_num2).ToString()];
            actions.Enqueue(act);
        }


    }

    void RunCMD() {
        string strCmdText = "/C metricff.exe -O -o domain.pddl -f problem.pddl -h 4 > output.txt";
        System.Diagnostics.Process.Start("CMD.exe", strCmdText).WaitForExit();
    }

    void CreateFile() {
        string text = "(define (problem one)\n" +
                            "\t(:domain plushistics)\n\n" +
                            "\t(:objects\n";

        text += "\t\t";
        foreach (string loc in locations.Keys) { text += loc + " "; }
        text += "- city\n";

        text += "\t\t";
        for (int i = 1; i <= vans.Count; ++i) { text += "V" + i + " "; }
        text += "- van\n\n\t)\n";

        text += "\t(:init\n" +
                "\t\t(= (gas) 0)\n";

        for (int i = 1; i <= vans.Count; ++i)
        {
            text += "\t\t(= (capacity V" + i + ") " + vans["V" + i.ToString()].maxLoad + ")\n";
            text += "\t\t(= (cargo V" + i + ") 0)\n";
            text += "\t\t(parked V" + i + " " + vans["V"+i.ToString()].start.id + ")\n";
        }

        foreach (Path p in paths.Values)
        {
            text += "\t\t(path " + p.one.id + " " + p.two.id + ")\n";
            text += "\t\t(path " + p.two.id + " " + p.one.id + ")\n";
            text += "\t\t(= (cost " + p.one.id + " " + p.two.id + ") " + p.fuel + ")\n";
            text += "\t\t(= (cost " + p.two.id + " " + p.one.id + ") " + p.fuel + ")\n";
        }

        foreach (var loc in locations)
        {
            text += "\t\t(= (sharks " + loc.Key + ") " + loc.Value.sharksAvailable + ")\n";
            text += "\t\t(= (demand " + loc.Key + ") " + loc.Value.sharksNeeded + ")\n";
        }
        text += "\t)\n\n";

        text += "\t(:goal\n\t\t(forall (?c - city)\n";
        text += "\t\t\t(>= (sharks ?c) (demand ?c)))\n\t)\n\n";
        text += "\t(:metric minimize\n\t\t\t(gas)\n\t)\n)";

        // Write the string to a file
        File.WriteAllText("./problem.pddl", text);

        Console.WriteLine("Text written to file successfully.");
    }
}

