using SharpDX.DirectWrite;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Test1 : MonoBehaviour
{

    /// <summary>
    /// 母球.
    /// </summary>
    public Cueball cueball;

    /// <summary>
    /// 目标测试球.
    /// </summary>
    public Ball destball;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DelayedStrike());
    }

    private System.Collections.IEnumerator DelayedStrike()
    {
        yield return new WaitForSeconds(2f); 
        CueballStrike();                    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CueballStrike()
    {

        Vector3 dir = (this.destball.transform.position - this.cueball.transform.position).normalized;
        Vector3 impulse = dir * 2.0f;

        Debug.DrawLine(this.cueball.transform.position, this.destball.transform.position, Color.red, 5.0f);

        this.cueball.Strike(this.cueball.transform.position, impulse);

    }
}
