using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Moonshot.Scripts.Spaceship.Subsystems;

public class LunarBase : MonoBehaviour
{
    public float _repairTimer = 5f;
    public float _timer = 0f;
    public Rigidbody2D _rb2d;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    _rb2d = collision.rigidbody;
    //    if (_rb2d != null)
    //    {
    //        var fuelSystem = _rb2d.GetComponent<SystemFuelTank>();
    //        if (fuelSystem != null)
    //        {
    //            fuelSystem.Fuel = Mathf.Lerp(fuelSystem.Fuel, fuelSystem.FuelMax, 0.5f * Time.fixedDeltaTime);
    //        }

    //        var damageSystem = _rb2d.GetComponent<SystemDamageControl>();
    //        if (damageSystem != null)
    //        {
    //            damageSystem.CurrentHealth = Mathf.Lerp(damageSystem.CurrentHealth, damageSystem.MaxHealth, 0.25f * Time.fixedDeltaTime);
    //        }



    //        _timer += Time.fixedDeltaTime;
    //        if (_timer > _repairTimer)
    //        {
    //            var lifeSupport = _rb2d.GetComponent<SystemLifeSupport>();
    //            if (lifeSupport != null)
    //            {
    //                lifeSupport.Crew = lifeSupport.CrewMax;
    //            }

    //            foreach (var sys in _rb2d.GetComponentsInChildren<Assets.Moonshot.Scripts.Spaceship.Subsystems.Subsystem>())
    //            {
    //                sys.Status.State = eSystemState.ONLINE;
    //            }
    //        }
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    if (collision.rigidbody == _rb2d)
    //    {
    //        _timer = 0f;
    //        _rb2d = null;
    //    }
    //}
}
