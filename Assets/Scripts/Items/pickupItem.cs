using System.ComponentModel;
using UnityEngine;

public class pickupItem : MonoBehaviour
{
    public GameObject playerItems;
    public string itemName;

    public ItemType itemType;
}

public enum ItemType
{
    None,
    Flashlight,
    Taser
}
