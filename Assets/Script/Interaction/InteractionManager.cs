using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionManager : Singleton<InteractionManager>
{
    private Dictionary<int, InteractableObject> _InteractionDic;

    public InteractableObject this[int instanceID]
    {
        get
        {
            InteractableObject value;
            _InteractionDic.TryGetValue(instanceID, out value);

            return value;
        }
    }

    private void LazyInit()
    {
        _InteractionDic ??= new Dictionary<int, InteractableObject>();
    }
    public bool IsInteractable(GameObject instance, out InteractableObject interactableObject)
    {
        LazyInit();
        
        return _InteractionDic.TryGetValue(instance.GetInstanceID(), out interactableObject);
    }
    public void Register(InteractableObject interactableObject)
    {
        LazyInit();

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
    public void DisRegister(int instanceID)
    {
        LazyInit();
        if (_InteractionDic.ContainsKey(instanceID))
        {
            _InteractionDic.Remove(instanceID);
        }
    }
}
