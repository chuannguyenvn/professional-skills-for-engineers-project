using UnityEngine;

namespace Map
{
    public static class MapUtilities
    {
        public static readonly float ConversionConst = 0.000008950159495243074f;
        public static readonly Vector2 GeoAnchor = new (10.772802f, 106.659716f);

        public static Vector2 GeoToWorldPosition(Vector2 geoPosition)
        {
            var reversedPos = (geoPosition - GeoAnchor) / ConversionConst;
            return new Vector2(reversedPos.y, reversedPos.x);
        }
        
        public static Vector2 WorldToGeoPosition(Vector2 geoPosition)
        {
            var reversedPos = (geoPosition - GeoAnchor) / ConversionConst;
            return new Vector2(reversedPos.y, reversedPos.x);
        }
    }
}