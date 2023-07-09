using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    private Champion champion;
    public CardDefinition cardDefinition;
    public bool isDraggable;

    [Header("Component References")]
    [SerializeField] private RectTransform animAnchor;
    [SerializeField] private TextMeshProUGUI cardNameText;
    [SerializeField] private TextMeshProUGUI cardManacostText;
    [SerializeField] private Image cardbackImage;
    [SerializeField] private Image cardManacostImage;
    [SerializeField] private Image cardPortraitImage;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    [SerializeField] private LineRenderer fullLineRend;
    [SerializeField] private LineRenderer rightsideLineRend;
    [SerializeField] private LineRenderer leftsideLineRend;
    [SerializeField] private Image dragLayerImage;
    [SerializeField] private CardSelectionHitbox rightsideHitbox;
    [SerializeField] private CardSelectionHitbox leftsideHitbox;
    
    private enum Mouseover { None, Rightside, Leftside }
    private Mouseover mouseover;
    private bool IsMousedOver => isDraggable && mouseover != Mouseover.None;

    private float upDownSdVelocity;
    private float cardTurnSdVelocity;
    private float cardTurnAnim;

    private static Card dragCard;

    private bool IsBeingDragged => dragCard == this;

    private static int siblingIndexToSet = -1;

    public static event Action OnDeckChanged;

    public Card Bootup(CardDefinition cardDefinition, bool isDraggable, Champion champion)
    {
        this.cardDefinition = cardDefinition;
        this.isDraggable = isDraggable;
        this.champion = champion;
        Render();

        return this;
    }

    private void Update()
    {
        RenderDescriptionOnly();

        if(IsMousedOver && Input.GetMouseButtonDown(0))
        {
            dragCard = this;
        }

        if(IsBeingDragged && !Input.GetMouseButton(0))
        {
            if(siblingIndexToSet != -1)
            {
                bool rising = transform.GetSiblingIndex() < siblingIndexToSet;
                for (int si = transform.GetSiblingIndex(); rising ? si < siblingIndexToSet + 1 : si > siblingIndexToSet -1; si += rising ? 1 : -1)
                {
                    transform.parent.GetChild(si).GetComponent<Card>().CardTurnAnimation(rising ? -5f : 5f);
                }

                dragCard.transform.SetSiblingIndex(siblingIndexToSet);
                CardSelectionHitboxGlobalCooldownManager.Cooldown();

                dragCard.CardTurnAnimation(rising ? -90f : 90f);
                CardTurnAnimation(rising ? -5f : 5f);
                MusicManager.Instance.PlayCard();

                OnDeckChanged?.Invoke();
            }
            
            dragCard = null;
            siblingIndexToSet = -1;
        }

        mouseover = rightsideHitbox.Mouseover ? Mouseover.Rightside : leftsideHitbox.Mouseover ? Mouseover.Leftside : Mouseover.None;

        dragLayerImage.enabled = IsBeingDragged;

        fullLineRend.enabled = (IsMousedOver && dragCard == null) || IsBeingDragged;
        
        if(dragCard != null && dragCard != this)
        {
            if ((mouseover == Mouseover.Rightside && dragCard.transform.GetSiblingIndex() - transform.GetSiblingIndex() != 1) ||
                (mouseover == Mouseover.Leftside && dragCard.transform.GetSiblingIndex() - transform.GetSiblingIndex() == -1))
                    siblingIndexToSet = transform.GetSiblingIndex();
            else if ((mouseover == Mouseover.Leftside && dragCard.transform.GetSiblingIndex() - transform.GetSiblingIndex() != -1) ||
                (mouseover == Mouseover.Rightside && dragCard.transform.GetSiblingIndex() - transform.GetSiblingIndex() == 1))
                    siblingIndexToSet = transform.GetSiblingIndex();
            

            rightsideLineRend.enabled = (mouseover == Mouseover.Rightside && dragCard.transform.GetSiblingIndex() - transform.GetSiblingIndex() != 1) ||
                                        (mouseover == Mouseover.Leftside && dragCard.transform.GetSiblingIndex() - transform.GetSiblingIndex() == -1);
            leftsideLineRend.enabled = (mouseover == Mouseover.Leftside && dragCard.transform.GetSiblingIndex() - transform.GetSiblingIndex() != -1) ||
                                        (mouseover == Mouseover.Rightside && dragCard.transform.GetSiblingIndex() - transform.GetSiblingIndex() == 1);
        }
        else
        {
            rightsideLineRend.enabled = false;
            leftsideLineRend.enabled = false;
        }
        
        float targetZPos = (IsMousedOver && dragCard == null) || IsBeingDragged ? -0.0125f : 0;
        animAnchor.localPosition = new Vector3(animAnchor.localPosition.x, animAnchor.localPosition.y, Mathf.SmoothDamp(animAnchor.localPosition.z, targetZPos, ref upDownSdVelocity, 0.125f));
        if (cardTurnAnim != 0)
            cardTurnAnim = Mathf.SmoothDamp(cardTurnAnim, 0, ref cardTurnSdVelocity, 0.25f);
        animAnchor.localEulerAngles = new Vector3(0, 0, cardTurnAnim);
    }

    private void Render()
    {
        if (cardDefinition != null)
        {
            name = cardDefinition.name;

            cardNameText.text = cardDefinition.name;
            cardbackImage.color = cardDefinition.color;
            cardManacostText.color = cardDefinition.manaCost > 0 ? Color.white : new Color32(255, 255, 255, 32);
            cardManacostText.text = cardDefinition.manaCost.ToString();
            cardPortraitImage.sprite = cardDefinition.portrait;
            if (cardDefinition.portrait != null)
                cardPortraitImage.color = Color.white;
            
            cardDescriptionText.text = cardDefinition.GetDescription(champion);
        }
    }

    private void RenderDescriptionOnly()
    {
        cardDescriptionText.text = cardDefinition.GetDescription(champion);
    }

    private void OnValidate()
    {
        Render();
    }


    private void CardTurnAnimation(float amount)
    {
        cardTurnAnim = amount;
    }
}
