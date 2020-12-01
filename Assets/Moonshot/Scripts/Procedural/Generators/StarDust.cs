namespace SpaceGenerator.Generators
{
    using UnityEngine;
    using SharpNoise.Modules;

    [System.Serializable]
    public class GeneratorStarDust : GeneratorBase
    {
        public int      _seed       = 42;
        public int      _height     = 1024;

        public float    _frequency  = 2;
        public float    _lacunarity = 1.5f;

        public float    _ridgness   = 2;
        public float    _roughness  = 1;

        public override Module GetModule(int seed)
        {
            _seed = seed;
            return GetModule();
        }

        public override Module GetModule()
        {
            int octaves = 2 + (int)Mathf.Log(_height, 2);

            var map = new RidgedMulti()
            {
                Frequency = _frequency,
                Lacunarity = _lacunarity,
                OctaveCount = octaves,
                Gain = _ridgness,
                Offset = _roughness,
                Seed = _seed
            };

            return map;
        }
    }
}
