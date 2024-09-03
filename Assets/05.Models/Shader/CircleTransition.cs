using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Collections.Shaders.CircleTransition
{
    public class CircleTransition : MonoBehaviour
    {
        public Transform player;
        public Image blackScreen;

        private Canvas _canvas;

        private Vector2 _playerCanvasPos;

        private static readonly int RADIUS = Shader.PropertyToID("_Radius");
        private static readonly int CENTER_X = Shader.PropertyToID("_CenterX");
        private static readonly int CENTER_Y = Shader.PropertyToID("_CenterY");

        private void Awake()
        {
            player = GameObject.Find("BS_Transform").transform;
            _canvas = GetComponent<Canvas>();
        }

        private void DrawBlackScreen()
        {
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            var playerScreenPos = Camera.main.WorldToScreenPoint(player.position);

            var canvasRect = _canvas.GetComponent<RectTransform>().rect;
            var canvasWidth = canvasRect.width;
            var canvasHeight = canvasRect.height;

            _playerCanvasPos = new Vector2
            {
                x = (playerScreenPos.x / screenWidth) * canvasWidth,
                y = (playerScreenPos.y / screenHeight) * canvasHeight,
            };

            var squareValue = 0f;
            if (canvasWidth > canvasHeight)
            {
                // Landscape
                squareValue = canvasWidth;
                _playerCanvasPos.y += (canvasWidth - canvasHeight) * 0.5f;
            }
            else
            {
                // Portrait            
                squareValue = canvasHeight;
                _playerCanvasPos.x += (canvasHeight - canvasWidth) * 0.5f;
            }

            _playerCanvasPos /= squareValue;

            var mat = blackScreen.material;
            mat.SetFloat(CENTER_X, _playerCanvasPos.x);
            mat.SetFloat(CENTER_Y, _playerCanvasPos.y);

            blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);
        }

        public void Transition(float duration, float beginRadius, float endRadius)
        {
            DrawBlackScreen();

            var mat = blackScreen.material;
            DOTween.To(() => beginRadius, x => mat.SetFloat(RADIUS, x), endRadius, duration).SetEase(Ease.OutSine);
        }
    }
}