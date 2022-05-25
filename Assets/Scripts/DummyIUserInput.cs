using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 敵人輸入信號
/// </summary>
public class DummyIUserInput : IUserInput
{
    // Start is called before the first frame update
    IEnumerator Start()//start使用携程
    {
        while (true)
        {
            // Dup = 1.0f;
            // Dright = 0f;
            // Jright = 1f;
            // Jup = 0;
            // yield return new WaitForSeconds(3.0f);//走三秒
            // Dup = 0f;
            // Dright = 0f;
            // Jright = 0f;
            // Jup = 0;
            // yield return new WaitForSeconds(1.0f);//停一秒 
            rb = true;
            yield return 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateDmagDvec(Dup, Dright);
    }
}
