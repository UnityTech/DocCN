using UnityEngine;

namespace DocCN
{
    public class ScrollListener : MonoBehaviour
    {
        private void Update()
        {
            if (!Input.anyKeyDown) return;
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Debug.Log("Down");
            }
        }
    }
}