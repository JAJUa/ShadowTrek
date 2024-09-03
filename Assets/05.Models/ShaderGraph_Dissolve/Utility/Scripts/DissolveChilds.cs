using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using VInspector;

namespace DissolveExample
{
    public class DissolveChilds : MonoBehaviour
    {
        // Start is called before the first frame update
        List<Material> materials = new List<Material>();
        bool PingPong = false;
        bool is1;
        float a;
        void Start()
        {
            var renders = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                materials.AddRange(renders[i].materials);
            }
        }

        private void Reset()
        {
            Start();
            SetValue(0);
        }

        // Update is called once per frame
        void Update()
        {

            //var value = Mathf.PingPong(Time.time * 0.5f, 1f);
            //SetValue(value);

            if (is1)
            {
                
                if (a < 1)
                {
                    a += Time.deltaTime * 1.5f;
                    SetValue(a);
                }
                else
                {
                    is1= false;
                    a = 0;

                }
            }
           
        }

        // IEnumerator enumerator()
        //  {

        //    //float value =         while (true)
        //    //{
        //    //    Mathf.PingPong(value, 1f);
        //    //    value += Time.deltaTime;
        //    //    SetValue(value);
        //    //    yield return new WaitForEndOfFrame();
        //    //}
        //}

        [Button]
        public void DIssolvessad(bool is1)
        {
            if (!is1)
            {
                SetValue(0);
                return;
            }

            this.is1 = is1;
          
        }

        public void SetValue(float value)
        {

            for (int i = 0; i < materials.Count; i++)
            {
                Debug.Log("sadaw");
                materials[i].SetFloat("_Dissolve", value);
            }
        }
    }
}