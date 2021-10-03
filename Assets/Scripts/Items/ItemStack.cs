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

    //removes a random amount of items when hit by hazard
    public void RemoveItems()
    {
        int numToRemove = Random.Range(1, 3);

        //Set num to remove to num left
        if (numToRemove > items.Count)
            numToRemove = items.Count;

        Debug.Log("Removing " + numToRemove + " items");

        for (int i = 0; i < numToRemove; i++)
        {
            RemoveItem();
        }
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
        i.gameObject.transform.SetParent(cart.transform, true); //set parent to cart
        i.transform.position = cart.transform.position; //move position to the cart
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
        i.gameObject.transform.SetParent(null, true); //Set parent to nothing
        
        i.ItemRemoval(); //kills item

    }

}
