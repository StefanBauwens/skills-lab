// FluidMixer.cs
// Copyright (c) 2011-2017 Thinksquirrel Inc.
#pragma warning disable 169
#if !UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2
#define UNITY_5_3_PLUS
#endif
using System;
using System.Runtime.InteropServices;
using Thinksquirrel.Fluvio.Internal;
using Thinksquirrel.Fluvio.Plugins;
using UnityEngine;

namespace Thinksquirrel.Fluvio.SamplePlugins
{
    /// <summary>
    ///     This is an advanced sample plugin that provides simple, non-physical mixing for different fluids.
    /// </summary>
    /// <remarks>
    ///     Three mixing configurations are supported:
    /// <list type="bullet">
    /// <item><description>Fluid A + Fluid B -> Fluid C + Fluid D</description></item>
    /// <item><description>Fluid A + Fluid B -> Fluid A + Fluid D (if Fluid C is unassigned)</description></item>
    /// <item><description>Fluid A + Fluid B -> Fluid C + Fluid C (if Fluid D is unassigned)</description></item>
    /// </list>
    /// </remarks>
    [AddComponentMenu("Fluvio/Example Project/Sample Plugins/Fluid Mixer")]
#if !UNITY_5_0
    [HelpURL("https://docs.getfluv.io/" + VersionInfo.version + "/reference/fluidmixer.html?utm_source=fluvio_inspector_helpbutton&utm_campaign=fluvio_inspector_links&utm_medium=unity_editor")]
#endif
    public class FluidMixer : FluidParticlePairPlugin
    {
        /// <summary>
        ///     The second fluid to mix with. This must be in the same fluid group as the first fluid.
        /// </summary>
        public FluidBase fluidB;
        /// <summary>
        ///     A particle system belonging to the optional mixed fluid to output, at Fluid A's position (and also Fluid B's position, if Fluid D is not assigned). If unassigned, Fluid A will not be destroyed.
        /// </summary>
        /// <remarks>
        ///     This particle system does not have to be a fluid.
        /// </remarks>
        public ParticleSystem fluidC;
        /// <summary>
        ///     A particle system belonging to the optional second mixed fluid to output, at Fluid B's position.
        /// </summary>
        /// <remarks>
        ///     This particle system does not have to be a fluid.
        /// </remarks>
        public ParticleSystem fluidD;
        /// <summary>
        ///     The multiplier applied to the smoothing distance that determines if two neighboring particles are close enough to mix.
        /// </summary>
        public float distanceMultiplier;

        [StructLayout(LayoutKind.Sequential)]
        struct FluidMixerData
        {
            public Vector4 emitPosition; // xyz - position, w[0] - mixing distance squared
            public Vector4 emitVelocity; // xyz - velocity, w - particle system
            public int fluidB; // only index 0
            public int fluidC; // only index 0
            public int fluidD; // only index 0
            public int mixingFluidsAreTheSame; // only index 0
        }
        int m_Count;
        int m_OldCount;
        int m_FluidAID;
        int m_FluidBID;
        int m_FluidCID;
        int m_FluidDID;
        FluidMixerData[] m_MixerData;
        bool m_FirstFrame;
        FluvioTimeStep m_TimeStep;
        Vector3 m_Gravity;
        float m_SimulationScale;
        bool m_MixingFluidsAreTheSame;
        float m_MixingDistanceSq;

#if UNITY_5_5
        private bool _disableInEditMode;
        private static bool _showedWarning;
#endif

