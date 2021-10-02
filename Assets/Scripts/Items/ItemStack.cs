using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack : MonoBehaviour
{
    Player player;

    //Items variables
    Stack<Item> items;
    float totalWeight = 0;
    int totalValue = 0;

    private void Awake()
    {
        items = new Stack<Item>();
        player = GetComponent<Player>();
    }

    float GetTotalWeight() => totalWeight;
    int GetTotalValue() => totalValue;


    //Adds an item to stack and handles total value/weight
    public void AddItem(Item i)
    {
        Debug.Log("Adding Item to Stack");
        items.Push(i);
        totalValue += i.GetValue(); //add to total value collected
        totalWeight += i.GetWeight(); //add to total weight
        player.AddMass(i.GetWeight()); //add mass to player

        //Play add animation idealy
    }

    //Pops item from stack and handles total value/weight
    public void RemoveItem()
    {
        Debug.Log("Removing Item from Stack");
        Item i = items.Pop();
        totalValue -= i.GetValue(); //remove from total value collected
        totalWeight -= i.GetWeight(); //remove from total weight
        player.LoseMass(i.GetWeight()); //remove mass from player

        //Play remove animation idealy
    }

}
