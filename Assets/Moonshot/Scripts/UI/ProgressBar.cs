using UnityEngine;
using System.Collections;
using Utils;
using Utils.Extensions;
using UnityEngine.UI;

namespace Assets.Moonshot.Scripts.UI
{
    public class ProgressBar : MonoBehaviour
    {
        public enum eBarDirections { LeftToRight, RightToLeft, UpToDown, DownToUp }

        public eBarDirections _barDirection = eBarDirections.RightToLeft;
        public float _startValue = 0f;
        public float _endValue = 1f;
        public Color _tint = Color.white;
        public Transform _bar;
        public Image _image;

        private Vector2 _size;
        private RectTransform _rectangle;
        private float _width = 0f;
        private float _height = 0f;

        // Use this for initialization
        void Start()
        {
            if (_bar != null)
            {
                _image = _bar.GetComponent<Image>();
                _size = _image.rectTransform.sizeDelta;
                _width = _size.x;
                _height = _size.y;
                //_image.GetComponent<RectTransform>();
                //_width = _image.GetComponent<RectTransform>().rect.width;
                //_height = _image.GetComponent<RectTransform>().rect.height;
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void FillBar(float currentValue, float minValue, float maxValue)
        {
            var targetFill = Math.Remap(currentValue, minValue, maxValue, _startValue, _endValue);
            _image.fillAmount = targetFill;

        }

        public void TileBar(float currentValue, float minValue, float maxValue)
        {
            var targetFill = Math.Remap(currentValue, minValue, maxValue, _startValue, _endValue);
            var size = Vector3.one;
            switch (_barDirection)
            {
                case eBarDirections.LeftToRight:
                    size.x = targetFill;
                    break;
                case eBarDirections.RightToLeft:
                    size.x = 1f - targetFill;
                    break;
                case eBarDirections.DownToUp:
                    size.y = targetFill;
                    break;
                case eBarDirections.UpToDown:
                    size.y = 1f - targetFill;
                    break;
            }
            //_image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _width * size.x);
            
            size.x = size.x * _width;
            size.y = size.y * _height;
            _image.rectTransform.sizeDelta = size;
            if (_image.color != _tint) _image.color = _tint;

            //_bar.localScale = targetLocalScale;
            //var rectangle = _rectangle.rect;
            //rectangle.width = _width * size.x;
            //rectangle.height = _height * size.y;
            //_rectangle.rect.Set(rectangle.x, rectangle.y, rectangle.width, rectangle.height);

        }
    }
}