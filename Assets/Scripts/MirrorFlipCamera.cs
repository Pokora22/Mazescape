using UnityEngine;
[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class MirrorFlipCamera : MonoBehaviour {
    new Camera camera;
    public bool flipVertical;
    void Awake () {
        camera = GetComponent<Camera>();
    }
    void OnPreCull() {
        camera.ResetWorldToCameraMatrix();
        camera.ResetProjectionMatrix();
        Vector3 scale = new Vector3(1, 1, flipVertical ? -1 : 1);
        camera.projectionMatrix = camera.projectionMatrix * Matrix4x4.Scale(scale);
    }
    void OnPreRender () {
        GL.invertCulling = flipVertical;
    }
     
    void OnPostRender () {
        GL.invertCulling = false;
    }
}