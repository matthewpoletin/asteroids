using UnityEngine;

namespace Asteroids.Field
{
    // TODO: Rewrite with mathematics (look into FieldView.cs)
    public class BoundsController : MonoBehaviour
    {
        private enum InversionType
        {
            AxisY,
            AxisX,
        }

        [SerializeField] private InversionType _inversionType = default;

        private void OnTriggerExit2D(Collider2D other)
        {
            switch (_inversionType)
            {
                case InversionType.AxisY:
                    InvertPositionY(other);
                    break;
                case InversionType.AxisX:
                    InvertPositionX(other);
                    break;
            }
        }

        private static void InvertPositionX(Collider2D other)
        {
            var transformPosition = other.gameObject.transform.position;
            transformPosition.x = -transformPosition.x;
            other.gameObject.transform.position = transformPosition;
        }

        private static void InvertPositionY(Collider2D other)
        {
            var transformPosition = other.gameObject.transform.position;
            transformPosition.y = -transformPosition.y;
            other.gameObject.transform.position = transformPosition;
        }
    }
}
