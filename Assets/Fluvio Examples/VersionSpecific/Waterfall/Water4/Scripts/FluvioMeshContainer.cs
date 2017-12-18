using UnityEngine;

public class FluvioMeshContainer 
{
  public Mesh mesh;
  public Vector3[] vertices;
  public Vector3[] normals;
  
  public FluvioMeshContainer(Mesh m) {
    mesh = m;
    vertices = m.vertices;
    normals = m.normals;
  }
  
  public void Update() {
    mesh.vertices = vertices;
    mesh.normals = normals;
  }
}