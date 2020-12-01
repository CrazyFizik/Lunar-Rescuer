namespace SpaceGenerator.Generators
{
    using UnityEngine;
    using SharpNoise.Modules;

    public class PlanetBlendGenerator : GeneratorBase
    {
        public int      _seed                   = 1115133112;
        public int      _offset                 = 1;
        public int      _height                 = 1024;
        public float    _clamp                  = 1.5f;

        public float    _coastFrequency         = 1.5f;
        public float    _coastLacunarity        = 2.031242124f;

        public float    _hillsFrequency         = 2.2f;
        public float    _hillsLacunarity        = 2.1023423f;

        public float    _mountainFrequency      = 2.2f;
        public float    _mountainLacunarity     = 1.9532562354f;

        public override Module GetModule(int seed)
        {
            _seed = seed;
            return GetModule();
        }

        public Module GetModuleA()
        {
            int octaves = 2 + (int)Mathf.Log(_height, 2);
            octaves = Mathf.Max(8, octaves);

            var map = new Clamp()
            {
                Source0 = new ScaleBias()
                {
                    Source0 = new Perlin()
                    {
                        Frequency = 1.0f,
                        Lacunarity = 2.0f,
                        OctaveCount = octaves,
                        Seed = _seed,
                    },
                    Scale = 1.0f,
                }
            };

            var control = new ScaleBias()
            {
                Source0 = new Perlin
                {
                    Frequency = 0.8f,
                    //Frequency = 1.8f,  // sharp
                    //Lacunarity = 1.9f, // for clouds
                    Lacunarity = 2.5f, // for land
                    OctaveCount = (int)(octaves / 2f) + 1,
                    Seed = _seed + 4,
                },
                Scale = 0.8f,
            };

            var displace = new Displace()
            {
                Source0 = map,
                XDisplace = control,
                YDisplace = control,
                ZDisplace = new Constant()
                {
                    ConstantValue = 0,
                },
            };

            return new Clamp()
            {
                Source0 = displace,

            };
        }


        public override Module GetModule()
        {
            int octaves = 2 + (int)Mathf.Log(_height, 2);

            var blended = new Blend()
            {
                Source0 = new Billow()
                {
                    Frequency   = _hillsFrequency,
                    Lacunarity  = _hillsLacunarity,

                    Seed        = _seed + 256 * _offset,
                    OctaveCount = octaves
                },
                Source1 = new RidgedMulti()
                {
                    Frequency   = _mountainFrequency,
                    Lacunarity  = _mountainLacunarity,

                    SpectralWeightsExponent = 0.5,
                    Offset      = 1.1,
                    Gain        = 2.1,
                    
                    Seed        = _seed + 128 * _offset,
                    OctaveCount = octaves
                },
                Control = new Perlin()
                {
                    Frequency   = _coastFrequency,
                    Lacunarity = _coastLacunarity,

                    Seed        = _seed + 64 * _offset,
                    OctaveCount = octaves
                }
            };

            var turbulence = new Turbulence()
            {
                Seed        = _seed,
                Frequency   = 4,
                Power       = 0.125,

                Source0     = blended
            };

            return new Clamp()
            {
                Source0     = blended,
                LowerBound  = -_clamp,
                UpperBound  =  _clamp,
            };
        }
    }
}
