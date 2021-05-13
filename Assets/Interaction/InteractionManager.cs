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
    public bool IsInteractable(GameObject instance, out InteractableObject interactableObject)
    {
        return _InteractionDic.TryGetValue(instance.GetInstanceID(), out interactableObject);
    }
    public void Register(InteractableObject interactableObject)
    {
        int instanceID = interactableObject.gameObject.GetInstanceID();
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
