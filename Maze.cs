using System;
using System.Collections;
using System.Drawing;
using System.Windows.Forms;

namespace MazeAdvanced
{
	/// <summary>
	/// Maze 的摘要说明。
	/// </summary>
	public class Maze
	{
		public enum Errors
		{
			Unkonw,
			NoError,
			NoPath,
			StartEndOutOfMaze,
			StartEndDisable,
		}
		public enum GridState
		{
			Null=int.MinValue,
			Disable=0,
			Enable=1,
			Seeked=2
		}
		public enum Direction
		{
			Null=int.MinValue,
			Right=0,
			Down=1,
			Left=2,
			Up=3
		}
		private Direction NextDir(Direction Dir)
		{
			switch(Dir)
			{
				case Direction.Right:
					return Direction.Down;
				case Direction.Down:
					return Direction.Left;
				case Direction.Left:
					return Direction.Up;
				case Direction.Up:
					return Direction.Null;
				default:
					return Direction.Null;
			}
		}
		public class Grid
		{
			public int X;
			public int Y;
			public Grid PreGrid;
			public Grid():this(-1,-1,null){}
			public Grid(int X,int Y):this(X,Y,null){}
			private Grid(int X,int Y,Grid PreG)
			{
				//
				// TODO: 在此处添加构造函数逻辑
				//
				this.X=X;
				this.Y=Y;
				this.PreGrid=PreG;
			}
			public Grid Copy()
			{
				return new Grid(this.X,this.Y,this.PreGrid);
			}
			public Grid CopyWithoutPre()
			{
				return new Grid(this.X,this.Y);
			}
			public Grid SuperSet(Grid G)
			{
				return new Grid(G.X,G.Y,new Grid(this.X,this.Y,this.PreGrid));
			}
			public Grid NextGrid(Direction Dir)
			{
				switch(Dir)
				{
					case Direction.Down:
						return new Grid(this.X,this.Y+1);
					case Direction.Left:
						return new Grid(this.X-1,this.Y);
					case Direction.Right:
						return new Grid(this.X+1,this.Y);
					case Direction.Up:
						return new Grid(this.X,this.Y-1);
					default:
						return this.CopyWithoutPre();
				}
			}
			#region 重载的"=="和"!="
		/*	public static bool operator ==(Grid GL,Grid GR)
			{
				int XL,XR,YL,YR;
				bool LNull=false;
				bool RNull=false;
				try
				{
					XL=GL.X;
					YL=GL.Y;
				}
				catch(NullReferenceException)
				{
					LNull=true;
				}
				try
				{
					XR=GR.X;
					YR=GR.Y;
				}
				catch(NullReferenceException)
				{
					RNull=true;
				}
				if(LNull && RNull)
					return true;
				else if(!LNull && !RNull) 
				{
					if(GL.X==GR.X && GL.Y==GR.Y)
						return true;
					else
						return false;
				}
				else
					return false;
			}
			public static bool operator !=(Grid GL,Grid GR)
			{
				int XL,XR,YL,YR;
				bool LNull=false;
				bool RNull=false;
				try
				{
					XL=GL.X;
					YL=GL.Y;
				}
				catch(NullReferenceException)
				{
					LNull=true;
				}
				try
				{
					XR=GR.X;
					YR=GR.Y;
				}
				catch(NullReferenceException)
				{
					RNull=true;
				}
				if(LNull && RNull)
					return false;
				else if(!LNull && !RNull) 
				{
					if(GL.X==GR.X && GL.Y==GR.Y)
						return false;
					else
						return true;
				}
				else
					return true;
			}*/
			#endregion
		}
		private bool GenFinish()
		{
			for(int i=0;i<this.Height;i+=2)
			{
				for(int j=0;j<this.Width;j+=2)
				{
					if(this.Grids[i,j]==GridState.Disable)
						return false;
				}
			}
			return true;
		}
		public bool Generate(int Width,int Height)
		{
			if(Width%2==0)
				Width++;
			if(Height%2==0)
				Height++;
			this.Grids=new GridState[Height,Width];
			this.Width=Width;
			this.Height=Height;

			byte dir;
			//this.Grids[0,0]=0;
			int currX=0;
			int currY=0;

			while(true)
			{
				if(this.GenFinish())
					break;
				dir=(byte)this.Rnd.Next(4);
				if(currX==0)
				{
					if(currY==0)
					{
						if(dir%2==0)
							dir=0;
						else
							dir=1;
					}
					else if(currY==this.Height-1)
					{
						if(dir%2==0)
							dir=0;
						else
							dir=3;
					}
					else
					{
						dir%=3;
						if(dir==0)
							dir=0;
						else if(dir==1)
							dir=1;
						else
							dir=3;
					}
				}
				else if(currX==this.Width-1)
				{
					if(currY==0)
					{
						if(dir%2==0)
							dir=2;
						else
							dir=1;
					}
					else if(currY==this.Height-1)
					{
						if(dir%2==0)
							dir=2;
						else
							dir=3;
					}
					else
					{
						dir%=3;
						if(dir==0)
							dir=2;
						else if(dir==1)
							dir=1;
						else
							dir=3;
					}
				}
				else
				{
					if(currY==0)
					{
						dir%=3;
						if(dir==0)
							dir=1;
						else if(dir==1)
							dir=0;
						else
							dir=2;
					}
					else if(currY==this.Height-1)
					{
						dir%=3;
						if(dir==0)
							dir=3;
						else if(dir==1)
							dir=0;
						else
							dir=2;
					}
					else
						dir=(byte)this.Rnd.Next(4);
				}
				if(dir==0)
				{
					currX+=2;
					if(this.Grids[currY,currX]==GridState.Disable)
					{
						this.Grids[currY,currX]=GridState.Enable;
						this.Grids[currY,currX-1]=GridState.Enable;
					}
					continue;
				}
				else if(dir==2)
				{
					currX-=2;
					if(this.Grids[currY,currX]==GridState.Disable)
					{
						this.Grids[currY,currX]=GridState.Enable;
						this.Grids[currY,currX+1]=GridState.Enable;
					}
					continue;
				}
				else if(dir==1)
				{
					currY+=2;
					if(this.Grids[currY,currX]==GridState.Disable)
					{
						this.Grids[currY,currX]=GridState.Enable;
						this.Grids[currY-1,currX]=GridState.Enable;
					}
					continue;
				}
				else if(dir==3)
				{
					currY-=2;
					if(this.Grids[currY,currX]==GridState.Disable)
					{
						this.Grids[currY,currX]=GridState.Enable;
						this.Grids[currY+1,currX]=GridState.Enable;
					}
					continue;
				}
			}

			this.Player.X=0;
			this.Player.Y=0;
			return true;
		}
		public Errors Solve(out Stack S)
		{
			return Solve(new Grid(0,0),new Grid(this.Width-1,this.Height-1),out S);
		}
		public Errors Solve(Grid Start,Grid End,out Stack S)
		{
			S=new Stack();
			if(!IsInMaze(Start) || !IsInMaze(End))
				return Errors.StartEndOutOfMaze;
			if(this[Start]!=GridState.Enable || this[End]!=GridState.Enable)
				return Errors.StartEndDisable;
			Queue Q=new Queue();
			Q.Enqueue(Start);
			Direction CurDir;
			Grid CurG=new Grid();
			Grid tempG=new Grid();
			bool Find=false;
			while(!Find)
			{
				if(Q.Count>0)
					CurG=(Grid)Q.Dequeue();
				else
					return Errors.NoPath;
				CurDir=Direction.Right;
				while(CurDir!=Direction.Null)
				{
					tempG=CurG.NextGrid(CurDir);
					if(this[tempG]==GridState.Enable)
					{
						if(tempG.X==End.X && tempG.Y==End.Y)
						{
							Find=true;
							break;
						}
						Q.Enqueue(CurG.SuperSet(tempG));
						this[tempG]=GridState.Seeked;
					}
					CurDir=NextDir(CurDir);
				}
			}
			CurG=CurG.SuperSet(tempG);
			while(CurG!=null)
			{
				S.Push(CurG.CopyWithoutPre());
				CurG=CurG.PreGrid;
			}
			ClearSeek();
			return Errors.NoError;
		}
		private void ClearSeek()
		{
			int I,J;
			for(I=0;I<this.Height;I++)
				for(J=0;J<this.Width;J++)
					if(this.Grids[I,J]==GridState.Seeked)
						this.Grids[I,J]=GridState.Enable;
		}
		public bool Draw(Graphics Graph)
		{
			if(Graph==null)
				return false;
			if(Width==0 || Height==0)
				return false;
			int I,J;
			for(I=0;I<this.Width;I++)
				for(J=0;J<this.Height;J++)
				{
					if(this.Grids[J,I]==GridState.Disable)
						Graph.FillRectangle(this.WallBrush,I*this.GridSize,J*this.GridSize,this.GridSize,this.GridSize);
					else if(this.Grids[J,I]==GridState.Enable)
						Graph.FillRectangle(this.PathBrush,I*this.GridSize,J*this.GridSize,this.GridSize,this.GridSize);
				}
			return true;
		}
		public bool DrawAnswer(Stack Answer,Graphics Graph)
		{
			if(Graph==null)
				return false;
			foreach(Grid Gd in Answer.ToArray())
				Graph.FillRectangle(this.AnswerBrush,Gd.X*this.GridSize,Gd.Y*this.GridSize,this.GridSize,this.GridSize);
			return true;
		}
		public bool ClearAnswer(Stack Answer,Graphics Graph)
		{
			if(Graph==null)
				return false;
			foreach(Grid Gd in Answer.ToArray())
				Graph.FillRectangle(this.PathBrush,Gd.X*this.GridSize,Gd.Y*this.GridSize,this.GridSize,this.GridSize);
			return true;
		}
		public bool MovePlayer(Direction Dir)
		{
			int tX=this.Player.X;
			int tY=this.Player.Y;
			switch(Dir)
			{
				case Direction.Down:
				{
					this.Player.Y++;
					break;
				}
				case Direction.Left:
				{
					this.Player.X--;
					break;
				}
				case Direction.Right:
				{
					this.Player.X++;
					break;
				}
				case Direction.Up:
				{
					this.Player.Y--;
					break;
				}
			}
			if(this[this.Player]==GridState.Enable)
				return true;
			else
			{
				this.Player.X=tX;
				this.Player.Y=tY;
				return false;
			}
		}
		public bool DrawPlayer(Graphics Graph)
		{
			if(Graph==null)
				return false;
			Graph.FillRectangle(this.PlayerBrush,this.Player.X*this.GridSize,this.Player.Y*this.GridSize,this.GridSize,this.GridSize);
			return true;
		}
		public bool ClearPlayer(Graphics Graph)
		{
			if(Graph==null)
				return false;
			Graph.FillRectangle(this.PathBrush,this.Player.X*this.GridSize,this.Player.Y*this.GridSize,this.GridSize,this.GridSize);
			return true;
		}
		public Maze() :this(new GridState[0,0]){}
		public Maze(int Width,int Height) :this(new GridState[Height,Width]){}
		public Maze(GridState[,] Grids)
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
			ReLoad(Grids);
		}
		public void ReLoad(GridState[,] Grids)
		{
			int I,J;
			this.Height=Grids.GetLength(0);
			this.Width=Grids.GetLength(1);
			this.Grids=new GridState[this.Height,this.Width];
			for(I=0;I<this.Grids.GetLength(0);I++)
				for(J=0;J<this.Grids.GetLength(1);J++)
					this.Grids[I,J]=Grids[I,J];
		}
		public Maze Copy()
		{
			return new Maze(this.Grids);
		}
		public GridState this [Grid Grid]//获取设置Grid处的状态
		{
			get
			{
				if(IsInMaze(Grid))
					return this.Grids[Grid.Y,Grid.X];
				else
					return GridState.Null;
			}
			set
			{
				if(IsInMaze(Grid))
					this.Grids[Grid.Y,Grid.X]=value;
			}
		}
		public GridState this [int X,int Y]
		{
			
			get
			{
				if(IsInMaze(X,Y))
					return this.Grids[Y,X];
				else
					return GridState.Null;
			}
			set
			{
				if(IsInMaze(X,Y))
					this.Grids[Y,X]=value;
			}
		}
		public bool IsInMaze(Grid Grid)
		{
			if(Grid.X>=0 && Grid.X<this.Width && Grid.Y>=0 && Grid.Y<this.Height)
				return true;
			else
				return false;
		}
		public bool IsInMaze(int X,int Y)
		{
			if(X>=0 && X<this.Width && Y>=0 && Y<this.Height)
				return true;
			else
				return false;
		}
		public int get_Width()
		{
			return this.Width;
		}
		public int get_Height()
		{
			return this.Height;
		}
		public Grid get_Player()
		{
			return Player;
		}
		public bool set_Player(Grid G)
		{
			return set_Player(G.X,G.Y);
		}
		public bool set_Player(int X,int Y)
		{
			if(IsInMaze(X,Y))
			{
				Player.X=X;
				Player.Y=Y;
				return true;
			}
			else
				return false;
		}
		private GridState[,] Grids;
		private int Width;
		private int Height;
		public ushort GridSize=4;
		private Grid Player=new Grid(0,0);
		public Brush WallBrush=new SolidBrush(Color.Black);
		public Brush PathBrush=new SolidBrush(Color.White);
		public Brush AnswerBrush=new SolidBrush(Color.FromArgb(128,255,128));
		public Brush PlayerBrush=new SolidBrush(Color.Red);
		Random Rnd=new Random();
	}
}
