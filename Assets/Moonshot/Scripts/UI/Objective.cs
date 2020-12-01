using UnityEngine;
using System.Collections;
using Utils;
using Utils.Extensions;
using UnityEngine.UI;

namespace Assets.Moonshot.Scripts.UI
{
    public class Objective : MonoBehaviour
    {
        public float _width = 45f;
        public float _height = 33f;
        public Image _marker;

        // Use this for initialization
        void Start()
        {
            _width = MissionManager.width;
            _height = MissionManager.height;
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void SetTarget(Vector2 position, Vector2 target, bool hide = false)
        {
            if (hide)
            {
                _marker.enabled = false;
                return;
            }
            else if (!_marker.enabled)
            {
                _marker.enabled = true;
            }

            var r = target - position;
            var x = Mathf.Clamp(r.x, -_width, +_width);
            var y = Mathf.Clamp(r.y, -_height, _height);
            var t = new Vector2(x / (2 * _width), y / (2 * _height));
            var mainRectangle = this.GetComponent<RectTransform>().rect;
            var px = mainRectangle.width * t.x;
            var py = mainRectangle.height * t.y;
            _marker.GetComponent<RectTransform>().localPosition = new Vector2(px, py);
            //transform.position = t;
            //RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, t.x);
            //RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, t.y)
        }
    }
}