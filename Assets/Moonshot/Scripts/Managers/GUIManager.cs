using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Moonshot.Scripts.UI;
using Assets.Moonshot.Scripts.Controllers;

public class GUIManager : MonoBehaviour
{
	public PlayerController _player;
	public Canvas _mainCanvas;
	public GameObject _hud;
	public ProgressBar _healthBar;
	public ProgressBar _fuelBar;
	public ProgressBar _crewBar;
	public SystemIndicator _systemIndicator;
	public TextIndicator _horizontalSpeed;
	public TextIndicator _verticalSpeed;
	public TextIndicator _altitude;
	public Objective _objective;
	public TextIndicator _score;


	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void LateUpdate()
	{
		if (_player == null) return;
		if (_player._ship == null) return;

		UpdateHealthBar();
		UpdateFuelBar();
		UpdateCrewBar();
		UpdateSybsytems();
		UpdateAltitude();
		UpdateObjective();
	}

	public void Init(PlayerController Player)
	{
		this._player = Player;
		if (_systemIndicator != null) _systemIndicator.Init(this._player);
	}

	public void UpdateHealthBar()
	{
		if (_healthBar == null) return;
		var currentHealth = _player._ship.DamageControl.CurrentHealth;
		var minHealth = 0f;
		var maxHealth = _player._ship.DamageControl.MaxHealth;
		_healthBar.FillBar(currentHealth, minHealth, maxHealth);
	}

	public void UpdateFuelBar()
	{
		if (_fuelBar == null) return;
		var fuel = _player._ship.FlightComputer.FuelSystem;
		if (fuel == null) return;
		var currentFuel = fuel.FuelRatio;
		var minFuel = 0f;
		var maxFuel = 1f;
		_fuelBar.FillBar(currentFuel, minFuel, maxFuel);
	}

	public void UpdateCrewBar()
	{
		if (_crewBar == null) return;

		if (_player._ship.LifeSupport.Status.GetCondition(Assets.Moonshot.Scripts.Spaceship.Subsystems.eSystemState.OFFLINE))
		{
			_crewBar._tint = Color.red;
		}
		else if (_player._ship.LifeSupport.Status.GetCondition(Assets.Moonshot.Scripts.Spaceship.Subsystems.eSystemState.DAMAGED))
        {
			_crewBar._tint = Color.yellow;
		}
		else
        {
			_crewBar._tint = Color.white;
		}

		var currentCrew = _player._ship.LifeSupport.Crew;
		var minCrew = 0;
		var maxCrew = 7;
		_crewBar.TileBar(currentCrew, minCrew, maxCrew);
	}

	public void UpdateSybsytems()
	{
		_systemIndicator.UpdateSybsytems();
	}

	public void UpdateAltitude()
    {
		_horizontalSpeed.UpdateText(_player._ship.FlightComputer.Data.Velocity.x);
		_verticalSpeed.UpdateText(_player._ship.FlightComputer.Data.Velocity.y);
		_altitude.UpdateText(_player._ship.FlightComputer.Data.Altitude);
	}

	public void UpdateObjective()
    {
		var position = _player._ship.FlightComputer.Data.Position;
		var mission = _player._mission._activeMission;
		if (mission == null)
		{
			_objective.SetTarget(position, Vector2.zero, false);
		}
		else
		{
			var target = mission.Position;			
			var hide = mission._done;
			_objective.SetTarget(position, target, hide);
			_score.UpdateText(_player._mission._score);
		}		

    }

}
