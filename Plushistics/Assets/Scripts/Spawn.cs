using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.GraphicsBuffer;
using static UnityEditor.PlayerSettings;
using UnityEngine.Pool;
using System.Linq;
using System;
using Unity.VisualScripting;

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
    };

    public struct Path {
        public Location one;
        public Location two;
        public int fuel; 
    };

    public struct Van {
        public Location start;
        public int maxLoad;
    }

    HashSet<Location> locations = new HashSet<Location>();
    HashSet<Path> paths = new HashSet<Path>();
    HashSet<Van> vans = new HashSet<Van>();

    Location first = new Location();
    Location nearest = new Location();
    Location created = new Location();
    Location vanNearest = new Location();

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
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            if (freeze) return;
            Debug.Log($"Pressed {locationSelected} {pathSelected}");
            if (locationSelected) createLocation();
            else if (pathSelected) drawLine();
            else if (vanSelected) selectVanLocation();
        }
    }

    void createLocation() {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 2.0f;
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (objectPos.y > -0.5 && objectPos.x < -6.4) return;
        lastCreated = Instantiate(locationPrefab, new Vector3(objectPos.x, objectPos.y, 0), Quaternion.identity);
        created = new Location();
        created.pos = new Vector2(objectPos.x, objectPos.y);
        locations.Add(created);
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
        lastCreated.GetComponentInChildren<TMP_Text>().text = name;
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
        Instantiate(vanPrefab, new Vector3(vanNearest.pos.x, vanNearest.pos.y, -0.01f), Quaternion.identity);
    }

    Location GetNearest(Vector2 mouseLoc) {
        Location max = new Location();
        if (locations.Count == 0) return max;
        max = locations.First();
        foreach (Location loc in locations) {
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
        GameObject line = Instantiate(linePrefab, new Vector3(0, 0, 0), Quaternion.identity);
        LineRenderer lineRenderer = line.GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(first.pos.x, first.pos.y, 0));
        lineRenderer.SetPosition(1, new Vector3(nearest.pos.x, nearest.pos.y, 0));
        Path path = new Path();
        path.one = nearest;
        path.two = first;
        Debug.Log($"Line in {first} , {new Vector3(nearest.pos.x, nearest.pos.y, 0)}");
        first = new Location();
        first.pos = new Vector2(1000, 1000);
        int fuelValue;
        if (int.TryParse(fuel, out fuelValue)) path.fuel = fuelValue;
        else path.fuel = 1;
        paths.Add(path);
        pathSelected = false;
    }

    void drawLine() {
        if (first.pos.x > 100)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            if (objectPos.y > -2.5 && objectPos.x < -6.4) return;
            first = GetNearest(objectPos);
        }
        else {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 2.0f;
            Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
            if (objectPos.y > -2.5 && objectPos.x < -6.4) return;
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
        Debug.Log(boxContent);
        fuel = boxContent;
    }

    public void ReadAvailableBox(string boxContent)
    {
        Debug.Log(boxContent);
        available = boxContent;
    }

    public void ReadNeededBox(string boxContent)
    {
        Debug.Log(boxContent);
        needed = boxContent;
    }

    public void ReadName(string boxContent) {
        Debug.Log(boxContent);
        name = boxContent;
    }

    public void ReadLoad(string boxContent) {
        Debug.Log(boxContent);
        maxLoad = boxContent;
    }

    public void runSimulation() {
    
    }
}
