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

            Cv.ComputeCorrespondEpilines(matA, 1, FundamentalMat, out correspondent_lines);//correspondent_lines的列表示点的个数，经过证明图像指数是2，将其极线映射到参考图

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
        private void save_ply(string filepath, double[][][] vertex3d, double[] face)
        {
            FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);

            int count = 0;

            for (int i = 0; i < vertex3d.Length; i++)
            {
                for (int j = 0; j < vertex3d[i].Length; j++)
                {
                    //if (vertex3d[i][j][10] == 1)
                    //{
                    //    count++;
                    //}

                    count++;
                }
            }

            string ply_header = "ply\nformat ascii 1.0\ncomment made by Road Liu\ncomment this file is a cube\nelement vertex " + count.ToString() + "\nproperty float x\n"
                                + "property float y\nproperty float z\nproperty   uint8 red\nproperty   uint8 green\nproperty   uint8 blue\nelement face " + face.Length.ToString() + "\nproperty list uchar int vertex_index\nend_header\n";

            sw.Write(ply_header);
            for (int i = 0; i < vertex3d.Length; i++)
            {
                for (int j = 0; j < vertex3d[i].Length; j++)
                {
                    //if (vertex3d[i][j][10] == 0)
                    //{
                    //    continue;
                    //}
                    sw.Write(vertex3d[i][j][1]);
                    sw.Write(" ");
                    sw.Write(vertex3d[i][j][2]);
                    sw.Write(" ");
                    sw.Write(vertex3d[i][j][3]);
                    sw.Write(" ");
                    sw.Write(vertex3d[i][j][5]);
                    sw.Write(" ");
                    sw.Write(vertex3d[i][j][6]);
                    sw.Write(" ");
                    sw.Write(vertex3d[i][j][7]);
                    sw.Write("\n");
                }
            }

            sw.Close();
            for (int i = 0; i < face.Length; i++)
            {
                //Todo 写入面片信息
            }
            
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
        private void getContourLine()
        {
            FileStream fs = new FileStream("dataset-dinosaur\\dinosaur-contour-my\\dinosaur-contour.txt",  FileMode.Truncate,FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            for (int nFile = 0; nFile < 8; nFile++)
            {
                IplImage imgContour = new IplImage("dataset-dinosaur\\contour\\"+nFile.ToString("00")+".jpg", LoadMode.GrayScale);
                
                CvSeq<CvPoint> contourPointSeq;
                CvMemStorage memStore = new CvMemStorage(0);
                //Cv.FindContours(imgContour, memStore, out contourPointSeq);
                CvContourScanner cs = Cv.StartFindContours(imgContour, memStore);
                CvPoint[] contourPointArray = null;
                IplImage imgContourDraw = new IplImage(new CvSize(imgContour.Width, imgContour.Height), BitDepth.U8, 3);
                imgContourDraw.Set(new CvScalar(255, 255, 255));
                for (int i = 0; i < 100; i++)
                {
                    contourPointSeq = cs.FindNextContour();
                    contourPointArray = new CvPoint[contourPointSeq.Total];
                    Cv.CvtSeqToArray<CvPoint>(contourPointSeq, out contourPointArray);
                    if (contourPointArray.Length > 10)
                    {
                        for (int j = 0; j < contourPointArray.Length-1; j++)
                        {
                            Cv.DrawLine(imgContourDraw, new CvPoint(contourPointArray[j].X, contourPointArray[j].Y),new CvPoint(contourPointArray[j+1].X, contourPointArray[j+1].Y), Cv.RGB(0, 0, 0), 2);
                            if (j == contourPointArray.Length - 1)
                            {
                                Cv.DrawLine(imgContourDraw, new CvPoint(contourPointArray[j].X, contourPointArray[j].Y), new CvPoint(contourPointArray[1].X, contourPointArray[1].Y), Cv.RGB(0, 0, 0), 2);
                            }
                        }
                        //sw = new StreamWriter("dataset-dinosaur\\dinosaur-contour-my\\dinosaur-contour.txt", true);

                        //sw.Write(nFile);
                        //sw.Write(" ");
                        //sw.Write(contourPointArray.Length);
                        //sw.Write("\n");
                        //for (int k = 0; k < contourPointArray.Length; k++)
                        //{
                        //    sw.Write(contourPointArray[k].X);
                        //    sw.Write(" ");
                        //    sw.Write(contourPointArray[k].Y);
                        //    sw.Write(" ");
                        //    //sw.Write("\n");
                        //}
                        //sw.Write("\n");
                        Cv.SaveImage("dataset-dinosaur\\contourline\\" + nFile.ToString("00") + ".jpg", imgContourDraw);
                    }
                }
            }
        }
        private double[][] getTwoImage3DPoint(int imgNum1,int imgNum2,string imgFormat)
        {
            //getContour();
            //getContourLine();

            int target_img_contour_num;
            int ref_img_contour_num;
            double[,] camPara1 = Read_camerpar_txt("Camera parameters.txt", imgNum1.ToString("00")+imgFormat);
            double[,] camPara2 = Read_camerpar_txt("Camera parameters.txt", imgNum2.ToString("00") + imgFormat);

            double[,] target_img_contour = Read_Contour_txt("dinosaur-contour.txt", imgNum1.ToString("00") + imgFormat, out target_img_contour_num);
            double[,] ref_img_contour = Read_Contour_txt("dinosaur-contour.txt", imgNum2.ToString("00") + imgFormat, out ref_img_contour_num);

            //IplImage imgContour = Cv.LoadImage("contour.jpg", LoadMode.Color);
            //imgContour.Zero();
            //for (int i = 0; i < target_img_contour.GetLength(1); i++)
            //{
            //    Cv.DrawCircle(imgContour,new CvPoint((int)target_img_contour[0,i],(int)target_img_contour[1,i]),2,Cv.RGB(255,0,0));
            //}
            //Cv.SaveImage("saveContour.jpg",imgContour);

            CvMat right_epiline_point = Computecorrespondepilines(camPara1, camPara2, target_img_contour);

            int n = 7;//共分为n个bin,每个bin弧度(max_radius-min_radius)/n
            List<double[]> intersection_point_list = Compute_epiline_contour_intersection(right_epiline_point, ref_img_contour, n);

            double[][] vertex_3d = new double[intersection_point_list.Count][];

            for (int i = 0; i < intersection_point_list.Count; i++)
            {
                int temp = (int)intersection_point_list[i][0];
                //vertex_3d[i] = Cross_intersection_3D(
                //    intersection_point_list[i], 
                //    camPara2, 
                //    camPara1, 
                //    new CvPoint((int)(target_img_contour[0,temp]), (int)(target_img_contour[1,temp])), 
                //    i, 0, 0, 0, 0, 1, 0);

                vertex_3d[i] = Cross_intersection_3D(
                    intersection_point_list[i],
                    camPara1,
                    camPara2,
                    new CvPoint((int)(target_img_contour[0, temp]), (int)(target_img_contour[1, temp])),
                    i, 0, 0, 0, 0, 1, 0);

                //vertex_3d[i] = Cross_intersection_3D_Direct_Solve(
                //    intersection_point_list[i],
                //    camPara1,
                //    camPara2,
                //    new CvPoint((int)(target_img_contour[0, temp]), (int)(target_img_contour[1, temp])),
                //    i, 0, 0, 0, 0, 1, 0);
            }

            double[] face = new double[0];

            //save_ply("ply0.ply", vertex_3d, face);

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


            return vertex_3d;
        }
        private void project2D(double[][][] all_3d_point)
        {
            //读取轮廓图片
            IplConvKernel ick = new IplConvKernel(7,7,4,4,ElementShape.Rect);
            IplImage[] img = new IplImage[8];
            for (int i = 0; i < img.Length; i++)
            {
                img[i] = Cv.LoadImage("dataset-dinosaur\\contour\\" + i.ToString("00") + ".jpg",LoadMode.Color);
                img[i].Erode(img[i],ick,2);
            }

            //读取相机参数
            double[][,] camPara = new double[8][,];
            CvMat[] camPara_mat = new CvMat[8];
            for (int i = 0; i < 8; i++)
            {
                camPara[i] = Read_camerpar_txt("Camera parameters.txt", i.ToString("00") + ".jpg");
                camPara_mat[i] = new CvMat(3, 4, MatrixType.F64C1,camPara[i]);
            }


            //将三维点云投影到各个图像上
            int flag = 1;
            CvMat point_3d_mat = new CvMat(4, 1, MatrixType.F64C1);
            CvMat point_2d_mat = new CvMat(3, 1, MatrixType.F64C1);
            
            for (int i = 0; i < all_3d_point.Length; i++)
            {
                for (int j = 0; j < all_3d_point[i].Length; j++)
                {
                    double[] point_3d = new double[4] { all_3d_point[i][j][1], all_3d_point[i][j][2], all_3d_point[i][j][3] ,1};
                    point_3d_mat[0,0] = point_3d[0];
                    point_3d_mat[1,0] = point_3d[1];
                    point_3d_mat[2,0] = point_3d[2];
                    point_3d_mat[3,0] = point_3d[3];
                    all_3d_point[i][j][10] = 1;
      
                    for (int k = 0; k < 2; k++)
                    {
                        point_2d_mat = camPara_mat[1-k]*point_3d_mat;
                        
                        point_2d_mat[0, 0] = point_2d_mat[0, 0] / point_2d_mat[2, 0];
                        point_2d_mat[1, 0] = point_2d_mat[1, 0] / point_2d_mat[2, 0];

                        if (point_2d_mat[0,0] <= 0 || point_2d_mat[0, 0] > img[k].Width || point_2d_mat[1, 0] > img[k].Height || point_2d_mat[1,0]<=0)
                        {
                            all_3d_point[i][j][10] = 0;              
                            continue;                           
                        }

                        else if(img[k][(int)(point_2d_mat[1, 0]),(int)(point_2d_mat[0, 0])].Val0 > 128)//如果>-1画出所有的点，如果>128画出在轮廓之外的点
                        {
                            flag =(int)img[k][(int)(point_2d_mat[1, 0]), (int)(point_2d_mat[0, 0])].Val0;
                            CvScalar tempScalar = img[k][(int)(point_2d_mat[1, 0]), (int)(point_2d_mat[0, 0])].Val0;
                            all_3d_point[i][j][10] = 0;
                            Cv.DrawCircle(img[k], new CvPoint((int)(point_2d_mat[0, 0]), (int)(point_2d_mat[1, 0])), 4, Cv.RGB(255, 0, 0));
                        }
                    }


                }
            }

            for (int i = 0; i < 8; i++)
            {
                Cv.SaveImage("dataset-dinosaur\\pointProject\\" + i.ToString("00") + ".jpg", img[i]);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int nCamera = 8;
            double[][][] all_3d_point= new double[56][][];
            int index = 0;
            for (int i = 0; i < nCamera; i++)
            {
                for (int j = 0; j < nCamera; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    //获得三维点
                    all_3d_point[index] = getTwoImage3DPoint(0, 1, ".jpg");
                    index++;
                }
            }

            project2D(all_3d_point);

            IplImage imgTemp = Cv.CreateImage(new CvSize(2000, 1500), BitDepth.U8, 3);
            imgTemp.SetZero();
            double[,] camParaTemp = Read_camerpar_txt("Camera parameters.txt", "00.jpg");
            CvMat camParaTemp_mat = new CvMat(3, 4, MatrixType.F64C1,camParaTemp);
            CvMat point3dTemp = new CvMat(4, 1, MatrixType.F64C1);
            CvMat point2dTemp = new CvMat(3, 1, MatrixType.F64C1);
            for (int i = 0; i < all_3d_point[0].Length; i++)
            {
                point3dTemp[0, 0] = all_3d_point[0][i][1];
                point3dTemp[1, 0] = all_3d_point[0][i][2];
                point3dTemp[2, 0] = all_3d_point[0][i][3];
                point3dTemp[3, 0] = 1;
                point2dTemp = camParaTemp_mat * point3dTemp;
                point2dTemp[0, 0] = point2dTemp[0, 0] / point2dTemp[2, 0];
                point2dTemp[1, 0] = point2dTemp[1, 0] / point2dTemp[2, 0];

                Cv.DrawCircle(imgTemp, new CvPoint((int)point2dTemp[0, 0], (int)point2dTemp[1, 0]), 4, Cv.RGB(255, 0, 0));
            }
            
            Cv.SaveImage("temp.jpg", imgTemp);

            double[] face = new double[0];
            save_ply("ply0.ply", all_3d_point, face);
            //将这些点保存
        
        }

        private void readContour(string path,out double[][,] contourPoint,out double[][,] contourLine,int nCamera,string imgFormat)
        {
            contourPoint = new double[nCamera][,];
            contourLine = new double[nCamera][,];
            int contourPointNum = 0;
            double xa, ya, xb, yb;
            double A, B, C;

            for (int i = 0; i < nCamera; i++)
            {
                readContourPointOneImage(path, i.ToString("00") + imgFormat, out contourPoint[i]);
                
                //将轮廓点连成轮廓线
                contourPointNum = contourPoint[i].GetLength(0);
                contourLine[i] = new double[contourPointNum, 7];
                for (int j = 0; j < contourPointNum; j++)
                {
                    if (j == 0)
                    {
                        xa = contourPoint[i][contourPointNum - 1, 0];
                        ya = contourPoint[i][contourPointNum - 1, 1];
                        xb = contourPoint[i][0, 0];
                        yb = contourPoint[i][0, 1];
                    }
                    else
                    {
                        xa = contourPoint[i][j-1, 0];
                        ya = contourPoint[i][j-1, 1];
                        xb = contourPoint[i][j, 0];
                        yb = contourPoint[i][j, 1];
                    }
                    A = yb - ya; B = xa - xb; C = xb * ya - xa * yb;
                    contourLine[i][j, 0] = A;
                    contourLine[i][j, 1] = B;
                    contourLine[i][j, 2] = C;
                    contourLine[i][j, 3] = xa;
                    contourLine[i][j, 4] = ya;
                    contourLine[i][j, 5] = xb;
                    contourLine[i][j, 6] = yb;

                }
            }
        }
        private void readContourPointOneImage(string path, string fileName, out double[,] contourPointOneImage)
        {
            FileStream fs = new FileStream(path, FileMode.Open,FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string fileContentStr = sr.ReadToEnd();
            string[] fileContentStrArr = fileContentStr.Split(new char[] { '\n', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int contourPointNum = 0;
            int firstContourPointIndex = 0;
            for (int i = 0; i < fileContentStrArr.Length; i++)
            {
                if (fileContentStrArr[i] == fileName)
                {
                    contourPointNum = Convert.ToInt32(fileContentStrArr[i + 1]);
                    firstContourPointIndex = i + 2;
                    break;
                }
            }

            contourPointOneImage = new double[contourPointNum, 2];
            for (int j = 0; j < contourPointNum; j++)
            {
                for (int k = 0; k < 2; k++)
                {
                    contourPointOneImage[j, k] = Convert.ToDouble(fileContentStrArr[firstContourPointIndex + 2 * j + k]);
                }
            }

        }
        private void readCamPara(string path, out double[][,] camPara,int nCamera, string imgFormat)
        {
            camPara = new double[nCamera][,];
            FileStream fs = new FileStream(path,FileMode.Open,FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string fileContent = sr.ReadToEnd();
            string[] fileContentSplit = fileContent.Split(new char[] { '\n', '\r', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int nthImgNamePos=0;
            for (int i = 0; i < nCamera; i++)
            {
                camPara[i] = new double[3, 4];
                nthImgNamePos = 13*i;
                
                if (fileContentSplit[nthImgNamePos] == i.ToString("00") + imgFormat)
                {
                    for (int j = 0; j < 12; j++)
                    {
                        camPara[i][j / 4, j % 4] = Convert.ToDouble(fileContentSplit[nthImgNamePos + 1 + j]);
                    }
                }
            }
        }
        private void cacuFundMatTwoView(double[,] project1, double[,] project2, out double[,] fundMatOne)
        {
            fundMatOne = new double[3, 3];
            CvMat fundMatOneMat = new CvMat(3, 3, MatrixType.F64C1);
            CvMat projectMat1 = new CvMat(3, 4, MatrixType.F64C1,project1);
            CvMat projectMat2 = new CvMat(3, 4, MatrixType.F64C1,project2);
            CvMat P11 = projectMat1.GetCols(0, 3);
            CvMat P21 = projectMat2.GetCols(0, 3);
            CvMat p1 = projectMat1.GetCols(3, 4);
            CvMat p2 = projectMat2.GetCols(3, 4);
            CvMat P11Inv = new CvMat(3,3,MatrixType.F64C1);
            CvMat P21Inv = new CvMat(3,3,MatrixType.F64C1);
            CvMat p = new CvMat(3,1,MatrixType.F64C1);
            P11.Inv(P11Inv);
            P21.Inv(P21Inv);

            p = p2 - P21*P11Inv*p1;
            double[,] pSkewSymetricArray = new double[3, 3]{{0,       -p[2,0], p[1,0]},
                                                            {p[2,0],  0,      -p[0,0]},
                                                            {-p[1,0], p[0,0], 0}};
            CvMat pSkewSymetric = new CvMat(3, 3, MatrixType.F64C1, pSkewSymetricArray);
            fundMatOneMat = pSkewSymetric * P21 * P11Inv;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    fundMatOne[i, j] = fundMatOneMat[i, j];
                }
            }
        }
        private void cacuFundMat(double[][,] camPara, out double[,][,] fundMat)
        {
            int nCamera = camPara.GetLength(0);
            fundMat = new double[nCamera, nCamera][,];
            for (int i = 0; i < nCamera; i++)
            {
                for (int j = 0; j < nCamera; j++)
                {
                    if (i == j)
                    {
                        fundMat[i,j] = null;
                        continue;
                    }
                    else
                    {
                        cacuFundMatTwoView(camPara[i], camPara[j], out fundMat[i,j]);
                    }
                }
            }
        }
        private void getImgContour(string imgDir, out string contourDir,int nCamera)
        {
            contourDir = Path.GetDirectoryName(Path.GetFullPath(imgDir)) + "\\contour";
            if (!Directory.Exists(contourDir))
            {
                Directory.CreateDirectory(contourDir);
            }
            for (int i = 0; i < nCamera; i++)
            {
                IplImage foreground = new IplImage(imgDir+"\\"+i.ToString("00")+".jpg",LoadMode.GrayScale);
                IplImage imgContour = Cv.Clone(foreground);
                Cv.Threshold(foreground, imgContour, 240, 255, ThresholdType.Binary);
                Cv.SaveImage(contourDir + "\\"+ i.ToString("00") + ".jpg", imgContour);
            }
        }
        private void getImgContourLine(string contourDir, out string contourLineDir, int nCamera)
        {
            contourLineDir = Path.GetDirectoryName(Path.GetFullPath(contourDir)) + "\\contourLine";
            //FileStream fs = new FileStream("dataset-dinosaur\\dinosaur-contour-my\\dinosaur-contour.txt", FileMode.Truncate, FileAccess.Write);
            //StreamWriter sw = new StreamWriter(fs);
            if (!Directory.Exists(contourLineDir))
            {
                Directory.CreateDirectory(contourLineDir);
            }

            for (int nFile = 0; nFile < nCamera; nFile++)
            {
                IplImage imgContour = new IplImage(contourDir+"\\" + nFile.ToString("00") + ".jpg", LoadMode.GrayScale);

                CvSeq<CvPoint> contourPointSeq;
                CvMemStorage memStore = new CvMemStorage(0);
                //Cv.FindContours(imgContour, memStore, out contourPointSeq);
                CvContourScanner cs = Cv.StartFindContours(imgContour, memStore);
                CvPoint[] contourPointArray = null;
                IplImage imgContourDraw = new IplImage(new CvSize(imgContour.Width, imgContour.Height), BitDepth.U8, 3);
                imgContourDraw.Set(new CvScalar(255, 255, 255));
                for (int i = 0; i < 100; i++)
                {
                    contourPointSeq = cs.FindNextContour();
                    contourPointArray = new CvPoint[contourPointSeq.Total];
                    Cv.CvtSeqToArray<CvPoint>(contourPointSeq, out contourPointArray);
                    if (contourPointArray.Length > 10)
                    {
                        for (int j = 0; j < contourPointArray.Length - 1; j++)
                        {
                            Cv.DrawLine(imgContourDraw, new CvPoint(contourPointArray[j].X, contourPointArray[j].Y), new CvPoint(contourPointArray[j + 1].X, contourPointArray[j + 1].Y), Cv.RGB(0, 0, 0), 2);
                            if (j == contourPointArray.Length - 1)
                            {
                                Cv.DrawLine(imgContourDraw, new CvPoint(contourPointArray[j].X, contourPointArray[j].Y), new CvPoint(contourPointArray[1].X, contourPointArray[1].Y), Cv.RGB(0, 0, 0), 2);
                            }
                        }
                        //sw = new StreamWriter("dataset-dinosaur\\dinosaur-contour-my\\dinosaur-contour.txt", true);

                        //sw.Write(nFile);
                        //sw.Write(" ");
                        //sw.Write(contourPointArray.Length);
                        //sw.Write("\n");
                        //for (int k = 0; k < contourPointArray.Length; k++)
                        //{
                        //    sw.Write(contourPointArray[k].X);
                        //    sw.Write(" ");
                        //    sw.Write(contourPointArray[k].Y);
                        //    sw.Write(" ");
                        //    //sw.Write("\n");
                        //}
                        //sw.Write("\n");
                        Cv.SaveImage(contourLineDir + "\\" + nFile.ToString("00") + ".jpg", imgContourDraw);
                    }
                }
            }
        }
        private void getContourPoint(string contourDir, out double[][,] contourPoint, int nCamera,string imgFormat)
        {
            contourPoint = new double[nCamera][,];
            List<CvPoint> list = new List<CvPoint>();
            CvScalar black = new CvScalar(0,0,0,0);
            IplImage img = null;
            for (int i = 0; i < nCamera; i++)
            {
                list.Clear();
                img = Cv.LoadImage(contourDir + "\\" + i.ToString("00") + imgFormat, LoadMode.GrayScale);
                for(int j=0;j<img.Height;j++)
                {
                    for(int k=0;k<img.Width;k++)
                    {
                        if(img[j,k].Equals(black))
                        {
                            list.Add(new CvPoint(j,k));
                        }
                    }
                }
                contourPoint[i] = new double[list.Count,2];
                for(int j=0;j<list.Count;j++)
                {
                    contourPoint[i][j,0] = list[j].X;
                    contourPoint[i][j,1] = list[j].Y;
                }
                
            }
        }
        private void getEpiline(double [][,] contourPoint,double[,][,] fundMat, out double[,][][] epiline)
        {
            epiline = new double[fundMat.GetLength(0), fundMat.GetLength(1)][][];
            for(int i=0;i<fundMat.GetLength(0);i++)
            {
                for (int j = 0; j < fundMat.GetLength(1);j++ )
                {
                    if(i==j)
                    {
                        continue;
                    }
                    else
                    {
                        getEpilineMatMul(contourPoint[i],fundMat[i,j],out epiline[i,j]);
                    }
                }
            }
        }
        private void getEpilineMatMul(double[,] contourPointOne, double[,] fundMatOne,out double[][] epilineOneView)
        {
            CvMat contourPointOneMat = new CvMat(contourPointOne.GetLength(0),contourPointOne.GetLength(1),MatrixType.F64C1,contourPointOne);
            CvMat fundMatOneMat = new CvMat(3,3,MatrixType.F64C1,fundMatOne);
            contourPointOneMat.Transpose();

            CvMat epilineOneViewMat;
            Cv.ComputeCorrespondEpilines(contourPointOneMat,1,fundMatOneMat,out epilineOneViewMat);
            epilineOneView = new double[contourPointOne.GetLength(0)][];
            for(int i=0;i<contourPointOne.GetLength(0);i++)
            {
                epilineOneView[i] = new double[3];
                for(int j=0;j<3;j++)
                {
                    epilineOneView[i][j] = epilineOneViewMat[j, i];
                }
            }
            
        }
        private void getEpilineContourlineIntersection(double[,][][] epiline,double[][,] contourLine, out double[,][][] epiConIntersection)
        {
            epiConIntersection = new double[epiline.GetLength(0), epiline.GetLength(1)][][];

            for (int i = 0; i < epiline.GetLength(0); i++)
            {
                for (int j = 0; j < epiline.GetLength(1); j++)
                {
                    epiConIntersection[i, j] = new double[epiline[i, j].GetLength(0)][];
                    if(i==j)
                    {
                        continue;
                    }
                    for (int k = 0; k < epiline[i, j].GetLength(0); k++)
                    {
                        getEpiConInter(epiline[i,j][k],contourLine[j],out epiConIntersection[i,j][k]);
                    }
                }
            }
        }

        private void getEpiConInter(double[] epilineOne, double[,] contourLineOneImage, out double[] intersection)
        {
            CvMat epilineOneMat = new CvMat(3, 1, MatrixType.F64C1);
            CvMat contourLineOneMat = new CvMat(3, 1, MatrixType.F64C1);
            CvMat intersectionMat = new CvMat(3, 1, MatrixType.F64C1);
            List<double[]> intersectionList = new List<double[]>();
            double[] intersectionPoint = new double[2];
            double[] seg = new double[4];
            for (int i = 0; i < contourLineOneImage.GetLength(0); i++)
            {
                epilineOneMat[0, 0] = epilineOne[0];
                epilineOneMat[1, 0] = epilineOne[1];
                epilineOneMat[2, 0] = epilineOne[2];
                contourLineOneMat[0, 0] = contourLineOneImage[i, 0];
                contourLineOneMat[1, 0] = contourLineOneImage[i, 1];
                contourLineOneMat[2, 0] = contourLineOneImage[i, 2];
                Cv.CrossProduct(epilineOneMat, contourLineOneMat, intersectionMat);

                intersectionPoint[0] = intersectionMat[0, 0] / intersectionMat[2, 0];
                intersectionPoint[1] = intersectionMat[1, 0] / intersectionMat[2, 0];
                for(int j=0;j<4;j++)
                {
                    seg[i] = contourLineOneImage[i,j+3];
                }
                if (isPointOnSeg(intersectionPoint, seg))
                {
                    intersectionList.Add(intersectionPoint);
                }
            }

            intersection = new double[2 * intersectionList.Count];
            for (int i = 0; i < intersectionList.Count; i++)
            {
                intersection[2 * i] = intersectionList[i][0];
                intersection[2 * i + 1] = intersectionList[i][1];
            }
        }

        private bool isPointOnSeg(double[] intersectionPoint, double[] seg)
        {
            bool isIntersect = false;
            if(intersectionPoint[0]>=Math.Min(seg[0],seg[2])
                &&intersectionPoint[0]<=Math.Max(seg[0],seg[2])
                &&intersectionPoint[1]>=Math.Min(seg[1],seg[3])
                &&intersectionPoint[1]<=Math.Max(seg[1],seg[3]))
            {
                isIntersect = true;
            }

            return isIntersect;
        }

        private void NewRun_Click(object sender, EventArgs e)
        {
            const int nCamera = 2;
            const string imgFormat = ".jpg";
            //------------------------输入处理--------------------------
            //对输入多视图图像进行背景差分二值化
            //----对二值化图像求解获得图像外轮廓
            //in1:二值化的图像文件夹路径
            //in2:nCamera
            //in3:imgPath eg.".jpg"
            //out:轮廓点txt文件【图像文件名\n轮廓点数\nx1 y1 x2 y2....】
            //----------------------------------
            //string contourDir;
            //string contourLineDir;
            //getImgContour(".\\dataset-dinosaur\\dinosaur-images", out contourDir, nCamera);
            //getImgContourLine(contourDir, out contourLineDir, nCamera);

            double[][,] contourPoint;
            getContourPoint(".\\dataset-dinosaur\\contour", out contourPoint, nCamera, imgFormat);
            //----读取轮廓线点
            //in:轮廓点文件夹路径
            //out1:轮廓点【n个图像|每个图像m个轮廓点，m不一样】
            //out2:轮廓线【n个图像|每个图像m个轮廓点，m不一样|每个轮廓点对应一条轮廓线，每条轮廓线
            //                                      格式x1 y1 x2 y2 A B C 分组】
            //----------------------------------
            double[][,] contourLinePoint;
            double[][,] contourLine;
            readContour(".\\dinosaur-contour.txt", out contourLinePoint,out contourLine, nCamera, ".jpg");
            
            //----用输入参数矩阵求解基础矩阵和极点
            //in:相机参数矩阵
            //out1:基础矩阵【nCamera*(nCamera-1)个基础矩阵|每个基础矩阵3*3】
            //out2:极点【nCamera个极点|每个极点2*1】
            //-----------------------------------
            double[][,] camPara;
            double[,][,] fundMat;
            readCamPara(".\\Camera parameters.txt", out camPara,nCamera,".jpg");
            cacuFundMat(camPara, out fundMat);

            //------------------------求两幅图像的对应点----------------
            //----求点对应极线
            //in:原图像轮廓点
            //out:轮廓点对应极线【nCamera*(nCamera-1)|m个轮廓点，每个轮廓点对应一条极线|每条极线 A B C】
            //----------------
            double[,][][] epiline;
            getEpiline(contourPoint, fundMat, out epiline);

            
            //----求极线与轮廓线的交点
            //in:极线【nCamera*(nCamera-1)|m个轮廓点，每个轮廓点对应一条极线|每条极线 A B C】
            //out:极线与轮廓线的交点【nCamera个视图
            //                        每个视图上有m个轮廓点，m各不相同
            //                        每个轮廓点在另外(nCamera-1)个图上有对应点，不确定有多少个，但应为偶数|n个点对(x1 y1 x2 y2) （x3 y3 x4 y4)... 】
            //------------------------
            double[,][][] epiConIntersection;
            getEpilineContourlineIntersection(epiline, contourLine, out epiConIntersection);


            //------------------------模型生成--------------------------
            //----求三维点
            //in：极线与轮廓线的交点【nCamera个视图|
            //                        每个视图上有m个轮廓点，m各不相同|
            //                        每个轮廓点在另外(nCamera-1)个图上有对应点，不确定有多少个，但应为偶数|n个点对(x1 y1 x2 y2) （x3 y3 x4 y4)... 】
            //out:三维交点【nCamera个视图|
            //              每个视图上有m个轮廓点，m各不相同|
            //              每个轮廓点在另外(nCamera-1)个图上有对应点，不确定有多少个,但应为偶数|n个三维点对（x1 y1 z1  x2 y2 z2) (x3 y3 z3 x4 y4 z4)...】
            //-----------

            //剪裁
            //in:三维交点【nCamera个视图|
            //              每个视图上有m个轮廓点，m各不相同|
            //              每个轮廓点在另外(nCamera-1)个图上有对应点，不确定有多少个,但应为偶数|n个三维点对（x1 y1 z1  x2 y2 z2) (x3 y3 z3 x4 y4 z4)...】
            //process:求每个视图上每个点的射线上三维交点到相机中心的距离：
            //三维点到光心距离【nCamera个视图|
            //              每个视图上有m个轮廓点，m各不相同|
            //              每个轮廓点在另外(nCamera-1)个图上有对应三维点，不确定有多少个,但应为偶数|n个距离-序号对（dist1 ind1 flag1) (dist2 ind2 flag2)...】
            //排序后的距离【nCamera个视图|
            //              每个视图上有m个轮廓点，m各不相同|
            //              每个轮廓点在另外(nCamera-1)个图上有对应三维点，不确定有多少个,但应为偶数|根据dist排序后的n个距离（dist1 ind1 flag1) (dist2 ind2 flag2)...】
            //间隔【nCamera个视图|
            //              每个视图上有m个轮廓点，m各不相同|
            //              每个轮廓点在另外(nCamera-1)个图上有对应三维点，不确定有多少个,但应为偶数|根据dist排序后的n个距离（dist1 ind1 flag1) (dist2 ind2 flag2)...】
        
        }
    }
}
    



