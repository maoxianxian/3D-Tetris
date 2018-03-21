using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace gam
{
    public class handController
    {
        public Leap.Hand lefthand;
        Leap.Hand righthand;
        Leap.Controller controller;
        GameObject grabbed;
        float objtime = 0;
        Vector3 leaporigin;
        public GameObject leapspace;
        Camera camera;
        RaycastHit info;
        Vector3 prepos = Vector3.zero;
        gameController gamer;
        LineRenderer debug1;
        LineRenderer debug2;
        float correctpos = 0.9f;
        GameObject sphereswitch;
        GameObject sphererand;
        GameObject sphereexpand;
        GameObject spheresmall;
        GameObject menu;
        UnityEngine.UI.Text txt1;
        UnityEngine.UI.Text txt2;
        UnityEngine.UI.Text txt3;
        UnityEngine.UI.Text txt4;
        bool translate = true;
        public bool release = false;
        Vector3 bombdir = Vector3.zero;
        public static handController getctr;
        public GameObject bomb;
        public GameObject cub;
        float movetime = 0;
        float switchtime = 0;
        float releasetime=0;
        Vector3 prebombpos=Vector3.zero;
        Vector3 leftprepos=Vector3.zero;
        float cubereleasetime = 0;
        float leavetime = 0;
        public handController(gameController game, GameObject sph1,GameObject sph2, GameObject sph3,GameObject sph4, UnityEngine.UI.Text xt, UnityEngine.UI.Text yt, UnityEngine.UI.Text zt, UnityEngine.UI.Text wt)
        {
            menu = GameObject.Find("menu");
            leapspace = GameObject.Find("LeapSpace");
            controller = new Leap.Controller();
            camera = GameObject.Find("CenterEyeAnchor").GetComponent<Camera>();
            gamer = game;
            debug1 = DrawLine(Vector3.zero, Vector3.zero, Color.white, 0.05f);
            debug2 = DrawLine(Vector3.zero, Vector3.zero, Color.white, 0.05f);
            sphereswitch = sph1;
            sphererand = sph2;
            sphereexpand = sph3;
            spheresmall = sph4;
            txt1 = xt;
            txt2 = yt;
            txt3 = zt;
            txt4 = wt;
            getctr = this;
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

        public void addCub()
        {
            if (cub == null&&bomb==null)
            {
                cubereleasetime = 0;
                this.cub = GameObject.Instantiate(GameObject.Find("Cubeee"));
                GameObject.Destroy(cub.GetComponent<grabCube>());
            }
        }
        void updatecube()
        {
            if (cub != null)
            {
                if (lefthand != null)
                {
                    cub.transform.position = leapToWorld(lefthand.PalmPosition);
                }
            }
        }
        public void addBomb(GameObject bomb)
        {
            if (this.bomb == null)
            {
                this.bomb = bomb;
                bomb.transform.position = leapToWorld(lefthand.PalmPosition);
                release = false;
            }
        }
        public void Update()
        {
            switchtime+=Time.deltaTime;
            displayarmwidget();
            updatebomb();
            updatecube();
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
                    Vector3 normal = Vector3.Normalize(leapVectorToWorld(righthand.PalmNormal));
                    Vector3 dir = leapToWorld(righthand.PalmPosition) - leaporigin;
                    if (translate)
                    {//translate
                        if (decideDirection(normal) != Vector3.zero)
                        {//valid pos
                            if (detectColli(leftoffset, rightoffset, upoffset, downoffset, dir))
                            {// has target
                                leavetime = 0;
                                GameObject target = info.collider.gameObject;
                                if (prepos == Vector3.zero)
                                {//first hit
                                    prepos = leapToWorld(righthand.PalmPosition);
                                }
                                else
                                {
                                    if ((prepos - leapToWorld(righthand.PalmPosition)).magnitude > 0.07f)
                                    {//move obj
                                        int id = Int32.Parse(target.transform.parent.gameObject.name);
                                        puzzle p = gameController.GetPuzzle(id);
                                        if (p.startMove(decideDirection(normal)))
                                        {
                                            p.highlight();
                                            objtime = 0;
                                        }
                                        prepos = Vector3.zero;
                                    }
                                }
                            }
                            else
                            { //reset pre
                                leavetime += Time.deltaTime;
                                if (leavetime > 0.5f)
                                {
                                    prepos = Vector3.zero;
                                }
                            }
                        }
                    }
                    else
                    {//rotation
                        if (detectColli(leftoffset, rightoffset, upoffset, downoffset, dir))
                        {// has target
                            GameObject target = info.collider.gameObject;
                            GameObject parent = target.transform.parent.gameObject;
                            int id = Int32.Parse(target.transform.parent.gameObject.name);
                            puzzle p = gameController.GetPuzzle(id);
                            Vector3 hitpoint=info.point;
                            if (righthand.GrabStrength > 0.8f)
                            {
                                p.highlight();
                                Vector3 ax = Vector3.Cross(leapVectorToWorld(righthand.Direction), leapVectorToWorld(righthand.PalmNormal));
                                if (p.rotate(findclosestunit(ax)))
                                {
                                    objtime = 0;
                                }
                            }
                        }
                    }
                }
            }
            movetime += Time.deltaTime;
            if (movetime > 1)
            {
                if (lefthand != null)
                {
                    if (bomb == null && cub == null)
                    {
						if (isFist (lefthand)) {
							if (leftprepos == Vector3.zero) {
								leftprepos = leapToWorld (lefthand.PalmPosition);
							} else {
								if ((leftprepos - leapToWorld (lefthand.PalmPosition)).magnitude > 0.14f) {
									Vector3 dir = findclosestunit (leftprepos - leapToWorld (lefthand.PalmPosition));
									gamer.moveplayer (dir);
									leftprepos = Vector3.zero;
									movetime = 0;
								}
							}
						} else {
							leftprepos = Vector3.zero;
						}
                    }
                    else if (bomb != null)
                    {
                        if (!isFist(lefthand) && !release)
                        {
                            releasetime += Time.deltaTime;
                            if (prebombpos == Vector3.zero)
                            {
                                prebombpos = bomb.transform.position;
                            }
                            if (releasetime > 0.3f)
                            {
                                release = true;
                                bombdir = 0.2f * Vector3.Normalize(bomb.transform.position - prebombpos);
                            }
                        }
                        else
                        {
                            releasetime = 0;
                            prebombpos = Vector3.zero;
                        }
                    }
                    else
                    {
                        if (!isFist(lefthand) && !release)
                        {
                            cubereleasetime += Time.deltaTime;
                            if (cubereleasetime > 1)
                            {
                                if(gameController.ctr.addCube(leaporigin + leapspace.transform.forward))
                                {
                                    cubereleasetime = 0;
                                    GameObject.Destroy(cub);
                                    cub = null;
                                }
                            }
                        }
                        else
                        {
                            cubereleasetime = 0;
                        }
                    }
                }
            }
        }

        void updatebomb()
        {
            if (bomb != null)
            {
                if (!release)
                {
                    if (lefthand != null)
                    {
                        bomb.transform.position = leapToWorld(lefthand.PalmPosition);
                    }
                }
                else
                {
                    bomb.transform.position += 0.1f * bombdir;
                }
            }
        }
        void displayarmwidget()
        {
            Vector3 far = new Vector3(-0.5f, -0.5f, -0.5f);
            GameObject player = GameObject.Find("CenterEyeAnchor");
            if (righthand != null&& Vector3.Dot(leapVectorToWorld(righthand.PalmNormal),player.transform.forward)<-0.5f)
            {
                Vector3 palmpos = leapToWorld(righthand.PalmPosition);
                Vector3 mid = -Vector3.Cross(leapVectorToWorld(righthand.PalmNormal), leapVectorToWorld(righthand.Direction));
                sphereswitch.transform.position = palmpos + 0.1f * mid + 0.1f * leapVectorToWorld(righthand.Direction) + 0.02f * leapVectorToWorld(righthand.PalmNormal);
                sphererand.transform.position = palmpos + 0.1f * mid + 0.05f * leapVectorToWorld(righthand.Direction) + 0.02f * leapVectorToWorld(righthand.PalmNormal);
                sphereexpand.transform.position = palmpos + 0.1f * mid - 0.05f * leapVectorToWorld(righthand.Direction) + 0.02f * leapVectorToWorld(righthand.PalmNormal);
                spheresmall.transform.position = palmpos + 0.1f * mid - 0.1f * leapVectorToWorld(righthand.Direction) + 0.02f * leapVectorToWorld(righthand.PalmNormal);

                menu.transform.position = palmpos + 0.2f * mid;
                menu.transform.LookAt(palmpos + 0.25f * mid + leapVectorToWorld(righthand.Direction), mid);
                txt1.transform.position = menu.transform.position - 0.03f * player.transform.forward + 0.08f * menu.transform.forward;
                txt1.transform.LookAt((txt1.transform.position - menu.transform.right), menu.transform.forward);
                txt2.transform.position = menu.transform.position - 0.03f * player.transform.forward + 0.03f * menu.transform.forward;
                txt2.transform.LookAt(txt1.transform.position - menu.transform.right, menu.transform.forward);
                txt3.transform.position = menu.transform.position - 0.03f * player.transform.forward - 0.03f * menu.transform.forward;
                txt3.transform.LookAt(txt1.transform.position - menu.transform.right, menu.transform.forward);
                txt4.transform.position = menu.transform.position - 0.03f * player.transform.forward - 0.08f * menu.transform.forward;
                txt4.transform.LookAt(txt1.transform.position - menu.transform.right, menu.transform.forward);
            }
            else
            {
                menu.transform.position = far;
                sphererand.transform.position = far;
                sphereswitch.transform.position = far;
                sphereexpand.transform.position = far;
                spheresmall.transform.position = far;
                txt1.transform.position = far;
                txt2.transform.position = far;
                txt3.transform.position = far;
                txt4.transform.position = far;
            }
        }
        public bool switchmode()
        {
            if (switchtime > 1)
            {
                switchtime = 0;
                translate = !translate;
                return true;
            }
            else
            {
                return false;
            }
        }
        bool detectColli(Vector3 left, Vector3 right, Vector3 up, Vector3 down, Vector3 dir)
        {
            if(Physics.Raycast(leaporigin + left, dir, out info) || Physics.Raycast(leaporigin + right, dir, out info)||
                Physics.Raycast(leaporigin + up, dir, out info) || Physics.Raycast(leaporigin + up, dir, out info))
            {
                if (info.collider.gameObject.transform.parent!=null&&info.collider.gameObject.transform.parent.gameObject.tag=="puzzle")
                {
                    return true;
                }
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
            result.x = -v.x;
            result.y = -v.z;
            result.z = -v.y;
            return result;
        }

        Vector3 leapToWorld(Leap.Vector v)
        {
            Matrix4x4 m = camera.cameraToWorldMatrix;
            Vector3 temp = leapToUnity(v)/1000.0f;
            Vector4 cameravec = m * new Vector4(temp.x, temp.y, temp.z, 0);
            Vector3 res = new Vector3(cameravec[0], cameravec[1], cameravec[2]);
            res = res + leaporigin;
            return res;
        }

        Vector3 leapVectorToWorld(Leap.Vector v)
        {
            Matrix4x4 m = camera.cameraToWorldMatrix;
            //Vector4 camori = m * new Vector4 (0, 0, 0, 1);
            Vector3 temp = leapToUnity(v);
            Vector4 cameravec = m * new Vector4(temp.x, temp.y, temp.z, 0);
            Vector3 res = new Vector3(cameravec[0], cameravec[1], cameravec[2]);
            return res;
        }

        public bool isFist(Leap.Hand h)
        {
            if (h == null)
            {
                return false;
            }
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