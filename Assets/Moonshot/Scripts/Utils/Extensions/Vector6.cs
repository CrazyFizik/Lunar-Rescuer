using System;
using UnityEngine;

namespace Utils.Extensions
{
    [System.Serializable]
    public class Vector6
    {
        [SerializeField] private Vector3 _positive = Vector3.zero;
        [SerializeField] private Vector3 _negative = Vector3.zero;

        public static readonly Vector3[] _directions = { Vector3.forward, Vector3.back, Vector3.up, Vector3.down, Vector3.right, Vector3.left };

        public enum Direction { FORWARD = 0, BACK = 1, UP = 2, DOWN = 3, RIGHT = 4, LEFT = 5 };
        public static readonly Direction[] _indexes = (Direction[])Enum.GetValues(typeof(Direction));

        public Vector3 Positive
        {
            get
            {
                return _positive;
            }
            set
            {
                _positive = value;
            }
        }

        public Vector3 Negative
        {
            get
            {
                return _negative;
            }
            set
            {
                _negative = value;
            }
        }

        public float Forward
        {
            get
            {
                return _positive.z;
            }
            set
            {
                _positive.z = value;
            }
        }
        public float Back
        {
            get
            {
                return _negative.z;
            }
            set
            {
                _negative.z = value;
            }
        }
        public float Up
        {
            get
            {
                return _positive.y;
            }
            set
            {
                _positive.y = value;
            }
        }
        public float Down
        {
            get
            {
                return _negative.y;
            }
            set
            {
                _negative.y = value;
            }
        }
        public float Right
        {
            get
            {
                return _positive.x;
            }
            set
            {
                _positive.x = value;
            }
        }
        public float Left
        {
            get
            {
                return _negative.x;
            }
            set
            {
                _negative.x = value;
            }
        }

        public float this[Direction index]
        {
            get
            {
                switch (index)
                {
                    case Direction.FORWARD:
                        return Forward;
                    case Direction.BACK:
                        return Back;
                    case Direction.UP:
                        return Up;
                    case Direction.DOWN:
                        return Down;
                    case Direction.RIGHT:
                        return Right;
                    case Direction.LEFT:
                        return Left;
                }
                return 0;
            }
            set
            {
                switch (index)
                {
                    case Direction.FORWARD:
                        Forward = value;
                        break;
                    case Direction.BACK:
                        Back = value;
                        break;
                    case Direction.UP:
                        Up = value;
                        break;
                    case Direction.DOWN:
                        Down = value;
                        break;
                    case Direction.RIGHT:
                        Right = value;
                        break;
                    case Direction.LEFT:
                        Left = value;
                        break;
                }
            }
        }

        public float this[int index]
        {
            get
            {
                return this[_indexes[index]];
            }
        }

        public float Max
        {
            get
            {
                return Mathf.Max(_positive.x, _positive.y, _positive.z, _negative.x, _negative.y, _negative.z);
            }
        }

        public float Min
        {
            get
            {
                return Mathf.Min(_positive.x, _positive.y, _positive.z, _negative.x, _negative.y, _negative.z);
            }
        }

        public Vector6()
        {
            // Nothing
        }

        public Vector6(float x, float y, float z)
        {
            this._positive = new Vector3(x, y, z);
            this._negative = new Vector3(x, y, z);
        }

        public Vector6(Vector3 positive)
        {
            this._positive = positive;
            this._negative = positive;
        }

        public Vector6(Vector3 positive, Vector3 negative)
        {
            this._positive = positive;
            this._negative = negative;
        }

        public Vector6(Vector6 vector6)
        {
            this.Positive = vector6.Positive;
            this.Negative = vector6.Negative;
        }

        public void Reset()
        {
            _positive = Vector3.zero;
            _negative = Vector3.zero;
        }

        public void Add(Vector3 vector)
        {
            for (int i = 0; i < _indexes.Length; i++)
            {
                Direction axis = _indexes[i];
                var projection = Vector3.Dot(vector, _directions[(int)axis]);
                if (projection > 0)
                {
                    this[axis] += projection;
                }
            }
        }

        public float GetMagnitude(Vector3 direction)
        {
            float sqrMagnitude = 0;
            for (int i = 0; i < _indexes.Length; i++)
            {
                Direction axis = _indexes[i];
                float projection = Vector3.Dot(direction.normalized, _directions[(int)axis]);
                if (projection > 0)
                {
                    sqrMagnitude += Mathf.Pow(projection * this[axis], 2);
                }
            }
            return Mathf.Sqrt(sqrMagnitude);
        }

        public void Scale(float factor)
        {
            _positive *= factor;
            _negative *= factor;
        }

        public void Scale(Vector3 factor)
        {
            _positive = Vector3.Scale(_positive, factor);
            _negative = Vector3.Scale(_negative, factor);
        }

        public void Scale(Vector6 factor)
        {
            _positive = Vector3.Scale(_positive, factor.Positive);
            _negative = Vector3.Scale(_negative, factor.Negative);
        }

        public void Normalize()
        {
            if (Max == 0) return;
            var max = Max;
            Positive = Positive / max;
            Negative = Negative / max;
        }

    }
}
