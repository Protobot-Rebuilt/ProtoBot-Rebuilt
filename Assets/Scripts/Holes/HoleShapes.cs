using UnityEngine;

namespace Protobot
{
    [CreateAssetMenu(fileName = "New Hole Shapes")]
    public class HoleShapes : ScriptableObject
    {
        //Singleton set up
        private static HoleShapes _instance;
        public static HoleShapes instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<HoleShapes>("General Data/Hole Shapes");

                return _instance;
            }
            private set => _instance = value;
        }

        public HoleShapes()
        {
            instance = this;
        }

        //actual hole shape data
        public HoleShape[] shapeList;

        public Mesh GetShapeMesh(string name)
        {
            foreach (HoleShape shape in shapeList)
            {
                if (name.ToLower().Contains(shape.shapeName.ToLower()))
                    return shape.shapeMesh;
            }

            return null;
        }

        public string GetShapeName(Mesh mesh)
        {
            foreach (HoleShape shape in shapeList)
            {
                if (shape.shapeMesh == mesh)
                    return shape.shapeName;
            }

            return null;
        }
    }

    [System.Serializable]
    public class HoleShape
    {
        public string shapeName;
        public Mesh shapeMesh;
    }
}