// FluidTouch.cs
// Copyright (c) 2011-2017 Thinksquirrel Inc.

using Thinksquirrel.Fluvio.Internal;
using Thinksquirrel.Fluvio.Plugins;
using UnityEngine;

namespace Thinksquirrel.Fluvio.SamplePlugins
{
    /// <summary>
    ///     This is a sample plugin that provides mouse/touch control for manipulating fluids.
    /// </summary>
    [AddComponentMenu("Fluvio/Example Project/Sample Plugins/Fluid Touch")]
#if !UNITY_5_0
    [HelpURL("https://docs.getfluv.io/" + VersionInfo.version + "/reference/fluidtouch.html?utm_source=fluvio_inspector_helpbutton&utm_campaign=fluvio_inspector_links&utm_medium=unity_editor")]
#endif
    public class FluidTouch : FluidParticlePlugin
    {
        /// <summary>
        ///     Controls the amount of acceleration to use when pulling/pushing particles (ignores particle mass).
        /// </summary>
        /// <remarks>
        ///     This field is a min-max curve, which allows customization of the force over radius.
        /// </remarks>
        public FluvioMinMaxCurve acceleration;
        /// <summary>
        ///     Controls the maximum radius of the touch force from the mouse/touch position, in world space.
        /// </summary>
        public float radius;
        /// <summary>
        ///     If true, always pull or push particles when not touching the screen, if the platform supports reading the mouse position.
        /// </summary>
        public bool alwaysOn;
        /// <summary>
        ///     The mouse button to use, when not using touch input.
        /// </summary>
        public int mouseButton;
        /// <summary>
        ///     If true, use multi touch on platforms that support it.
        /// </summary>
        public bool useMultiTouch;

        readonly Vector4[] m_TouchPoints = new Vector4[20];
        int m_TouchPointsCount;

        protected override void OnResetPlugin()
        {
            acceleration = new FluvioMinMaxCurve { scalar = 1000.0f, minConstant = 0.0f, maxConstant = 1000.0f };
            SetCurveAsSigned(acceleration, true);
            radius = 10.0f;
            alwaysOn = false;
            mouseButton = 0;
            useMultiTouch = true;
        }
        protected override bool OnStartPluginFrame(ref FluvioTimeStep timeStep)
        {
            // Clear all touch points
            m_TouchPointsCount = 0;

            // This plugin only runs in Play mode
            if (!Application.isPlaying)
                return false;

            // Get camera and near clip
            var cam = Camera.main;

            if (!cam)
                return false;

            // Get plane passing through fluid position facing the camera
            var plane = new Plane(-cam.transform.forward, fluid.transform.position);

            if (Input.touchCount == 0 || !useMultiTouch)
            {
                // Check for mouse button
                var mouseIsPressed = Input.GetMouseButton(mouseButton);

                if (alwaysOn || mouseIsPressed)
                {
                    var touchPosition = Input.mousePosition;
                    AddTouchPoint(touchPosition, cam, plane);
                }
            }
            else
            {
                // Check all touches
                for (int i = 0, l = Input.touchCount; i < l; ++i)
                {
                    var touch = Input.GetTouch(i);
                    AddTouchPoint(touch.position, cam, plane);
                }
            }

            // Make sure we have an acceleration curve, and that it is signed (-1 to 1)
            if (acceleration == null)
                acceleration = new FluvioMinMaxCurve { scalar = 1000.0f, minConstant = 0.0f, maxConstant = 1000.0f };

            SetCurveAsSigned(acceleration, true);

            // Return false if we didn't add any touch points
            return m_TouchPointsCount > 0;
        }
        void AddTouchPoint(Vector3 touchPosition, Camera cam, Plane plane)
        {
            if (m_TouchPointsCount == m_TouchPoints.Length)
                return;

            // Get orthographic world point
            var ortho = cam.orthographic;
            cam.orthographic = true;
            var worldPoint = cam.ScreenToWorldPoint(new Vector3(touchPosition.x, touchPosition.y, cam.nearClipPlane));
            cam.orthographic = ortho;

            // Create ray
            var ray = new Ray(worldPoint, -plane.normal);
            m_TouchPoints[m_TouchPointsCount] = ray.GetPoint(plane.GetDistanceToPoint(ray.origin));

            ++m_TouchPointsCount;
        }
        protected override void OnEnablePlugin()
        {
            SetComputeShader(FluvioComputeShader.Find("ComputeShaders/SamplePlugins/FluidTouch"), "OnUpdatePlugin");
        }
        protected override void OnSetComputeShaderVariables()
        {
            SetComputePluginMinMaxCurve(0, acceleration);
            m_TouchPoints[0].w = radius;
            m_TouchPoints[1].w = m_TouchPointsCount;
            SetComputePluginBuffer(1, m_TouchPoints);
        }
        protected override void OnUpdatePlugin(SolverData solverData, int particleIndex)
        {
            var seed = solverData.GetRandomSeed(particleIndex);

            for (int i = 0, l = m_TouchPointsCount; i < l; ++i)
            {
                var pt = m_TouchPoints[i];
                pt.w = 0;
                var worldPosition = (Vector4)solverData.GetPosition(particleIndex);
                worldPosition.w = 0;
                var d = pt - worldPosition;
                var len = d.magnitude;
                if (len < radius)
                    solverData.AddForce(particleIndex, (d / len) * acceleration.Evaluate(seed, len / radius), ForceMode.Acceleration);
            }
        }
    }
}
