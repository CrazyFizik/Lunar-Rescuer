namespace SpaceGenerator.Generators
{
    using UnityEngine;
    using SharpNoise.Modules;

    [System.Serializable]
    public class GeneratorMask2 : GeneratorBase
    {
        public int      _seed               = -1757626130;
        public int      _offset             = 4;

        public int      _octaves            = 5;
        public float    _frequency          = 0.5f;
        public float    _scale              = 2f;        

        public float    _bound              = 0.2f;
        public float    _edgeFalloff        = 0.1f;

        public override Module GetModule(int seed)
        {
            _seed = seed;
            return GetModule();
        }

        public override Module GetModule()
        {
            var A = new Clamp()
            {
                Source0 = new ScaleBias()
                {
                    Source0 = new Perlin()
                    {
                        Frequency = _frequency,
                        OctaveCount = _octaves,
                        Seed = _seed,
                    },
                    Scale = _scale,
                }
            };

            var B = new Clamp()
            {
                Source0 = new ScaleBias()
                {
                    Source0 = new Perlin()
                    {
                        Frequency = _frequency,
                        OctaveCount = _octaves,
                        Seed = _seed + _offset,
                    },
                    Scale = _scale,
                }
            };

            var select = new Select()
            {
                Source0 = new Constant()
                {
                    ConstantValue = -1,
                },

                Source1 = A,
                Control = B,
                LowerBound = _bound,
                UpperBound = 2,
                EdgeFalloff = _edgeFalloff
            };

            var clamp = new Clamp()
            {
                Source0 = select,
            };

            return clamp;
        }
    }
}
