using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData 
{
   public static Dictionary<SignType, SignBehaviourType> signBehaviour = new Dictionary<SignType, SignBehaviourType>();

    public static SignBehaviourType GetSignBehaviourType(SignType signType)
    {
        return signBehaviour[signType];
    }
}
