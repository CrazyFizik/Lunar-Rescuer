namespace SpaceGenerator.Generators
{
    using UnityEngine;
    using SharpNoise.Modules;

    public class PlanetSelectGenerator : GeneratorBase
    {
        public int  _seed   = 1022282649;
        public int  _offset = 0;
        public int  _height = 1024;

        [Range(0.2f , 2.2f)]
        public float _frequency = 0.5f;

        public override Module GetModule(int seed)
        {
            _seed = seed;
            return GetModule();
        }

        public override Module GetModule()
        {
            int n = 2 + (int)Mathf.Log(_height, 2);

            var mountainTerrain = new RidgedMulti()
            {
                Frequency = 1,
                Lacunarity = 1.9532562354f,
                OctaveCount = n,
                Seed = _seed + 2 * _offset
            };

            var baseFlatTerrain = new Billow()
            {
                Frequency = 2,
                Lacunarity = 2.031242124f,
                OctaveCount = n,
                Seed = _seed + 1 * _offset,
            };

            var flatTerrain = new ScaleBias()
            {
                Source0 = baseFlatTerrain,
                Scale = 0.75,
                Bias = -0.25,
            };

            var terrainType = new Perlin()
            {
                Frequency = _frequency,
                Lacunarity = 2.1023423f,
                Persistence = 0.25,
                OctaveCount = n,
                Seed = _seed
            };

            var terrainSelector = new Select()
            {
                Source0 = flatTerrain,
                Source1 = mountainTerrain,
                Control = terrainType,
                LowerBound = 0,
                UpperBound = 1000,
                EdgeFalloff = 0.125,
            };

            var finalTerrain = new Turbulence()
            {
                Source0 = terrainSelector,
                Frequency = 4,
                Roughness = 8,
                Power = 0.125,
            };

            return finalTerrain;
        }
    }
}
