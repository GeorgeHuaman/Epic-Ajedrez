using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArrayMap;

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
    public void UpdateMapOccupancy()
    {
        foreach (var m in maps)
        {
            m.occupiedBy = null;
        }

        PiecePositionDetector[] allPieces = GameObject.FindObjectsOfType<PiecePositionDetector>();

        foreach (var pieceDetector in allPieces)
        {
            string letra = pieceDetector.currentLetter;
            int numero = pieceDetector.currentNumber;

            foreach (var m in maps)
            {
                if (m.name == letra && m.number == numero)
                {
                    m.occupiedBy = pieceDetector.gameObject;
                    break;
                }
            }
        }
    }
    public bool IsGroundEmpty(string letter, int number)
    {
        foreach (var m in maps)
        {
            if (m.name == letter && m.number == number)
            {
                return m.occupiedBy == null;
            }
        }
        return false;
    }
    public Map GetMap(string letter, int number)
    {
        return maps.Find(m => m.name == letter && m.number == number);
    }
    [System.Serializable]
    public class Map
    {
        public GameObject ground;
        public string name;
        public int number;
        public GameObject occupiedBy;

        public Map(GameObject ground, string name, int number)
        {
            this.ground = ground;
            this.name = name;
            this.number = number;
            this.occupiedBy = null;
        }
    }
}
