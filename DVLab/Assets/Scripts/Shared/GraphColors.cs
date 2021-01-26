using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class GraphColors
    {
        private Gradient gradient;
        GradientColorKey[] colorKey;
        GradientAlphaKey[] alphaKey;
        private float min;
        private float max;

        public GraphColors()
        {
            gradient = new Gradient();

            // Populate the color keys at the relative time 0 and 1 (0 and 100%)
            colorKey = new GradientColorKey[4];
            colorKey[0].color = Color.red;
            colorKey[0].time = 0.0f;
            colorKey[1].color = Color.yellow;
            colorKey[1].time = 0.33f;
            colorKey[2].color = Color.green;
            colorKey[2].time = 0.66f;
            colorKey[3].color = Color.blue;
            colorKey[3].time = 1f;

            // Populate the alpha  keys at relative time 0 and 1  (0 and 100%)
            alphaKey = new GradientAlphaKey[2];
            alphaKey[0].alpha = 1.0f;
            alphaKey[0].time = 0.0f;
            alphaKey[1].alpha = 1.0f;
            alphaKey[1].time = 1.0f;

            gradient.SetKeys(colorKey, alphaKey);
        }

        public Color Evaluate(float time)
        {
            return gradient.Evaluate((time - min) / (max - min));
        }

        internal void SetMinMax()
        {
            List<string> attributes = new List<string>(Dataset.ListOfPoints[0].Keys);

            string attribute = attributes[attributes.Count - 1];

            min = Dataset.FindMin(attribute);
            max = Dataset.FindMax(attribute);
        }
    }
}
