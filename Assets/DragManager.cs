using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour
{
    public static DragManager Instance { get; private set; }

    [SerializeField] private CardDefinition dragging;

    public Card selectedCard;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
            
    }

    private void LateUpdate()
    {
        selectedCard = null;
    }


}
