using Mono.Cecil;
using UnityEngine;

public class PlayerRes{


    public enum ResourceType{
        Thirst,
        Hunger,
        Wood
    }
    public int ThisResourceAmount;
    public ResourceType ThisResourceType;

    public PlayerRes(ResourceType resourceType, int amount){
        ThisResourceType = resourceType;
        ThisResourceAmount = amount;
    }
}