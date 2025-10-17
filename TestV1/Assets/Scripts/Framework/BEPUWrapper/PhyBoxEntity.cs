using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PhyBoxEntity : MonoBehaviour
{

    BEPUphysics.Entities.Prefabs.Box box;

    public bool isStatic = false;

    private float width = 1;
    private float height = 1;
    private float length = 1;

    private float centerX = 0, centerY = 0, centerZ = 0;

    private void Awake()
    {
        BoxCollider boxPhy = this.GetComponent<BoxCollider>();
        this.width = boxPhy.size.x;
        this.height = boxPhy.size.y;
        this.length = boxPhy.size.z;

        this.centerX = boxPhy.center.x;
        this.centerY = boxPhy.center.y;
        this.centerZ = boxPhy.center.z;
    }

    // Start is called before the first frame update
    void Start()
    {
        
        if (isStatic)
        {
            this.box = new BEPUphysics.Entities.Prefabs.Box(new BEPUutilities.Vector3(0, 0, 0),
                System.Convert.ToDecimal(this.width), System.Convert.ToDecimal(this.height),
                System.Convert.ToDecimal(this.length));
        }
        else
        {
            this.box = new BEPUphysics.Entities.Prefabs.Box(new BEPUutilities.Vector3(0, 0, 0),
                System.Convert.ToDecimal(this.width),System.Convert.ToDecimal(this.height),
                System.Convert.ToDecimal(this.length), 1);
        }


        Vector3 pos = this.transform.position;
        this.box.position = new BEPUutilities.Vector3(
            System.Convert.ToDecimal(pos.x + this.centerX),
            System.Convert.ToDecimal(pos.y + this.centerY),
            System.Convert.ToDecimal(pos.z + this.centerZ));

        BEPUPhyMgr.Instance.space.Add(box);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if (this.isStatic)
        {
            return;
        }

        BEPUutilities.Vector3 worldPos = this.box.position;
        // Debug.Log(worldPos.ToString());

        double x = System.Convert.ToDouble((decimal)worldPos.X);
        double y = System.Convert.ToDouble((decimal)worldPos.Y);
        double z = System.Convert.ToDouble((decimal)worldPos.Z);


        this.transform.position = new Vector3((float)x - this.centerX, (float)y - this.centerY, (float)z - this.centerY);
    }
}
