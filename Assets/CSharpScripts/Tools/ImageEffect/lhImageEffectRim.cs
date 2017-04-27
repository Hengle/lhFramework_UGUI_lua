using UnityEngine;
using System.Collections;
using LaoHan.Infrastruture;

namespace LaoHan.Tools
{
    [RequireComponent(typeof(Camera))]
    public class lhImageEffectRim : MonoBehaviour
    {

        public Camera outterLineCamera;
        public int iterations = 3;
        public float spread = 0.7f;
        public float blurOffset = 1;
        public Color outterColor = new Color(0.133f, 1, 0, 1);
        public LayerMask layerMask;

        private Shader m_compositeShader;
        private Shader m_blurShader;
        private Shader m_cutoffShader;

        private Material m_compositeMaterial = null;
        private Material m_shapeblurMaterial = null;
        private Material m_cutoffMaterial = null;
        private Material m_outterLineMat = null;

        private RenderTexture m_outterLineTexture = null;
        void Start()
        {
            if (!SystemInfo.supportsImageEffects)
            {
                enabled = false;
                return;
            }

            if (!outterLineCamera)
            {
                Camera thisCamera = GetComponent<Camera>();
                outterLineCamera = new GameObject("lhImageEffect").AddComponent<Camera>();
                outterLineCamera.fieldOfView = thisCamera.fieldOfView;
                outterLineCamera.clearFlags = CameraClearFlags.SolidColor;
                outterLineCamera.cullingMask = layerMask;
                outterLineCamera.renderingPath = RenderingPath.Forward;
                outterLineCamera.backgroundColor = new Color(0, 0, 0, 0);
                outterLineCamera.transform.parent = transform;
                outterLineCamera.transform.localPosition = Vector3.zero;
                outterLineCamera.transform.localEulerAngles = Vector3.zero;
                outterLineCamera.gameObject.SetActive(false);
            }
        }
        void OnEnable()
        {

            m_blurShader = Shader.Find("lhHidden/OutterLineShapeBlur");
            if (!m_blurShader)
            {
                LaoHan.Infrastruture.lhDebug.LogError((object)"LaoHan:lhHidden/OutterLineShapeBlur shader is nont Exists");
                return;
            }
            m_shapeblurMaterial = new Material(m_blurShader);
            m_shapeblurMaterial.hideFlags = HideFlags.HideAndDontSave;
            m_shapeblurMaterial.SetFloat("_BlurOffsets", blurOffset);
            if (!m_shapeblurMaterial.shader.isSupported)
            {
                LaoHan.Infrastruture.lhDebug.LogError((object)"LaoHan:m_shapeblurMaterial shader is nont Supported");
                enabled = false;
            }

            m_compositeShader = Shader.Find("lhHidden/OutterLineComposite");
            if (!m_compositeShader)
            {
                LaoHan.Infrastruture.lhDebug.LogError((object)"LaoHan:lhHidden/OutterLineComposite shader is nont Exists");
                return;
            }
            m_compositeMaterial = new Material(m_compositeShader);
            m_compositeMaterial.hideFlags = HideFlags.HideAndDontSave;
            if (!m_compositeMaterial.shader.isSupported)
            {
                LaoHan.Infrastruture.lhDebug.LogError((object)"LaoHan:m_compositeMaterial shader is nont Supported");
                enabled = false;
            }

            m_cutoffShader = Shader.Find("lhHidden/OutterLineCutoff");
            if (!m_cutoffShader)
            {
                LaoHan.Infrastruture.lhDebug.LogError((object)"LaoHan:lhHidden/OutterLineCutoff shader is nont Exists");
                return;
            }
            m_cutoffMaterial = new Material(m_cutoffShader);
            m_cutoffMaterial.hideFlags = HideFlags.HideAndDontSave;
            if (!m_cutoffMaterial.shader.isSupported)
            {
                LaoHan.Infrastruture.lhDebug.LogError((object)"LaoHan:m_cutoffMaterial shader is nont Supported");
                enabled = false;
            }

            m_outterLineMat = new Material("Shader\"lhHidden/SolidBody1\"{SubShader{Pass{Color(" +
                outterColor.r + "," + outterColor.g + "," + outterColor.b + "," + outterColor.a + ")}}}");
            m_outterLineMat.hideFlags = HideFlags.HideAndDontSave;
            m_outterLineMat.shader.hideFlags = HideFlags.HideAndDontSave;
            if (!m_outterLineMat.shader.isSupported)
            {
                LaoHan.Infrastruture.lhDebug.LogError((object)"LaoHan:m_outterLineMat shader is nont Supported");
                enabled = false;
            }

            if (!m_outterLineTexture)
            {
                m_outterLineTexture = new RenderTexture((int)GetComponent<Camera>().pixelWidth, (int)GetComponent<Camera>().pixelHeight, 16);
                m_outterLineTexture.hideFlags = HideFlags.DontSave;
            }
        }

        void OnDisable()
        {
            if (m_outterLineTexture)
            {
                DestroyImmediate(m_outterLineTexture);
                m_outterLineTexture = null;
            }
            if (m_compositeMaterial)
            {
                DestroyImmediate(m_compositeMaterial);
            }
            if (m_shapeblurMaterial)
            {
                DestroyImmediate(m_shapeblurMaterial);
            }
            if (m_outterLineMat)
            {
                DestroyImmediate(m_outterLineMat);
            }
        }

        public void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
        {
            float off = 0.5f + iteration * spread;
            Graphics.BlitMultiTap(source, dest, m_shapeblurMaterial,
                new Vector2(off, off),
                new Vector2(-off, off),
                new Vector2(off, -off),
                new Vector2(-off, -off)
            );
        }

        void OnPreRender()
        {
            outterLineCamera.targetTexture = m_outterLineTexture;
            outterLineCamera.RenderWithShader(m_outterLineMat.shader, "");
        }

        void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            iterations = Mathf.Clamp(iterations, 0, 15);
            spread = Mathf.Clamp(spread, 0.5f, 6.0f);

            RenderTexture buffer = RenderTexture.GetTemporary(m_outterLineTexture.width, m_outterLineTexture.height, 0);
            RenderTexture buffer2 = RenderTexture.GetTemporary(m_outterLineTexture.width, m_outterLineTexture.height, 0);

            Graphics.Blit(m_outterLineTexture, buffer);

            bool oddEven = true;
            for (int i = 0; i < iterations; i++)
            {
                if (oddEven)
                    FourTapCone(buffer, buffer2, i);
                else
                    FourTapCone(buffer2, buffer, i);
                oddEven = !oddEven;
            }
            Graphics.Blit(source, destination);
            if (oddEven)
            {
                Graphics.Blit(m_outterLineTexture, buffer, m_cutoffMaterial);
                Graphics.Blit(buffer, destination, m_compositeMaterial);
            }
            else
            {
                Graphics.Blit(m_outterLineTexture, buffer2, m_cutoffMaterial);
                Graphics.Blit(buffer2, destination, m_compositeMaterial);
            }

            RenderTexture.ReleaseTemporary(buffer);
            RenderTexture.ReleaseTemporary(buffer2);
        }
    }
}
