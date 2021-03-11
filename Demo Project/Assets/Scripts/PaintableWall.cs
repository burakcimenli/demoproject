using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaintableWall : MonoBehaviour {

    public Camera orthoCam;
    public GameObject defaultWall;
    public GameObject brush;
    public Text paintPercentageText;

    private Color wallColor;
    private int totalPixelCount;

    public void Init() {
        // Get total pixel count
        Texture2D rt = GetTextureFromRT();
        totalPixelCount = rt.width * rt.height;

        StartCoroutine(ResetWallPaint(true));
    }

    public void UpdatePercentageText() {
        Texture2D texture = GetTextureFromRT();
        int coloredPixelCount = 0;
        for (int x = 0; x < texture.width; x++) {
            for (int y = 0; y < texture.height; y++) {
                if (texture.GetPixel(x, y) != wallColor)
                    coloredPixelCount++;
            }
        }

        float percentage = Mathf.InverseLerp(0, totalPixelCount, coloredPixelCount);
        paintPercentageText.text = "Painted : %" + Mathf.FloorToInt(percentage * 100);
    }

    public void ActivatePercentageText(bool activate) {
        paintPercentageText.gameObject.SetActive(activate);
    }

    private void InitAfterPaintReset() {        
        brush.SetActive(false);
        wallColor = GetWallColor();
    }


    Color GetWallColor() {
        return GetTextureFromRT().GetPixel(0, 0);
    }

    // Get texture of render texture
    Texture2D GetTextureFromRT() {
        RenderTexture renderTexture = orthoCam.targetTexture;
        RenderTexture.active = renderTexture;
        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height);
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        return texture;
    }

    // Revert wall paint to default color
    private IEnumerator ResetWallPaint(bool initAfter) {
        defaultWall.SetActive(true);
        yield return new WaitForSeconds(.1f);
        defaultWall.SetActive(false);

        if(initAfter)
            InitAfterPaintReset();
    }
}
