using UnityEngine;


namespace Components
{
    public class SetTargetFramerate : MonoBehaviour
    {
        public int Value = 30;

        void Start()
        {
            Application.targetFrameRate = Value;
        }
    }
}