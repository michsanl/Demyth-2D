using System.Collections;
using System.Collections.Generic;
using Core;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class DeathDescriptionManager : SceneService
{
    public string SelectedDeathDescription => _selectedDeathDescription;
    
    [SerializeField] private DeathDescriptionSO _deathDescriptionSO;
    private string _selectedDeathDescription;

    public void SetDeathDescMariaDialogueA()
    {
        if (Localization.language == "id")
        {
            _selectedDeathDescription = _deathDescriptionSO.IDMariaDialogueA;
        }
        else
        {
            _selectedDeathDescription = _deathDescriptionSO.ENMariaDialogueA;
        }
    }

    public void SetDeathDescMariaDialogueB()
    {
        if (Localization.language == "id")
        {
            _selectedDeathDescription = _deathDescriptionSO.IDMariaDialogueB;
        }
        else
        {
            _selectedDeathDescription = _deathDescriptionSO.ENMariaDialogueB;
        }
    }

    public void SetDeathDescPetraFight()
    {
        if (Localization.language == "id")
        {
            _selectedDeathDescription = _deathDescriptionSO.IDPetraFight;
        }
        else
        {
            _selectedDeathDescription = _deathDescriptionSO.ENPetraFight;
        }
    }

    public void SetDeathDescSriDialogueA()
    {
        if (Localization.language == "id")
        {
            _selectedDeathDescription = _deathDescriptionSO.IDSriDialogueA;
        }
        else
        {
            _selectedDeathDescription = _deathDescriptionSO.ENSriDialogueA;
        }
    }

    public void SetDeathDescSriDialgoueB()
    {
        if (Localization.language == "id")
        {
            _selectedDeathDescription = _deathDescriptionSO.IDSriDialogueB;
        }
        else
        {
            _selectedDeathDescription = _deathDescriptionSO.ENSriDialogueB;
        }
    }

    public void SetDeathDescSriFight()
    {
        if (Localization.language == "id")
        {
            _selectedDeathDescription = _deathDescriptionSO.IDSriFight;
        }
        else
        {
            _selectedDeathDescription = _deathDescriptionSO.ENSriFight;
        }
    }

}
