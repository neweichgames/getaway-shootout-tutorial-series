using UnityEngine;

public class PlayerUsableController : MonoBehaviour
{

    private bool isUsing;
    private UsableController controller;

    void Start()
    {
        controller = GetComponentInChildren<UsableController>();
    }

    
    void Update()
    {
        if(isUsing)
            controller.Use();
    }

    public void Use()
    {
        isUsing = true;
    }

    public void CancelUse()
    {
        controller.CancelUse();
        isUsing = false;
    }

    private void OnDisable()
    {
        isUsing = false;   
    }
}
