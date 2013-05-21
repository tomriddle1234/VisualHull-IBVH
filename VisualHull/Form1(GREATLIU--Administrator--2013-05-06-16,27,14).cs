using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Emgu.CV;//PS:调用的Emgu dll   
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using OpenCvSharp;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Management;
using System.Threading;
using System.ComponentModel;
using System.Data;
using CsGL.OpenGL;  // 引入 CsGL.OpenGL


namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public Form1()
        {           
            InitializeComponent();
        }
        private double[,] Read_camerpar_txt(string filePath, string image_name)//读相机参数txt文本函数.返回投影矩阵（数组）
        {
            FileStream fs = File.OpenRead(filePath);
            byte[] data = new byte[fs.Length];//表示一次性读取的数据
            fs.Read(data, 0, data.Length);

            string str = System.IO.File.ReadAllText(filePath);
            Regex reg = new Regex(image_name);
            Match mat = reg.Match(str);
            int Start_count = 0;
            while (mat.Success)
            {
                Start_count = mat.Index;
                mat = reg.Match(str, mat.Index + mat.Length);

            }

            // int i = 0;//字符数 
            // double[,] inter_parameter = new double[3, 3];
            // double[,] outer_parameter = new double[3, 4];
            string tmp_string = "";//临时空格之间的字符串
            char tmp;//临时存储每一次提取的字符
            double[,] Project = new double[3, 4];
            int j = 1;//记录空格数（单幅图像的各个参数之间的空格）

            for (Start_count = Start_count + image_name.Length + 1; Start_count < data.Length; Start_count++)
            {

                tmp = (char)data[Start_count];
                tmp_string = tmp_string + (char)data[Start_count];
                //MessageBox.Show("单字符" + tmp.ToString());
                if (tmp.ToString() == " ")
                {

                    if (j == 1)//第二个空格开始取数据
                    {

                        //MessageBox.Show("k11" + tmp_string.Substring(0, tmp_string.Length - 1));
                        Project[0, 0] = Convert.ToDouble(tmp_string.ToString());
                        //  MessageBox.Show("k11:" + Project[0, 0].ToString());
                        tmp_string = " ";
                        //i = Start_count + 1;
                        // Start_count++;
                    }

                    if (j == 2)
                    {
                        Project[0, 1] = Convert.ToDouble(tmp_string.ToString());
                        //MessageBox.Show("k12" + tmp_string.ToString());
                        //  MessageBox.Show("k11:" + Project[0, 1].ToString());
                        tmp_string = " ";
                        // Start_count++;
                    }

                    if (j == 3)
                    {
                        Project[0, 2] = Convert.ToDouble(tmp_string.ToString());
                        // MessageBox.Show("k11:" + Project[0, 2].ToString());
                        tmp_string = " ";
                        // i = Start_count + 1;
                        //Start_count++;
                        // MessageBox.Show("k13" + Project[0, 2].ToString());
                    }


                    if (j == 4)
                    {
                        Project[0, 3] = Convert.ToDouble(tmp_string.ToString());
                        //  MessageBox.Show("k11:" + Project[0, 3].ToString());
                        tmp_string = " ";
                        // Start_count++;

                        // MessageBox.Show("k13" + inter_parameter[0, 2].ToString());
                    }

                    if (j == 5)
                    {
                        Project[1, 0] = Convert.ToDouble(tmp_string.ToString());
                        //MessageBox.Show("k11:" + Project[1, 0].ToString());
                        tmp_string = " ";
                        // Start_count++;

                        // MessageBox.Show("k21" + inter_parameter[1, 0].ToString());
                    }

                    if (j == 6)
                    {
                        Project[1, 1] = Convert.ToDouble(tmp_string.ToString());
                        // MessageBox.Show("k11:" + Project[1, 1].ToString());
                        tmp_string = " ";
                        // Start_count++;

                        // MessageBox.Show("k22" + inter_parameter[1, 1].ToString());
                    }

                    if (j == 7)
                    {
                        Project[1, 2] = Convert.ToDouble(tmp_string.ToString());
                        //  MessageBox.Show("k11:" + Project[1, 2].ToString());
                        tmp_string = " ";
                        //Start_count++;

                        // MessageBox.Show("k23" + inter_parameter[1, 2].ToString());
                    }


                    if (j == 8)
                    {
                        Project[1, 3] = Convert.ToDouble(tmp_string.ToString());
                        // MessageBox.Show("k11:" + Project[1, 3].ToString());
                        tmp_string = " ";
                        // Start_count++;

                        // MessageBox.Show("k23" + inter_parameter[1, 2].ToString());
                    }

                    if (j == 9)
                    {
                        Project[2, 0] = Convert.ToDouble(tmp_string.ToString());
                        //MessageBox.Show("k11:" + Project[2, 0].ToString());
                        tmp_string = " ";
                        // Start_count++;

                        //MessageBox.Show("k31" + inter_parameter[2, 0].ToString());
                    }

                    if (j == 10)
                    {
                        Project[2, 1] = Convert.ToDouble(tmp_string.ToString());
                        //MessageBox.Show("k11:" + Project[2, 1].ToString());
                        tmp_string = " ";
                        //Start_count++;

                        // MessageBox.Show("k32" + inter_parameter[2, 1].ToString());
                        // MessageBox.Show("开始注意1");
                    }

                    if (j == 11)
                    {
                        // MessageBox.Show("j11" + tmp_string.ToString());
                        Project[2, 2] = Convert.ToDouble(tmp_string.ToString());
                        // MessageBox.Show("k11:" + Project[2, 2].ToString());
                        tmp_string = " ";
                        //Start_count++;
                        //MessageBox.Show("j1" + Project[2, 2].ToString());
                        // MessageBox.Show("k33" + inter_parameter[2, 2].ToString());
                        //MessageBox.Show("开始注意" );
                    }
                    j++;
                }
                else if (tmp.ToString() == "\n")
                {
                    Project[2, 3] = Convert.ToDouble(tmp_string.ToString());
                    //MessageBox.Show("k11:" + Project[2, 3].ToString());
                    //i = Start_count + 1;
                    tmp_string = " ";

                    //MessageBox.Show("j" +Project[2, 2].ToString());
                    break;
                }

            }         
            fs.Dispose();
            str = "";//清空空间CV_64FC1
            return Project;
        }
        public double[,] Read_Contour_txt(string filePath, string image_name,out int contour_number)//读轮廓点txt文本函数
        {

           //MessageBox.Show("图像名称"+image_name.ToString ());
            FileStream fs = File.OpenRead(filePath);
            byte[] data = new byte[fs.Length];//表示一次性读取的数据
            fs.Read(data, 0, data.Length);

            string str = System.IO.File.ReadAllText(filePath);
            Regex reg = new Regex(image_name);

            Match mat = reg.Match(str);
            int Start_count = 0;
            while (mat.Success)//如果找不到文件名，则默认用第一个的轮廓
            {
                Start_count = mat.Index;
                mat = reg.Match(str, mat.Index + mat.Length);
                //MessageBox.Show("111每一次起点" + mat.Index.ToString());
            } 

            int i = 0;
            //int contour_number = 0;
            contour_number = 0;
            char tmp;
            string tmp_string = "";
            //MessageBox.Show("每一次起点" + Start_count.ToString());
            for (i = Start_count + image_name.Length; i < data.Length; i++)//先把图像轮廓点的数量字符提取出来
            {

                tmp = (char)(data[i]);
                //MessageBox.Show(i.ToString());
                if (tmp.ToString() != "\n")
                {
                    tmp_string = tmp_string + (char)data[i];
                }
                else if (tmp.ToString() == "\n")
                {
                    // contour_number = (int)Convert.ToDouble(tmp_string);
                    contour_number = (int)Convert.ToDouble(tmp_string);
                   // MessageBox.Show("轮廓数量" + contour_number.ToString());
                    tmp_string = "";
                    //Start_count = i;//先把图像轮廓点的数量字符提取出来

                    break;
                }
            }//先把图像的数量字符提取出来

            double[,] contour_point = new double[2, contour_number - 1];//最后一个点的最后位置符号无法确定，实在不想折腾了，少一个点就少一个点吧
            int j = 0;
            int k = 1;
            for (Start_count = i + 1; Start_count < data.Length; Start_count++)
            {
                //MessageBox.Show("开始循环" + Start_count.ToString());
                tmp = (char)data[Start_count];
                // MessageBox.Show("字符" + tmp.ToString());
                if (tmp.ToString() == " ")
                {
                    if (k == 1)
                    {
                        tmp_string = tmp_string.Substring(0, (tmp_string.Length));

                        // MessageBox.Show(tmp_string.ToString());  
                        if (j < contour_number - 1)
                        {
                            //contour_point[0, j] = (int)Convert.ToDouble(tmp_string);//轮廓点的x分量
                           // MessageBox.Show("x分量" + tmp_string.ToString());
                            contour_point[0, j] = (int)Convert.ToDouble(tmp_string);
                            //MessageBox.Show("x分量" + contour_point[0, j].ToString());
                            tmp_string = " ";
                          // MessageBox.Show("x分量" + contour_point[0, j].ToString());
                            //j++;
                        }
                        else
                        {
                            break;
                        }
                        k++;
                    }
                    else
                    {
                        tmp_string = tmp_string.Substring(0, (tmp_string.Length));
                        if (j < contour_number - 1)
                        {
                            //MessageBox.Show("y分量" + tmp_string.Substring(2, tmp_string.Length - 2).ToString());
                            // contour_point[1, j] = (int)Convert.ToDouble(tmp_string.Substring(2, tmp_string.Length - 2).ToString());//轮廓点的y分量
                            contour_point[1, j] = (int)Convert.ToDouble(tmp_string.Substring(2, tmp_string.Length - 2).ToString());
                            //MessageBox.Show("y分量" + contour_point[1, j].ToString());
                            tmp_string = " ";
                            // MessageBox.Show(contour_point[1, j-1].ToString());
                            j++;
                        }
                        else
                        {
                            break;
                        }
                        k--;
                    }
                    //  j++;  
                }

                tmp_string = tmp_string + (char)data[Start_count];
                //j++;
            }

            fs.Dispose();

            contour_number--;//因为输出的轮廓数组的最后一个是没有输出的，所以对外面来说是少了一个轮廓点
           
            return contour_point;//返回的是一个轮廓点坐标的二维数组



            //return intersection;//返回的是一个轮廓点坐标的二维数组
            //需要轮廓点的数两
        }
        private CvMat Computecorrespondepilines(double[,] first_projcet, double[,] second_projcet, double[,] right_contour_point)//求两幅图的极线，此算法在窄基线情况下成立,返回的是位于基线上的两个点（因为求交的时候是用点作为参数，而不是A,B,C）               
        {
            //double[,] Epiline_point = new double[4, right_contour_point.GetLength(1)];
            CvMat Epiline_point = new CvMat(4, right_contour_point.GetLength(1), MatrixType.F64C1);
            CvMat correspondent_lines = new CvMat(3, right_contour_point.GetLength(1), MatrixType.F64C1);
            CvMat FundamentalMat = new CvMat(3, 3, MatrixType.F64C1);

            //《根据投影矩阵求基础矩阵》网页来求基础矩阵
            double[,] M11 = new double[3, 3]{{first_projcet[0,0],first_projcet[0,1],first_projcet[0,2]},
                                             {first_projcet[1,0],first_projcet[1,1],first_projcet[1,2]},
                                             {first_projcet[2,0],first_projcet[2,1],first_projcet[2,2]}};
            double[,] M21 = new double[3, 3]{{second_projcet[0,0],second_projcet[0,1],second_projcet[0,2]},
                                             {second_projcet[1,0],second_projcet[1,1],second_projcet[1,2]},
                                             {second_projcet[2,0],second_projcet[2,1],second_projcet[2,2]}};

            double[,] m1 = new double[3, 1] { { first_projcet[0, 3] }, { first_projcet[1, 3] }, { first_projcet[2, 3] } };
            double[,] m2 = new double[3, 1] { { second_projcet[0, 3] }, { second_projcet[1, 3] }, { second_projcet[2, 3] } };

            CvMat M11_mat = new CvMat(3, 3, MatrixType.F64C1, M11);
            CvMat M21_mat = new CvMat(3, 3, MatrixType.F64C1, M21);
            CvMat m1_mat = new CvMat(3, 1, MatrixType.F64C1, m1);
            CvMat m2_mat = new CvMat(3, 1, MatrixType.F64C1, m2);

            CvMat M11_matInv = M11_mat.Clone();
            M11_mat.Inv(M11_matInv);
            CvMat temp3 = M21_mat * M11_matInv * m1_mat;

            double[,] temp3_arry = new double[3, 1] { { temp3[0, 0] }, { temp3[1, 0] }, { temp3[2, 0] } };

            double[,] m_arry = new double[3, 1];
            m_arry = MatrixSubtration(m2, temp3_arry);
            CvMat m_mat = new CvMat(3, 1, MatrixType.F64C1, m_arry);

            double[,] mx_mat_arry = new double[3, 3] { { 0,           -m_mat[2, 0], m_mat[1, 0] }, 
                                                       { m_mat[2, 0], 0,            -m_mat[0, 0] }, 
                                                       { -m_mat[1, 0], m_mat[0,0],  0} };

            CvMat mx_mat = new CvMat(3, 3, MatrixType.F64C1, mx_mat_arry);
            //MessageBox.Show(m_mat.ToString());
            //MessageBox.Show(mx_mat.ToString());
            FundamentalMat = mx_mat * M21_mat * M11_matInv;


            CvMat matA = new CvMat(2, right_contour_point.GetLength(1), MatrixType.F64C1, right_contour_point);//将数组转换为矩阵，列表示点的个数

            Cv.ComputeCorrespondEpilines(matA, 2, FundamentalMat, out correspondent_lines);//correspondent_lines的列表示点的个数，经过证明图像指数是2，将其极线映射到参考图

            double A = 0, B = 0, C = 0;//方程系数  

            //double A1 = correspondent_lines[0, 0];
            //double B1 = correspondent_lines[1, 0];
            //double C1 = correspondent_lines[2, 0];
            //double A2 = correspondent_lines[0, 1];
            //double B2 = correspondent_lines[1, 1];
            //double C2 = correspondent_lines[2, 1];
            //double[] epipole_temp = new double[2];
            //epipole_temp[0] = (-1) * (B2 * C1 - B1 * C2) / (A1 * B2 - A2 * B1);
            //epipole_temp[1] = (-1) * (A2 * C1 - A1 * C2) / (A2 * B1 - A1* B2);
            //epipole = epipole_temp;

            for (int i = 0; i < right_contour_point.GetLength(1); i++)//一个轮廓点对应一条极线(有些轮廓点无相应图像的极线)，一条极线要获得其上两个点，因为求交要用
            {
                A = correspondent_lines[0, i];
                B = correspondent_lines[1, i];
                C = correspondent_lines[2, i];

                Epiline_point[0, i] = 0;
                Epiline_point[1, i] = ((-C) / B);
                Epiline_point[2, i] = ((-C) / A);
                Epiline_point[3, i] = 0;


                //if (i != right_contour_point.GetLength(1) - 1)
                //{
                //    A1 = correspondent_lines[0, i];
                //    B1 = correspondent_lines[1, i];
                //    C1 = correspondent_lines[2, i];
                //    A2 = correspondent_lines[0, i + 1];
                //    B2 = correspondent_lines[1, i + 1];
                //    C2 = correspondent_lines[2, i + 1];
                //    epipole_temp = new double[2];
                //    epipole_temp[0] = (-1) * (B2 * C1 - B1 * C2) / (A1 * B2 - A2 * B1);
                //    epipole_temp[1] = (-1) * (A2 * C1 - A1 * C2) / (A2 * B1 - A1 * B2);
                //}

            }//轮询轮廓点的循环在此结束


            //MessageBox.Show(correspondent_lines.ToString());  
             Cv.ReleaseMat(correspondent_lines);
             Cv.ReleaseMat(FundamentalMat);
             Cv.ReleaseMat(M11_mat);
             Cv.ReleaseMat(M21_mat);
             Cv.ReleaseMat(m1_mat);
             Cv.ReleaseMat(m2_mat);
             Cv.ReleaseMat(M11_matInv );
             Cv.ReleaseMat(temp3 );
             Cv.ReleaseMat(m_mat);   
             Cv.ReleaseMat(mx_mat);
             Cv.ReleaseMat(matA);            
            return Epiline_point;
        }        
        public double[,] MatrixSubtration(double[,] a, double[,] b)
        {
            double[,] c = new double[a.GetLength(0), a.GetLength(1)];
            if (a.GetLength(0) != b.GetLength(0) || a.GetLength(1) != b.GetLength(1))
                MessageBox.Show("不满足相加条件");

            for (int i = 0; i < a.GetLength(0); i++)
            {
                for (int j = 0; j < a.GetLength(1); j++)
                {
                    c[i, j] = a[i, j] - b[i, j];
                }
            }
            return c;
        }// 矩阵的减        
        public double[] SL_Cross_Intersection(CvPoint c1, CvPoint c2, CvPoint c3, CvPoint c4)
        {
            double[] Intersection = new double[3];
            double t1 = 0;            
            double k = 0;           
            k  = (c4.X - c3.X) * (c2.Y - c1.Y) - (c4.Y - c3.Y) * (c2.X - c1.X);
            t1 = ((c3.Y - c1.Y) * (c4.X - c3.X) - (c3.X - c1.X) * (c4.Y - c3.Y)) / k; 

            if ((k != 0)&&((t1 >= 0) && (t1 <= 1)))
              {
                  Intersection[0] = 1;
                  Intersection[1] = c1.X + t1 * (c2.X - c1.X);
                  Intersection[2] = c1.Y + t1 * (c2.Y - c1.Y);                
              }
            else          
             {
                 Intersection[0] = 0;
              }
           // MessageBox.Show(Intersection[1].ToString() + " " + Intersection[2]);
            return Intersection;//第一位是0没交点，是1表示有交点
        }//求平面交点，输入交点的顺利有讲究（前两个点是是第一副图的轮廓点，后两个是极线上的点）
        List<double[]> Compute_epiline_contour_intersection(CvMat right_epiline_point, double[,] ref_img_contour ,int n)//n表示划分bin的个数
        {
            //计时
            //Stopwatch sw = new Stopwatch();
            //sw.Start();

            double[] epipole = new double[2];

            //计算极点
            double X1 = right_epiline_point[2, 0];
            double Y1 = right_epiline_point[1, 0];
            double X2 = right_epiline_point[2, 1];
            double Y2 = right_epiline_point[1, 1];
            epipole[0] = (Y2 - Y1) / (Y2 / X2 - Y1 / X1);
            epipole[1] = (X2 - X1) / (X2 / Y2 - X1 / Y1);
            double temp;
            double radius;
            double x, y;
            double min_radius = 200.0;
            double max_radius = -200.0;
            //double min_radius_contour_point;
            //double max_radius_contour_point;
            double[] temp_radius = new double[ref_img_contour.GetLength(1)];

            for (int i = 0; i < ref_img_contour.GetLength(1); i++)//不用最后一个轮廓点
            {
                //求角度 tan -> 角度，将其分到一个盒子中，一共有n=360/m个盒子
                x = ref_img_contour[0, i] - epipole[0];
                y = ref_img_contour[1, i] - epipole[1];

                //cacu_angle
                temp_radius[i] = Math.Atan2(y, x) + Math.PI;//x轴负方向为0,2pi,x轴正方向为pi

                if (temp_radius[i] < min_radius)
                {
                    min_radius = temp_radius[i];
                    //min_radius_contour_point = i;
                }
                else if (temp_radius[i] > max_radius)
                {
                    max_radius = temp_radius[i];
                    //max_radius_contour_point = i;
                }
            }



            
            List<int>[] bins = new List<int>[n + 1];
            for (int i = 0; i < bins.GetLength(0); i++)
            {
                bins[i] = new List<int>();
            }



            for (int i = 0; i < ref_img_contour.GetLength(1); i++)//不用最后一个轮廓点
            {
                //put_in_bins
                temp = (temp_radius[i] - min_radius) / ((max_radius - min_radius) / n);
                bins[(int)((temp_radius[i] - min_radius) / ((max_radius - min_radius) / n))].Add(i);//将第i个点放入盒子中
            }


            IplImage img = Cv.CreateImage(new CvSize(2000, 1500),BitDepth.U8, 3);
            Cv.Set(img, new CvScalar(255, 255, 255));
            for (int i = 0; i < bins.Length; i++)
            {
                Random rand = new Random();
                int r=rand.Next()%255;
                int g=rand.Next()%255;
                int b=rand.Next()%255;
                if (i == 0)
                {
                    r = 255; g = 0; b = 0;
                }
                else if (i == 1)
                {
                    r = 0; g = 255; b = 0;
                }
                else if (i == 2)
                {
                    r = 0; g = 0; b = 255;
                }
                else if (i == 3)
                {
                    r = 0; g = 255; b = 255;
                }
                else if (i == 4)
                {
                    r = 255; g = 0; b = 255;
                }
                else if (i == 5)
                {
                    r = 255; g = 255; b = 0;
                }

                for (int j = 0; j < bins[i].Count; j++)
                {
                    int k = bins[i][j];

                    img.DrawLine(new CvPoint((int)ref_img_contour[0, k], (int)ref_img_contour[1, k]),
                                new CvPoint((int)(ref_img_contour[0, k]+1), (int)(ref_img_contour[1, k])+1), Cv.RGB(r, g, b),2);
                }
            }

            Cv.SaveImage("bins.jpg", img);

            ////将最后一个点和第一个点连起来
            //y = ref_img_contour[0, 1] - ref_img_contour[0, ref_img_contour_num-1];
            //x = ref_img_contour[0, 0] - ref_img_contour[0, ref_img_contour_num-2];
            //radius = Math.Atan2(y, x)+Math.PI;
            //bins[(int)(radius / (2*Math.PI / n))].Add(ref_img_contour_num/2-1);//将第i个点放入盒子中

            double[] intersection_point;
            List<double[]> intersection_point_list = new List<double[]>();

            CvPoint epiline_point_start;
            CvPoint epiline_end_point_end;
            CvPoint contour_line_point_start;
            CvPoint contour_line_point_end;

            //计算极线所在的bin
            int current_bin;
            for (int i = 0; i < right_epiline_point.GetDimSize(1); i++)
            {
                y = right_epiline_point[1, i] - right_epiline_point[3, i];
                x = right_epiline_point[0, i] - right_epiline_point[2, i];

                radius = Math.Atan2(y, x) + Math.PI;
                if (radius >= min_radius && radius <= max_radius)
                {
                    current_bin = (int)((radius - min_radius) / ((max_radius - min_radius) / n));

                    for (int j = 0; j < bins[current_bin].Count - 1; j++)
                    {
                        epiline_point_start.X = (int)(right_epiline_point[0, i]);
                        epiline_point_start.Y = (int)(right_epiline_point[1, i]);
                        epiline_end_point_end.X = (int)(right_epiline_point[2, i]);
                        epiline_end_point_end.Y = (int)(right_epiline_point[3, i]);
                        contour_line_point_start.X = (int)(ref_img_contour[0, bins[current_bin][j]]);
                        contour_line_point_start.Y = (int)(ref_img_contour[1, bins[current_bin][j]]);
                        contour_line_point_end.X = (int)(ref_img_contour[0, bins[current_bin][j] + 1]);
                        contour_line_point_end.Y = (int)(ref_img_contour[1, bins[current_bin][j] + 1]);

                        intersection_point = SL_Cross_Intersection(contour_line_point_start, contour_line_point_end, epiline_point_start, epiline_end_point_end);
                        
                        if (intersection_point[0] > 0.0)
                        {
                            intersection_point[0] = i;
                            intersection_point_list.Add(intersection_point);
                        }
                    }
                }
            }

            ////显示计时时间
            //sw.Stop();
            //MessageBox.Show(sw.ElapsedMilliseconds.ToString());

            return intersection_point_list;

        }      
        private double[] Cross_intersection_3D(double[] Intersection, double[,] first_projcet, double[,] second_projcet, CvPoint Correspond_point, int contour_point_index, double red, double green, double blue, int left_image_number, int right_image_number, int contour_index_model_operation_number)//求空间点坐标
        {
            //IplImage first_image = new IplImage(image2, LoadMode.Color);
            //根据《空间点三维重建新方法及其不确定性研究》论文完成

            double[,] A = new double[4, 3]{{Intersection[1]*first_projcet[2,0]-first_projcet[0,0],     Intersection[1]*first_projcet[2,1]-first_projcet[0,1],     Intersection[1]*first_projcet[2,2]-first_projcet[0,2]},
                                           {Intersection[2]*first_projcet[2,0]-first_projcet[1,0],     Intersection[2]*first_projcet[2,1]-first_projcet[1,1],     Intersection[2]*first_projcet[2,2]-first_projcet[1,2]},
                                           {Correspond_point.X*second_projcet[2,0]-second_projcet[0,0],Correspond_point.X*second_projcet[2,1]-second_projcet[0,1],Correspond_point.X*second_projcet[2,2]-second_projcet[0,2]},
                                           {Correspond_point.Y*second_projcet[2,0]-second_projcet[1,0],Correspond_point.Y*second_projcet[2,1]-second_projcet[1,1],Correspond_point.Y*second_projcet[2,2]-second_projcet[1,2]}};


            double[,] y = new double[6, 1]{{first_projcet[0,3]-Intersection[1]*first_projcet[2,3]},
                                           {first_projcet[1,3]-Intersection[2]*first_projcet[2,3]},
                                           {second_projcet[0,3]-(Correspond_point.X*second_projcet[2,3])},
                                           {second_projcet[1,3]-(Correspond_point.Y*second_projcet[2,3])},
                                           {0},{0}};



            double[,] s1 = new double[1, 3] { { A[0, 1] * A[1, 2] - A[0, 2] * A[1, 1], A[1, 0] * A[0, 2] - A[0, 0] * A[1, 2], A[0, 0] * A[1, 1] - A[1, 0] * A[0, 1] } };


            double[,] s2 = new double[1, 3] { { A[2, 1] * A[3, 2] - A[2, 2] * A[3, 1], A[3, 0] * A[2, 2] - A[2, 0] * A[3, 2], A[2, 0] * A[3, 1] - A[3, 0] * A[2, 1] } };


            double[,] D = new double[6, 6] {{ A[0, 0], A[0, 1], A[0, 2], 0,      0,      0       },
                                            { A[1, 0], A[1, 1], A[1, 2], 0,      0,      0       }, 
                                            { 0,       0,       0,       A[2, 0],A[2, 1],A[2, 2] },
                                            { 0,       0,       0,       A[3, 0],A[3, 1],A[3, 2] },
                                            { s1[0,0], s1[0,1], s1[0,2], -s1[0,0],-s1[0,1], -s1[0,2]}, 
                                            { s2[0,0], s2[0,1], s2[0,2], -s2[0,0],-s2[0,1], -s2[0,2]} };


            CvMat D_mat = new CvMat(6, 6, MatrixType.F64C1, D);
            CvMat D1_mat = new CvMat(6, 1, MatrixType.F64C1, y);

            CvMat matAInv1 = new CvMat(6, 6, MatrixType.F64C1, D);
            matAInv1 = D_mat.Clone();
            D_mat.Inv(matAInv1);
            CvMat result = new CvMat(6, 1, MatrixType.F64C1);
            result = matAInv1 * D1_mat;

            //CvMat result= matAInv1 * D1_mat;

            //double Xb = result[0].Val0; double Yb = result[1].Val0; double Zb = result[2].Val0;
            double Xc = result[3].Val0; double Yc = result[4].Val0; double Zc = result[5].Val0;

            //如果映射像素颜色则运行时间很慢
            //CvScalar first_image_pixel;
            //int first_image_pixel_x = (int)Intersection[1];
            //int first_image_pixel_y = (int)Intersection[2];
            //first_image_pixel = Cv.Get2D(first_image, first_image_pixel_y, first_image_pixel_x);//
            //double[,] point_3D_location = new double[1, 10] { { contour_point_index, (Xb + Xc) / 2, (Yb + Yc) / 2, (Zb + Zc) / 2, 1, red, green, blue,left_image_number,right_image_number } };

            double[] point_3D_location;
            point_3D_location = new double[11] { contour_point_index, Xc, Yc, Zc, 1, red, green, blue, left_image_number, right_image_number, contour_index_model_operation_number };//可视线上

            // point_3D_location = new double[1, 10] { { contour_point_index, Xb, Yb, Zb, 1, red, green, blue, left_image_number, right_image_number } };//参考图上
            //point_3D_location = new double[1, 11] { { contour_point_index, (Xb + Xc) / 2, (Yb + Yc) / 2, (Zb + Zc) / 2, 1, red, green, blue, left_image_number, right_image_number, contour_index_model_operation_number } };

            Cv.ReleaseMat(D_mat);
            Cv.ReleaseMat(D1_mat);
            Cv.ReleaseMat(matAInv1);
            Cv.ReleaseMat(result);
            //GC.Collect();
            return point_3D_location;
        }        
        private double[] Cross_intersection_3D_Direct_Solve(double[] Intersection, double[,] first_projcet, double[,] second_projcet, CvPoint Correspond_point, int contour_point_index, double red, double green, double blue, int left_image_number, int right_image_number, int contour_index_model_operation_number)//求空间点坐标
        {
            //IplImage first_image = new IplImage(image2, LoadMode.Color);
            //根据《空间点三维重建新方法及其不确定性研究》论文完成

            double[,] A = new double[4, 3]{{Intersection[1]*first_projcet[2,0]-first_projcet[0,0],     Intersection[1]*first_projcet[2,1]-first_projcet[0,1],     Intersection[1]*first_projcet[2,2]-first_projcet[0,2]},
                                           {Intersection[2]*first_projcet[2,0]-first_projcet[1,0],     Intersection[2]*first_projcet[2,1]-first_projcet[1,1],     Intersection[2]*first_projcet[2,2]-first_projcet[1,2]},
                                           {Correspond_point.X*second_projcet[2,0]-second_projcet[0,0],Correspond_point.X*second_projcet[2,1]-second_projcet[0,1],Correspond_point.X*second_projcet[2,2]-second_projcet[0,2]},
                                           {Correspond_point.Y*second_projcet[2,0]-second_projcet[1,0],Correspond_point.Y*second_projcet[2,1]-second_projcet[1,1],Correspond_point.Y*second_projcet[2,2]-second_projcet[1,2]}};


            double[,] y = new double[4, 1]{{first_projcet[0,3]-Intersection[1]*first_projcet[2,3]},
                                           {first_projcet[1,3]-Intersection[2]*first_projcet[2,3]},
                                           {second_projcet[0,3]-(Correspond_point.X*second_projcet[2,3])},
                                           {second_projcet[1,3]-(Correspond_point.Y*second_projcet[2,3])},
                                           };


            CvMat A_mat = new CvMat(4,3,MatrixType.F64C1,A);
            CvMat y_mat = new CvMat(4,1,MatrixType.F64C1,y);
            CvMat X = new CvMat(3,1,MatrixType.F64C1);

            Cv.Solve(A_mat, y_mat, X,InvertMethod.Svd);

            double Xc = X[0,0]; double Yc = X[1,0]; double Zc = X[2,0];

            //如果映射像素颜色则运行时间很慢
            //CvScalar first_image_pixel;
            //int first_image_pixel_x = (int)Intersection[1];
            //int first_image_pixel_y = (int)Intersection[2];
            //first_image_pixel = Cv.Get2D(first_image, first_image_pixel_y, first_image_pixel_x);//
            //double[,] point_3D_location = new double[1, 10] { { contour_point_index, (Xb + Xc) / 2, (Yb + Yc) / 2, (Zb + Zc) / 2, 1, red, green, blue,left_image_number,right_image_number } };

            double[] point_3D_location;
            point_3D_location = new double[11] { contour_point_index, Xc, Yc, Zc, 1, red, green, blue, left_image_number, right_image_number, contour_index_model_operation_number };//可视线上

            // point_3D_location = new double[1, 10] { { contour_point_index, Xb, Yb, Zb, 1, red, green, blue, left_image_number, right_image_number } };//参考图上
            //point_3D_location = new double[1, 11] { { contour_point_index, (Xb + Xc) / 2, (Yb + Yc) / 2, (Zb + Zc) / 2, 1, red, green, blue, left_image_number, right_image_number, contour_index_model_operation_number } };

            Cv.ReleaseMat(A_mat);
            Cv.ReleaseMat(y_mat);
            Cv.ReleaseMat(X);
            //GC.Collect();
            return point_3D_location;
        }
        private void save_ply(string filepath, double[][] vertex3d, double[] face)
        {
            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            string ply_header = "ply\nformat ascii 1.0\ncomment made by Road Liu\ncomment this file is a cube\nelement vertex " + vertex3d.Length.ToString() + "\nproperty float x\n"
                                + "property float y\nproperty float z\nproperty   uint8 red\nproperty   uint8 green\nproperty   uint8 blue\nelement face " + face.Length.ToString() + "\nproperty list uchar int vertex_index\nend_header\n";

            for (int i = 0; i < vertex3d.Length; i++)
            {
                ply_header += vertex3d[i][1].ToString();
                ply_header += " ";
                ply_header += vertex3d[i][2].ToString();
                ply_header += " ";
                ply_header += vertex3d[i][3].ToString();
                ply_header += " ";
                ply_header += vertex3d[i][5].ToString();
                ply_header += " ";
                ply_header += vertex3d[i][6].ToString();
                ply_header += " ";
                ply_header += vertex3d[i][7].ToString();
                ply_header += "\n";
            }

            for (int i = 0; i < face.Length; i++)
            {
                //Todo 写入面片信息
            }
            
            Byte[] ply_data = Encoding.Default.GetBytes(ply_header);

            fs.Write(ply_data, 0, ply_data.Length);
        }


        private void getContour()
        {
            for (int i = 0; i < 8; i++)
            {
                IplImage foreground = new IplImage("dataset-dinosaur\\dinosaur-images\\"+i.ToString("00")+".jpg",LoadMode.GrayScale);
                IplImage background = new IplImage("dataset-dinosaur\\dinosaur-images\\background.jpg",LoadMode.GrayScale);
                IplImage imgContour = Cv.Clone(foreground);
                Cv.Threshold(foreground, imgContour, 240, 255, ThresholdType.Binary);
                Cv.SaveImage("dataset-dinosaur\\contour\\" + i.ToString("00") + ".jpg", imgContour);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            getContour();

            IplImage imgContour = new IplImage("dataset-dinosaur\\contour\\00.jpg", LoadMode.GrayScale);
            IplImage imgContourDraw = new IplImage(new CvSize(imgContour.Width, imgContour.Height),BitDepth.U8, 3);
            CvSeq<CvPoint> contourPointSeq;
            CvMemStorage memStore = new CvMemStorage(0);
            Cv.FindContours(imgContour, memStore, out contourPointSeq);
            CvContourScanner cs= Cv.StartFindContours(imgContour, memStore);
            CvPoint[] contourPointArray = null;

            for (int i = 0; i < 10; i++)
            {
                contourPointSeq = cs.FindNextContour();
                contourPointArray = new CvPoint[contourPointSeq.Total];
                Cv.CvtSeqToArray<CvPoint>(contourPointSeq, out contourPointArray);
                if (contourPointArray.Length > 100)
                {
                    for (int j = 0; j < contourPointArray.Length; j++)
                    {
                        Cv.DrawCircle(imgContourDraw, new CvPoint(contourPointArray[j].X, contourPointArray[j].Y), 10, Cv.RGB(255, 0, 0), 10);
                    }
                    StreamWriter sw = new StreamWriter("file.txt", false);


                    for (int k = 0; k < contourPointArray.Length; k++)
                    {
                        sw.Write(contourPointArray[k].X);
                        sw.Write(" ");
                        sw.Write(contourPointArray[k].Y);
                        sw.Write("\n");
                    }
                }

                Cv.SaveImage("save.jpg", imgContourDraw);
            }
            
            

            

                    


            int target_img_contour_num;
            int ref_img_contour_num;
            double[,] camPara1 = Read_camerpar_txt("Camera parameters.txt", "00.jpg");
            double[,] camPara2 = Read_camerpar_txt("Camera parameters.txt", "01.jpg");

            double[,] target_img_contour = Read_Contour_txt("目标图轮廓点.txt", "00.jpg", out target_img_contour_num);
            double[,] ref_img_contour = Read_Contour_txt("参考图轮廓点.txt", "01.jpg", out ref_img_contour_num);
            CvMat right_epiline_point = Computecorrespondepilines(camPara2, camPara1, target_img_contour);

            int n = 7;//共分为n个bin,每个bin弧度(max_radius-min_radius)/n
            List<double[]> intersection_point_list = Compute_epiline_contour_intersection(right_epiline_point, ref_img_contour, n);

            double[][] vertex_3d = new double[intersection_point_list.Count][];
            for(int i=0;i<intersection_point_list.Count;i++)
            {
                int temp = (int)intersection_point_list[i][0];
                //vertex_3d[i] = Cross_intersection_3D(
                //    intersection_point_list[i], 
                //    camPara2, 
                //    camPara1, 
                //    new CvPoint((int)(target_img_contour[0,temp]), (int)(target_img_contour[1,temp])), 
                //    i, 0, 0, 0, 0, 1, 0);

                //vertex_3d[i] = Cross_intersection_3D(
                //    intersection_point_list[i],
                //    camPara1,
                //    camPara2,
                //    new CvPoint((int)(target_img_contour[0, temp]), (int)(target_img_contour[1, temp])),
                //    i, 0, 0, 0, 0, 1, 0);

                vertex_3d[i] = Cross_intersection_3D_Direct_Solve(
                    intersection_point_list[i],
                    camPara1,
                    camPara2,
                    new CvPoint((int)(target_img_contour[0, temp]), (int)(target_img_contour[1, temp])),
                    i, 0, 0, 0, 0, 1, 0);
            }

            double[] face = new double[0];
            save_ply("ply0.ply", vertex_3d, face);

            IplImage img1 = Cv.LoadImage("01.jpg");
            //Cv.NamedWindow("win1");
            //Cv.ShowImage("win1",img1);
            //Cv.WaitKey(0);

            for (int i = 1; i < ref_img_contour.GetLength(1); i++)
            {
                Cv.DrawCircle(img1, new CvPoint((int)(ref_img_contour[0, i]), (int)(ref_img_contour[1, i])), 1, Cv.RGB(0, 255, 255));
            }

            //for (int i = 0; i < right_epiline_point.GetDimSize(1); i++)
            //{
            //    Cv.DrawLine(img1, new CvPoint((int)(right_epiline_point[0,i]), (int)(right_epiline_point[1, i])), new CvPoint((int)(right_epiline_point[2, i]), (int)(right_epiline_point[3, i])),Cv.RGB(0, 255, 0),1);
            //}

            for (int i = 0; i < intersection_point_list.Count; i++)
            {
                Cv.DrawCircle(img1, new CvPoint((int)(intersection_point_list[i][1]), (int)(intersection_point_list[i][2])), 2, Cv.RGB(255, 0, 0));
            }
            Cv.SaveImage("01_save.jpg", img1);

            IplImage img2 = Cv.LoadImage("00.jpg");
            for (int i = 1; i < target_img_contour.GetLength(1); i++)
            {
                Cv.DrawCircle(img2, new CvPoint((int)(target_img_contour[0, i]), (int)(target_img_contour[1, i])), 2, Cv.RGB(255, 0, 0));
            }
            Cv.SaveImage("00_save.jpg", img2);


            //for(int i=0;i<right_epiline_point.GetDimSize(1);i++)
            //{
            //    CvPoint epiline_start_point = new CvPoint(right_epiline_point[0,i],
            //                                                right_epiline_point[1,i]);
            //    CvPoint epiline_end_point =new CvPoint(right_epiline_point[2,i],
            //                                                right_epiline_point[3,i]);
            //    CvPoint contour_line_point = new CvPoint

            //    SL_Cross_Intersection(new CvPoint(,)


            //CvMat Epilines_point_mat = new CvMat(4, second_image_contour_point_number, MatrixType.F64C1);  
            //Epilines_point_mat = Computecorrespondepilines(first_projcet, second_projcet, second_Contour_point_temp);//求第二幅图中的所有轮廓点在第第一副图中的极线（一次性求出）
            
            //c3.X = (int)Epilines_point_mat[0, k5];
            //c3.Y = (int)Epilines_point_mat[1, k5];
           // c4.X = (int)Epilines_point_mat[2, k5];
            //c4.Y = (int)Epilines_point_mat[3, k5];

        }
       
    }
}
    



