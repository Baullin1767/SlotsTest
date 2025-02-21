using System.Collections;
using System.Collections.Generic;
using AxGrid;
using AxGrid.Base;
using UnityEngine;
using UnityEngine.UI;

namespace LootboxFSM
{
    public class SlotScroller : MonoBehaviourExt
    {
        public List<Image> slotImages;
        public float spinSpeed = 500f;
        public float deceleration = 100f;
        public float alignDuration = 0.5f;
        public int slotIndex;

        public Transform centerPoint;

        private float currentSpeed;
        private bool isSpinning = false;
        private bool isAligning = false;

        [OnStart]
        private void Init()
        {
            Settings.Model.Set($"SlotScroller{slotIndex}", this);
            currentSpeed = 0;
        }

        [OnUpdate]
        private void UpdateScroll()
        {
            if (isSpinning)
            {
                foreach (var img in slotImages)
                    img.transform.localPosition -= new Vector3(0, currentSpeed * Time.deltaTime, 0);

                RecycleImages();
            }
        }

        public void StartSpinning()
        {
            isSpinning = true;
            currentSpeed = spinSpeed + slotIndex * 50;
        }

        public void StopSpinning()
        {
            StartCoroutine(SmoothStop());
        }

        private IEnumerator SmoothStop()
        {
            while (currentSpeed > 0)
            {
                currentSpeed -= deceleration * Time.deltaTime;
                yield return null;
            }

            isSpinning = false;
            StartCoroutine(AlignToCenterSmooth());
        }

        private IEnumerator AlignToCenterSmooth()
        {
            if (centerPoint == null)
            {
                yield break;
            }

            isAligning = true;
            float elapsedTime = 0f;

            Image closestImage = null;
            float closestDistance = float.MaxValue;

            foreach (var img in slotImages)
            {
                float distance = Mathf.Abs(img.transform.position.y - centerPoint.position.y);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestImage = img;
                }
            }

            if (closestImage != null)
            {
                float offsetY = centerPoint.position.y - closestImage.transform.position.y;

                List<Vector3> startPositions = new List<Vector3>();
                List<Vector3> targetPositions = new List<Vector3>();

                foreach (var img in slotImages)
                {
                    startPositions.Add(img.transform.position);
                    targetPositions.Add(new Vector3(img.transform.position.x, img.transform.position.y + offsetY, img.transform.position.z));
                }

                while (elapsedTime < alignDuration)
                {
                    float t = elapsedTime / alignDuration;
                    for (int i = 0; i < slotImages.Count; i++)
                    {
                        slotImages[i].transform.position = Vector3.Lerp(startPositions[i], targetPositions[i], t);
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }

                for (int i = 0; i < slotImages.Count; i++)
                {
                    slotImages[i].transform.position = targetPositions[i];
                }

                Settings.Model.Set($"SlotResult{slotIndex}", closestImage.sprite.name);
            }

            isAligning = false;

            if (slotIndex == 2)
            {
                Settings.Model.EventManager.Invoke("CheckWinCondition");
            }
        }


        private void RecycleImages()
        {
            for (int i = 0; i < slotImages.Count; i++)
            {
                if (slotImages[i].transform.localPosition.y < -200)
                {
                    Vector3 highestPosition = slotImages[0].transform.localPosition;
                    foreach (var img in slotImages)
                    {
                        if (img.transform.localPosition.y > highestPosition.y)
                            highestPosition = img.transform.localPosition;
                    }

                    slotImages[i].transform.localPosition = new Vector3(0, highestPosition.y + 200, 0);
                }
            }
        }
    }
}
