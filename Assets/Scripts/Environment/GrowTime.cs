using System.Collections;
using UnityEngine;

namespace Farming
{
    public class GrowTime : MonoBehaviour
    {
        [Header("Growth Stage Objects")]
        [Tooltip("Visible at 0s (right after planting).")]
        [SerializeField] private GameObject plantedStage;      // 0 sec

        [Tooltip("Visible at 15s.")]
        [SerializeField] private GameObject mediumStage;       // 15 sec

        [Tooltip("Visible at 30s (fully grown).")]
        [SerializeField] private GameObject fullyGrownStage;   // 30 sec

        [Header("Timing")]
        [SerializeField] private float mediumTime = 15f;
        [SerializeField] private float fullTime = 30f;

        private Coroutine growthCoroutine;

        public bool IsGrown { get; private set; } = false;
        public bool IsGrowing { get; private set; } = false;

        private void Awake()
        {
            // Assume the objects are already implement but leave comments on making them visible.
            // TODO: Assign plantedStage / mediumStage / fullyGrownStage in the Inspector.

            // ✅ AUTO-FIND (so prefab instances work without manual wiring)
            // These names match what you showed: SM_Tomato_Lv1, SM_Tomato_Lv2, SM_Tomato_Lv3
            if (plantedStage == null)
            {
                Transform t = FindDeepChild(transform, "SM_Tomato_Lv1");
                if (t != null) plantedStage = t.gameObject;
            }
            if (mediumStage == null)
            {
                Transform t = FindDeepChild(transform, "SM_Tomato_Lv2");
                if (t != null) mediumStage = t.gameObject;
            }
            if (fullyGrownStage == null)
            {
                Transform t = FindDeepChild(transform, "SM_Tomato_Lv3");
                if (t != null) fullyGrownStage = t.gameObject;
            }

            SetStage(0, false);
            SetStage(1, false);
            SetStage(2, false);
        }

        public void StartGrowth()
{
  
    if (!gameObject.activeInHierarchy)
    {
        gameObject.SetActive(true);
    }

    StopGrowth();

    IsGrown = false;
    IsGrowing = true;

    // 0 sec: planted stage
    SetStage(0, true);
    SetStage(1, false);
    SetStage(2, false);

    growthCoroutine = StartCoroutine(GrowRoutine());
}

        public void StopGrowth()
        {
            if (growthCoroutine != null)
            {
                StopCoroutine(growthCoroutine);
                growthCoroutine = null;
            }
            IsGrowing = false;
        }

        private IEnumerator GrowRoutine()
        {
            // 0 -> 15 sec
            yield return new WaitForSeconds(mediumTime);

            // 15 sec: medium stage
            SetStage(0, false);
            SetStage(1, true);
            SetStage(2, false);

            // 15 -> 30 sec (remaining)
            float remaining = Mathf.Max(0f, fullTime - mediumTime);
            yield return new WaitForSeconds(remaining);

            // 30 sec: fully grown
            SetStage(0, false);
            SetStage(1, false);
            SetStage(2, true);

            IsGrown = true;
            IsGrowing = false;
            growthCoroutine = null;
        }

        private void SetStage(int stageIndex, bool active)
        {
            // Assume the objects are already implement but leave comments on making them visible.
            // TODO: Make the correct stage visible by enabling it with SetActive(true).

            if (stageIndex == 0 && plantedStage != null) plantedStage.SetActive(active);
            if (stageIndex == 1 && mediumStage != null) mediumStage.SetActive(active);
            if (stageIndex == 2 && fullyGrownStage != null) fullyGrownStage.SetActive(active);
        }

        // ✅ Helper: find a child by name anywhere under this object (including inactive)
        private Transform FindDeepChild(Transform parent, string name)
        {
            foreach (Transform child in parent)
            {
                if (child.name == name)
                    return child;

                Transform result = FindDeepChild(child, name);
                if (result != null)
                    return result;
            }
            return null;
        }
    }
}