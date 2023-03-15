using UnityEngine;

namespace Map
{
    public static class MapHelper
    {
        public static Vector2 GeoToWorldPosition(Vector2 geoPosition)
        {
            return (geoPosition - MapData.GeoAnchor) / MapData.ConversionConst;
        }
    }
}