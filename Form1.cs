using System;
using System.Drawing;
using System.Collections;
//using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
//using System.Data;

namespace MazeAdvanced
{
	/// <summary>
	/// Form1 的摘要说明。
	/// </summary>
	public class MazeAdvanced : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.PictureBox picM;
		private System.Windows.Forms.TextBox txtX;
		private System.Windows.Forms.TextBox txtY;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox Chk1;
		private System.Windows.Forms.CheckBox Chk2;
		private System.Windows.Forms.Timer Timer;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button3;
		private System.ComponentModel.IContainer components;

		public MazeAdvanced()
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();

			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
		}

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows 窗体设计器生成的代码
		/// <summary>
		/// 设计器支持所需的方法 - 不要使用代码编辑器修改
		/// 此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.picM = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.txtX = new System.Windows.Forms.TextBox();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Chk1 = new System.Windows.Forms.CheckBox();
            this.Chk2 = new System.Windows.Forms.CheckBox();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picM)).BeginInit();
            this.SuspendLayout();
            // 
            // picM
            // 
            this.picM.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picM.Location = new System.Drawing.Point(8, 40);
            this.picM.Name = "picM";
            this.picM.Size = new System.Drawing.Size(461, 461);
            this.picM.TabIndex = 0;
            this.picM.TabStop = false;
            this.picM.Paint += new System.Windows.Forms.PaintEventHandler(this.picM_Paint);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(80, 8);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(40, 24);
            this.button1.TabIndex = 2;
            this.button1.Text = "生成";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(16, 10);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(24, 21);
            this.txtX.TabIndex = 0;
            this.txtX.Text = "25";
            this.txtX.Enter += new System.EventHandler(this.txtX_Enter);
            this.txtX.Leave += new System.EventHandler(this.txtX_Leave);
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(48, 10);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(24, 21);
            this.txtY.TabIndex = 1;
            this.txtY.Text = "25";
            this.txtY.Enter += new System.EventHandler(this.txtY_Enter);
            this.txtY.Leave += new System.EventHandler(this.txtY_Leave);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(40, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(8, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "X";
            // 
            // Chk1
            // 
            this.Chk1.Appearance = System.Windows.Forms.Appearance.Button;
            this.Chk1.Location = new System.Drawing.Point(121, 8);
            this.Chk1.Name = "Chk1";
            this.Chk1.Size = new System.Drawing.Size(40, 24);
            this.Chk1.TabIndex = 3;
            this.Chk1.TabStop = false;
            this.Chk1.Text = "答案";
            this.Chk1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Chk1.CheckedChanged += new System.EventHandler(this.Chk1_CheckedChanged);
            // 
            // Chk2
            // 
            this.Chk2.Appearance = System.Windows.Forms.Appearance.Button;
            this.Chk2.Location = new System.Drawing.Point(162, 8);
            this.Chk2.Name = "Chk2";
            this.Chk2.Size = new System.Drawing.Size(40, 24);
            this.Chk2.TabIndex = 4;
            this.Chk2.TabStop = false;
            this.Chk2.Text = "开始";
            this.Chk2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Chk2.CheckedChanged += new System.EventHandler(this.Chk2_CheckedChanged);
            // 
            // Timer
            // 
            this.Timer.Interval = 83;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(203, 8);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(40, 24);
            this.button2.TabIndex = 7;
            this.button2.Text = "三维";
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(244, 8);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(40, 24);
            this.button3.TabIndex = 8;
            this.button3.Text = "帮助";
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // MazeAdvanced
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
            this.ClientSize = new System.Drawing.Size(476, 506);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Chk2);
            this.Controls.Add(this.Chk1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.picM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(260, 220);
            this.Name = "MazeAdvanced";
            this.Text = "三维迷宫";
            this.Load += new System.EventHandler(this.Test_MazeAdvanced_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Key_Down);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Key_Up);
            ((System.ComponentModel.ISupportInitialize)(this.picM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

		/// <summary>
		/// 应用程序的主入口点。
		/// </summary>
		[STAThread]
		static void Main() 
		{
		//	Application.Run(new Test_MazeAdvanced());
			MazeAdvanced frm=new MazeAdvanced();
			frm.Show();
			while(frm.Created)
				Application.DoEvents();
		}

		Maze M;
		bool Solved;
		bool Gened=false;
		Stack S;
		bool MoveUp=false;
		bool MoveDown=false;
		bool MoveLeft=false;
		bool MoveRight=false;

		private void button1_Click(object sender, System.EventArgs e)
		{
			M.Generate(Convert.ToInt16(txtX.Text),Convert.ToInt16(txtY.Text));
			this.Solved=false;
			this.Chk1.Checked=false;
			this.Chk2.Checked=false;
			if(M.get_Width()>M.get_Height())
			{
				M.GridSize=(ushort)(505/M.get_Width());
			}
			else
			{
				M.GridSize=(ushort)(505/M.get_Height());
			}
			if(M.GridSize>54)
				M.GridSize=54;
			picM.Width=M.get_Width()*M.GridSize+2;
			picM.Height=M.get_Height()*M.GridSize+2;
			this.Width=M.get_Width()*M.GridSize+28;
			this.Height=M.get_Height()*M.GridSize+84;
			M.Draw(picM.CreateGraphics());
			M.DrawPlayer(picM.CreateGraphics());
			picM.Width=M.get_Width()*M.GridSize+2;
			picM.Height=M.get_Height()*M.GridSize+2;
			Gened=true;
			GC.Collect();
		}
		private void txtX_Leave(object sender, System.EventArgs e)
		{
			int X;
			try
			{
				X=Convert.ToInt16(txtX.Text);
			}
			catch(Exception)
			{
				MessageBox.Show("请输入一个5到150之间的数字!","错误",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				txtX.Text="";
				txtX.Focus();
				return;
			}
			if(X<5 || X>150)
			{
				MessageBox.Show("请输入一个5到150之间的数字!","错误",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				txtX.Text="";
				txtX.Focus();
			}
		}

		private void txtY_Leave(object sender, System.EventArgs e)
		{
			int Y;
			try
			{
				Y=Convert.ToInt16(txtY.Text);
			}
			catch(Exception)
			{
				MessageBox.Show("请输入一个5到150之间的数字!","错误",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				txtY.Text="";
				txtY.Focus();
				return;
			}
			if(Y<5 || Y>150)
			{
				MessageBox.Show("请输入一个5到150之间的数字!","错误",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
				txtY.Text="";
				txtY.Focus();
			}
		}
		private void Test_MazeAdvanced_Load(object sender, System.EventArgs e)
		{
			M=new Maze();
			try
			{
				M.WallBrush=new TextureBrush(new Bitmap(new FileStream("..\\..\\WallBrush.bmp",FileMode.Open,FileAccess.Read,FileShare.Read)));
			}
			catch(Exception)
			{
			}
			try
			{
				M.WallBrush=new TextureBrush(new Bitmap(new FileStream(".\\WallBrush.bmp",FileMode.Open,FileAccess.Read,FileShare.Read)));
			}
			catch(Exception)
			{
			}
			this.button1_Click(this,new EventArgs());
		}

		private void Key_Down(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.W:
					this.MoveUp=true;
					break;
				case Keys.S:
					this.MoveDown=true;
					break;
				case Keys.A:
					this.MoveLeft=true;
					break;
				case Keys.D:
					this.MoveRight=true;
					break;
			}
		}
		private void Key_Up(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
				case Keys.W:
					this.MoveUp=false;
					break;
				case Keys.S:
					this.MoveDown=false;
					break;
				case Keys.A:
					this.MoveLeft=false;
					break;
				case Keys.D:
					this.MoveRight=false;
					break;
			}
			if(M.get_Player().X==M.get_Width()-1 && M.get_Player().Y==M.get_Height()-1)
			{
				Timer.Enabled=false;
				MessageBox.Show("成功","Success");
				Chk1.Checked=true;
				Chk2.Checked=false;
				M.set_Player(0,0);
			}
		}
		private void Timer_Tick(object sender, System.EventArgs e)
		{
			if(!Chk2.Checked)
				return;
			if(Chk1.Checked)
				return;
			M.ClearPlayer(picM.CreateGraphics());
			if(this.MoveUp)
				M.MovePlayer(Maze.Direction.Up);
			if(this.MoveDown)
				M.MovePlayer(Maze.Direction.Down);
			if(this.MoveLeft)
				M.MovePlayer(Maze.Direction.Left);
			if(this.MoveRight)
				M.MovePlayer(Maze.Direction.Right);
			M.DrawPlayer(picM.CreateGraphics());
		}
		private void Chk1_CheckedChanged(object sender, System.EventArgs e)
		{
			if(!Gened)
			{
				Chk1.Checked=false;
				return;
			}
			if(Chk1.Checked)
			{
				if(this.Solved==false)
				{
					M.Solve(out S);
					this.Solved=true;
				}
				M.DrawAnswer(S,picM.CreateGraphics());
			}
			else
			{
				M.ClearAnswer(S,picM.CreateGraphics());
				M.DrawPlayer(picM.CreateGraphics());
			}
			GC.Collect();
		}

		private void Chk2_CheckedChanged(object sender, System.EventArgs e)
		{
			if(!Gened)
			{
				Chk2.Checked=false;
				return;
			}
			if(Chk2.Checked)
			{
				txtX.Enabled=false;
				txtY.Enabled=false;
				Chk2.Text="停止";
				Timer.Enabled=true;
			}
			else
			{
				txtX.Enabled=true;
				txtY.Enabled=true;
				Chk2.Text="开始";
				Timer.Enabled=false;
			}
		}

		private void picM_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			M.Draw(e.Graphics);
			M.DrawPlayer(e.Graphics);
			if(Chk1.Checked)
			{
				M.DrawAnswer(S,e.Graphics);
			}
		}

		private void txtX_Enter(object sender, System.EventArgs e)
		{
			txtX.SelectAll();
		}

		private void txtY_Enter(object sender, System.EventArgs e)
		{
			txtY.SelectAll();
		}

		private void button2_Click(object sender, System.EventArgs e)
		{
			float[] Setting=new float[5];
			frmSetting frmSet=new frmSetting(ref Setting,this.M.get_Height()*this.M.get_Width()<30*30);
			if(frmSet.ShowDialog()==DialogResult.Cancel)
				return;
			try
			{
				frm3D frmGame=new frm3D(this.M,Setting[0],Setting[1],Setting[2],Setting[3]>0.5f,Setting[4]>0.5f);
				this.Hide();
				frmGame.ShowDialog();
				this.Show();
			}
			catch(Exception)
			{
				MessageBox.Show("初始化失败","错误",MessageBoxButtons.OK,MessageBoxIcon.Error);
				return;
			}
			GC.Collect();
		}

		private void button3_Click(object sender, System.EventArgs e)
		{
			string Help=string.Format("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}\n{9}\n{10}",
				"左边两个文本框用于输入欲生成的迷宫的宽和高(5到150之间).",
				"点击\"生成\"按钮将生成新的迷宫.",
				"按下\"答案\"按钮将提示最佳路径.",
				"按下\"开始\"按钮即可用:",
				"\t\"W\"(上)\"S\"(下)\"A\"(左)\"D\"(右)控制红色小方块移动",
				"\t目标是当将其移动到右下角即终点",
				"\t(注意:提示路径时不能移动)",
				"点击\"三维\"按钮将进入三维迷宫,此时:",
				"\t可用\"W\"(前)\"S\"(后)\"A\"(左)\"D\"(右)键控制平移,",
				"\t用光标键中的左右控制旋转,上下键也可用于控制前进后退.",
				"\t\t\t\t\t\t——loywong 2005.6");
			MessageBox.Show(Help,"帮助",MessageBoxButtons.OK,MessageBoxIcon.Information);
		}
	}
}
