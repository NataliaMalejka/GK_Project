using System.Collections.Generic;
using UnityEngine;

/** 
 * 
 * @author Krzysztof Gach
 * @version 1.0
 */
public interface IExplodable : IDestructible
{
    void Explode(Vector3 explosionOrigin, HashSet<Fences> damagedFences);
}