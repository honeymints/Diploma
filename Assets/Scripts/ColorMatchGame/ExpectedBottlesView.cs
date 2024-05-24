using System.Collections.Generic;
using UnityEngine;

namespace ColorMatchGame
{
    public class ExpectedBottlesView : MonoBehaviour
    {
        public List<SpriteRenderer> bottleMasks;

        public static void SetBottleEmpty(ref SpriteRenderer expectedBottleColor)
        {
            if (expectedBottleColor.material.GetColor("_C1").Equals(Color.black) || expectedBottleColor.material.GetColor("_C1").Equals(Color.white))
            {
                var fillAmount = -0.25f;
                expectedBottleColor.material.SetFloat("_FillAmount", fillAmount);
            }
        }
        
        public void SetColorForBottles(int i, List<Color> expectedBottleColors)
        {
            var fillAmount = -0.25f;
            for (int k = 0; k < expectedBottleColors.Count; k++)
            {
                fillAmount += 0.105f;
                bottleMasks[i].material.SetColor($"_C{k + 1}", expectedBottleColors[k]);
                bottleMasks[i].material.SetFloat("_FillAmount", fillAmount);

                var expectedBottleColor = bottleMasks[i];
                SetBottleEmpty(ref expectedBottleColor);
            }
        }

        public void CreateBotlles(List<ExpectedBottle> expectedBottle)
        {
            for (int i = 0; i < expectedBottle.Count; i++)
            {
                bottleMasks.Add(transform.GetChild(i).GetChild(0).GetComponentInChildren<SpriteRenderer>());
                SetColorForBottles(i, expectedBottle[i].ExpectedBottleColors);
            }
        }

    }
}
