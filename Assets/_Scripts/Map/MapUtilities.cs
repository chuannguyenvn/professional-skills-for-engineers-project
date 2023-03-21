using UnityEngine;

namespace Map
{
    public static class MapUtilities
    {
        private static readonly float ConversionConst = 0.000008950159495243074f;
        private static readonly Vector2 GeoAnchor = new (10.772802f, 106.659716f);
        private static readonly Vector2 WorldAnchor = new (0, 0);

        public static Vector2 GeoToWorldPosition(Vector2 geoPosition)
        {
            var reversedPos = (geoPosition - GeoAnchor) / ConversionConst;
            return new Vector2(reversedPos.y, reversedPos.x);
        }
        
        public static Vector2 WorldToGeoPosition(Vector2 worldPosition)
        {
            var reversedPos = (worldPosition - WorldAnchor) * ConversionConst;
            return new Vector2(reversedPos.y, reversedPos.x);
        }
    }
}