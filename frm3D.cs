//#define Debug

using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using US=Microsoft.DirectX.UnsafeNativeMethods;
namespace MazeAdvanced
{
	/// <summary>
	/// frm3D 的摘要说明。
	/// </summary>
	public class frm3D : System.Windows.Forms.Form
	{
		private System.ComponentModel.IContainer components;

		public frm3D(Maze theMaze,float FD,float MS,float RS,bool CP,bool Light)
		{
			//
			// Windows 窗体设计器支持所必需的
			//
			InitializeComponent();
			//
			// TODO: 在 InitializeComponent 调用后添加任何构造函数代码
			//
			this.FogDensity=FD;
#if(!Debug)
			this.MoveStep=MS;
			this.RotaStep=RS;
#endif
			this.Compass=CP;
			this.FlashLight=Light;

			this.CopyMazeAddFence(theMaze);
			if(this.FlashLight)
			{
				this.EstablishModelWithLight();
				this.TextBrush=new SolidBrush(Color.LightGreen);
			}
			else
			{
				this.EstablishModelWithoutLight();
				this.TextBrush=new SolidBrush(Color.FromArgb(255,0,63,0));
			}
			this.EyePos=new Vector3(1.5f,0.3f,1.5f);
			this.EyeAt=new Vector3(2.0f,0.3f,2.0f);
			this.EyeUp=new Vector3(0.0f,1.0f,0.0f);
			this.EyeLook=this.EyeAt-this.EyePos;
			this.EyeLook.Normalize();
			this.EyeAt=this.EyePos+this.EyeLook;

			if(this.Compass)
			{
				this.TextFont=new System.Drawing.Font("Lucida Console",8);
				this.ArrowBrush=new SolidBrush(Color.Red);
				this.ArrDiskBrush=new SolidBrush(Color.FromArgb(0x3f7fff7f));
				this.Arrow=new Point[] {new Point(0,-30),new Point(5,30),new Point(0,20),new Point(-5,30)};
			}

			if(!this.InitDirect3D())
			{
				MessageBox.Show("Direct3D初始化失败!","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
				throw(new Exception("Initialize Direct3D Error!"));
			}
		}

		private void CopyMazeAddFence(Maze tM)
		{
			int W,H,x,y;
			W=tM.get_Width()+2;
			H=tM.get_Height()+2;
			this.M=new Maze(W,H);
			for(x=0;x<W;x++)
			{
				M[x,0]=Maze.GridState.Disable;
				M[x,H-1]=Maze.GridState.Disable;
			}
			for(y=1;y<H-1;y++)
			{
				M[0,y]=Maze.GridState.Disable;
				M[W-1,y]=Maze.GridState.Disable;
			}
			for(x=1;x<W-1;x++)
				for(y=1;y<H-1;y++)
					M[x,y]=tM[x-1,y-1];
			this.TargetX=(float)W-1.5f;
			this.TargetY=(float)H-1.5f;
		}
		private void EstablishModelWithoutLight()
		{
			#region Establish Sky
			this.SkyVertices[0].Position=(new Vector3(-25000f,20f,-25000f));
			this.SkyVertices[0].Tu=0f;
			this.SkyVertices[0].Tv=0f;
			this.SkyVertices[1].Position=(new Vector3(25000f,20f,-25000f));
			this.SkyVertices[1].Tu=100f;
			this.SkyVertices[1].Tv=0f;
			this.SkyVertices[2].Position=(new Vector3(-25000f,20f,25000f));
			this.SkyVertices[2].Tu=0f;
			this.SkyVertices[2].Tv=100f;
			this.SkyVertices[3].Position=(new Vector3(25000f,20f,25000f));
			this.SkyVertices[3].Tu=100f;
			this.SkyVertices[3].Tv=100f;
			#endregion
			#region Establish Ground
			this.GroundVertices=new CustomVertex.PositionTextured[4];
			this.GroundVertices[0].Position=(new Vector3(1f,0f,1f));
			this.GroundVertices[0].Tu=0f;
			this.GroundVertices[0].Tv=0f;
			this.GroundVertices[1].Position=(new Vector3((float)(this.M.get_Height()-1),0f,1f));
			this.GroundVertices[1].Tu=(float)(this.M.get_Height()-2);
			this.GroundVertices[1].Tv=0f;
			this.GroundVertices[2].Position=(new Vector3(1f,0f,(float)(this.M.get_Width()-1)));
			this.GroundVertices[2].Tu=0f;
			this.GroundVertices[2].Tv=(float)(this.M.get_Width()-2);
			this.GroundVertices[3].Position=(new Vector3((float)(this.M.get_Height()-1),0f,(float)(this.M.get_Width()-1)));
			this.GroundVertices[3].Tu=(float)(this.M.get_Height()-2);
			this.GroundVertices[3].Tv=(float)(this.M.get_Width()-2);
			#endregion
			#region Establish Target
			this.TargetVertices=new CustomVertex.PositionTextured[4];
			this.TargetVertices[0].Position=(new Vector3((float)M.get_Height()-2f,0.001f,(float)M.get_Width()-2f));
			this.TargetVertices[0].Tu=0f;
			this.TargetVertices[0].Tv=0f;
			this.TargetVertices[1].Position=(new Vector3((float)M.get_Height()-1f,0.001f,(float)M.get_Width()-2f));
			this.TargetVertices[1].Tu=1f;
			this.TargetVertices[1].Tv=0f;
			this.TargetVertices[2].Position=(new Vector3((float)M.get_Height()-2f,0.001f,(float)M.get_Width()-1f));
			this.TargetVertices[2].Tu=0f;
			this.TargetVertices[2].Tv=1f;
			this.TargetVertices[3].Position=(new Vector3((float)M.get_Height()-1f,0.001f,(float)M.get_Width()-1f));
			this.TargetVertices[3].Tu=1f;
			this.TargetVertices[3].Tv=1f;
			#endregion
			#region Establish Wall
			MaxWallRectNum=(this.M.get_Width()*this.M.get_Height()+1)*2+this.M.get_Width()+this.M.get_Height();
			CustomVertex.PositionTextured[] Verts=new CustomVertex.PositionTextured[MaxWallRectNum*4];
			int[] Index=new int[MaxWallRectNum*6];
			int x,y;
			int Start=0;
			this.WallVertsCount=0;
			this.WallIndexCount=0;
			bool WasCliff=false;
			Vector3 CliffNormal=new Vector3(0f,0f,0f);
			for(y=1;y<this.M.get_Height();y++)
			{
				for(x=1;x<this.M.get_Width();x++)
				{
					if(this.M[x,y-1]!=this.M[x,y])	//Now Cliff
					{
						if(!WasCliff)	//No Cliff before
						{
							WasCliff=true;
							Start=x;
							if(this.M[x,y]==Maze.GridState.Enable)
								CliffNormal.X=1;
							else
								CliffNormal.X=-1;
						}
					}
					else	//Now No Cliff
					{
						if(WasCliff)	//Was Cliff before
						{
							WasCliff=false;
							Verts[this.WallVertsCount].Position=(new Vector3(y,0f,(float)Start));
							Verts[this.WallVertsCount].Tu=0f;
							Verts[this.WallVertsCount].Tv=1f;
							Index[this.WallIndexCount++]=this.WallVertsCount;
							this.WallVertsCount++;
							Verts[this.WallVertsCount].Position=(new Vector3(y,1f,(float)Start));
							Verts[this.WallVertsCount].Tu=0f;
							Verts[this.WallVertsCount].Tv=0f;
							Index[this.WallIndexCount++]=this.WallVertsCount;
							this.WallVertsCount++;
							Verts[this.WallVertsCount].Position=(new Vector3(y,0f,(float)x));
							Verts[this.WallVertsCount].Tu=(float)(x-Start);
							Verts[this.WallVertsCount].Tv=1f;
							Index[this.WallIndexCount++]=this.WallVertsCount;
							Index[this.WallIndexCount++]=this.WallVertsCount-1;
							Index[this.WallIndexCount++]=this.WallVertsCount;
							this.WallVertsCount++;
							Verts[this.WallVertsCount].Position=(new Vector3(y,1f,(float)x));
							Verts[this.WallVertsCount].Tu=(float)(x-Start);
							Verts[this.WallVertsCount].Tv=0f;
							Index[this.WallIndexCount++]=this.WallVertsCount;
							this.WallVertsCount++;
						}
					}
				}
			}
			WasCliff=false;
			CliffNormal.X=0f;
			for(x=1;x<this.M.get_Width();x++)
			{
				for(y=1;y<this.M.get_Height();y++)
				{
					if(this.M[x-1,y]!=this.M[x,y])	//Now Cliff
					{
						if(!WasCliff)	//No Cliff before
						{
							WasCliff=true;
							Start=y;
							if(this.M[x,y]==Maze.GridState.Enable)
								CliffNormal.Z=1;
							else
								CliffNormal.Z=-1;
						}
					}
					else	//Now No Cliff
					{
						if(WasCliff)	//Was Cliff before
						{
							WasCliff=false;
							Verts[this.WallVertsCount].Position=(new Vector3((float)Start,0f,x));
							Verts[this.WallVertsCount].Tu=0f;
							Verts[this.WallVertsCount].Tv=1f;
							Index[this.WallIndexCount++]=this.WallVertsCount;
							this.WallVertsCount++;
							Verts[this.WallVertsCount].Position=(new Vector3((float)Start,1f,x));
							Verts[this.WallVertsCount].Tu=0f;
							Verts[this.WallVertsCount].Tv=0f;
							Index[this.WallIndexCount++]=this.WallVertsCount;
							this.WallVertsCount++;
							Verts[this.WallVertsCount].Position=(new Vector3((float)y,0f,x));
							Verts[this.WallVertsCount].Tu=(float)(y-Start);
							Verts[this.WallVertsCount].Tv=1f;
							Index[this.WallIndexCount++]=this.WallVertsCount;
							Index[this.WallIndexCount++]=this.WallVertsCount-1;
							Index[this.WallIndexCount++]=this.WallVertsCount;
							this.WallVertsCount++;
							Verts[this.WallVertsCount].Position=(new Vector3((float)y,1f,x));
							Verts[this.WallVertsCount].Tu=(float)(y-Start);
							Verts[this.WallVertsCount].Tv=0f;
							Index[this.WallIndexCount++]=this.WallVertsCount;
							this.WallVertsCount++;
						}
					}
				}
			}
			this.WallVertices=new CustomVertex.PositionTextured[this.WallVertsCount];
			this.WallVertexIndex=new int[this.WallIndexCount];
			for(x=0;x<this.WallVertsCount;x++)
				this.WallVertices[x]=Verts[x];
			for(x=0;x<this.WallIndexCount;x++)
				this.WallVertexIndex[x]=Index[x];
			#endregion
		}

		private void EstablishModelWithLight()
		{
			#region Establish Sky
			this.SkyVertices[0].Position=(new Vector3(-25000f,20f,-25000f));
			this.SkyVertices[0].Tu=0f;
			this.SkyVertices[0].Tv=0f;
			this.SkyVertices[1].Position=(new Vector3(25000f,20f,-25000f));
			this.SkyVertices[1].Tu=100f;
			this.SkyVertices[1].Tv=0f;
			this.SkyVertices[2].Position=(new Vector3(-25000f,20f,25000f));
			this.SkyVertices[2].Tu=0f;
			this.SkyVertices[2].Tv=100f;
			this.SkyVertices[3].Position=(new Vector3(25000f,20f,25000f));
			this.SkyVertices[3].Tu=100f;
			this.SkyVertices[3].Tv=100f;
			#endregion
			#region Establish Wall
			MaxWallRectNum=(this.M.get_Width()*this.M.get_Height()+1)*2+this.M.get_Width()+this.M.get_Height();
			CustomVertex.PositionTextured[] WVerts=new CustomVertex.PositionTextured[(int)(MaxWallRectNum*4/this.DivStep/this.DivStep)];
			int[] WIndex=new int[(int)(MaxWallRectNum*6/this.DivStep/this.DivStep)];
			int x,y;
			float i,j;
			this.WallVertsCount=0;
			this.WallIndexCount=0;
			for(y=1;y<this.M.get_Height();y++)
			{
				for(x=1;x<this.M.get_Width()-1;x++)
				{
					if(this.M[x,y-1]!=this.M[x,y])	//Cliff
					{
						for(i=0.0f;i<0.999f;i+=this.DivStep)
							for(j=0.0f;j<0.999f;j+=this.DivStep)
							{
								WVerts[this.WallVertsCount].Position=(new Vector3(y,j,x+i));
								WVerts[this.WallVertsCount].Tu=i;
								WVerts[this.WallVertsCount].Tv=j+this.DivStep;
								WIndex[this.WallIndexCount++]=this.WallVertsCount;
								this.WallVertsCount++;
								WVerts[this.WallVertsCount].Position=(new Vector3(y,j+this.DivStep,x+i));
								WVerts[this.WallVertsCount].Tu=i;
								WVerts[this.WallVertsCount].Tv=j;
								WIndex[this.WallIndexCount++]=this.WallVertsCount;
								this.WallVertsCount++;
								WVerts[this.WallVertsCount].Position=(new Vector3(y,j,x+i+this.DivStep));
								WVerts[this.WallVertsCount].Tu=i+this.DivStep;
								WVerts[this.WallVertsCount].Tv=j+this.DivStep;
								WIndex[this.WallIndexCount++]=this.WallVertsCount;
								WIndex[this.WallIndexCount++]=this.WallVertsCount-1;
								WIndex[this.WallIndexCount++]=this.WallVertsCount;
								this.WallVertsCount++;
								WVerts[this.WallVertsCount].Position=(new Vector3(y,j+this.DivStep,x+i+this.DivStep));
								WVerts[this.WallVertsCount].Tu=i+this.DivStep;
								WVerts[this.WallVertsCount].Tv=j;
								WIndex[this.WallIndexCount++]=this.WallVertsCount;
								this.WallVertsCount++;
							}
					}
				}
			}
			for(x=1;x<this.M.get_Width();x++)
			{
				for(y=1;y<this.M.get_Height()-1;y++)
				{
					if(this.M[x-1,y]!=this.M[x,y])	//Now Cliff
					{
						for(i=0.0f;i<0.999f;i+=this.DivStep)
							for(j=0.0f;j<0.999f;j+=this.DivStep)
							{
								WVerts[this.WallVertsCount].Position=(new Vector3(y+i,j,x));
								WVerts[this.WallVertsCount].Tu=i;
								WVerts[this.WallVertsCount].Tv=j+this.DivStep;
								WIndex[this.WallIndexCount++]=this.WallVertsCount;
								this.WallVertsCount++;
								WVerts[this.WallVertsCount].Position=(new Vector3(y+i,j+this.DivStep,x));
								WVerts[this.WallVertsCount].Tu=i;
								WVerts[this.WallVertsCount].Tv=j;
								WIndex[this.WallIndexCount++]=this.WallVertsCount;
								this.WallVertsCount++;
								WVerts[this.WallVertsCount].Position=(new Vector3(y+i+this.DivStep,j,x));
								WVerts[this.WallVertsCount].Tu=i+this.DivStep;
								WVerts[this.WallVertsCount].Tv=j+this.DivStep;
								WIndex[this.WallIndexCount++]=this.WallVertsCount;
								WIndex[this.WallIndexCount++]=this.WallVertsCount-1;
								WIndex[this.WallIndexCount++]=this.WallVertsCount;
								this.WallVertsCount++;
								WVerts[this.WallVertsCount].Position=(new Vector3(y+i+this.DivStep,j+this.DivStep,x));
								WVerts[this.WallVertsCount].Tu=i+this.DivStep;
								WVerts[this.WallVertsCount].Tv=j;
								WIndex[this.WallIndexCount++]=this.WallVertsCount;
								this.WallVertsCount++;
							}
					}
				}
			}
			this.WallVertices=new CustomVertex.PositionTextured[this.WallVertsCount];
			this.WallVertexIndex=new int[this.WallIndexCount];
			for(x=0;x<this.WallVertsCount;x++)
				this.WallVertices[x]=WVerts[x];
			for(x=0;x<this.WallIndexCount;x++)
				this.WallVertexIndex[x]=WIndex[x];
			#endregion
			#region Establish Ground and Target
			CustomVertex.PositionTextured[] GVerts=new CustomVertex.PositionTextured[(int)(this.M.get_Width()*this.M.get_Height()*3/this.DivStep/this.DivStep)];
			int[] GIndex=new int[(int)(this.M.get_Width()*this.M.get_Height()*4.5f/this.DivStep/this.DivStep)];
			this.TargetVertsCount=(int)(4/this.DivStep/this.DivStep+0.1);
			this.TargetIndexCount=(int)(6/this.DivStep/this.DivStep+0.1);
			this.TargetVertices=new CustomVertex.PositionTextured[this.TargetVertsCount];
			this.TargetVertexIndex=new int[this.TargetIndexCount];
			this.GroundVertsCount=0;
			this.GroundIndexCount=0;
			int TVC=0;
			int TIC=0;
			for(y=1;y<this.M.get_Height()-1;y++)
			{
				for(x=1;x<this.M.get_Width()-1;x++)
				{
					if(this.M[x,y]==Maze.GridState.Enable)	//Ground
					{
						if(x==this.M.get_Width()-2 && y==this.M.get_Height()-2)
						{
							for(i=0.0f;i<0.999f;i+=this.DivStep)
								for(j=0.0f;j<0.999f;j+=this.DivStep)
								{
									this.TargetVertices[TVC].Position=(new Vector3(y+j,0f,x+i));
									this.TargetVertices[TVC].Tu=i;
									this.TargetVertices[TVC].Tv=j;
									this.TargetVertexIndex[TIC++]=TVC;
									TVC++;
									this.TargetVertices[TVC].Position=(new Vector3(y+j+this.DivStep,0f,x+i));
									this.TargetVertices[TVC].Tu=i;
									this.TargetVertices[TVC].Tv=j+this.DivStep;
									this.TargetVertexIndex[TIC++]=TVC;
									TVC++;
									this.TargetVertices[TVC].Position=(new Vector3(y+j,0f,x+i+this.DivStep));
									this.TargetVertices[TVC].Tu=i+this.DivStep;
									this.TargetVertices[TVC].Tv=j;
									this.TargetVertexIndex[TIC++]=TVC;
									this.TargetVertexIndex[TIC++]=TVC-1;
									this.TargetVertexIndex[TIC++]=TVC;
									TVC++;
									this.TargetVertices[TVC].Position=(new Vector3(y+j+this.DivStep,0f,x+i+this.DivStep));
									this.TargetVertices[TVC].Tu=i+this.DivStep;
									this.TargetVertices[TVC].Tv=j+this.DivStep;
									this.TargetVertexIndex[TIC++]=TVC;
									TVC++;
								}
						}
						else
						{
							for(i=0.0f;i<0.999f;i+=this.DivStep)
								for(j=0.0f;j<0.999f;j+=this.DivStep)
								{
									GVerts[this.GroundVertsCount].Position=(new Vector3(y+j,0f,x+i));
									GVerts[this.GroundVertsCount].Tu=i;
									GVerts[this.GroundVertsCount].Tv=j;
									GIndex[this.GroundIndexCount++]=this.GroundVertsCount;
									this.GroundVertsCount++;
									GVerts[this.GroundVertsCount].Position=(new Vector3(y+j+this.DivStep,0f,x+i));
									GVerts[this.GroundVertsCount].Tu=i;
									GVerts[this.GroundVertsCount].Tv=j+this.DivStep;
									GIndex[this.GroundIndexCount++]=this.GroundVertsCount;
									this.GroundVertsCount++;
									GVerts[this.GroundVertsCount].Position=(new Vector3(y+j,0f,x+i+this.DivStep));
									GVerts[this.GroundVertsCount].Tu=i+this.DivStep;
									GVerts[this.GroundVertsCount].Tv=j;
									GIndex[this.GroundIndexCount++]=this.GroundVertsCount;
									GIndex[this.GroundIndexCount++]=this.GroundVertsCount-1;
									GIndex[this.GroundIndexCount++]=this.GroundVertsCount;
									this.GroundVertsCount++;
									GVerts[this.GroundVertsCount].Position=(new Vector3(y+j+this.DivStep,0f,x+i+this.DivStep));
									GVerts[this.GroundVertsCount].Tu=i+this.DivStep;
									GVerts[this.GroundVertsCount].Tv=j+this.DivStep;
									GIndex[this.GroundIndexCount++]=this.GroundVertsCount;
									this.GroundVertsCount++;
								}
						}
					}
				}
			}
			this.GroundVertices=new CustomVertex.PositionTextured[this.GroundVertsCount];
			this.GroundVertexIndex=new int[this.GroundIndexCount];
			for(x=0;x<this.GroundVertsCount;x++)
				this.GroundVertices[x]=GVerts[x];
			for(x=0;x<this.GroundIndexCount;x++)
				this.GroundVertexIndex[x]=GIndex[x];
			#endregion
		}

		private bool InitDirect3D()
		{
			try
			{
				PresentParameters Param=new PresentParameters();
				Param.Windowed=true;
				Param.SwapEffect=SwapEffect.Discard;
				Param.EnableAutoDepthStencil=true;
				Param.AutoDepthStencilFormat=DepthFormat.D16;
				this.D3DD=new Device(0,DeviceType.Hardware,this,CreateFlags.HardwareVertexProcessing,Param);
				this.D3DD.DeviceReset+=new EventHandler(D3DD_DeviceReset);
				this.D3DD_DeviceReset(D3DD,null);
				return true;
			}
			catch(Exception)
			{
				return false;
			}
		}

		private void D3DD_DeviceReset(object sender, EventArgs e)
		{
			Device Dev=(Device)sender;
			
			this.WallVB=new VertexBuffer(typeof(CustomVertex.PositionTextured),this.WallVertsCount,Dev,0,CustomVertex.PositionTextured.Format,Pool.Managed);
			this.WallVB.Created+=new EventHandler(WallVB_Created);
			this.WallVB_Created(this.WallVB,null);
			
			this.WallIB=new IndexBuffer(typeof(int),this.WallIndexCount,Dev,0,Pool.Managed);
			this.WallIB.Created+=new EventHandler(WallIB_Created);
			this.WallIB_Created(this.WallIB,null);
			
			if(this.FlashLight)
			{
				this.GroundVB=new VertexBuffer(typeof(CustomVertex.PositionTextured),this.GroundVertsCount,Dev,0,CustomVertex.PositionTextured.Format,Pool.Managed);
				this.GroundVB.Created+=new EventHandler(GroundVB_Created);
				this.GroundVB_Created(this.GroundVB,null);
				
				this.GroundIB=new IndexBuffer(typeof(int),this.GroundIndexCount,Dev,0,Pool.Managed);
				this.GroundIB.Created+=new EventHandler(GroundIB_Created);
				this.GroundIB_Created(this.GroundIB,null);

				this.TargetVB=new VertexBuffer(typeof(CustomVertex.PositionTextured),(int)(4/this.DivStep/this.DivStep+0.1f),Dev,0,CustomVertex.PositionTextured.Format,Pool.Managed);
				this.TargetVB.Created+=new EventHandler(TargetVB_Created);
				this.TargetVB_Created(this.TargetVB,null);

				this.TargetIB=new IndexBuffer(typeof(int),(int)(6/this.DivStep/this.DivStep+0.1f),Dev,0,Pool.Managed);
				this.TargetIB.Created+=new EventHandler(TargetIB_Created);
				this.TargetIB_Created(this.TargetIB,null);
			}
			else
			{
				this.GroundVB=new VertexBuffer(typeof(CustomVertex.PositionTextured),4,Dev,0,CustomVertex.PositionTextured.Format,Pool.Managed);
				this.GroundVB.Created+=new EventHandler(GroundVB_Created);
				this.GroundVB_Created(this.GroundVB,null);

				this.TargetVB=new VertexBuffer(typeof(CustomVertex.PositionTextured),4,Dev,0,CustomVertex.PositionTextured.Format,Pool.Managed);
				this.TargetVB.Created+=new EventHandler(TargetVB_Created);
				this.TargetVB_Created(this.TargetVB,null);
			}

			this.SkyVB=new VertexBuffer(typeof(CustomVertex.PositionTextured),4,Dev,0,CustomVertex.PositionTextured.Format,Pool.Managed);
			this.SkyVB.Created+=new EventHandler(SkyVB_Created);
			this.SkyVB_Created(this.SkyVB,null);

			bool TextureLoaded=false;
			
			Dev.RenderState.CullMode=Cull.None;
			Dev.RenderState.Lighting=this.FlashLight;
			Dev.RenderState.ZBufferEnable=true;
			Dev.RenderState.Clipping = true;
			Dev.ClipPlanes.DisableAll();
			if(this.FogDensity>0.01)
			{
				if(this.FlashLight)
					Dev.RenderState.FogColor=Color.FromArgb(0x7f070707);
				else
					Dev.RenderState.FogColor=Color.LightGray;
				Dev.RenderState.FogDensity=this.FogDensity;
				Dev.RenderState.FogStart=0.2f;
				Dev.RenderState.FogEnd=100.0f;
				Dev.RenderState.FogTableMode=FogMode.Exp2;
				Dev.RenderState.FogEnable=true;
			}
			try
			{
				this.WallTexture=TextureLoader.FromFile(Dev,@"Wall.bmp");
				this.GroundTexture=TextureLoader.FromFile(Dev,@"Ground.bmp");
				this.TargetTexture=TextureLoader.FromFile(Dev,@"Target.bmp");
				this.SkyTexture=TextureLoader.FromFile(Dev,@"Sky.bmp");
				TextureLoaded=true;
			}
			catch(Exception)
			{
			}
			if(!TextureLoaded)
			{
				try
				{
					this.WallTexture=TextureLoader.FromFile(Dev,@"..\..\Wall.bmp");
					this.GroundTexture=TextureLoader.FromFile(Dev,@"..\..\Ground.bmp");
					this.TargetTexture=TextureLoader.FromFile(Dev,@"..\..\Target.bmp");
					this.SkyTexture=TextureLoader.FromFile(Dev,@"..\..\Sky.bmp");
					TextureLoaded=true;
				}
				catch(Exception)
				{
				}
			}
			if(!TextureLoaded)
			{
				MessageBox.Show("无法加载纹理,\n纹理文件不存在!","Error!",MessageBoxButtons.OK,MessageBoxIcon.Error);
			}
			if(this.FlashLight)
			{
				this.Material.Diffuse=Color.White;
				this.Material.Ambient=Color.White;
				Dev.Material=Material;
				Dev.RenderState.AmbientMaterialSource=ColorSource.Material;
				Dev.RenderState.DiffuseMaterialSource=ColorSource.Material;
				Dev.Lights[0].Type=LightType.Spot;
				Dev.Lights[0].InnerConeAngle=0.6f;
				Dev.Lights[0].OuterConeAngle=1.2f;
				Dev.Lights[0].Attenuation0=1.0f;
				Dev.Lights[0].Falloff=1.0f;
				Dev.Lights[0].Range=100.0f;
				Dev.Lights[0].Diffuse=Color.White;
				Dev.Lights[0].Ambient=Color.White;
				Dev.Lights[0].Enabled=true;
			}
		}

		private void WallVB_Created(object sender, EventArgs e)
		{
			VertexBuffer wvb=(VertexBuffer)sender;
			GraphicsStream stm=wvb.Lock(0,0,0);
			stm.Write(this.WallVertices);
			wvb.Unlock();
		}

		private void WallIB_Created(object sender, EventArgs e)
		{
			IndexBuffer wib=(IndexBuffer)sender;
			GraphicsStream stm=wib.Lock(0,0,0);
			stm.Write(this.WallVertexIndex);
			wib.Unlock();
		}

		private void GroundVB_Created(object sender, EventArgs e)
		{
			VertexBuffer gvb=(VertexBuffer)sender;
			GraphicsStream stm=gvb.Lock(0,0,0);
			stm.Write(this.GroundVertices);
			gvb.Unlock();
		}

		private void GroundIB_Created(object sender, EventArgs e)
		{
			IndexBuffer gib=(IndexBuffer)sender;
			GraphicsStream stm=gib.Lock(0,0,0);
			stm.Write(this.GroundVertexIndex);
			gib.Unlock();
		}

		private void TargetVB_Created(object sender, EventArgs e)
		{
			VertexBuffer tvb=(VertexBuffer)sender;
			GraphicsStream stm=tvb.Lock(0,0,0);
			stm.Write(this.TargetVertices);
			tvb.Unlock();
		}

		private void TargetIB_Created(object sender, EventArgs e)
		{
			IndexBuffer tib=(IndexBuffer)sender;
			GraphicsStream stm=tib.Lock(0,0,0);
			stm.Write(this.TargetVertexIndex);
			tib.Unlock();
		}

		private void SkyVB_Created(object sender, EventArgs e)
		{
			VertexBuffer svb=(VertexBuffer)sender;
			GraphicsStream stm=svb.Lock(0,0,0);
			stm.Write(this.SkyVertices);
			svb.Unlock();
		}

		private void SetLights()
		{			
			D3DD.Lights[0].Direction=this.EyeLook;
			D3DD.Lights[0].Position=this.EyePos;
			D3DD.Lights[0].Update();
		}

		private unsafe void SetMatrices()
		{
			this.D3DD.Transform.World=Matrix.Identity;
			Matrix MView;
			fixed(Vector3* pEyePos= &this.EyePos,pEyeAt= &this.EyeAt,pEyeUp= &this.EyeUp)
				UnsafeNativeMethods.Matrix.LookAtLH(&MView,pEyePos,pEyeAt,pEyeUp);
			this.D3DD.Transform.View=MView;
			this.D3DD.Transform.Projection=Matrix.PerspectiveFovLH((float)Math.PI/2.5f,1.33333333f,0.1f,500.0f);
		}

		private void Render()
		{
			if(this.D3DD==null || this.Pause)
				return;
			D3DD.Clear(ClearFlags.Target | ClearFlags.ZBuffer,Color.LightSkyBlue,1.0f,0);
			D3DD.BeginScene();
			this.SetMatrices();
			if(this.FlashLight)
				this.SetLights();

			D3DD.VertexFormat=CustomVertex.PositionTextured.Format;
			//Wall
			D3DD.SetTexture(0,this.WallTexture);

			D3DD.SetStreamSource(0,this.WallVB,0);
			D3DD.Indices=this.WallIB;
			D3DD.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,this.WallVertsCount,0,this.WallIndexCount/3);
			
			if(this.FlashLight)
			{	
				//Ground
				D3DD.SetTexture(0,this.GroundTexture);
				D3DD.SetStreamSource(0,this.GroundVB,0);
				D3DD.Indices=this.GroundIB;
				D3DD.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,this.GroundVertsCount,0,this.GroundIndexCount/3);
				
				//Target
				D3DD.SetTexture(0,this.TargetTexture);
				D3DD.SetStreamSource(0,this.TargetVB,0);
				D3DD.Indices=this.TargetIB;
				D3DD.DrawIndexedPrimitives(PrimitiveType.TriangleList,0,0,this.TargetVertsCount,0,this.TargetIndexCount/3);
			}
			else
			{
				//Ground
				D3DD.SetTexture(0,this.GroundTexture);
				D3DD.SetStreamSource(0,this.GroundVB,0);
				D3DD.DrawPrimitives(PrimitiveType.TriangleStrip,0,2);
				//Target
				D3DD.SetTexture(0,this.TargetTexture);
				D3DD.SetStreamSource(0,this.TargetVB,0);
				D3DD.DrawPrimitives(PrimitiveType.TriangleStrip,0,2);
			}
			
