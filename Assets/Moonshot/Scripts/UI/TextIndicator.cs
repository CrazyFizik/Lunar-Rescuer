using UnityEngine;
using System.Collections;
using Utils;
using Utils.Extensions;
using UnityEngine.UI;

namespace Assets.Moonshot.Scripts.UI
{
    public class TextIndicator : MonoBehaviour
    {
        public Text _text;

        // Use this for initialization
        void Start()
        {
            if (_text == null) _text = GetComponentInChildren<Text>();
        }


        public void UpdateText(float value)
        {
            if (_text == null) return;
            _text.text = value.ToString("f0");
        }

    }
}