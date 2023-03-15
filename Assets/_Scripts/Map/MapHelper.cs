using UnityEngine;

namespace Map
{
    public static class MapHelper
    {
        public static Vector2 GeoToWorldPosition(Vector2 geoPosition)
        {
            var reversedPos = (geoPosition - MapData.GeoAnchor) / MapData.ConversionConst;
            return new Vector2(reversedPos.y, reversedPos.x);
        }
    }
}