			//Sky
			D3DD.SetTexture(0,this.SkyTexture);

			D3DD.SetStreamSource(0,this.SkyVB,0);
			D3DD.DrawPrimitives(PrimitiveType.TriangleStrip,0,2);
			
			D3DD.EndScene();
			D3DD.Present();
		}

		#region ### Member Variables ###
		private Maze M;
		private int MaxWallRectNum;
		private Device D3DD;

		private float FogDensity;
		
		private CustomVertex.PositionTextured[] WallVertices;
		private int WallVertsCount=0;
		private VertexBuffer WallVB;
		private int[] WallVertexIndex;
		private int WallIndexCount=0;
		private IndexBuffer WallIB;
		
		private CustomVertex.PositionTextured[] GroundVertices;
		private int GroundVertsCount=0;
		private VertexBuffer GroundVB;
		private int[] GroundVertexIndex;
		private int GroundIndexCount=0;
		private IndexBuffer GroundIB;

		private CustomVertex.PositionTextured[] TargetVertices;
		private int TargetVertsCount=0;
		private VertexBuffer TargetVB;
		private int[] TargetVertexIndex;
		private int TargetIndexCount=0;
		private IndexBuffer TargetIB;

		private CustomVertex.PositionTextured[] SkyVertices=new CustomVertex.PositionTextured[4];
		private VertexBuffer SkyVB;
		
