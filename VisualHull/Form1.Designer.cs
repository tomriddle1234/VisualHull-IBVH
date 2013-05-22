using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Emgu.CV;//PS:调用的Emgu dll   
//using Emgu.CV.Structure;
//using Emgu.Util;
using System.Threading;

namespace WindowsFormsApplication3
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
       
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        /// //private System.ComponentModel.IContainer components = null;
        
        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.button1 = new System.Windows.Forms.Button();
            this.NewRun = new System.Windows.Forms.Button();
            this.segsIntersection = new System.Windows.Forms.Button();
            this.epiConInter = new System.Windows.Forms.Button();
            this.drawConLine = new System.Windows.Forms.Button();
            this.p2dto3d = new System.Windows.Forms.Button();
            this.targetContourPoint = new System.Windows.Forms.Button();
            this.epiConInterBins = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            this.fileSystemWatcher1.Path = "C:\\";
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(323, 51);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "极线求交程序";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // NewRun
            // 
            this.NewRun.Location = new System.Drawing.Point(63, 157);
            this.NewRun.Name = "NewRun";
            this.NewRun.Size = new System.Drawing.Size(133, 23);
            this.NewRun.TabIndex = 0;
            this.NewRun.Text = "目标图用外轮廓线";
            this.NewRun.UseVisualStyleBackColor = true;
            this.NewRun.Click += new System.EventHandler(this.NewRun_Click);
            // 
            // segsIntersection
            // 
            this.segsIntersection.Location = new System.Drawing.Point(344, 157);
            this.segsIntersection.Name = "segsIntersection";
            this.segsIntersection.Size = new System.Drawing.Size(75, 23);
            this.segsIntersection.TabIndex = 2;
            this.segsIntersection.Text = "segsIntersection";
            this.segsIntersection.UseVisualStyleBackColor = true;
            this.segsIntersection.Click += new System.EventHandler(this.segsIntersection_Click);
            // 
            // epiConInter
            // 
            this.epiConInter.Location = new System.Drawing.Point(343, 221);
            this.epiConInter.Name = "epiConInter";
            this.epiConInter.Size = new System.Drawing.Size(75, 23);
            this.epiConInter.TabIndex = 3;
            this.epiConInter.Text = "epiConInter";
            this.epiConInter.UseVisualStyleBackColor = true;
            this.epiConInter.Click += new System.EventHandler(this.epiConInter_Click);
            // 
            // drawConLine
            // 
            this.drawConLine.Location = new System.Drawing.Point(345, 271);
            this.drawConLine.Name = "drawConLine";
            this.drawConLine.Size = new System.Drawing.Size(75, 23);
            this.drawConLine.TabIndex = 4;
            this.drawConLine.Text = "drawConLine";
            this.drawConLine.UseVisualStyleBackColor = true;
            this.drawConLine.Click += new System.EventHandler(this.drawConLine_Click);
            // 
            // p2dto3d
            // 
            this.p2dto3d.Location = new System.Drawing.Point(343, 325);
            this.p2dto3d.Name = "p2dto3d";
            this.p2dto3d.Size = new System.Drawing.Size(75, 23);
            this.p2dto3d.TabIndex = 5;
            this.p2dto3d.Text = "2dto3d";
            this.p2dto3d.UseVisualStyleBackColor = true;
            this.p2dto3d.Click += new System.EventHandler(this.p2dto3d_Click);
            // 
            // targetContourPoint
            // 
            this.targetContourPoint.Location = new System.Drawing.Point(63, 221);
            this.targetContourPoint.Name = "targetContourPoint";
            this.targetContourPoint.Size = new System.Drawing.Size(133, 23);
            this.targetContourPoint.TabIndex = 6;
            this.targetContourPoint.Text = "目标图用轮廓";
            this.targetContourPoint.UseVisualStyleBackColor = true;
            this.targetContourPoint.Click += new System.EventHandler(this.targetContourPoint_Click);
            // 
            // epiConInterBins
            // 
            this.epiConInterBins.Location = new System.Drawing.Point(63, 271);
            this.epiConInterBins.Name = "epiConInterBins";
            this.epiConInterBins.Size = new System.Drawing.Size(75, 23);
            this.epiConInterBins.TabIndex = 7;
            this.epiConInterBins.Text = "epiConInterBins";
            this.epiConInterBins.UseVisualStyleBackColor = true;
            this.epiConInterBins.Click += new System.EventHandler(this.epiConInterBins_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 429);
            this.Controls.Add(this.epiConInterBins);
            this.Controls.Add(this.targetContourPoint);
            this.Controls.Add(this.p2dto3d);
            this.Controls.Add(this.drawConLine);
            this.Controls.Add(this.epiConInter);
            this.Controls.Add(this.segsIntersection);
            this.Controls.Add(this.NewRun);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);

        }

        

       

        #endregion

        private IContainer components;
        public System.IO.FileSystemWatcher fileSystemWatcher1;
        private Button button1;
        private Button NewRun;
        private Button segsIntersection;
        private Button epiConInter;
        private Button drawConLine;
        private Button p2dto3d;
        private Button targetContourPoint;
        private Button epiConInterBins;
    }
}

