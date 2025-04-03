using UnityEngine;

namespace Core.Extension
{
    public static class CanvasExtension
    {
        public static void SetActive(this CanvasGroup canvasGroup, bool isActive)
        {
            canvasGroup.alpha = isActive ? 1 : 0;
            canvasGroup.interactable = isActive;
            canvasGroup.blocksRaycasts = isActive;
        }
    }
}