		private Texture WallTexture;
		private Texture GroundTexture;
		private Texture TargetTexture;
		private Texture SkyTexture;

		private Material Material=new Material();
		
		private Vector3 EyePos=new Vector3(0f,10f,0f);
		private Vector3 EyeAt=new Vector3(3f,10f,3f);
		private Vector3 EyeLook;
		private Vector3 EyeUp;
		private float EyeTheta;
		private float TargetX,TargetY;
		private float TargetTheta;
		private float XDistance;
		private float YDistance;
		private float Dis;

		private long t0;
		private long t1;

		private Graphics frmG;
		private SolidBrush ArrowBrush;
		private SolidBrush ArrDiskBrush;
		private System.Drawing.Font TextFont;
		private SolidBrush TextBrush;
		private Point[] Arrow;

		private System.Windows.Forms.Timer Timer;
		private bool Compass;
		private bool FlashLight;
		private float DivStep=0.25f;
		private bool Pause;
		private bool SuccessShowed=false;
		private bool MoveL=false;
		private bool MoveR=false;
		private bool MoveF=false;
		private bool MoveB=false;
		private bool RotaL=false;
		private bool RotaR=false;
#if(Debug)
		private bool MoveU=false;
		private bool MoveD=false;
		private bool RotaU=false;
		private bool RotaD=false;
		private bool RotaYL=false;
		private bool RotaYR=false;
		private Vector3 tempV;
		private Matrix tempM;
		readonly float MoveStep=0.15f;
		readonly float RotaStep=0.015f;
#else
		readonly float MoveStep;
		readonly float RotaStep;
#endif
		#endregion

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
            this.components = new System.ComponentModel.Container();
            this.Timer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Timer
            // 
            this.Timer.Interval = 10;
            this.Timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // frm3D
            // 
            this.ClientSize = new System.Drawing.Size(640, 480);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frm3D";
            this.Text = "三维迷宫";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frm3D_Paint);
            this.Resize += new System.EventHandler(this.frm_Resize);
            this.Closing += new System.ComponentModel.CancelEventHandler(this.frm3D_Closing);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frm3D_KayUp);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm3D_KeyDown);
            this.Load += new System.EventHandler(this.frm3D_Load);
            this.ResumeLayout(false);

		}
		#endregion

		private unsafe void Timer_Tick(object sender, System.EventArgs e)
		{
			t0=Environment.TickCount;
			t1=Environment.TickCount;
			this.Render();
			if(this.Compass)
			{
				this.frmG.ResetTransform();
				this.frmG.FillEllipse(this.ArrDiskBrush,5,5,90,90);
				this.EyeTheta=(float)Math.Atan2(this.EyeLook.X,this.EyeLook.Z);
				this.XDistance=this.TargetX-this.EyePos.Z;
				this.YDistance=this.TargetY-this.EyePos.X;
				this.Dis=(float)Math.Sqrt(this.XDistance*this.XDistance+this.YDistance*this.YDistance);
				this.TargetTheta=(float)Math.Atan2(this.YDistance,this.XDistance);
				this.frmG.TranslateTransform(50f,50f);
				this.frmG.RotateTransform((this.TargetTheta-this.EyeTheta)*180f/(float)Math.PI);
				this.frmG.FillPolygon(this.ArrowBrush,this.Arrow);
				this.frmG.DrawString(this.Dis.ToString().PadRight(5,'0').Substring(0,5),this.TextFont,this.TextBrush,-18f,-40f);
			}
			Vector3 Vtemp;
			Matrix Mtemp;
			fixed(Vector3* pEyeUp=&this.EyeUp,pEyeLook=&this.EyeLook)
			{
				if(MoveF)
				{
#if(Debug)
					tempV=MoveStep*this.EyeLook;
					this.EyePos+=tempV;
#else
					this.ChangeEyePos(MoveStep*this.EyeLook);
#endif
					this.EyeAt=this.EyePos+this.EyeLook;
				}
				if(MoveB)
				{
#if(Debug)
					tempV=-MoveStep*this.EyeLook;
					this.EyePos+=tempV;
#else
					this.ChangeEyePos(-MoveStep*this.EyeLook);
#endif
					this.EyeAt=this.EyePos+this.EyeLook;
				}
				if(MoveR)
				{
#if(Debug)
					tempV=MoveStep*Vector3.Cross(this.EyeUp,this.EyeLook);
					this.EyePos+=tempV;
#else
					this.ChangeEyePos(MoveStep*(*US.Vector3.Cross(&Vtemp,pEyeUp,pEyeLook)));
#endif
					this.EyeAt=this.EyePos+this.EyeLook;
				}
				if(MoveL)
				{
#if(Debug)
					tempV=-MoveStep*Vector3.Cross(this.EyeUp,this.EyeLook);
					this.EyePos+=tempV;
#else
					this.ChangeEyePos(-MoveStep*(*US.Vector3.Cross(&Vtemp,pEyeUp,pEyeLook)));
#endif
					this.EyeAt=this.EyePos+this.EyeLook;
				}
				if(RotaL)
				{
					US.Vector3.TransformNormal(pEyeLook,pEyeLook,US.Matrix.RotationAxis(&Mtemp,pEyeUp,-RotaStep));
					this.EyeAt=this.EyePos+this.EyeLook;
				}
				if(RotaR)
				{
					US.Vector3.TransformNormal(pEyeLook,pEyeLook,US.Matrix.RotationAxis(&Mtemp,pEyeUp,RotaStep));
					this.EyeAt=this.EyePos+this.EyeLook;
				}
			}
#if(Debug)
			if(MoveU)
			{
				tempV=MoveStep*this.EyeUp;
				this.EyePos+=tempV;
				this.EyeAt=this.EyePos+this.EyeLook;
			}
			if(MoveD)
			{
				tempV=MoveStep*this.EyeUp;
				this.EyePos-=tempV;
				this.EyeAt-=tempV;
			}
			if(RotaU)
			{
				tempM=Matrix.RotationAxis(Vector3.Cross(this.EyeLook,this.EyeUp),RotaStep);
				this.EyeLook.TransformNormal(tempM);
				this.EyeUp.TransformNormal(tempM);
				this.EyeAt=this.EyePos+this.EyeLook;
			}
			if(RotaD)
			{
				tempM=Matrix.RotationAxis(Vector3.Cross(this.EyeLook,this.EyeUp),-RotaStep);
				this.EyeLook.TransformNormal(tempM);
				this.EyeUp.TransformNormal(tempM);
				this.EyeAt=this.EyePos+this.EyeLook;
			}
			if(RotaYL)
			{
				this.EyeUp.TransformNormal(Matrix.RotationAxis(this.EyeLook,RotaStep));
			}
			if(RotaYR)
			{
				this.EyeUp.TransformNormal(Matrix.RotationAxis(this.EyeLook,-RotaStep));
			}
#endif
		}
		
		private void ChangeEyePos(Vector3 Vect)
		{
			if((int)(this.EyePos.Z)==M.get_Width()-2 && (int)(this.EyePos.X)==M.get_Height()-2)
			{
				if(!this.SuccessShowed)
				{
					this.Timer.Enabled=false;
					MessageBox.Show("成功","Successs");
					this.Timer.Enabled=true;
					this.SuccessShowed=true;		
					this.MoveL=false;
					this.MoveR=false;
					this.MoveF=false;
					this.MoveB=false;
				}
			}
			else
				this.SuccessShowed=false;
			float Xmin,Xmax,Ymin,Ymax,WallX,WallY;
			this.EyePos.Z+=Vect.Z;
			this.EyePos.X+=Vect.X;
			WallX=(int)(this.EyePos.Z+0.5f);
			WallY=(int)(this.EyePos.X+0.5f);
			bool[,] Afoul=new bool[2,2];
			byte AfoulCount=0;
			Xmax=this.EyePos.Z+0.2f;
			Xmin=this.EyePos.Z-0.2f;
			Ymax=this.EyePos.X+0.2f;
			Ymin=this.EyePos.X-0.2f;
			Afoul[0,0]=this.M[(int)Xmin,(int)Ymin]!=Maze.GridState.Enable;
			Afoul[1,0]=this.M[(int)Xmax,(int)Ymin]!=Maze.GridState.Enable;
			Afoul[0,1]=this.M[(int)Xmin,(int)Ymax]!=Maze.GridState.Enable;
			Afoul[1,1]=this.M[(int)Xmax,(int)Ymax]!=Maze.GridState.Enable;
			foreach(bool af in Afoul)
				if(af)
					AfoulCount++;
			switch(AfoulCount)
			{
				case 0:
					return;
				case 1:
					if(Afoul[0,0])
					{
						if(WallX-Xmin<WallY-Ymin)
						{
							this.EyePos.Z=WallX+0.2f;
							return;
						}
						else
						{
							this.EyePos.X=WallY+0.2f;
							return;
						}
					}
					else if(Afoul[1,0])
					{
						if(Xmax-WallX<WallY-Ymin)
						{
							this.EyePos.Z=WallX-0.2f;
							return;
						}
						else
						{
							this.EyePos.X=WallY+0.2f;
							return;
						}
					}
					else if(Afoul[0,1])
					{
						if(WallX-Xmin<Ymax-WallY)
						{
							this.EyePos.Z=WallX+0.2f;
							return;
						}
						else
						{
							this.EyePos.X=WallY-0.2f;
							return;
						}
					}
					else if(Afoul[1,1])
					{
						if(Xmax-WallX<Ymax-WallY)
						{
							this.EyePos.Z=WallX-0.2f;
							return;
						}
						else
						{
							this.EyePos.X=WallY-0.2f;
							return;
						}
					}
					break;
				case 2:
					if(Afoul[0,0] && Afoul[0,1])	//Xmin Side Afoul
					{
						this.EyePos.Z=WallX+0.2f;
						return;
					}
					else if(Afoul[1,0] && Afoul[1,1])	//Xmax Side Afoul
					{
						this.EyePos.Z=WallX-0.2f;
						return;
					}
					else if(Afoul[0,0] && Afoul[1,0])	//Ymin Side Afoul
					{
						this.EyePos.X=WallY+0.2f;
						return;
					}
					else if(Afoul[0,1] && Afoul[1,1])	//Ymax Side Afoul
					{
						this.EyePos.X=WallY-0.2f;
						return;
					}
					break;
				case 3:
					if(!Afoul[0,0])
					{
							this.EyePos.Z=WallX-0.2f;
							this.EyePos.X=WallY-0.2f;
							return;
					}
					else if(!Afoul[1,0])
					{
							this.EyePos.Z=WallX+0.2f;
							this.EyePos.X=WallY-0.2f;
							return;
					}
					else if(!Afoul[0,1])
					{
							this.EyePos.Z=WallX-0.2f;
							this.EyePos.X=WallY+0.2f;
							return;
					}
					else if(!Afoul[1,1])
					{
							this.EyePos.Z=WallX+0.2f;
							this.EyePos.X=WallY+0.2f;
							return;
					}
					break;
			}
		}

		private void frm_Resize(object sender, System.EventArgs e)
		{
			Pause=((this.WindowState==FormWindowState.Minimized) || !this.Visible);
		}

		private void frm3D_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			this.Timer.Enabled=false;
			GC.Collect();
		}

		private void frm3D_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
