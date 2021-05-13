using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : Singleton<InteractionManager>
{
    private Dictionary<int, InteractableObject> _InteractionDic;

    private void Awake()
    {
        _InteractionDic = new Dictionary<int, InteractableObject>();
    }
    public bool IsInteractable(int instanceID, out InteractableObject interactableObject)
    {
        return _InteractionDic.TryGetValue(instanceID, out interactableObject);
    }
    public void Register(InteractableObject interactableObject)
    {
        int instanceID = interactableObject.GetInstanceID();
        if (_InteractionDic.ContainsKey(instanceID))
        {
            _InteractionDic[instanceID] = interactableObject;
        }
        else
        {
            _InteractionDic.Add(instanceID, interactableObject);
        }
    }
}
