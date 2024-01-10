using CustomExtensions;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Core;
using UnityEngine.Events;
using echo17.Signaler.Core;
using Demyth.Gameplay;

public class LevelManager : SceneService, ISubscriber
{
    public Level CurrentLevel { get; private set; }

    [SerializeField] 
    private EnumId starterLevel;
    [DictionaryDrawerSettings][ShowInInspector]
    private Dictionary<EnumId, Level> _levelCollections = new();

    public UnityEvent OnOpenMainMenu;
    public UnityEvent OnOpenGameLevel;

    private Transform _player;
    private ISubscription _playerSpawnSubs;

    public override IEnumerator StartService()
    {
        PrepareLevels();
        yield return new WaitForSeconds(0.5f);
    }

    private void Awake()
    {
        _playerSpawnSubs = Signaler.Instance.Subscribe<PlayerSpawnEvent>(this, OnPlayerSpawned);
    }

    private void Start()    
    {
        OpenLevel(starterLevel);
    }

    public void OpenLevel(EnumId targetLevelId)
    {
        foreach (var lvl in _levelCollections.Values)
        {
            lvl.SetActive(lvl.ID == targetLevelId);
        }

        var level = GetLevelByID(targetLevelId);
        SetLevel(level);
    }

    public void SetLevel(Level targetLevel)
    {
        CurrentLevel = targetLevel;

        if (targetLevel == starterLevel)
        {
            OnOpenMainMenu?.Invoke();
        }
        else
        {
            OnOpenGameLevel?.Invoke();
        }

        SetPlayerPosition(targetLevel.StarterPosition);
    }

    public void ChangeLevelByGate(EnumId previousLevelID, EnumId nextLevelID)
    {
        var nextLevel = GetLevelByID(nextLevelID);
        var previousLevel = GetLevelByID(previousLevelID);
        
        var previousLevelPoint = nextLevel.GetLevelPoint(previousLevelID);
        SetPlayerPosition(previousLevelPoint);

        previousLevel.SetActive(false);
        nextLevel.SetActive(true);

        CurrentLevel = nextLevel;
    }

    public Level GetLevelByID(EnumId levelId)
    {
        return _levelCollections[levelId];
    }

    private void SetPlayerPosition(Vector3 targetPosition)
    {
        if (_player != null)
            _player.position = targetPosition;
    }

    private void PrepareLevels()
    {
        var levels = GetComponentsInChildren<Level>(true);
        foreach (var level in levels)
        {
            level.InjectLevelManager(this);
            _levelCollections.TryAdd(level.ID, level);
        }        
    }

    private bool OnPlayerSpawned(PlayerSpawnEvent signal)
    {
        _player = signal.Player.transform;

        return true;
    }
}