#if(Debug)
				case Keys.Space:
					this.MoveF=true;
					break;
				case Keys.NumPad0:
					this.MoveB=true;
					break;
				case Keys.A:
					this.MoveL=true;
					break;
				case Keys.D:
					this.MoveR=true;
					break;
				case Keys.W:
					this.MoveU=true;
					break;
				case Keys.S:
					this.MoveD=true;
					break;
				case Keys.NumPad8:
					this.RotaU=true;
					break;
				case Keys.NumPad5:
					this.RotaD=true;
					break;
				case Keys.NumPad4:
					this.RotaL=true;
					break;
				case Keys.NumPad6:
					this.RotaR=true;
					break;
				case Keys.NumPad7:
					this.RotaYL=true;
					break;
				case Keys.NumPad9:
					this.RotaYR=true;
					break;
#else
				case Keys.W:
					this.MoveF=true;
					break;
				case Keys.S:
					this.MoveB=true;
					break;
				case Keys.Up:
					this.MoveF=true;
					break;
				case Keys.Down:
					this.MoveB=true;
					break;
				case Keys.A:
					this.MoveL=true;
					break;
				case Keys.D:
					this.MoveR=true;
					break;
				case Keys.Left:
					this.RotaL=true;
					break;
				case Keys.Right:
					this.RotaR=true;
					break;
