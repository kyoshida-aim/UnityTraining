using UnityEngine;


[RequireComponent(typeof(AudioListener))]
public class GetSpectrumDataExample : MonoBehaviour
{
    void Update()
    {
        float[] spectrum = new float[256];

        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

		Vector3 positionfix = new Vector3(80, 30, 0);
        for (int i = 1; i < spectrum.Length - 1; i++)
        {
            // Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0) + positionfix,
			// 	new Vector3(i, spectrum[i + 1] + 10, 0) + positionfix,
			// 	Color.red);
            Debug.DrawLine(new Vector3(i - 1, (Mathf.Log(spectrum[i - 1]) + 10) * 10, 2) + positionfix,
				new Vector3(i, (Mathf.Log(spectrum[i]) + 10) * 10, 2) + positionfix,
				Color.cyan);
            // Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1) + positionfix,
			// 	new Vector3(Mathf.Log(i), spectrum[i] - 10, 1) + positionfix,
			// 	Color.green);
            // Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3) + positionfix,
			// 	new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3) + positionfix,
			// 	Color.blue);
        }
    }
}