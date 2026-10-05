using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Protobot
{
    public class PlatformEvents : MonoBehaviour
    {
        private const float MacMemoryCleanupIntervalSeconds = 120f;
        private const long MacManagedMemoryGrowthThresholdBytes = 64L * 1024L * 1024L;

        public UnityEvent OnWebStart;
        public UnityEvent OnWindowsStart;
        public UnityEvent OnMacStart;
        public UnityEvent OnLinuxStart;

        private Coroutine macMemoryCleanupRoutine;
        private bool isMemoryCleanupRunning;
        private long lastManagedMemoryBytes;

        private void OnEnable()
        {
            Application.lowMemory += HandleLowMemory;
        }

        private void OnDisable()
        {
            Application.lowMemory -= HandleLowMemory;

            if (macMemoryCleanupRoutine != null)
            {
                StopCoroutine(macMemoryCleanupRoutine);
                macMemoryCleanupRoutine = null;
            }
        }

        private void Awake()
        {
            if (AppPlatform.OnWindows)
            {
                Application.targetFrameRate = 60;
                OnWindowsStart?.Invoke();
            }
            else if (AppPlatform.OnWeb)
            {
                OnWebStart?.Invoke();
            }
            else if (AppPlatform.OnMac)
            {
                Application.targetFrameRate = 60;
                ConfigureMacMemoryMaintenance();
                OnMacStart?.Invoke();
            }
            else if (AppPlatform.OnLinux)
            {
                OnLinuxStart?.Invoke();
            }
        }

        public void SetFullscreen(bool value)
        {
            Screen.fullScreenMode = value ? FullScreenMode.ExclusiveFullScreen : FullScreenMode.Windowed;
        }

        private void ConfigureMacMemoryMaintenance()
        {
            // Keep quality unchanged; only reduce memory pressure from unused assets/managed allocations.
            lastManagedMemoryBytes = GC.GetTotalMemory(false);

            if (macMemoryCleanupRoutine == null)
            {
                macMemoryCleanupRoutine = StartCoroutine(MacMemoryCleanupLoop());
            }
        }

        private IEnumerator MacMemoryCleanupLoop()
        {
            while (AppPlatform.OnMac)
            {
                yield return new WaitForSecondsRealtime(MacMemoryCleanupIntervalSeconds);

                var currentManagedMemory = GC.GetTotalMemory(false);
                var managedGrowth = currentManagedMemory - lastManagedMemoryBytes;

                if (managedGrowth >= MacManagedMemoryGrowthThresholdBytes)
                {
                    yield return CleanupMemory(forceFullGc: false);
                }

                lastManagedMemoryBytes = GC.GetTotalMemory(false);
            }
        }

        private void HandleLowMemory()
        {
            if (!AppPlatform.OnMac || isMemoryCleanupRunning)
            {
                return;
            }

            StartCoroutine(CleanupMemory(forceFullGc: true));
        }

        private IEnumerator CleanupMemory(bool forceFullGc)
        {
            isMemoryCleanupRunning = true;

            yield return Resources.UnloadUnusedAssets();

            if (forceFullGc)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

            isMemoryCleanupRunning = false;
        }
    }
}