        protected override void OnResetPlugin()
        {
            fluidB = null;
            fluidC = null;
            fluidD = null;
            distanceMultiplier = 0.5f;
        }
        protected override void OnEnablePlugin()
        {
            m_FirstFrame = true;

            SetComputeShader(FluvioComputeShader.Find("ComputeShaders/SamplePlugins/FluidMixer"), "OnUpdatePlugin");

#if UNITY_5_5
            // BUG: Unity 5.5.0 and 5.5.1 may crash in edit mode when emitting particles
            var unityVersion = Application.unityVersion;
            _disableInEditMode = unityVersion.StartsWith("5.5.0") || unityVersion.StartsWith("5.5.1");
            if (_disableInEditMode && !Application.isPlaying && !_showedWarning)
            {
                FluvioDebug.LogWarning(string.Format("Due to Unity issue #862897, the {0} fluid plugin cannot run in edit mode.", GetType()));
                _showedWarning = true;
            }
#endif
        }
        protected override void OnSetComputeShaderVariables()
        {
            m_MixerData[0].emitPosition.w = m_MixingDistanceSq;
            m_MixerData[0].fluidB = m_FluidBID;
            m_MixerData[0].fluidC = m_FluidCID;
            m_MixerData[0].fluidD = m_FluidDID;
            m_MixerData[0].mixingFluidsAreTheSame = m_MixingFluidsAreTheSame ? 1 : 0;
            SetComputePluginBuffer(0, m_MixerData, true);
        }
        protected override bool OnStartPluginFrame(ref FluvioTimeStep timeStep)
        {
#if UNITY_5_5
            if (_disableInEditMode && !Application.isPlaying) return false;
#endif

            // Always process the full fluid group - this plugin requires sub-fluids to work properly.
            includeFluidGroup = true;

            // Skip the first frame
            if (m_FirstFrame)
            {
                m_FirstFrame = false;
                return false;
            }

            if (!fluidB || !fluidB.enabled)
                return false;

            // Create arrays
            var oldCount = m_Count;
            m_Count = fluid.GetTotalParticleCount();
            Array.Resize(ref m_MixerData, m_Count);

            for (var i = oldCount; i < m_Count; ++i)
            {
                m_MixerData[i].emitVelocity.w = 0;
            }

            // Get timestep
            m_TimeStep = timeStep;
            // Get gravity
            m_Gravity = fluid.gravity;
            // Get simulation scale
            m_SimulationScale = fluid.simulationScale;
            // Set flag indicating whether or not the mixing fluids are the same
            m_MixingFluidsAreTheSame = fluid == fluidB;
            // Get squared mixing distance
            m_MixingDistanceSq = fluid.smoothingDistance * fluid.smoothingDistance * distanceMultiplier * distanceMultiplier;
            // Get fluid IDs
            m_FluidAID = fluid.GetFluidID();
            m_FluidBID = fluidB.GetFluidID();
            m_FluidCID = fluidC ? 1 : 0;
            m_FluidDID = fluidD ? 2 : 0;

            // Fluid B and (C or D) at least must be enabled
            return fluidC || fluidD;
        }
        protected override void OnUpdatePlugin(SolverData solverData, int particleIndex, int neighborIndex)
        {
            // Get fluid IDs
            var particleFluid = solverData.GetFluid(particleIndex).GetFluidID();
            var neighborFluid = solverData.GetFluid(neighborIndex).GetFluidID();

            var particleIsA = particleFluid == m_FluidAID;

            if ((m_MixingFluidsAreTheSame || particleFluid != neighborFluid) && // Fluids are not the same, unless A and B are the same
                (particleIsA || particleFluid == m_FluidBID) && // First fluid is A or B
                (neighborFluid == m_FluidAID || neighborFluid == m_FluidBID)) // Second fluid is A or B
            {
                var position = solverData.GetPosition(particleIndex);
                var neighborPosition = solverData.GetPosition(neighborIndex);

                // Make sure the particles are actually close
                if ((position - neighborPosition).sqrMagnitude > m_MixingDistanceSq)
                    return;

                var velocity = solverData.GetVelocity(particleIndex);
                var invMass = 1.0f / solverData.GetMass(particleIndex);

                var emitC = false;

                // Despawn the current particle, unless Fluid C is null and the particle is from Fluid A
                if (m_FluidCID != 0 || !particleIsA)
                {
                    emitC = true;
                    solverData.SetLifetime(particleIndex, 0.0f);
                }

                // Emit a particle. We don't emit from the neighbor position, that gets handled in the opposite pair
                // We handle actual emission on the main thread.
                if (m_FluidDID == 0 || particleIsA)
                {
                    if (emitC)
                    {
                        // Set the system to Fluid C
                        m_MixerData[particleIndex].emitVelocity.w = 1;
                    }
                    else
                    {
                        m_MixerData[particleIndex].emitVelocity.w = 0;
                    }
                }
                else
                {
                    // Set the system to Fluid D
                    m_MixerData[particleIndex].emitVelocity.w = 2;
                }

                // Set emit position and velocity. These must be manually integrated since they won't be applied until after the solver has run.
                // The below is a standard Euler explicit integrator.
                var acceleration = m_Gravity + solverData.GetForce(particleIndex) * invMass;

                for (var iter = 0; iter < m_TimeStep.solverIterations; ++iter)
                {
                    var t = (m_TimeStep.dtIter) * acceleration;

                    // Ignore very large velocity changes
                    if (t.IsNaN() || t.IsInf() || Vector3.Dot(t, t) > (FluvioSettings.kMaxSqrVelocityChange * m_SimulationScale))
                    {
                        t = Vector3.zero;
                    }

                    velocity += t;
                }

                var distSq = m_MixerData[particleIndex].emitPosition.w;
                var id = m_MixerData[particleIndex].emitVelocity.w;

                m_MixerData[particleIndex].emitVelocity = velocity;
                m_MixerData[particleIndex].emitPosition = position + velocity * m_TimeStep.deltaTime;

                m_MixerData[particleIndex].emitPosition.w = distSq;
                m_MixerData[particleIndex].emitVelocity.w = id;
            }
        }
        protected override void OnReadComputeBuffers()
        {
            GetComputePluginBuffer(0, m_MixerData);
        }
        protected override void OnPluginPostSolve()
        {
            // We do actual emission after the solver has run
            for (var particleIndex = 0; particleIndex < m_Count; ++particleIndex)
            {
                var id = m_MixerData[particleIndex].emitVelocity.w;
                if (Mathf.Abs(id) < FluvioSettings.kEpsilon) continue;

                var system = Mathf.Abs(id - 1.0f) < FluvioSettings.kEpsilon ? fluidC : fluidD;

                m_MixerData[particleIndex].emitVelocity.w = 0;

                // Emit from particle systems
                var toSystemSpace = Matrix4x4.identity;

                var localTransform = system.transform;
#if UNITY_5_5_OR_NEWER
                var simulationSpace = system.main.simulationSpace;
                var scalingMode = system.main.scalingMode;
                if (system.main.simulationSpace == ParticleSystemSimulationSpace.Custom)
                {
                    var customSpace = system.main.customSimulationSpace;
                    if (customSpace) localTransform = customSpace;
                }
#else
                var simulationSpace = system.simulationSpace;
#endif

                if (simulationSpace != ParticleSystemSimulationSpace.World)
                {
#if UNITY_5_3_PLUS
#if UNITY_5_5_OR_NEWER
                    switch (system.main.scalingMode)
#else
                    switch (system.scalingMode)
#endif
                    {
                        case ParticleSystemScalingMode.Hierarchy:
                            toSystemSpace = localTransform.localToWorldMatrix;
                            break;
                        case ParticleSystemScalingMode.Local:
                            toSystemSpace = Matrix4x4.TRS(localTransform.position, localTransform.rotation, localTransform.localScale);
                            break;
                        case ParticleSystemScalingMode.Shape:
#endif
                            toSystemSpace = Matrix4x4.TRS(localTransform.position, localTransform.rotation, Vector3.one);
#if UNITY_5_3_PLUS
                            break;
                    }
#endif
                }

#if UNITY_5_3_PLUS
                var emitParams = new ParticleSystem.EmitParams
                {
                    position = toSystemSpace.MultiplyPoint3x4(m_MixerData[particleIndex].emitPosition),
                    velocity = toSystemSpace.MultiplyVector(m_MixerData[particleIndex].emitVelocity)
                };
                system.Emit(emitParams, 1);
#else
                system.Emit(
                    toSystemSpace.MultiplyPoint3x4(m_MixerData[particleIndex].emitPosition),
                    toSystemSpace.MultiplyVector(m_MixerData[particleIndex].emitVelocity),
                    system.startSize,
                    system.startLifetime,
                    system.startColor);
#endif
            }
        }
    }
}
