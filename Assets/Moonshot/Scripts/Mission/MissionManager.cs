using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Moonshot.Scripts.Controllers;
using Assets.Moonshot.Scripts.Mission;
using SpaceGenerator.Generators;

public class MissionManager : MonoBehaviour
{
    public static float width = 45f;
    public static float height = 33f;

    public WorldBuilder _worldBuilder;
    public LunarBase _lunarBase;
    public PlayerController _player;

    public int _score = 0;
    public Mission _activeMission;
    public Mission _lastMission;

    public float _time = 6f;
    public float _timer;

    public GameObject _rescuePrefab;
    public GameObject _patrolPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponent<PlayerController>();
        _activeMission = null;
        _lastMission = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0f) _timer -= Time.deltaTime;

        if (_player._ship != null)
        {
            var healthRatio = _player._ship.DamageControl.CurrentHealth / _player._ship.DamageControl.MaxHealth;
            var fuelRatio = _player._ship.FlightComputer.FuelSystem.Fuel / _player._ship.FlightComputer.FuelSystem.FuelMax;
            var r = Mathf.Min(healthRatio, fuelRatio);
            if (r < 0.5f)
            {
                CreateRepairMission();
            }
        }
        else
        {
            return;
        }

        if (_activeMission == null & _lastMission == null)
        {
            CreateRescueMission();
            return;
        }
        else if (_activeMission == null & _lastMission != null)
        {
            _activeMission = new Mission(_lastMission);
            _lastMission = null;
            return;
        }
        else if (_activeMission._done & _timer <= 0f)
        {
            CreateRescueMission();
            return;
        }
        else if (_activeMission._fail)
        {
            CreateRescueMission();
            return;
        }
        else
        {
            var d = (Vector2)transform.position - _activeMission.Position;
            if (Mathf.Abs(d.x) <= 2 & Mathf.Abs(d.y) <= 2 & !_activeMission._done)
            {
                _activeMission._done = true;
                _score++;
                Debug.Log("Mission done!");
                _timer = Random.Range(_time, 2f * _time);
            }

        }
    }

    void CreateRescueMission()
    {
        var leftBound = -_worldBuilder._width + width;
        var rightBound = _worldBuilder._width - width;
        var position = transform.position;

        for (int i = 0; i < 10; i++)
        {
            var x = Mathf.RoundToInt(position.x);
            x = x + Mathf.RoundToInt(Random.Range(leftBound, rightBound));
            if (x > 0) x = Mathf.Clamp(x, (int)(width + position.x), (int)(rightBound - 0));
            if (x < 0) x = Mathf.Clamp(x, (int)(leftBound + 0), (int)(-width-position.x));
            var y0 = _worldBuilder._heightMap[x + _worldBuilder._width - 1] - _worldBuilder._height;
            var y1 = _worldBuilder._heightMap[x + _worldBuilder._width + 1] - _worldBuilder._height;
            var y = _worldBuilder._heightMap[x + _worldBuilder._width] - _worldBuilder._height;
            if (y >= y0 & y >= y1)
            {
                position = new Vector2(x, y + 1);
                var revard = Mathf.RoundToInt(Mathf.Abs(position.x - transform.position.x));
                var target = (GameObject.Instantiate(_patrolPrefab, position, Quaternion.identity, null)).transform;
                _activeMission = new Mission(target, revard);
                _activeMission._description = "Rescue";
                break;
            }
        }
    }

    void CreateRepairMission()
    {
        if (_activeMission != null) _lastMission = new Mission(_activeMission);
        _activeMission = new Mission(_lunarBase.transform, 1);
        _activeMission._description = "Return to base!";
    }
}

[System.Serializable]
public class Mission
{
    public int _reward = 1;    
    public bool _done = false;
    public bool _fail = false;
    public string _description;
    public Transform _target;

    Vector2 _position = Vector2.zero;
    public Vector2 Position
    {
        get
        {
            if (_target != null) _position = _target.position;
            else _fail = true;
            return _position;
        }
    }

    public Mission(Transform target, int reward)
    {
        _target = target;
        _position = target.position;
        _reward = reward;
    }

    public Mission(Mission mission)
    {
        _reward = mission._reward;
        _position = mission._position;
        _done = mission._done;
        _fail = mission._fail;
        _description = mission._description;
        _target = mission._target;
    }
}
