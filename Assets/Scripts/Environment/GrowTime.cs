using System;
using System.Collections;
using UnityEngine;

namespace Farming
{
    public class GrowTime : MonoBehaviour
    {
        [Header("Growth Stage Objects")]
        [SerializeField] private GameObject plantedStage;
        [SerializeField] private GameObject mediumStage;
        [SerializeField] private GameObject fullyGrownStage;
        [SerializeField] private GameObject witheredStage;

        [Header("Timing")]
        [SerializeField] private float mediumTime = 15f;
        [SerializeField] private float fullTime = 30f;
        [SerializeField] private float witherTime = 30f;

        private Coroutine growthCoroutine;

        SeasonManager seasonManager;

        public bool IsGrown { get; private set; } = false;
        public bool IsGrowing { get; private set; } = false;
        public bool IsMedium { get; private set; } = false;

        private void Awake()
        {
            if (plantedStage == null)
                plantedStage = FindDeepChild(transform, "SM_Tomato_Lv1")?.gameObject;

            if (mediumStage == null)
                mediumStage = FindDeepChild(transform, "SM_Tomato_Lv2")?.gameObject;

            if (fullyGrownStage == null)
                fullyGrownStage = FindDeepChild(transform, "SM_Tomato_Lv3")?.gameObject;

            if (witheredStage == null)
                witheredStage = FindDeepChild(transform, "SM_Tomato_Lv4")?.gameObject;

            SetAllOff();

            seasonManager = FindAnyObjectByType<SeasonManager>();
            seasonManager.OnSeasonChanged.AddListener(HandleSeasonChange);
            HandleSeasonChange();
        }

        private void OnDestroy()
        {
            if (seasonManager != null)
                seasonManager.OnSeasonChanged.RemoveListener(HandleSeasonChange);
        }


        private void SetAllOff()
        {
            if (plantedStage) plantedStage.SetActive(false);
            if (mediumStage) mediumStage.SetActive(false);
            if (fullyGrownStage) fullyGrownStage.SetActive(false);
            if (witheredStage) witheredStage.SetActive(false);
        }

        public void StartGrowth()
        {
            StopGrowth();
            SetAllOff();

            IsGrown = false;
            IsGrowing = true;
            IsMedium = false;

            plantedStage.SetActive(true);

            growthCoroutine = StartCoroutine(GrowRoutine());
        }

        private IEnumerator GrowRoutine()
        {
            // Planted → Medium
            yield return new WaitForSeconds(mediumTime);

            SetAllOff();
            mediumStage.SetActive(true);
            IsMedium = true;

            // Medium → Full
            float remaining = Mathf.Max(0f, fullTime - mediumTime);
            yield return new WaitForSeconds(remaining);

            SetAllOff();
            fullyGrownStage.SetActive(true);

            IsGrown = true;
            IsGrowing = false;
            IsMedium = false;

            growthCoroutine = null;

            StartCoroutine(WitherRoutine());
        }

        private IEnumerator WitherRoutine()
        {
            yield return new WaitForSeconds(witherTime);

            if (IsGrown)
                SetWithered();
        }

        public void SetWithered()
        {
            StopGrowth();

            IsGrown = false;
            IsGrowing = false;
            IsMedium = false;

            SetStage(0, false);
            SetStage(1, false);
            SetStage(2, false);

            if (witheredStage != null)
                witheredStage.SetActive(true);

            // IMPORTANT: tell the tile it is withered
            GetComponentInParent<FarmTile>().ForceSetWithered();
        }

        private void SetStage(int stageIndex, bool active)
        {
            if (stageIndex == 0 && plantedStage != null) plantedStage.SetActive(active);
            if (stageIndex == 1 && mediumStage != null) mediumStage.SetActive(active);
            if (stageIndex == 2 && fullyGrownStage != null) fullyGrownStage.SetActive(active);
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

        public void RemovePlant()
        {
            Destroy(gameObject);
        }

        public void LoadState(int stage)
        {
            StopGrowth();
            SetAllOff();

            switch (stage)
            {
                case 0: // planted
                    plantedStage.SetActive(true);
                    IsGrown = false;
                    IsGrowing = true;
                    IsMedium = false;
                    growthCoroutine = StartCoroutine(GrowRoutine());
                    break;

                case 1: // medium
                    mediumStage.SetActive(true);
                    IsGrown = false;
                    IsGrowing = true;
                    IsMedium = true;

                    float remaining = Mathf.Max(0f, fullTime - mediumTime);
                    growthCoroutine = StartCoroutine(FinishFromMedium(remaining));
                    break;

                case 2: // fully grown
                    fullyGrownStage.SetActive(true);
                    IsGrown = true;
                    IsGrowing = false;
                    IsMedium = false;
                    StartCoroutine(WitherRoutine());
                    break;

                case 3: // withered
                    SetWithered();
                    break;
            }
        }

        private IEnumerator FinishFromMedium(float remaining)
        {
            yield return new WaitForSeconds(remaining);

            SetAllOff();
            fullyGrownStage.SetActive(true);

            IsGrown = true;
            IsGrowing = false;
            IsMedium = false;

            StartCoroutine(WitherRoutine());
        }

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

        private void HandleSeasonChange()
        {
            
            var data = seasonManager.GetCurrentSeason();
            UpdateWitherTime(data.witherRate);
        }
        public void UpdateWitherTime(float witherRate)
        {
            Debug.Log("Wither rate updated to * " + seasonManager.GetCurrentSeason().witherRate);
            witherTime *= witherRate;
        }


    }
}