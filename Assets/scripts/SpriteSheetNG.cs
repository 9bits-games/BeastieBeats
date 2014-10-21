using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Script from http://wiki.unity3d.com/index.php?title=Animating_Tiled_texture
public class SpriteSheetNG : MonoBehaviour9Bits
{   
    public int _uvTieX = 1;
    public int _uvTieY = 1;
    public int _fps = 10;
    public bool autoPlay = true;

    private float iX=0;
    private float iY=1;
    private Vector2 _size;
    private Renderer _myRenderer;
    private int _lastIndex = -1;

    private bool isPlaying;
    private float playTime;

    public void Play() {
        isPlaying = true;
    }

    public void Stop() {
        isPlaying = false;
        playTime = 0;
    }

    public void Pause() {
        isPlaying = false;
    }

    void Start ()
    {
        isPlaying = false;
        playTime = 0f;

        if (autoPlay) Play();

        _size = new Vector2 (1.0f / _uvTieX ,
            1.0f / _uvTieY);

        _myRenderer = renderer;

        if(_myRenderer == null) enabled = false;

        _myRenderer.material.SetTextureScale ("_MainTex", _size);
    }

    void Update()
    {
        if (isPlaying) {
            playTime += Time.deltaTime;
            int index = (int)(playTime * _fps) % (_uvTieX * _uvTieY);

            if (index != _lastIndex) {
                Vector2 offset = new Vector2(iX * _size.x,
                                 1 - (_size.y * iY));
                iX++;
                if (iX / _uvTieX == 1) {
                    if (_uvTieY != 1)
                        iY++;
                    iX = 0;
                    if (iY / _uvTieY == 1) {
                        iY = 1;
                    }
                }

                _myRenderer.material.SetTextureOffset("_MainTex", offset);
                _lastIndex = index;
            }
        }
    }
}