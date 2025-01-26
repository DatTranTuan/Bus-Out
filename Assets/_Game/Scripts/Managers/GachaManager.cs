using UnityEngine;
using System.Collections.Generic;

public class GachaManager 
{
    [SerializeField] private List<(int, ColorType)> gachaItems;

    public GachaManager (List<(int, ColorType)> gachaItems)
    {
        this.gachaItems = gachaItems;
    }

    public List<ColorType> RollGacha()
    {
        float totalRate = 0;
        List<ColorType> result = new List<ColorType>();

        while (gachaItems.Count > 0)
        {

            // Calculate the total drop rate
            foreach (var item in gachaItems)
            {
                totalRate += item.Item1;
            }

            // Generate a random value
            float randomValue = Random.Range(0, totalRate);

            // Determine which item is drawn
            float cumulativeRate = 0;

            for (int i = 0; i < gachaItems.Count; i++)
            {
                (int, ColorType) item = gachaItems[i];

                cumulativeRate += item.Item1;
                if (randomValue <= cumulativeRate)
                {
                    result.Add(item.Item2);
                    item.Item1 -= 1;

                    if (item.Item1 == 0)
                    {
                        gachaItems.RemoveAt(i);
                        break;
                    }
                }
            }
        }

        return result; // Fallback (shouldn't occur if rates are set correctly)
    }
}