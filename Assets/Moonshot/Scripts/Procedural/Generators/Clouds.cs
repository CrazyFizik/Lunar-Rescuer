namespace SpaceGenerator.Generators
{
    using UnityEngine;
    using SharpNoise.Modules;

    [System.Serializable]
    public class GeneratorClouds : GeneratorBase
    {
        public int      _seed       = -41558836;

        public int      _height     = 1024;
        public float    _frequency  = 1.5f;
        public float    _lacunarity = 4.0142578125f;
        public float    _spin       = 2f;

        public override Module GetModule(int seed)
        {
            _seed = seed;
            return GetModule();
        }

        public override Module GetModule()
        {
            int octaves = 2 + (int)Mathf.Log(_height, 2);

            var clouds = new Perlin()
            {
                Seed = _seed,
                Frequency = _frequency,
                Persistence = 0.5f,
                Lacunarity = _lacunarity,
                OctaveCount = octaves,
            };

            var displacement = new ScalePoint()
            {
                Source0 = clouds,

                XScale = _spin,
                YScale = 1f,
                ZScale = _spin,
            };

            var turbulence = new Turbulence()
            {
                Source0 = displacement,

                //Seed = rng.Next(),
                Frequency = 2,
                Power = 0.7f,
                Roughness = 2
            };

            return turbulence;
        }
    }
}
