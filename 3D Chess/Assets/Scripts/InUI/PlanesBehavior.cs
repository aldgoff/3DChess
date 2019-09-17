using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanesBehavior : MonoBehaviour
{
    public Dropdown planeBehavior;

    // Start is called before the first frame update
    void Start()
    {
        PopulateList();
    }

    void PopulateList()
    {
        List<string> behaviors = new List<string>() { "Mouse Over", "On Click" };

        planeBehavior.AddOptions(behaviors);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
