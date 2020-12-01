using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;
using SharpNoise;
using SharpNoise.Builders;

namespace SpaceGenerator.Generators
{
    public class WorldBuilder : MonoBehaviour
    {
        public static bool _generateAtStart = false;
        public static int _minSize = 64;
        public int _seed = 42;
        public int _width = 1024;
        public int _height = 64;
        public int _baseSize = 16;
        public int[] _heightMap;

        [System.NonSerialized] public int[,] _levelMap;

        [SerializeField] Tilemap _tilemap;
        [SerializeField] RuleTile _tile;
        [SerializeField] GameObject[] _structuresPrefab;

        // Start is called before the first frame update
        void Start()
        {
            if (_generateAtStart)
            {
                GenerateWorld();
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GenerateWorld()
        {
            _seed = System.DateTime.UtcNow.Second;
            BuildWorld();
        }

        [ContextMenu("Rebuild")]
        public void BuildWorld()
        {         
            UnityEngine.Debug.Log(_seed.ToString());
            GenerateMapAsync(_seed);
        }

        async void GenerateMapAsync(int seed)
        {
            GeneratorBase generator = null;
            generator = new GeneratorBase();
            //generator = new PlanetBlendGenerator();
            //generator = new PlanetSelectGenerator();
            var module = generator.GetModule(seed);
            var elevationMap = new NoiseMap();
            var builder = new PlaneNoiseMapBuilder();
            //builder.EnableSeamless = true;
            //builder.EnableSeamless = false;
            builder.SetBounds(0, _width / _minSize, 0, _width / _minSize);
            builder.SetDestSize(_width, 1);
            builder.SourceModule = module;
            builder.DestNoiseMap = elevationMap;

            var cts = new CancellationTokenSource();
            bool cancelled = false;

            try
            {
                await builder.BuildAsync(cts.Token);
            }
            catch (System.OperationCanceledException)
            {
                cancelled = true;
            }

            // Find max/min
            var map = builder.DestNoiseMap;
            float max = float.MinValue;
            float min = float.MaxValue;
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var value = map[x, y];
                    if (value >= max) max = value;
                    if (value <= min) min = value;
                }
            }

            // Generate height map from 2 maps
            var delta = max - min;
            var heightMap = new float[2 * map.Width];
            _width = map.Width;
            for (int x = 0; x < heightMap.Length; x++)
            {
                int index = x % map.Width;
                var value = map[index, 0];
                value = (value - min) / delta;
                heightMap[x] = value;
            }

            // Dig launcpad
            var launchpadSize = _baseSize / 2;
            var launchpadHeigth = 0f;
            var counter = 0f;
            for (int x = heightMap.Length / 2 - launchpadSize; x < heightMap.Length / 2 + launchpadSize; x++)
            {
                launchpadHeigth += heightMap[x];
                counter += 1f;
            }
            launchpadHeigth /= counter;
            for (int x = heightMap.Length / 2 - launchpadSize; x < heightMap.Length / 2 + launchpadSize; x++)
            {
                heightMap[x] = launchpadHeigth;
            }

            // generate level map  
            _heightMap = new int[2 * _width];
            _levelMap = new int[2 * _width, 2 * _height];
            int x0 = _levelMap.GetLength(0) / 2;
            int y0 = _levelMap.GetLength(1) / 2;
            int z0 = -1;
            int zeroLevel = y0 - Mathf.RoundToInt(_height * launchpadHeigth) - 1;
            for (int x = 0; x < _levelMap.GetLength(0); x++)
            {
                var value = heightMap[x];
                var height = Mathf.RoundToInt(_height * value) + zeroLevel;
                if (height >= 2 * _height)
                {
                    height = 2 * _height - 1;
                }
                _heightMap[x] = height;

                for (int y = 0; y < height; y++)
                {
                    _levelMap[x, y] = 1;
                }
            }

            // place tiles
            _tilemap.ClearAllTiles();
            for (int x = 0; x < _levelMap.GetLength(0); x++)
            {
                for (int y = 0; y < _levelMap.GetLength(1); y++)
                {
                    Vector3Int pos = new Vector3Int(x - x0, y - y0, z0);
                    var h = _levelMap[x, y];
                    if (h > 0)
                    {
                        _tilemap.SetTile(pos, _tile);
                    }
                }
            }

            if (cancelled)
            {
                return;
            }
        }

    }
}