#endif
			}		
		}

		private void frm3D_KayUp(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			switch(e.KeyCode)
			{
#if(Debug)
				case Keys.Space:
					this.MoveF=false;
					break;
				case Keys.NumPad0:
					this.MoveB=false;
					break;
				case Keys.A:
					this.MoveL=false;
					break;
				case Keys.D:
					this.MoveR=false;
					break;
				case Keys.W:
					this.MoveU=false;
					break;
				case Keys.S:
					this.MoveD=false;
					break;
				case Keys.NumPad8:
					this.RotaU=false;
					break;
				case Keys.NumPad5:
					this.RotaD=false;
					break;
				case Keys.NumPad4:
					this.RotaL=false;
					break;
				case Keys.NumPad6:
					this.RotaR=false;
					break;
				case Keys.NumPad7:
					this.RotaYL=false;
					break;
				case Keys.NumPad9:
					this.RotaYR=false;
					break;
#else
				case Keys.W:
					this.MoveF=false;
					break;
				case Keys.S:
					this.MoveB=false;
					break;
				case Keys.Up:
					this.MoveF=false;
					break;
				case Keys.Down:
					this.MoveB=false;
					break;
				case Keys.A:
					this.MoveL=false;
					break;
				case Keys.D:
					this.MoveR=false;
					break;
				case Keys.Left:
					this.RotaL=false;
					break;
				case Keys.Right:
					this.RotaR=false;
					break;
#endif
			}
		}

		private void frm3D_Load(object sender, System.EventArgs e)
		{
			this.Timer.Enabled=true;
			this.frmG=this.CreateGraphics();
		}

		private void frm3D_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			this.Render();
		}
	}
}
