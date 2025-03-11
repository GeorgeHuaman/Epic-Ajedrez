using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayMap : MonoBehaviour
{
    //1.79 - 8
    //-6.27 -0.419 6.276 Inicio
    public GameObject groundPrefab;
    public GameObject groundFather;
    public Vector3 inicio;
    private Vector3 positionChange;
    private string abc = "ABCDEFGH";
    public List<Map> maps = new List<Map>();
    // Start is called before the first frame update
    void Start()
    {
        positionChange = inicio;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject a = Instantiate(groundPrefab, positionChange, Quaternion.identity,groundFather.transform);
                positionChange.z -= 1.79f;
                Coordinate coordinate = a.GetComponent<Coordinate>();
                CoordinateGround(i, j, coordinate);
                maps.Add(new Map(a, coordinate.letra, coordinate.number));
            }
            positionChange.z = inicio.z;
            positionChange.x += 1.79f;
        }
    }

    private void CoordinateGround(int i, int j, Coordinate coordinate)
    {
        coordinate.number = i + 1;
        char b = abc[j];
        coordinate.letra = b.ToString();
    }

    [System.Serializable]
    public class Map
    {
        public GameObject ground;
        public string name;
        public int number;

        public Map(GameObject ground, string name, int number)
        {
            this.ground = ground;
            this.name = name;
            this.number = number;
        }
    }
}
