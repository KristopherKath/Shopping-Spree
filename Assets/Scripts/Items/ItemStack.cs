using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStack : MonoBehaviour
{
    Player player;
    GameObject cart;

    //Items variables
    Stack<Item> items;
    float totalWeight = 0;
    int totalValue = 0;

    private void Awake()
    {
        items = new Stack<Item>();
        player = GetComponent<Player>();
        cart = GameObject.FindGameObjectWithTag("Cart");
    }

    float GetTotalWeight() => totalWeight;
    int GetTotalValue() => totalValue;

    public void RemoveItems()
    {
        int numToRemove = 0;
    }


    //Adds an item to stack and handles total value/weight
    public void AddItem(Item i)
    {
        Debug.Log("Adding Item to Stack");
        items.Push(i);
        totalValue += i.GetValue(); //add to total value collected
        totalWeight += i.GetWeight(); //add to total weight
        player.AddMass(i.GetWeight()); //add mass to player
        player.AddRotationalEffect(i.GetRotationalEffect); //add rotational decrese effect

        //Play add animation idealy
        i.gameObject.transform.parent = cart.transform;
        i.transform.position = cart.transform.position;
    }

    //Pops item from stack and handles total value/weight
    public void RemoveItem()
    {
        Debug.Log("Removing Item from Stack");
        Item i = items.Pop();
        totalValue -= i.GetValue(); //remove from total value collected
        totalWeight -= i.GetWeight(); //remove from total weight
        player.LoseMass(i.GetWeight()); //remove mass from player
        player.RemoveRotationalEffect(i.GetRotationalEffect); //remove rotational decrese effect

        //Play remove animation idealy
        Destroy(i.gameObject);

    }

}
