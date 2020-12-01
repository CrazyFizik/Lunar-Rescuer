namespace SpaceGenerator.Generators
{
    using UnityEngine;
    using SharpNoise.Modules;

    [System.Serializable]
    public class GeneratorBase
    {
        public float _frequncy = 2f;
        public int _octaves = 8;

        public virtual Module GetModule(int seed)
        {
            var map = new Perlin()
            {
                Frequency = _frequncy,
                OctaveCount = _octaves,
                Seed = seed,
            };
            return map;
        }

        public virtual Module GetModule()
        {
            var map = new Perlin()
            {
                Frequency = _frequncy,
                OctaveCount = _octaves,
                Seed = new System.Random().Next()// UnityEngine.Random.Range(int.MinValue, int.MaxValue),
            };

            return map;
        }
    }
}
