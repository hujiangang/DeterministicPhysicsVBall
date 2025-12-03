using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Test{

    public class Ball : PhySphereEntity
    {
        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            AddTestMaterial();
        }

        /// <summary>
        /// 获取击打球的位置.
        /// </summary>
        /// <param name="cueHitType"></param>
        /// <returns></returns>
        public Vector3 GetCueHitPos(CueHitType cueHitType)
        {
            return Vector3.zero;
        }

        /// <summary>
        /// 获取击打球的偏移.
        /// </summary>
        /// <param name="cueHitType"></param>
        /// <returns></returns>
        public Vector3 GetCueHitPosOffSet(CueHitType cueHitType)
        {
            Vector3 center = this.transform.position;

            Vector3 offset = Vector3.zero;
            float max = 0.5f;
            float middle = 0.3f;

            switch (cueHitType)
            {
                case CueHitType.Center:
                    break;
                case CueHitType.TopSpin:
                    offset = new Vector3(0, radius * max, 0);
                    break;
                case CueHitType.BackSpin:
                    offset = new Vector3(0, -radius * max, 0);
                    break;
                case CueHitType.LeftSpin:
                    offset = new Vector3(-radius * max, 0, 0);
                    break;
                case CueHitType.RightSpin:
                    offset = new Vector3(radius * max, 0, 0);
                    break;
                case CueHitType.TopLeft:
                    offset = new Vector3(-radius * middle, radius * middle, 0);
                    break;
                case CueHitType.TopRight:
                    offset = new Vector3(radius * middle, radius * middle, 0);
                    break;
                case CueHitType.BottomLeft:
                    offset = new Vector3(-radius * middle, -radius * middle, 0);
                    break;
                case CueHitType.BottomRight:
                    offset = new Vector3(radius * middle, -radius * middle, 0);
                    break;
            }

            return offset;
        }


        /// <summary>
        /// 获取击打球的偏移.
        /// </summary>
        /// <param name="cueHitType"></param>
        /// <returns></returns>
        public Vector2 GetCueHitPosOffSet2(CueHitType cueHitType)
        {
            
            Vector2 offset = Vector2.zero;
            float max = 0.2f;
            float middle = 0.2f;

            switch (cueHitType)
            {
                case CueHitType.Center:
                    break;
                case CueHitType.TopSpin:
                    offset = new Vector2(0, max);
                    break;
                case CueHitType.BackSpin:
                    offset = new Vector2(0, -max);
                    break;
                case CueHitType.LeftSpin:
                    offset = new Vector2(-max, 0);
                    break;
                case CueHitType.RightSpin:
                    offset = new Vector2(max, 0);
                    break;
                case CueHitType.TopLeft:
                    offset = new Vector2(-middle, middle);
                    break;
                case CueHitType.TopRight:
                    offset = new Vector2(middle, middle);
                    break;
                case CueHitType.BottomLeft:
                    offset = new Vector2(-middle, -middle);
                    break;
                case CueHitType.BottomRight:
                    offset = new Vector2(middle, -middle);
                    break;
            }

            return offset;
        }

        /// <summary>
        /// 获取表面最近的点.
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Vector3 GetSurfacePoint(Vector3 pos)
        {
            return col.ClosestPoint(pos);
        }

        public void AddTestMaterial()
        {
            if (this.phyEntity != null)
            {
                int paramIndex = TestParamater.TestIndex;
                PhyMaterialTestParamater phyMaterialTest = TestParamater.BallMatParamaterList[paramIndex].ballMatParamater;

                this.phyEntity.Material = phyMaterialTest.material;
                this.phyEntity.LinearDamping = phyMaterialTest.linearDamping;
                this.phyEntity.AngularDamping = phyMaterialTest.angularDamping;
            }
        }
    }
}
