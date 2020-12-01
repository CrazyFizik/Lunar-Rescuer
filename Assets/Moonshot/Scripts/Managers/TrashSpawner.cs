using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public GameObject[] _prefabs;

    private float _dt = 1f;
    private float _timer = 0f;
    private Vector2 _speed = Vector2.zero;
    private Vector2 _position = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(_prefabs.Length.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _dt)
        {
            Spawn();
            _dt = Random.Range(0.25f, 4);
            _timer = 0f;
        }
    }

    private void FixedUpdate()
    {
        var position = (Vector2)transform.position;
        _speed =  position - _position;
        _speed = _speed / Time.fixedDeltaTime;
        _position = position;
    }

    private void Spawn()
    {
        var position = (Vector2)transform.position;
        var value = Mathf.PerlinNoise(position.x, position.y);
        var trials = Random.Range(1, _prefabs.Length);
        var spawnPosition = new Vector2();
        spawnPosition.y = position.y + _speed.y + 24;
        spawnPosition.x = position.x + _speed.x * Mathf.Sqrt(Mathf.Abs(spawnPosition.y - position.y));
        for (int i = 0; i < trials; i++)
        {
            var p = Random.value;
            if (p > value)
            {
                int index = Random.Range(0, _prefabs.Length);
                var sample = _prefabs[index];
                var obj = GameObject.Instantiate(sample, spawnPosition, Quaternion.Euler(0, 0, Random.Range(0,360)), null);
                var rb2d = obj.GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    var x = Random.Range(-4, 4);
                    var y = Random.Range(-4, 4);
                    var f = new Vector2(x, y);
                    f = f * rb2d.mass;
                    rb2d.AddRelativeForce(f, ForceMode2D.Impulse);
                }
            }
            spawnPosition.x += Random.Range(0, 24);
            spawnPosition.y += Random.Range(0, 24);
        }
    }
}
