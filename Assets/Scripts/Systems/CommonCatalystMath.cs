using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CommonCatalystMath {


    /// <summary>
    /// Uses math to determine unit circle positions for a shape of certain sides.
    /// </summary>
    /// <param name="numSides"> Number of sides the shape to calculate for has </param>
    /// <returns> A list of 3D Vector3 coordinates for each point on the unit circle </returns>
    public static List<Vector3> GetPositionsOnUnitCircleBySides(int numSides)
    {

        float rad2o2 = Mathf.Sqrt(2.0f) / 2.0f;

        // Calculates positions on unit circle and stores them into vectors.
        // Keep in mind that in 2D, we use cos(x) and sin(y). In 3D here, we use cos(x) and sin(z)!
        Vector3 unit45 = new Vector3(rad2o2, 0, rad2o2);
        Vector3 unit90 = new Vector3(0, 0, 1);
        Vector3 unit135 = new Vector3(-rad2o2, 0, rad2o2);
        Vector3 unit60 = new Vector3(Mathf.Cos(2 * Mathf.PI / 6), 0, Mathf.Sin(2 * Mathf.PI / 6));
        Vector3 unit105 = new Vector3(-Mathf.Cos(2 * Mathf.PI / 6), 0, Mathf.Sin(2 * Mathf.PI / 6));

        // Creates a list to return.
        List<Vector3> returnList = new List<Vector3>();

        int specialCaseCutoff = 5;

        if (numSides < specialCaseCutoff)
        {

            returnList.Add(unit90);

            if (numSides > 1)
            {

                returnList.Add(unit60);

            }

            if (numSides > 2)
            {

                returnList.Add(unit105);

            }

            if (numSides > 3)
            {

                returnList.Add(unit45);

            }
        }

        else
        {
            // Math works. Always believe. Genius Credit: Anish
            for (int i = 0; i < numSides; i++)
            {
                float radAngle = i * (2 * (Mathf.PI / numSides));
                radAngle += (Mathf.PI / 2);
                returnList.Add(new Vector3(Mathf.Cos(radAngle), 0, Mathf.Sin(radAngle)));
            }

        }

        // Returns the list.
        return returnList;
    }


}
