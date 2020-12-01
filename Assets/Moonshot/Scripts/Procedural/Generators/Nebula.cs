namespace SpaceGenerator.Generators
{
    using UnityEngine;
    using SharpNoise.Modules;

    [System.Serializable]
    public class GeneratorNebula : GeneratorBase
    {
        public int      _seed               = 801265826;
        public int      _offset             = 4;
        public float    _bias               = 0;

        public float    _frequencyBase      = 1f;
        public float    _lacunarityBase     = 2f;
        public float    _scaleBase          = 1f;
        public int      _octaveBase         = 8;

        public float    _frequencyDisplace  = 0.8f;
        public float    _lacunarityDisplace = 2f;
        public float    _scaleDisplace      = 0.8f;
        public int      _octaveDisplace     = 5;

        public override Module GetModule(int seed)
        {
            _seed = seed;
            return GetModule();
        }

        public override Module GetModule()
        {
            var map = new Clamp()
            {
                Source0 = new ScaleBias()
                {
                    Source0 = new Perlin()
                    {
                        Frequency   = _frequencyBase,
                        Lacunarity  = _lacunarityBase,
                        OctaveCount = _octaveBase,
                        Seed        = _seed,
                    },
                    Scale = _scaleBase,
                }
            };

            var control = new ScaleBias()
            {
                Source0 = new Perlin
                {
                    Frequency   = _frequencyDisplace,
                    Lacunarity  = _lacunarityDisplace,
                    OctaveCount = _octaveDisplace,
                    Seed        = _seed + _offset,
                },
                Scale = _scaleDisplace,
            };

            var displace = new Displace()
            {
                Source0     = map,
                XDisplace   = control,
                YDisplace   = control,
                ZDisplace   = new Constant()
                {
                    ConstantValue = _bias,
                },
            };

            return new Clamp()
            {
                Source0 = displace,
            };
        }
    }
}
