using GameManagerNamespace;
using System;
using Unity.Services.Core;
using Unity.Services.Mediation;
using UnityEngine;
namespace Unity.Ad.Interst�t�al
{
    public class Interst�t�alAd
    {
        public IInterstitialAd ad;
        string adUnitId = "Interstitial_Android";
        string gameId = "4760609";

        public async void InitServices()
        {
            try
            {
                InitializationOptions initializationOptions = new InitializationOptions();
                initializationOptions.SetGameId(gameId);
                await UnityServices.InitializeAsync(initializationOptions);

                InitializationComplete();
            }
            catch (Exception e)
            {
                InitializationFailed(e);
            }
        }
        
        public void SetupAd()
        {
            //Create
            ad = MediationService.Instance.CreateInterstitialAd(adUnitId);

            //Subscribe to events
            ad.OnLoaded += AdLoaded;
            ad.OnFailedLoad += AdFailedLoad;

            ad.OnShowed += AdShown;
            ad.OnFailedShow += AdFailedShow;
            ad.OnClosed += AdClosed;
            ad.OnClicked += AdClicked;

            // Impression Event
            MediationService.Instance.ImpressionEventPublisher.OnImpression += ImpressionEvent;
        }

        public void ShowAd()
        {
            if (ad.AdState == AdState.Loaded)
            {
                ad.Show();
            }
        }

        void InitializationComplete()
        {
            SetupAd();
            ad.Load();
        }

        void InitializationFailed(Exception e)
        {
            Debug.Log("Initialization Failed: " + e.Message);
        }

        public void AdLoaded(object sender, EventArgs args)
        {
            Debug.Log("Ad loaded");
        }

        void AdFailedLoad(object sender, LoadErrorEventArgs args)
        {
            Debug.Log("Failed to load ad");
            Debug.Log(args.Message);
        }

        void AdShown(object sender, EventArgs args)
        {
            Debug.Log("Ad shown!");
        }

        void AdClosed(object sender, EventArgs e)
        {
            // Pre-load the next ad
            Debug.Log("Ad has closed");
            ad.Load();

            GameManager.Instance.gameState = GameManager.GameState.Started;
        }

        void AdClicked(object sender, EventArgs e)
        {
            Debug.Log("Ad has been clicked");
            // Execute logic after an ad has been clicked.
        }

        void AdFailedShow(object sender, ShowErrorEventArgs args)
        {
            Debug.Log(args.Message);
        }

        void ImpressionEvent(object sender, ImpressionEventArgs args)
        {
            var impressionData = args.ImpressionData != null ? JsonUtility.ToJson(args.ImpressionData, true) : "null";
            Debug.Log("Impression event from ad unit id " + args.AdUnitId + " " + impressionData);
        }
    }
}