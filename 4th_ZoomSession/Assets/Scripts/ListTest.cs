using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ListTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SimpleList<string> list = new SimpleList<string>();
        list.Add("Hello");
        list.Add("World");

    }


}
