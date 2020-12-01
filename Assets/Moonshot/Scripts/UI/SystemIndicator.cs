using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Moonshot.Scripts.UI;
using Assets.Moonshot.Scripts.Controllers;
using Assets.Moonshot.Scripts.Spaceship.Subsystems;

namespace Assets.Moonshot.Scripts.UI
{
    public class SystemIndicator : MonoBehaviour
    {
        public Color _colorNormal = Color.green;
        public Color _colorDamaged = Color.yellow;
        public Color _colorOffline = Color.red;

        public Transform[] _indicators;
        public Text[] _texts;

        public PlayerController _player;
        public SystemDamageControl _damageControl;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Init(PlayerController player)
        {
            _player = player;
            if (_player._ship != null) _damageControl = _player._ship.DamageControl;
            
            _texts = GetComponentsInChildren<Text>();
            _indicators = new Transform[_texts.Length];
            for (int i = 0; i < _texts.Length; i++)
            {
                _indicators[i] = _texts[i].transform;
                _texts[i].text = "";
                _texts[i].enabled = false;
            }
            _texts = GetComponentsInChildren<Text>();
            _indicators = new Transform[_texts.Length];
            var mainRectangle = this.GetComponent<RectTransform>().rect;
            var width = mainRectangle.width;
            var step = width / _texts.Length;
            var x0 = -width / 2f + step/2f;
            var rectangle = _texts[0].rectTransform.rect;            
            
            int j = 0;
            for (; j < _damageControl.Systems.Count; j++)
            {
                _texts[j].enabled = true;
                _indicators[j] = _texts[j].GetComponent<Transform>();
                var x = x0 + j * step;
                var y = 0;
                _indicators[j].GetComponent<RectTransform>().localPosition = new Vector2(x, y);
                _texts[j].text = _damageControl.Systems[j].Type;
                _texts[j].color = Color.blue;
            }
        }

        public void UpdateSybsytems()
        {
            if (_damageControl == null) return;

            var systems = _damageControl.Systems;
            for (int i = 0; i < _texts.Length; i++)
            {
                if (i >= systems.Count) break;
                
                var offline = systems[i].Status.GetCondition(Assets.Moonshot.Scripts.Spaceship.Subsystems.eSystemState.OFFLINE);                
                var damaged = systems[i].Status.GetCondition(Assets.Moonshot.Scripts.Spaceship.Subsystems.eSystemState.DAMAGED);
                var invalide = systems[i].Status.GetCondition(Assets.Moonshot.Scripts.Spaceship.Subsystems.eSystemState.INVALID);

                if (offline)
                {
                    if (_texts[i].color != _colorOffline) _texts[i].color = _colorOffline;

                }
                else if (damaged)
                {
                    if (_texts[i].color != _colorDamaged) _texts[i].color = _colorDamaged;
                }
                else if (invalide)
                {
                    //if (_texts[i].text != string.Empty) _texts[i].text = string.Empty;
                }
                else
                {
                    if (_texts[i].color != _colorNormal) _texts[i].color = _colorNormal;
                }
            }
        }
    }
}