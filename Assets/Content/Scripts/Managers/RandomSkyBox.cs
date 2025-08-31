using UnityEngine;

public class RandomSkyBox : MonoBehaviour
{
    [Tooltip("Assign all skybox materials here")]
    public Material[] skyboxes;

    void Start()
    {
        if (skyboxes.Length > 0)
        {
            // Pick a random skybox
            int index = Random.Range(0, skyboxes.Length);
            RenderSettings.skybox = skyboxes[index];

            // Update ambient lighting based on new skybox
            DynamicGI.UpdateEnvironment();
        }
    }

}
