using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace gam
{
    public class handController
    {
        Leap.Hand lefthand;
        Leap.Hand righthand;
        Leap.Controller controller;
        GameObject grabbed;
        float objtime = 0;
        Vector3 leaporigin;
        GameObject leapspace;
        Camera camera;
        RaycastHit info;
        Vector3 prepos = Vector3.zero;
        Vector3 predir = Vector3.zero;
        gameController gamer;
        LineRenderer debug1;
        LineRenderer debug2;
        float correctpos = 0.9f;

        public handController(gameController game)
        {
            leapspace = GameObject.Find("LeapSpace");
            controller = new Leap.Controller();
            camera = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
            gamer = game;
            debug1 = DrawLine(Vector3.zero, Vector3.zero, Color.white, 0.05f);
            debug2 = DrawLine(Vector3.zero, Vector3.zero, Color.white, 0.05f);
        }

        public void connectToHands()
        {
            if (controller.Frame().Hands.Count == 1)
            {
                Leap.Hand temp = controller.Frame().Hands[0];
                if (temp.IsLeft)
                {
                    lefthand = temp;
                    righthand = null;
                }
                else
                {
                    righthand = temp;
                    lefthand = null;
                }
            }
            else if (controller.Frame().Hands.Count == 2)
            {
                Leap.Hand temp = controller.Frame().Hands[0];
                if (temp.IsLeft)
                {
                    lefthand = temp;
                    righthand = controller.Frame().Hands[1];
                }
                else
                {
                    righthand = temp;
                    lefthand = controller.Frame().Hands[1];
                }

            }
            else
            {
                lefthand = null;
                righthand = null;
            }
        }



        public void moveobj()
        {
            objtime += Time.deltaTime;
            leaporigin = leapspace.transform.position;
            Matrix4x4 wtc = camera.worldToCameraMatrix;
            Matrix4x4 ctw = camera.cameraToWorldMatrix;
            Vector3 leftoffset = vec4To3(ctw * vec3To4(Vector3.left * 0.1f, 0));
            Vector3 rightoffset = vec4To3(ctw * vec3To4(Vector3.right * 0.1f, 0));
            Vector3 upoffset = vec4To3(ctw * vec3To4(Vector3.up * 0.1f, 0));
            Vector3 downoffset = vec4To3(ctw * vec3To4(Vector3.down * 0.1f, 0));
            if (objtime > 1)
            {
                if (righthand != null)
                {
                    Vector3 normal = leapToUnity(righthand.PalmNormal) * 1000;
                    normal = vec4To3(ctw * vec3To4(normal, 0));
                    Vector3 dir = leapToWorld(righthand.PalmPosition) - leaporigin;
                    if (!isFist(righthand))
                    {//translate
                        if (decideDirection(normal) != Vector3.zero)
                        {//valid pos
                            if (detectColli(leftoffset,rightoffset,upoffset,downoffset,dir))
                            {// has target
                                GameObject target = info.collider.gameObject;
                                if (prepos == Vector3.zero)
                                {//first hit
                                    prepos = leapToWorld(righthand.PalmPosition);
                                }
                                else
                                {
                                    if ((prepos - leapToWorld(righthand.PalmPosition)).magnitude > 0.05f)
                                    {//move obj
                                        int id = Int32.Parse(target.transform.parent.gameObject.name);
                                        puzzle p = gameController.GetPuzzle(id);
                                        if (p.startMove(decideDirection(normal)))
                                        {
                                            objtime = 0;
                                        }
                                        prepos = Vector3.zero;
                                    }
                                }
                            }
                            else
                            { //reset pre
                                prepos = Vector3.zero;
                            }
                        }
                    }
                    else
                    {//rotation
                        if (righthand.Fingers[0].IsExtended)
                        {//valid pos
                            if (detectColli(leftoffset, rightoffset, upoffset, downoffset, dir))
                            {// has target
                                GameObject target = info.collider.gameObject;
                                GameObject parent = target.transform.parent.gameObject;

                                if (predir == Vector3.zero)
                                {//firsthit
                                    predir = normal;
                                }
                                else
                                {
                                    if (Vector3.Dot(normal, predir) < 0.6)
                                    {//rot obj
                                        int id = Int32.Parse(target.transform.parent.gameObject.name);
                                        puzzle p = gameController.GetPuzzle(id);
                                        if (p.rotate(findclosestunit(Vector3.Cross(predir, normal))))
                                        {
                                            objtime = 0;
                                        }
                                        predir = Vector3.zero;
                                    }
                                }
                                debug1.SetPosition(0, leapToWorld(righthand.PalmPosition));
                                debug1.SetPosition(1, leapToWorld(righthand.PalmPosition) + normal);
                                debug2.SetPosition(0, leapToWorld(righthand.PalmPosition));
                                debug2.SetPosition(1, leapToWorld(righthand.PalmPosition) + predir);
                            }
                        }
                    }

                }
            }
        }

        bool detectColli(Vector3 left, Vector3 right, Vector3 up, Vector3 down, Vector3 dir)
        {
            if(Physics.Raycast(leaporigin + left, dir, out info) || Physics.Raycast(leaporigin + right, dir, out info)||
                Physics.Raycast(leaporigin + up, dir, out info) || Physics.Raycast(leaporigin + up, dir, out info))
            {
                return true;
            }
            return false;
        }
        Vector3 vec4To3(Vector4 v)
        {
            Vector3 res = new Vector3(v.x, v.y, v.z);
            return res;
        }

        Vector4 vec3To4(Vector3 v, float w)
        {
            Vector4 res = new Vector4(v.x, v.y, v.z, w);
            return res;
        }
        Vector3 decideDirection(Vector3 vec)
        {
            if (Vector3.Dot(Vector3.left, vec) > correctpos)
            {
                return Vector3.left;
            }
            if (Vector3.Dot(Vector3.right, vec) > correctpos)
            {
                return Vector3.right;
            }
            if (Vector3.Dot(Vector3.forward, vec) > correctpos)
            {
                return Vector3.forward;
            }
            if (Vector3.Dot(Vector3.back, vec) > correctpos)
            {
                return Vector3.back;
            }
            if (Vector3.Dot(Vector3.down, vec) > correctpos)
            {
                return Vector3.down;
            }
            return Vector3.zero;
        }

        Vector3 findclosestunit(Vector3 v)
        {
            float x = Vector3.Dot(v, Vector3.right);
            float mx = Vector3.Dot(v, Vector3.left);
            float y = Vector3.Dot(v, Vector3.up);
            float my = Vector3.Dot(v, Vector3.down);
            float z = Vector3.Dot(v, Vector3.forward);
            float mz = Vector3.Dot(v, Vector3.back);
            float[] t = { x, mx, y, my, z, mz };
            float max = 0;
            for(int i = 0; i < 6; i++)
            {
                if (t[i] > max)
                {
                    max = t[i];
                }
            }
            if (max == x)
            {
                return Vector3.right;
            }
            if (max == mx)
            {
                return Vector3.left;
            }
            if (max == y)
            {
                return Vector3.up;
            }
            if (max == my)
            {
                return Vector3.down;
            }
            if (max == z)
            {
                return Vector3.forward;
            }
            if (max == mz)
            {
                return Vector3.back;
            }
            else
            {
                Debug.Log("What!!!");
                return Vector3.zero;
            }
        }
        Vector3 leapToUnity(Leap.Vector v)
        {
            Vector3 result = new Vector3(0, 0, 0);
            result.x = -v.x / 1000.0f;
            result.y = -v.z / 1000.0f;
            result.z = -v.y / 1000.0f;
            return result;
        }

        Vector3 leapToWorld(Leap.Vector v)
        {
            Matrix4x4 m = camera.cameraToWorldMatrix;
            //Vector4 camori = m * new Vector4 (0, 0, 0, 1);
            Vector3 temp = leapToUnity(v);
            Vector4 cameravec = m * new Vector4(temp.x, temp.y, temp.z, 0);
            Vector3 res = new Vector3(cameravec[0], cameravec[1], cameravec[2]);
            res = res + leaporigin;
            return res;
        }

        bool isFist(Leap.Hand h)
        {
            int badfinger = 0;
            foreach (Leap.Finger fig in h.Fingers)
            {
                if (!fig.IsExtended)
                {
                    badfinger++;
                }
            }
            if (badfinger > 1)
            {
                return true;
            }
            return false;
        }

        LineRenderer DrawLine(Vector3 start, Vector3 end, Color color, float width)
        {
            GameObject myline = new GameObject();
            myline.transform.position = start;
            myline.AddComponent<LineRenderer>();
            LineRenderer lr = myline.GetComponent<LineRenderer>();
            lr.material = new Material(Shader.Find("Transparent/Diffuse"));
            lr.SetWidth(width, width);
            lr.positionCount = 2;
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            return lr;
        }

    }
}