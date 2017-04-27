using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using LaoHan.Infrastruture;

namespace LaoHan.UGUI
{
    public class UITool_CameraRender : lhMonoBehaviour
    {

        public RawImage image;

        private RenderTexture m_renderTexture;
        private Camera m_camera;
        private CameraClearFlags m_clearFlags;
        public void SetRenderTexture(Camera cam, CameraClearFlags clearFlags)
        {
            if (m_renderTexture == null)
                m_renderTexture = new RenderTexture(Screen.width, Screen.height, 16);
            cam.targetTexture = m_renderTexture;
            image.texture = m_renderTexture;
            m_camera = cam;
            m_clearFlags = cam.clearFlags;
            m_camera.clearFlags = clearFlags;
        }
        public void CancelRenderTexture()
        {
            if (m_camera == null) return;
            m_camera.targetTexture = null;
            m_camera.clearFlags = m_clearFlags;
        }
        public void CancelRenderTexture(CameraClearFlags clearFlags)
        {
            m_camera.targetTexture = null;
            m_camera.clearFlags = clearFlags;
        }
    }
}