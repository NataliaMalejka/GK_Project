using UnityEngine;

/** 
 * IPickup is an interface that defines the contract for collectible items in Unity.
 *
 * @author Krzysztof Gach
 * @version 1.0
 */
public interface IPickup
{
    void Collect(GameObject collector);
}