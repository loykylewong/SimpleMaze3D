using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

namespace MazeAdvanced
{
	/// <summary>
	/// frmSetting 的摘要说明。
	/// </summary>
	public class frmSetting : System.Windows.Forms.Form
	{
		private System.Windows.Forms.HScrollBar hScrollBar1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.HScrollBar hSBMoveSpeed;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.HScrollBar hSBRotaSpeed;
		private System.Windows.Forms.CheckBox chkCompass;
		private System.Windows.Forms.CheckBox chkLight;
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmSetting(ref float[] Set,bool CanLight)
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();
			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
			this.Setting=Set;
			this.CanLight=CanLight;
		}

		private float[] Setting;
		private bool CanLight;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
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
			this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.hSBMoveSpeed = new System.Windows.Forms.HScrollBar();
			this.hSBRotaSpeed = new System.Windows.Forms.HScrollBar();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.chkCompass = new System.Windows.Forms.CheckBox();
			this.chkLight = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// hScrollBar1
			// 
			this.hScrollBar1.Location = new System.Drawing.Point(16, 28);
			this.hScrollBar1.Maximum = 129;
			this.hScrollBar1.Name = "hScrollBar1";
			this.hScrollBar1.Size = new System.Drawing.Size(200, 16);
			this.hScrollBar1.TabIndex = 0;
			this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.DensityChange);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(8, 12);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 12);
			this.label1.TabIndex = 1;
			this.label1.Text = "雾的浓度:";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(88, 12);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(96, 12);
			this.label2.TabIndex = 2;
			this.label2.Text = "0.5f";
			// 
			// btnOK
			// 
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Location = new System.Drawing.Point(164, 204);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(52, 20);
			this.btnOK.TabIndex = 4;
			this.btnOK.Text = "OK";
			// 
			// btnCancel
			// 
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(104, 204);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(52, 20);
			this.btnCancel.TabIndex = 5;
			this.btnCancel.Text = "Cancel";
			// 
			// hSBMoveSpeed
			// 
			this.hSBMoveSpeed.Location = new System.Drawing.Point(16, 68);
			this.hSBMoveSpeed.Maximum = 129;
			this.hSBMoveSpeed.Minimum = 1;
			this.hSBMoveSpeed.Name = "hSBMoveSpeed";
			this.hSBMoveSpeed.Size = new System.Drawing.Size(200, 16);
			this.hSBMoveSpeed.TabIndex = 6;
			this.hSBMoveSpeed.Value = 1;
			this.hSBMoveSpeed.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hSBMoveSpeed_Scroll);
			// 
			// hSBRotaSpeed
			// 
			this.hSBRotaSpeed.Location = new System.Drawing.Point(16, 108);
			this.hSBRotaSpeed.Maximum = 129;
			this.hSBRotaSpeed.Minimum = 1;
			this.hSBRotaSpeed.Name = "hSBRotaSpeed";
			this.hSBRotaSpeed.Size = new System.Drawing.Size(200, 16);
			this.hSBRotaSpeed.TabIndex = 7;
			this.hSBRotaSpeed.Value = 1;
			this.hSBRotaSpeed.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hSBRotaSpeed_Scroll);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(8, 52);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(160, 12);
			this.label3.TabIndex = 8;
			this.label3.Text = "移动速度:";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(8, 92);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(160, 12);
			this.label4.TabIndex = 9;
			this.label4.Text = "旋转速度:";
			// 
			// chkCompass
			// 
			this.chkCompass.Location = new System.Drawing.Point(12, 132);
			this.chkCompass.Name = "chkCompass";
			this.chkCompass.Size = new System.Drawing.Size(208, 20);
			this.chkCompass.TabIndex = 10;
			this.chkCompass.Text = "使用指南针(它始终指向目标)";
			this.chkCompass.CheckedChanged += new System.EventHandler(this.chkCompass_CheckedChanged);
			// 
			// chkLight
			// 
			this.chkLight.Location = new System.Drawing.Point(12, 160);
			this.chkLight.Name = "chkLight";
			this.chkLight.Size = new System.Drawing.Size(208, 20);
			this.chkLight.TabIndex = 11;
			this.chkLight.Text = "夜间";
			this.chkLight.CheckedChanged += new System.EventHandler(this.chkLight_CheckedChanged);
			// 
			// frmSetting
			// 
			this.AcceptButton = this.btnOK;
			this.AutoScaleBaseSize = new System.Drawing.Size(6, 14);
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(230, 236);
			this.Controls.Add(this.chkLight);
			this.Controls.Add(this.chkCompass);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.hSBRotaSpeed);
			this.Controls.Add(this.hSBMoveSpeed);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.hScrollBar1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "frmSetting";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "设置";
			this.Load += new System.EventHandler(this.frmSetting_Load);
			this.ResumeLayout(false);

		}
		#endregion

		private void frmSetting_Load(object sender, System.EventArgs e)
		{
			this.hScrollBar1.Value=50;
			this.label2.Text=Convert.ToString(0.5)+"f";
			this.hSBMoveSpeed.Value=60;
			this.hSBRotaSpeed.Value=80;
			this.chkCompass.Checked=true;
			if(!this.CanLight)
			{
				this.chkLight.Enabled=false;
				this.chkLight.Text="夜间(迷宫大小不得大于29X29)";
			}
			this.Setting[0]=0.5f;
			this.Setting[1]=0.04f;
			this.Setting[2]=0.06f;
			this.Setting[3]=1.0f;
			this.Setting[4]=0.0f;
		}

		private void DensityChange(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			this.Setting[0]=(float)(this.hScrollBar1.Value)/100f;
			this.label2.Text=Convert.ToString(this.Setting[0])+"f";
		}

		private void hSBMoveSpeed_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			this.Setting[1]=(float)(this.hSBMoveSpeed.Value)/2000f+0.01f;
		}

		private void hSBRotaSpeed_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
		{
			this.Setting[2]=(float)(this.hSBRotaSpeed.Value)/1500f+0.02f;
		}

		private void chkCompass_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.chkCompass.Checked)
				this.Setting[3]=1.0f;
			else
				this.Setting[3]=0.0f;
		}

		private void chkLight_CheckedChanged(object sender, System.EventArgs e)
		{
			if(this.chkLight.Checked)
				this.Setting[4]=1.0f;
			else
				this.Setting[4]=0.0f;
		}
	}
}
